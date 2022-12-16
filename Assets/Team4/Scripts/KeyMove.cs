using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

namespace PicoPark
{
    public class KeyMove : MonoBehaviourPun
    {
        public Action del_KeyCheck;
        [SerializeField] GameObject master = null;
        [SerializeField] bool haveMaster = false;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                haveMaster = true;
                master = other.gameObject;
                other.gameObject.GetPhotonView().RPC("KeyStateOn", RpcTarget.All);
                other.gameObject.GetPhotonView().RPC("KeyMarkOn", RpcTarget.All);
                //  del_KeyCheck();
            }
        }
        private void Update()
        {
            if (GameMgr.Instance.GetOverState == true) return;
            if (haveMaster == true && master != null)
            {
                Vector3 newPos = master.transform.position;
                newPos.y = newPos.y + 1;
                gameObject.transform.position = newPos;
            }
        }
    }
}

