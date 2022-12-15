using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5.0f; // 이동 속도
    private float xAxis, zAxis;
    Rigidbody rb = null;

    private float yRotate, yRotateMove;
    public float rotateSpeed = 500.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        zAxis = Input.GetAxis("Vertical");

        Vector3 moveDir = (Vector3.forward * zAxis) + (Vector3.right * xAxis);

        transform.Translate(moveDir.normalized * Time.deltaTime * moveSpeed, Space.Self);

        yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;
        
        yRotate = transform.eulerAngles.y + yRotateMove;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotate, 0);


    }

    private void LateUpdate()
    {
        Camera.main.transform.position = this.transform.position;
    }
}
