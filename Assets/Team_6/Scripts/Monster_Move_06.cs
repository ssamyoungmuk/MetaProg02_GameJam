using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Monster_Move_06 : MonoBehaviour
{
    [SerializeField] float monsterHP;
    [SerializeField] Player_06 player = null;
    float monsterMaxHP = 0;
    float speed = 10f;
    public Image hpBar;

    private void Awake()
    {
        player = FindObjectOfType<Player_06>();
        monsterHP = monsterMaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.transform.position) <= 3f)
        {
            player.Hit();
            transform.position = new Vector3(15, 0.5f, 0);
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Stone"))
        {

            monsterHP--;

            hpBar.rectTransform.sizeDelta = new Vector2(monsterHP, 0.1f);

            if (monsterHP <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
