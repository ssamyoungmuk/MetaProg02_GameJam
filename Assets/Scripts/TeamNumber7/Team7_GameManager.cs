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
        // 플레이어 랜덤 위치에 생성
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

    private Vector3 SetRandomPos() // 게임 매니저 내부 함수 중에 랜덤 위치값을 반환하는 경우 사용
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
