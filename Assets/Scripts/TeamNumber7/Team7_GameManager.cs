using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Team7_GameManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        // �÷��̾� ���� ��ġ�� ����
        GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(0, 0), 1, Random.Range(0, 0)), Quaternion.identity);
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
