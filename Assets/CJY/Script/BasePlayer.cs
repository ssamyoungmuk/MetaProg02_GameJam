using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOO
{
    [RequireComponent(typeof(PlayerData),typeof(Rigidbody),typeof(CapsuleCollider))]
    public abstract class BasePlayer : MonoBehaviour
    {
        [SerializeField] private FollowCamera cam = null;
        [HideInInspector] public PlayerData myData = null;

        private Rigidbody rb = null;

        float getAxisX = 0;
        float getAxisZ = 0;

        Quaternion rightArmOriginPos;
        bool rightAttackCheck = false;

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
            getAxisX = Input.GetAxis("Horizontal");
            getAxisZ = Input.GetAxis("Vertical");

            transform.Translate(myData.info.speed*getAxisX*Time.deltaTime,0, myData.info.speed * getAxisZ * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0, cam.mousAxisX * myData.info.rotationSensetive, 0);
        }


        protected void InputKey()
        {
            if(Input.GetMouseButtonDown(0)&& leftAttackCheck==false)
            {
                leftAttackCheck = true;

               StartCoroutine(AttackDown(myData.info.leftArm, leftArmOriginPos));
            }
            else if(Input.GetMouseButtonDown(1)&& rightAttackCheck==false)
            {
                rightAttackCheck = true;

               StartCoroutine(AttackDown(myData.info.rightArm, rightArmOriginPos));
            }


        }


        #region AttackCorutine

        private IEnumerator AttackDown(Transform arm,Quaternion originPos)
        {


            while (true)
            {
                arm.Rotate(10f, 0, 0, Space.Self);

                yield return new WaitForSeconds(Time.deltaTime);
               
                if (arm.localRotation.x <= -0.6f)
                {

                    StartCoroutine(AttackStart(arm,originPos));

                    yield break;
                }

            }
        }

        private IEnumerator AttackStart(Transform arm, Quaternion originPos)
        {
            while (true)
            {
                arm.Rotate(-10f, 0, 0,Space.Self);
                yield return new WaitForSeconds(Time.deltaTime);
                Debug.Log("GD");
                if (arm.localRotation.x >= 0.3f)
                {
                    arm.rotation = originPos;

                    if(arm.name=="LeftArm")
                    {
                        leftAttackCheck = false;
                    }
                    else if(arm.name == "RightArm")
                    {
                        rightAttackCheck = false;
                    }
                    
                    yield break;
                }

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