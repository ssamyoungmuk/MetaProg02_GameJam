using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Monster_Move : MonoBehaviour
{
    [SerializeField] float monsterHP;
    float monsterMaxHP = 0;
    float speed = 10f;
    public Image hpBar;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        StartCoroutine(MonsterDie());  // 삭제할것
    }

    IEnumerator MonsterDie()  // 트리거 만들고나면 삭제
    {
        yield return new WaitForSeconds(4f);

        transform.position = new Vector3(15, 0.5f, 0);
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Stone"))
        {
            monsterHP--;

            hpBar.rectTransform.sizeDelta = new Vector2(monsterHP, 0.1f);

            if(monsterHP <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
