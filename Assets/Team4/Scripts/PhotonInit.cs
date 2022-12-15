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
        Debug.Log("������ ���� ���� �Ϸ�");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ���� �Ϸ�");
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
