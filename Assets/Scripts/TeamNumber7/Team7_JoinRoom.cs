using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Team7_JoinRoom : MonoBehaviourPun
{
    
    // ��ư�� ������ �� �̵�
    public void OnClick_StartGame()
    {
        PhotonNetwork.LoadLevel("TeamNumber7");
    }
    // ���� �������� ��, ������ ���ٸ� ������ Ŭ���̾�Ʈ
    // ���� �������� ��, ������ �ִٸ� �Ϲ�
    
    // ������ Ŭ���̾�Ʈ�� ������ �ٸ� ������ ������Ŭ���̾�Ʈ�� �ȴ�.
}
