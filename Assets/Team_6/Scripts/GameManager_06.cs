using UnityEngine;
using UnityEngine.UI;

public class GameManager_06 : MonoBehaviour
{
    static GameManager_06 instance = null;
    public static GameManager_06 Instance { get { return instance; } set { instance = value; } }

    public float timer;
    public int money = 0;
    public int stage = 1;

    Monster_Move_06 monsterMove = null;

    public Text myMoney;
    public Text myTimer;
    public Text myStage;

    Player_06 player = null;

    private void Awake()
    {
        instance = this;
        player = FindObjectOfType<Player_06>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        myTimer.text = "TIME : " + Mathf.Round(timer);
        myMoney.text = "Money : " + money.ToString();
        myStage.text = "Stage : " + stage.ToString();
    }

    public void AddMoney(int addMoney)
    {
        money += addMoney;
    }

    public void ClickUpgrageButton()
    {
        if (money < 100)
            return;
        else
        {
            money -= 100;
            player.Upgrade();
        }
    }
}
