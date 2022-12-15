using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PicoPark
{
    public class PlayerLeg : MonoBehaviour
    {
        public Action del_GoundCheck;
        private void OnTriggerEnter(Collider other)
        {
            del_GoundCheck();
        }
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("???");
            del_GoundCheck();
        }
    }
}

