using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace OOO
{
    [RequireComponent(typeof(PlayerData),typeof(CapsuleCollider))]
    public abstract class BasePlayer : MonoBehaviourPun
    {
        [SerializeField] private FollowCamera cam = null;
        [HideInInspector] public PlayerData myData = null;

        private Rigidbody rb = null;
        private CapsuleCollider myCollider;

        Quaternion rightArmOriginPos;
        Quaternion leftArmOriginPos;

        Vector3 lookForward;
        Vector3 lookRight;
        Vector3 moveDir;

        float freezTime = 3f;
        bool hitFlag = false;
        bool dead = false;

        [HideInInspector] public bool leftAttackCheck = false;
        [HideInInspector] public bool rightAttackCheck = false;

        private void Awake()
        {
            hitFlag = false;
            myData = GetComponent<PlayerData>();
            myCollider = GetComponent<CapsuleCollider>();
            myData.info.curHp = myData.info.maxHp;

            if (photonView.IsMine)
            {
                myCollider.enabled = true;
                rb = this.gameObject.AddComponent<Rigidbody>();
                rb.freezeRotation = true;
            }


        }

        private void Update()
        {
            if (dead) return;
            if (hitFlag) return;

            InputKey();
        }
        private void FixedUpdate()
        {
            if (hitFlag) return;
            PlayerMoveAndRotation();
        }

        protected virtual void InputKey()
        {
            if (!photonView.IsMine) return;

            if (Input.GetMouseButtonDown(0))
            {
                leftAttackCheck = true;
                myData.info.leftArm.Rotate(100f, 0, -30f, Space.Self);
            }
            if (Input.GetMouseButtonUp(0))
            {
                leftAttackCheck = false;
                myData.info.leftArm.rotation = rightArmOriginPos;
            }

            if (Input.GetMouseButtonDown(1))
            {
                rightAttackCheck = true;
                myData.info.rightArm.Rotate(100f, 0, 30f, Space.Self);
            }
            if (Input.GetMouseButtonUp(1))
            {
                rightAttackCheck = false;
                myData.info.rightArm.rotation = leftArmOriginPos;
            }
        }

        protected void PlayerMoveAndRotation()
        {
            if (!photonView.IsMine) return;

            float getAxisX = Input.GetAxis("Horizontal");
            float getAxisZ = Input.GetAxis("Vertical");


            lookForward = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z).normalized;
            lookRight = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z).normalized;

            moveDir = lookForward * getAxisZ + lookRight * getAxisX;

            Vector3 newPos = myData.info.speed * Time.deltaTime * moveDir.normalized;

            if (getAxisX != 0 || getAxisZ != 0)
            {
                rb.transform.rotation = Quaternion.LookRotation(moveDir);
            }
            rb.position += newPos;
        }

        public void TransferDamage()
        {
            this.gameObject.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
            rb.AddForce(2f, freezTime, 4f,ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (hitFlag) return;

            if (collision.gameObject.tag == "DeadZone")
                Destroy(this.gameObject);

            if (collision.gameObject.tag == "Weapon")
            {
                Debug.Log(collision.gameObject.tag);
                StartCoroutine(nameof(Hit));
                TransferDamage();
            }
        }

        IEnumerator Hit()
        {
            hitFlag = true;
            float time = 0f;
            rb.freezeRotation = false;

            while (true)
            {
                time += Time.deltaTime;

                yield return null;  

                if (time > freezTime)
                {
                    freezTime += 1f;
                    this.transform.rotation = Quaternion.identity;
                    rb.freezeRotation = true;
                    hitFlag = false;
                    yield break;
                }
            }
        }
    }
}