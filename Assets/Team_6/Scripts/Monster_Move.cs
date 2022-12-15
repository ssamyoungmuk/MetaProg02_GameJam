using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Move : MonoBehaviour
{
    float speed = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        StartCoroutine(MonsterDie());
    }

    IEnumerator MonsterDie()
    {
        yield return new WaitForSeconds(2f);

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;


        gameObject.SetActive(false);
    }
}
