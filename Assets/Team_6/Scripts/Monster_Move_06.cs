using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Monster_Move_06 : MonoBehaviour
{
    [SerializeField] Player_06 player = null;
    float speed = 10f;
    public Image hpBar;

    private void OnEnable()
    {
        hpBar.fillAmount = 1;
    }

    private void Awake()
    {
        player = FindObjectOfType<Player_06>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.transform.position) <= 3f)
        {
            player.Hit();
            ReturnPool();
        }
    }

    public void Hit()
    {
        if(gameObject.name == "Monster_Gorilla(Clone)")
            hpBar.fillAmount -= 0.25f;
        else
            hpBar.fillAmount -= 0.33f;

        if (hpBar.fillAmount <= 0.1f)
        {
            ReturnPool();
        }
    }

    private void ReturnPool()
    {
        transform.position = new Vector3(15, 0.5f, 0);
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        gameObject.SetActive(false);
    }
}
