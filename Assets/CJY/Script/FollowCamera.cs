using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOO
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform playerTr = null;
        [SerializeField] private float rotationSensetive;

        [HideInInspector] public float mousAxisX = 0;
        [HideInInspector] public float mousAxisY = 0;

      
        void Update()
        {
            GetMouseAxis();

            transform.rotation = Quaternion.Euler(mousAxisY*-rotationSensetive, mousAxisX * rotationSensetive, 0);
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

            mousAxisX += Input.GetAxis("Mouse X");
            mousAxisY += Input.GetAxis("Mouse Y");
        }

    }


}

