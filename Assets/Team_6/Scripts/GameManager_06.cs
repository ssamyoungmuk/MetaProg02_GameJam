using UnityEngine;
using UnityEngine.UI;

public class GameManager_06 : MonoBehaviour
{
    public float timer;
    public Text text_Timer;

    Player_06 player = FindObjectOfType<Player_06>();

    void Update()
    {
        timer += Time.deltaTime;
        text_Timer.text = "TIME : " + Mathf.Round(timer);
    }
}
