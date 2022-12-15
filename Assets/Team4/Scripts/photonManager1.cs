using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; //����
using Photon.Realtime; //����
using UnityEngine.UI; //����

public class photonManager1 : MonoBehaviourPunCallbacks //Ŭ���� ���
{
    public Text StatusText;
    public InputField NickNameInput;
    public InputField roomNameInput;
    public GameObject uiPanel;
    public byte userNum = 5;

    private bool connect = false;

    //���� ���� ǥ�� 
    private void Update() => StatusText.text = PhotonNetwork.NetworkClientState.ToString();

    //������ ����
    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    //���� �Ǹ� ȣ��
    public override void OnConnectedToMaster()
    {
        print("�������ӿϷ�");
        string nickName = PhotonNetwork.LocalPlayer.NickName;
        nickName = NickNameInput.text;
        print("����� �̸��� " + nickName + " �Դϴ�.");
        connect = true;
    }

    //���� ����
    public void Disconnect() => PhotonNetwork.Disconnect();
    //���� ������ �� ȣ��
    public override void OnDisconnected(DisconnectCause cause) => print("�������");

    //�� ����
    public void JoinRoom()
    {
        if (connect)
        {
            PhotonNetwork.JoinRandomRoom();
            uiPanel.SetActive(false);
            print(roomNameInput.text + "�濡 �����Ͽ����ϴ�.");
        }
    }

    //���� �� ���忡 �����ϸ� ���ο� �� ���� (master �� ����)
    public override void OnJoinRandomFailed(short returnCode, string message) =>
    PhotonNetwork.CreateRoom(roomNameInput.text, new RoomOptions { MaxPlayers = userNum });

    //�濡 ���� ���� �� ȣ�� 
    public override void OnJoinedRoom()
    => PhotonNetwork.Instantiate("player", Vector3.zero, Quaternion.identity);
}