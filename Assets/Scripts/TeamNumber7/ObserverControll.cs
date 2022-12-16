using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ObserverControll : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("LobbyScene");
        }
    }
}
