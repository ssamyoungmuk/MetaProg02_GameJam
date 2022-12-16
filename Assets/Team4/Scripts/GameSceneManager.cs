using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

namespace PicoPark
{
    public class GameSceneManager : MonoBehaviourPunCallbacks
    {
        private void Awake()
        {
            PhotonNetwork.Instantiate("4_PicoPlayer", Vector3.zero,Quaternion.identity);
        }

        public void GameToLobby()
        {
            Debug.Log("Lobby");
            SceneManager.LoadScene("LobbyScene");
            Debug.Log("photonCut");
            PhotonNetwork.Disconnect();
        }
    }
}
