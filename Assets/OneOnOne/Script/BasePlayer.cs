using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace OOO
{
    [RequireComponent(typeof(PlayerData),typeof(Rigidbody),typeof(CapsuleCollider))]
    public abstract class BasePlayer : MonoBehaviourPun
    {
        [SerializeField] private FollowCamera cam = null;
        [HideInInspector] public PlayerData myData = null;

        private Rigidbody rb = null;

        Quaternion rightArmOriginPos;
        Quaternion leftArmOriginPos;

        bool hitFlag = false;
        bool dead = false;
        
        private void Awake()
        {
            hitFlag = false;
            myData = GetComponent<PlayerData>();
            rb = GetComponent<Rigidbody>();
            myData.info.curHp = myData.info.maxHp;
            rb.freezeRotation = true;
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
                myData.info.leftArm.Rotate(100f, 0, -30f, Space.Self);
            }
            if (Input.GetMouseButtonUp(0))
            {
                myData.info.leftArm.rotation = rightArmOriginPos;
            }

            if (Input.GetMouseButtonDown(1))
            {
                myData.info.rightArm.Rotate(100f, 0, 30f, Space.Self);
            }
            if (Input.GetMouseButtonUp(1))
            {
                myData.info.rightArm.rotation = leftArmOriginPos;
            }
        }
        protected void PlayerMoveAndRotation()
        {
            if (!photonView.IsMine) return;

            float  getAxisX = Input.GetAxis("Horizontal");
            float getAxisZ = Input.GetAxis("Vertical");
            
            float xMove = myData.info.speed * getAxisX * Time.fixedDeltaTime;
            float zMove = myData.info.speed * getAxisZ * Time.fixedDeltaTime;

            this.transform.position += new Vector3(xMove, 0f,zMove);
            this.transform.forward = new Vector3(xMove, 0f, zMove);
        }

        public void TransferDamage()
        {
            this.gameObject.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
            rb.AddForce(1f,2f,3f,ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (hitFlag) return;

            if (collision.gameObject.tag == "DeadZone")
                Destroy(this.gameObject);

            if (collision.gameObject.tag == "Weapon")
            {
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
                Debug.Log(time);

                yield return null;  

                if (time > 2f)
                {
                    Debug.Log("hi");
                    this.transform.rotation = Quaternion.identity;
                    rb.freezeRotation = true;
                    hitFlag = false;
                    yield break;
                }
            }
        }
    }
}