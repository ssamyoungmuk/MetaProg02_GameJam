using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PicoPark
{
    public class SwichToggle : MonoBehaviour
    {
        [SerializeField]
        bool isToggled = false;
        [SerializeField]
        float bridgeSpeed = 0.1f;
        [SerializeField]
        GameObject Bridge;

        private void OnCollisionEnter(Collision collision)
        {
            if (isToggled == false)
            {
                isToggled = true;
                gameObject.transform.Translate(-Vector3.up * 0.5f);
                StartCoroutine(Co_OpenBridge());
            }
        }

        IEnumerator Co_OpenBridge()
        {
            int i = 0;
            while (i < 80)
            {
                Bridge.transform.Translate(-Vector3.right * bridgeSpeed);
                i++;
                yield return null;
            }
        }
    }
}

