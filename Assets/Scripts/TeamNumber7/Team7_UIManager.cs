using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Team7_UIManager : MonoBehaviour
{

    public void Restart()
    {
        Team7_GameManager.Inst.Team7_Restart();
    }

    public void QuitGame()
    {
        Team7_GameManager.Inst.Team7_OutScene();
    }

}
