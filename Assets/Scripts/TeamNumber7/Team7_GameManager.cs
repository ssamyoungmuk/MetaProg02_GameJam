using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class Team7_GameManager : MonoBehaviourPunCallbacks
{
    Image deadLog;

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
        // �÷��̾� ���� ��ġ�� ����
        GameObject player = PhotonNetwork.Instantiate("Team7_Player", SetRandomPos(), Quaternion.identity);
        //Player[] userInfos = PhotonNetwork.
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
        return new Vector3(Random.Range(0, 0), 1, Random.Range(0, 0));
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
