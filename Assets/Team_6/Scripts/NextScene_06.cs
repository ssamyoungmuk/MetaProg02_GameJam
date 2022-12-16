using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene_06 : MonoBehaviour
{
    public void OnClick_GameStartButton()
    {
        SceneManager.LoadScene("Team_6");
        Time.timeScale = 1;
    }
}
