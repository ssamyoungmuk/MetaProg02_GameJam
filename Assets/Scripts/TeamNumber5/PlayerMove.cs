using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ElevatorSimulator
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5.0f; // 이동 속도
        [SerializeField] private float turnSpeed = 500f;

        [SerializeField] private float lookSensitivity; //민감도

        private float cameraRotationLimit;
        private float currentCameraRotationX;

        private float xAxis, zAxis;
        Rigidbody rb = null;
        [SerializeField] Camera cam;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {

            Move();
            CameraRotation();
            CharacterRotation();
        }
        private void Move()
        {
            xAxis = Input.GetAxis("Horizontal");
            zAxis = Input.GetAxis("Vertical");

            Vector3 moveDir = (Vector3.forward * zAxis) + (Vector3.right * xAxis);

            Vector3 velocity = moveDir.normalized * moveSpeed * Time.deltaTime;

            //rb.MovePosition(transform.position + velocity * Time.deltaTime);
            transform.Translate(velocity, Space.Self);

        }
        void CameraRotation()
        {
            float xRotation = Input.GetAxisRaw("Mouse Y");
            float cameraRoatationX = xRotation * lookSensitivity * turnSpeed;

            currentCameraRotationX -= cameraRoatationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
        void CharacterRotation()
        {
            float yRotation = Input.GetAxisRaw("Mouse X");
            Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity * turnSpeed;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(characterRotationY));
        }

    }
}