using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace OOO
{
    public class Weapon : MonoBehaviourPun
    {
        [SerializeField] BasePlayer player = null;

        [System.Serializable]
        public enum Type { Left,Right}

        [SerializeField] Type type;

        BoxCollider Collider = null;

        private void Awake()
        {
            Collider = GetComponent<BoxCollider>();
        }

        private void Update()
        {
            if (type == Type.Left)
            {
                Debug.Log("hi");
                if(player.leftAttackCheck)
                {
                    photonView.RPC(nameof(EnableCall), RpcTarget.All);
                }
                else
                {
                    photonView.RPC(nameof(DisableCall), RpcTarget.All);
                }

            }
            
            
            if(type == Type.Right)
            {
                if (player.rightAttackCheck)
                {
                    photonView.RPC(nameof(EnableCall), RpcTarget.All);
                }
                else
                {
                    photonView.RPC(nameof(DisableCall), RpcTarget.All);
                }
            }


        }

        [PunRPC]
        void EnableCall()
        {
            Collider.enabled = true;
        }
        [PunRPC]
        void DisableCall()
        {
            Collider.enabled = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name.Substring(0,9));
            //if()
        }
    }
}

