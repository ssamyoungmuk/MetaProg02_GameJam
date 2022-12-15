using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; //선언
using Photon.Realtime; //선언
using UnityEngine.UI; //선언

public class photonManager1 : MonoBehaviourPunCallbacks //클래스 상속
{
    public Text StatusText;
    public InputField NickNameInput;
    public InputField roomNameInput;
    public GameObject uiPanel;
    public byte userNum = 5;

    private bool connect = false;

    //현재 상태 표시 
    private void Update() => StatusText.text = PhotonNetwork.NetworkClientState.ToString();

    //서버에 접속
    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    //연결 되면 호출
    public override void OnConnectedToMaster()
    {
        print("서버접속완료");
        string nickName = PhotonNetwork.LocalPlayer.NickName;
        nickName = NickNameInput.text;
        print("당신의 이름은 " + nickName + " 입니다.");
        connect = true;
    }

    //연결 끊기
    public void Disconnect() => PhotonNetwork.Disconnect();
    //연결 끊겼을 때 호출
    public override void OnDisconnected(DisconnectCause cause) => print("연결끊김");

    //방 입장
    public void JoinRoom()
    {
        if (connect)
        {
            PhotonNetwork.JoinRandomRoom();
            uiPanel.SetActive(false);
            print(roomNameInput.text + "방에 입장하였습니다.");
        }
    }

    //랜덤 룸 입장에 실패하면 새로운 방 생성 (master 방 생성)
    public override void OnJoinRandomFailed(short returnCode, string message) =>
    PhotonNetwork.CreateRoom(roomNameInput.text, new RoomOptions { MaxPlayers = userNum });

    //방에 입장 했을 때 호출 
    public override void OnJoinedRoom()
    => PhotonNetwork.Instantiate("player", Vector3.zero, Quaternion.identity);
}