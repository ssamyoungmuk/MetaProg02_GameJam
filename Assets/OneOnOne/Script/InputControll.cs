using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControll : MonoBehaviour
{

    [SerializeField] private float rotationSensetive;

    [HideInInspector] public float mousAxisY = 0;
    [HideInInspector] public float mousAxisX = 0;

    // Update is called once per frame
    void Update()
    {
        GetMouseAxis();

        mousAxisX += Input.GetAxis("Mouse X");
    }

    void GetMouseAxis()
    {
        if (mousAxisY >= 1)
        {
            mousAxisY = 0.95f;
            return;
        }
        if (mousAxisY <= -5)
        {
            mousAxisY = -4.9f;
            return;
        }

        mousAxisY += Input.GetAxis("Mouse Y");
    }
}
