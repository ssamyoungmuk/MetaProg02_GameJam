using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOO
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] BasePlayer player = null;

        [System.Serializable]
        public enum Type { Left,Right}

        [SerializeField] Type type;

        BoxCollider collider = null;

        private void Awake()
        {
            collider = GetComponent<BoxCollider>();
        }

        private void Update()
        {
            if (type == Type.Left)
            {
                if(player.leftAttackCheck)
                {
                    collider.enabled = true;
                }
                else
                {
                    collider.enabled = false;
                }

            }
            
            
            if(type == Type.Right)
            {
                if (player.rightAttackCheck)
                {
                    collider.enabled = true;
                }
                else
                {
                    collider.enabled = false;
                }
            }


        }



        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name.Substring(0,9));
            //if()
        }
    }
}

