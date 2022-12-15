using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_06 : MonoBehaviour
{
    public float timer;
    public int money = 0;

    Monster_Move_06 monsterMove = null;

    int stage = 1;
    

    public Text myMoney;
    public Text text_Timer;

    private void Start()
    {
        monsterMove = GetComponent<Monster_Move_06>();
        myMoney = GameObject.Find("Money").GetComponent<Text>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        text_Timer.text = "TIME : " + Mathf.Round(timer);
    }

    void Setmoney()
    {
        myMoney.text = "Money : " + money.ToString();
    }
}
