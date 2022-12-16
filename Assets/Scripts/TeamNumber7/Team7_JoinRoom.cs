using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Team7_JoinRoom : MonoBehaviourPun
{
    
    // 버튼을 누르면 씬 이동
    public void OnClick_StartGame()
    {
        PhotonNetwork.LoadLevel("TeamNumber7");
    }
    // 씬에 입장했을 때, 유저가 없다면 마스터 클라이언트
    // 씬에 입장했을 때, 유저가 있다면 일반
    
    // 마스터 클라이언트가 죽으면 다른 유저가 마스터클라이언트가 된다.
}
