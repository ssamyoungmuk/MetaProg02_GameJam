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
    GameObject candy = null;

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
        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "fc387611-95cc-42c2-ae93-6e4d5bc85e09";
        PhotonNetwork.ConnectUsingSettings(); // ���� ������ ������ ���� ���� �õ�
    }
    //=============================================================================
    #region Photon_Callback Functions
    private void Start()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("������ ������ ���� ����");
        PhotonNetwork.JoinRoom("CandyKongRoom"); // �� ������ ������� �õ��϶�
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("���� ����! ������ ��������");
        GameStart();
        InstCandy(150);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("CandyKongRoom ���� ����! �κ�� ����");

        Debug.Log("�� ����");
        PhotonNetwork.CreateRoom("CandyKongRoom", new RoomOptions { MaxPlayers = 20 });


        /*Debug.Log("������ �濡 ����");
        PhotonNetwork.JoinRoom("CandyKongRoom"); // �� ������ ������� �õ��϶�*/

    }
    #endregion
    //=========================================================================

    private void GameStart()
    {
        //UIManager.transform.GetChild(1).gameObject.SetActive(true); // UI Score ����
        //UIManager.transform.GetChild(0).gameObject.SetActive(false); // ������ ��� ��Ȱ��ȭ

        // �÷��̾� ���� ��ġ�� ����
        GameObject player = PhotonNetwork.Instantiate("Team7_Player", SetRandomPos(), Quaternion.identity);
        Camera.main.GetComponent<Team7_FollowCam>().SetCam();
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

    private Vector3 SetRandomPos() // ���� �Ŵ��� ���� �Լ� �߿� ���� ��ġ���� ��ȯ�ϴ� ��� ���
    {
        return new Vector3(Random.Range(-45, 45), 1, Random.Range(-45, 45));
    }

    private void InstCandy(int Num)
    {
        int RanNum = 0;
        for (int i = 0; i < Num; i++)
        {
            RanNum = Random.Range(1, 4);
            if (RanNum % 3 == 0)
            {
                candy = PhotonNetwork.Instantiate("Candy_Large", SetRandomPos(), Quaternion.identity);
            }
            else if (RanNum % 3 == 1)
            {
                candy = PhotonNetwork.Instantiate("Candy_Normal", SetRandomPos(), Quaternion.identity);
            }
            else
            {
                candy = PhotonNetwork.Instantiate("Candy_Small", SetRandomPos(), Quaternion.identity);
            }
        }
    }


}
