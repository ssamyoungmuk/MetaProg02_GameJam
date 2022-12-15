using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform playerArm = null;
    [SerializeField] float throwAngle = 0f;
    [SerializeField] Vector3 angle = new Vector3(0,0,-1);

    [SerializeField] private bool isThrowing = false;

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
            Debug.Log("µ¹¾Æ µ¹¾Æ~");
            if (playerArm.rotation.z >= 2f)
            {
                Debug.Log("È¸Àü²ôÀÄ");
                isThrowing = false;
                yield break;
            }
            playerArm.localEulerAngles += angle * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
