using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace OOO
{
    public class FollowCamera : MonoBehaviourPun
    {
        [SerializeField] private Transform playerTr = null;
        [SerializeField] private float rotationSensetive;

        [HideInInspector] public float mousAxisX = 0;
        [HideInInspector] public float mousAxisY = 0;

        private void Awake()
        {
            if(!photonView.IsMine) Destroy(this.gameObject);
                
        }

        void Update()
        {
            mousAxisX += Input.GetAxis("Mouse X");
            mousAxisY += Input.GetAxis("Mouse Y");

            transform.rotation = Quaternion.Euler(mousAxisY*-rotationSensetive, mousAxisX * rotationSensetive, 0);
        }
    }


}

