using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PicoPark
{
    public class KeyMove : MonoBehaviour
    {
        public Action del_KeyCheck;
        [SerializeField] GameObject Master = null;
        [SerializeField] bool haveMaster = false;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Master = other.gameObject;
                haveMaster = true;
                del_KeyCheck();
            }
        }
        private void Update()
        {
            if (haveMaster == true)
            {
                Vector3 newPos = Master.transform.position;
                newPos.y = newPos.y + 1;
                gameObject.transform.position = newPos;
            }
        }
    }
}

