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

        private void FixedUpdate()
        {

            PlayerMoveAndRotation();

        }
        private void Update()
        {
            if (dead) return;

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


        protected virtual void InputKey()
        {
            if (!photonView.IsMine) return;

            if (Input.GetMouseButtonDown(0))
            {
                myData.info.leftArm.Rotate(100f, 0, -30f, Space.Self);
            }
            if(Input.GetMouseButtonUp(0))
            {
                myData.info.leftArm.rotation = rightArmOriginPos;
            }
                

            if(Input.GetMouseButtonDown(1))
            {
                myData.info.rightArm.Rotate(100f, 0, 30f, Space.Self);
            }
            if (Input.GetMouseButtonUp(1))
            {
                myData.info.rightArm.rotation = leftArmOriginPos;
            }

        }

        public void TransferDamage()
        {
            //myData.info.curHp -= damage;

            this.gameObject.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);

            //if(myData.info.curHp<=0)
            //{
            //    //dead = true;

            //    //rb.AddForce(Vector3.up*10f, ForceMode.Impulse);

            //    //Invoke("Active", 3f);
            //}
        }

        void Active()
        {
            this.gameObject.SetActive(false);
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "DeadZone")
                Destroy(this.gameObject);


            if (collision.gameObject.tag=="Weapon")
            {
                TransferDamage();
            }

        }


    }


    

    
}