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

    Player_06 player = FindObjectOfType<Player_06>();

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
