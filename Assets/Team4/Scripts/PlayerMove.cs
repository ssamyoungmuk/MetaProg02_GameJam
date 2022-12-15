using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PicoPark
{
    enum State
    {
        Jump,
        Ground,
    }

    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] bool isGetKey = false;
        [SerializeField] float jumpPower = 330;
        [SerializeField] float moveSpeed = 1.2f;
        [SerializeField] State playerState = State.Ground;

        Rigidbody rigidbody = null;
        Vector3 jumpVelocity = new Vector3(0, 330, 0);
        private void Awake()
        {
            rigidbody = gameObject.GetComponent<Rigidbody>();
        }
        private void Start()
        {
            jumpVelocity.y = jumpPower;

        }
        void Update()
        {
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
                newPos.x -= 5;
                gameObject.transform.position = newPos;
            }


            playerState = State.Ground;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag != "Wall")
                playerState = State.Ground;
        }
    }
}

