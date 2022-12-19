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
        PhotonNetwork.ConnectUsingSettings(); // ���� ������ ������ ���� ���� �õ�

    }

    private void InputName()
    {
        StartCoroutine(InputNameTime());
    }

    IEnumerator InputNameTime()
    {
        Debug.Log("�ڷ�ƾ ����");
        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            if (nameInput.text == null)
            {
                Debug.Log("���̵� �Է��ϼ���");
            }

            else
            {
                inputName = nameInput.text;
                saveName = inputName;
                Debug.Log("�Է� �Ϸ�");

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



}
