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
        private PlayerData myData = null;

        float getAxisX = 0;
        float getAxisZ = 0;

        Quaternion rightArmOriginPos;
        bool rightAttackCheck = false;

        Quaternion leftArmOriginPos;
        bool leftAttackCheck = false;


        private void Awake()
        {
            myData = GetComponent<PlayerData>();
            rightArmOriginPos = myData.info.rightArm.transform.rotation;
        }

        private void Update()
        {
            PlayerMoveAndRotation();

            InputKey();
        }


        protected virtual void PlayerMoveAndRotation()
        {
            getAxisX = Input.GetAxis("Horizontal");
            getAxisZ = Input.GetAxis("Vertical");

            transform.Translate(myData.info.speed*getAxisX*Time.deltaTime,0, myData.info.speed * getAxisZ * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0, cam.mousAxisX * myData.info.rotationSensetive, 0);
        }


        protected virtual void InputKey()
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
                arm.Rotate(10f, 0, 0);

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
                arm.Rotate(-10f, 0, 0);
                yield return new WaitForSeconds(Time.deltaTime);

                if (arm.localRotation.x >= 0.3f)
                {
                    arm.rotation = originPos;

                    if(arm.name=="LeftArm")
                    {
                        leftAttackCheck = false;
                    }
                    else
                    {
                        rightAttackCheck = false;
                    }
                    
                    yield break;
                }

            }
        }

        #endregion

    }
}