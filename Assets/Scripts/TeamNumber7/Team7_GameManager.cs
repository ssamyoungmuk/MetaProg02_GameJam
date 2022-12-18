using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Team7_GameManager : MonoBehaviourPunCallbacks
{
    Image deadLog;
    [SerializeField] GameObject dieLog;
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

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(cause);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("���� ����! ������ ��������");
        GameStart();
        InstCandy(50);
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
        GameObject player = PhotonNetwork.Instantiate("Team7_Player", SetRandomPos(0), Quaternion.identity);
        Camera.main.GetComponent<Team7_FollowCam>().SetCam();
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

    private Vector3 SetRandomPos(int num) // ���� �Ŵ��� ���� �Լ� �߿� ���� ��ġ���� ��ȯ�ϴ� ��� ���
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

    public void DieLog()
    {
        dieLog.gameObject.SetActive(true);
        StartCoroutine(CO_NowWhat());
        //Debug.Log(dieLog.gameObject);
    }

    IEnumerator CO_NowWhat()
    {
        while(true)
        {
            if(Input.GetKey(KeyCode.R))
            {
                GameStart();
            }

            else if (Input.GetKey(KeyCode.Q))
            {
                PhotonNetwork.Disconnect();
                Cursor.lockState = CursorLockMode.None; // ���콺 ���
                SceneManager.LoadScene("LobbyScene");
            }

            yield return null;
        }
    }

}
