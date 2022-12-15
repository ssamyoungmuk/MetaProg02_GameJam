using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_06 : MonoBehaviour
{
    public float timer;
    public Text text_Timer;


    void Update()
    {
        timer += Time.deltaTime;
        text_Timer.text = "TIME : " + Mathf.Round(timer);
    }
}
