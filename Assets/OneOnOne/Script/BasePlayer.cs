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

        float getAxisX = 0;
        float getAxisZ = 0;

        Quaternion rightArmOriginPos;
        bool rightAttackCheck = false;

        Quaternion AttackQuat = Quaternion.EulerAngles(-10f,0f,0f);

        Quaternion leftArmOriginPos;
        bool leftAttackCheck = false;

        bool dead = false;

        private void Awake()
        {
            myData = GetComponent<PlayerData>();
            rb = GetComponent<Rigidbody>();
            myData.info.curHp = myData.info.maxHp;
        }

        private void Update()
        {
            if (dead) return;

            PlayerMoveAndRotation();

            InputKey();
        }


        protected void PlayerMoveAndRotation()
        {
            if (!photonView.IsMine) return;
            getAxisX = Input.GetAxis("Horizontal");
            getAxisZ = Input.GetAxis("Vertical");

            transform.Translate(myData.info.speed*getAxisX*Time.fixedDeltaTime,0, myData.info.speed * getAxisZ * Time.fixedDeltaTime);

            transform.rotation = Quaternion.Euler(0, cam.mousAxisX * myData.info.rotationSensetive, 0);
        }


        protected void InputKey()
        {
            if (!photonView.IsMine) return;

            if (Input.GetMouseButtonDown(0)&& leftAttackCheck==false)
            {
                leftAttackCheck = true;
                StartCoroutine(LeftAttack());
            }
            else if(Input.GetMouseButtonDown(1)&& rightAttackCheck==false)
            {
                rightAttackCheck = true;
                StartCoroutine(RightAttack());
            }

        }


        #region AttackCorutine

        private IEnumerator LeftAttack()
        {
            while (true)
            {
                myData.info.leftArm.localRotation = Quaternion.Lerp(myData.info.leftArm.localRotation, AttackQuat, 0.1f);

                if (myData.info.leftArm.localRotation.x >= 0.9f)
                {
                    myData.info.leftArm.localRotation = Quaternion.identity;
                    leftAttackCheck = false;
                    yield break;
                }

                yield return null;
            }
        }

        private IEnumerator RightAttack()
        {
            while (true)
            {
                myData.info.rightArm.localRotation = Quaternion.Lerp(myData.info.rightArm.localRotation, AttackQuat, 0.1f);
                
                if (myData.info.rightArm.localRotation.x >= 0.9f)
                {
                    myData.info.rightArm.localRotation = Quaternion.identity;
                    rightAttackCheck = false;
                    yield break;
                }

                yield return null;
            }
        }

        #endregion

        public void TransferDamage(float damage)
        {
            myData.info.curHp -= damage;

            this.gameObject.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);

            if(myData.info.curHp<=0)
            {
                dead = true;

                rb.AddForce(Vector3.up*10f, ForceMode.Impulse);

                Invoke("Active", 3f);
            }
        }

        void Active()
        {
            this.gameObject.SetActive(false);
        }

    }


    
}