using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_06 : MonoBehaviour
{
    [SerializeField] Transform playerArm = null;
    [SerializeField] Vector3 angle = new Vector3(0,0,-1);

    [SerializeField] private float throwSpeed = 0f;
    [SerializeField] private bool isThrowing = false;
    [SerializeField] private int invert = 1;
    [SerializeField] GameObject rock = null;
    [SerializeField] GameObject[] rocks = null;

    [SerializeField] List<GameObject> hp = null;
    [SerializeField] List<GameObject> UI = null;
    private int curRockIdx = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isThrowing == false)
        {
            isThrowing = true;
            rock.SendMessage("Fire", SendMessageOptions.DontRequireReceiver);
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
            else if(invert == -1 && playerArm.rotation.z <= 0.3f)
            {
                isThrowing = false;
                invert = 1;
                yield break;
            }
            playerArm.localEulerAngles += angle * invert * Time.fixedDeltaTime * throwSpeed;
            yield return new WaitForFixedUpdate();           
        }
    }

    public void Hit()
    {
        hp[0].SetActive(false);
        hp.RemoveAt(0);

        if (hp.Count == 0)
        {
            UI[0].SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Upgrade()
    {
        throwSpeed += 2;
        rock.GetComponent<Rock_06>().MoveSpeed += 0.025f;
        rock = ChangeRock(++curRockIdx);
    }

    private GameObject ChangeRock(int index)
    {
        for(int i = 0; i < rocks.Length; i++)
        {
            rocks[i].SetActive(false);
        }
        rocks[index].SetActive(true);
        return rocks[index];
    }
}
