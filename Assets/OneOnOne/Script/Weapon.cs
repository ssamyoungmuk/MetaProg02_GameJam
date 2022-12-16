using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOO
{
    public class Weapon : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name.Substring(0,9));
            //if()
        }
    }
}

