using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform rockStartPos = null;
    [SerializeField] Transform playerArm = null;
    [SerializeField] float throwAngle = 0f;
    [SerializeField] Vector3 angle = new Vector3(0,0,-1);

    [SerializeField] private float throwSpeed = 0f;
    [SerializeField] private bool isThrowing = false;
    [SerializeField] private int invert = 1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isThrowing == false)
        {
            isThrowing = true;
            StartCoroutine(Throw());
        }
    }

    IEnumerator Throw()
    {
        while (true)
        {
            if (invert == 1 && playerArm.rotation.z >= 0.99f)
            {
                invert = -1;
            }
            if(invert == -1 && playerArm.rotation.z <= 0.01f)
            {
                isThrowing = false;
                invert = 1;
                yield break;
            }
            playerArm.localEulerAngles += angle * invert * Time.fixedDeltaTime * throwSpeed;
            yield return new WaitForFixedUpdate();
            
        }
    }
}
