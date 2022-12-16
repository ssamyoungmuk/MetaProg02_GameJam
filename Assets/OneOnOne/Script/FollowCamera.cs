using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace OOO
{
    public class FollowCamera : MonoBehaviourPun
    {
        [SerializeField] GameObject targetObj;
        private Transform targetTr;
        [SerializeField] private float rotSensetive;

        [HideInInspector] public float mousAxisY = 0;
        [HideInInspector] public float mousAxisX = 0;

        private void Awake()
        {
            if (!photonView.IsMine) Destroy(this.gameObject);
            targetTr = targetObj.transform;
            this.transform.parent = null;
        }

        private void Update()
        {
            if (targetObj == null)
            {
                Destroy(this.gameObject);
            }
            else
            {

                this.transform.position = targetTr.position;
                this.transform.LookAt(targetTr);

                GetMouseAxis();
                this.transform.localRotation = Quaternion.Euler(-mousAxisY, mousAxisX, 0);
            }
        }

        void GetMouseAxis()
        {
            if (mousAxisY >= 35)
            {
                mousAxisY = 35f;
            }
            if (mousAxisY <= -35)
            {
                mousAxisY = -35f;
            }

            mousAxisX += Input.GetAxis("Mouse X");
            mousAxisY += Input.GetAxis("Mouse Y");
        }
    }
}

