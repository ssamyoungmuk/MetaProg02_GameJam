using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class Team7_GameManager : MonoBehaviourPunCallbacks
{
    Image deadLog;
    [SerializeField] TextMeshProUGUI nameInput;
    [SerializeField] GameObject Panel;
    GameObject candy = null;

    private string saveName = null;
    private string inputName = null;

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
        //InputName();
        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "fc387611-95cc-42c2-ae93-6e4d5bc85e09";
        PhotonNetwork.ConnectUsingSettings(); // 설정 정보로 마스터 서버 접속 시도

    }

    private void InputName()
    {
        StartCoroutine(InputNameTime());
    }

    IEnumerator InputNameTime()
    {
        Debug.Log("코루틴 입장");
        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            if (nameInput.text == null)
            {
                Debug.Log("아이디를 입력하세요");
            }

            else
            {
                inputName = nameInput.text;
                saveName = inputName;
                Debug.Log("입력 완료");

                GameStart();
                InstCandy(50);
            }
        }
        yield return new WaitForSeconds(0.1f);
        //yield return new WaitUntil(() => nameInput.text != null && Input.GetKey(KeyCode.KeypadEnter));
        
    }


    //=============================================================================
    #region Photon_Callback Functions

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버에 연결 성공");
        PhotonNetwork.JoinRoom("CandyKongRoom"); // 방 들어오면 입장부터 시도하라
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(cause);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("진입 성공! 게임을 시작하지");
        GameStart();
        InstCandy(50);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("CandyKongRoom 진입 실패! 로비로 진입");

        Debug.Log("방 생성");
        PhotonNetwork.CreateRoom("CandyKongRoom", new RoomOptions { MaxPlayers = 20 });


        /*Debug.Log("생성한 방에 입장");
        PhotonNetwork.JoinRoom("CandyKongRoom"); // 방 들어오면 입장부터 시도하라*/

    }
    #endregion
    //=========================================================================

    private void GameStart()
    {
        //UIManager.transform.GetChild(1).gameObject.SetActive(true); // UI Score 띄우기
        //UIManager.transform.GetChild(0).gameObject.SetActive(false); // 연결중 배너 비활성화

        // 플레이어 랜덤 위치에 생성
        Panel.SetActive(false);
        GameObject player = PhotonNetwork.Instantiate("Team7_Player", SetRandomPos(0), Quaternion.identity);
        Camera.main.GetComponent<Team7_FollowCam>().SetCam();
        //player.GetComponent<Team7_Player>().myName.text = inputName;
    }


    public void Team7_ShowDieLog()
    {
        deadLog.gameObject.SetActive(true);
    }

    public void Team7_Restart()
    {
        PhotonNetwork.Instantiate("Team7_Player", SetRandomPos(0), Quaternion.identity);
    }

    public void Team7_OutScene()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    private Vector3 SetRandomPos(int num) // 게임 매니저 내부 함수 중에 랜덤 위치값을 반환하는 경우 사용
    {
        return new Vector3(Random.Range(-45, 45), num, Random.Range(-45, 45));
    }

    private void InstCandy(int Num)
    {
        int RanNum = 0;
        for (int i = 0; i < Num; i++)
        {
            RanNum = Random.Range(1, 4);
            if (RanNum % 3 == 0)
            {
                candy = PhotonNetwork.Instantiate("Candy_Large", SetRandomPos(1), Quaternion.identity);
                candy.transform.SetParent(transform);
            }
            else if (RanNum % 3 == 1)
            {
                candy = PhotonNetwork.Instantiate("Candy_Normal", SetRandomPos(1), Quaternion.identity);
                candy.transform.SetParent(transform);
            }
            else
            {
                candy = PhotonNetwork.Instantiate("Candy_Small", SetRandomPos(1), Quaternion.identity);
                candy.transform.SetParent(transform);
            }
        }
    }

    public void DestroyCandy(GameObject candy)
    {
        photonView.RPC("Remove", RpcTarget.All, candy);
    }

    [PunRPC]
    private void Remove(GameObject Candy)
    {
        Destroy(Candy);
    }



}
