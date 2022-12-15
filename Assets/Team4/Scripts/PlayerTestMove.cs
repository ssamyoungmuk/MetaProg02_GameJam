using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestMove : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotSpeed = 120.0f;
    private Transform tr;
    

    private void Start()
    {
        tr = GetComponent<Transform>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        tr.Translate(Vector3.forward * v * Time.deltaTime * speed);
        tr.Rotate(Vector3.up * h * Time.deltaTime * rotSpeed);
    }
}
