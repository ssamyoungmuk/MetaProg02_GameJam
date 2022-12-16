using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

namespace PicoPark
{
    enum State
    {
        Jump,
        Ground,
    }

    public class PlayerMove : MonoBehaviourPun
    {
        [SerializeField] bool isGetKey = false;
        [SerializeField] float jumpPower = 330;
        [SerializeField] float moveSpeed = 1.2f;
        [SerializeField] State playerState = State.Ground;
        [SerializeField] KeyMove keyMove;
        [SerializeField] GameObject keyMark;

        Rigidbody rigidbody = null;
        Vector3 jumpVelocity = new Vector3(0, 330, 0);
        private void Awake()
        {
            keyMove = FindObjectOfType<KeyMove>();
            rigidbody = gameObject.GetComponent<Rigidbody>();
        }
        private void Start()
        {
            jumpVelocity.y = jumpPower;
            keyMark.SetActive(false);
            //keyMove.del_KeyCheck = () => isGetKey = true;
        }
        void Update()
        {
            if (GameMgr.Instance.GetOverState == true) return;
            if (!photonView.IsMine) return;
            if (Input.GetKeyDown(KeyCode.Space) && playerState == State.Ground)
            {
                rigidbody.AddForce(jumpVelocity);
                playerState = State.Jump;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                // 문열고 들어갈 떄만 사용
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rigidbody.transform.Translate(-Vector3.right * Time.deltaTime * moveSpeed);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rigidbody.transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            }
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.tag == "DeadZone")
            {
                Vector3 newPos = gameObject.transform.position;
                newPos.y = 8;
                newPos.x -= 3;
                gameObject.transform.position = newPos;
            }
            playerState = State.Ground;
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Door" && isGetKey == true && Input.GetKeyDown(KeyCode.UpArrow))
            {
                isGetKey = false;
                Vector3 newPos = gameObject.transform.position;
                newPos.x = newPos.x + 12;
                gameObject.transform.position = newPos;
                GameMgr.Instance.uiMgr.gameObject.GetPhotonView().RPC("CountPeopleNum", RpcTarget.All);
            }
            else if (other.tag == "BackDoor" && isGetKey == false && Input.GetKeyDown(KeyCode.UpArrow))
            {
                isGetKey = true;
                Vector3 newPos = gameObject.transform.position;
                newPos.x = newPos.x - 12;
                gameObject.transform.position = newPos;
                GameMgr.Instance.uiMgr.gameObject.GetPhotonView().RPC("MinusPeopleNum", RpcTarget.All);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag != "Wall")
                playerState = State.Ground;
        }

        [PunRPC]
        public void KeyStateOn() => isGetKey = true;
        [PunRPC]
        public void KeyMarkOn() => keyMark.SetActive(true);


    }
}

