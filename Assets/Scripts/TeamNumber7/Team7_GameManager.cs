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
        PhotonNetwork.ConnectUsingSettings(); // ���� ������ ������ ���� ���� �õ�
    }
    //=============================================================================
    #region Photon_Callback Functions

    public override void OnConnectedToMaster()
    {
        Debug.Log("������ ������ ���� ����");
        PhotonNetwork.JoinRoom("CandyKongRoom"); // �� ������ ������� �õ��϶�
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("���� ����! ������ ��������");
        GameStart();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("CandyKongRoom ���� ����! ���� �� �����");

        PhotonNetwork.CreateRoom("CandyKongRoom", new RoomOptions { MaxPlayers = 20 }); ;
    }
    #endregion
    //=========================================================================

    private void GameStart()
    {
        UIManager.transform.GetChild(1).gameObject.SetActive(true); // UI Score ����
        UIManager.transform.GetChild(0).gameObject.SetActive(false); // ������ ��� ��Ȱ��ȭ

        // �÷��̾� ���� ��ġ�� ����
        GameObject player = PhotonNetwork.Instantiate("Team7_Player", SetRandomPos(), Quaternion.identity);

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
        return new Vector3(Random.Range(0, 20), 1, Random.Range(0, 20));
    }


}