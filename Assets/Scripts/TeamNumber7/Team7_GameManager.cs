using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class Team7_GameManager : MonoBehaviourPunCallbacks
{
    Image deadLog;
    [SerializeField] Team7_UIManager UIManager;

    #region Singleton
    private static Team7_GameManager instance;
    public static Team7_GameManager Inst
    {
        get
        {
            if (instance == null)
            {
                instance = new Team7_GameManager();
            }
            return instance;
        }
    }
    #endregion

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(); // 설정 정보로 마스터 서버 접속 시도
    }
    //=============================================================================
    #region Photon_Callback Functions

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버에 연결 성공");
        PhotonNetwork.JoinRoom("CandyKongRoom"); // 방 들어오면 입장부터 시도하라
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("진입 성공! 게임을 시작하지");
        GameStart();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("CandyKongRoom 진입 실패! 고로 방 만든다");

        PhotonNetwork.CreateRoom("CandyKongRoom", new RoomOptions { MaxPlayers = 20 }); ;
    }
    #endregion
    //=========================================================================

    private void GameStart()
    {
        //UIManager.transform.GetChild(1).gameObject.SetActive(true); // UI Score 띄우기
        //UIManager.transform.GetChild(0).gameObject.SetActive(false); // 연결중 배너 비활성화

        // 플레이어 랜덤 위치에 생성
         GameObject player = PhotonNetwork.Instantiate("Team7_Player", SetRandomPos(), Quaternion.identity);
        Camera.main.GetComponent<Team7_FollowCam>().SetCam();
        // GameObject player = 

    }


    public void Team7_ShowDieLog()
    {
        deadLog.gameObject.SetActive(true);
    }

    public void Team7_Restart()
    {
        PhotonNetwork.Instantiate("Team7_Player", SetRandomPos(), Quaternion.identity);
    }

    public void Team7_OutScene()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    private Vector3 SetRandomPos() // 게임 매니저 내부 함수 중에 랜덤 위치값을 반환하는 경우 사용
    {
        return new Vector3(Random.Range(0, 20), 1, Random.Range(0, 20));
    }


}
