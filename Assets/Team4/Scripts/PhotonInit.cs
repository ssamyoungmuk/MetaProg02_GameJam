using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        Debug.Log("마스터 서버 접속 완료");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");
    }

    void OnGUI()
    {
      
    }
}
