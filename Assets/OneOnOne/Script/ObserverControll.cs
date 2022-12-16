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
            Screen.SetResolution(1920, 1080, false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("LobbyScene");
        }
    }
}
