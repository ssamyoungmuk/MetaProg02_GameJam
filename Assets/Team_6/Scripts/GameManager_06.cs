using UnityEngine;
using UnityEngine.UI;

public class GameManager_06 : MonoBehaviour
{
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
        player = FindObjectOfType<Player_06>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        myTimer.text = "TIME : " + Mathf.Round(timer);
        myMoney.text = "Money : " + money.ToString();
        myStage.text = "Stage : " + stage.ToString();
    }
}
