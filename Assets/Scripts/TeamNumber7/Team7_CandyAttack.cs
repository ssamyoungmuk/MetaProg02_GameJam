using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Team7_CandyAttack : MonoBehaviourPunCallbacks
{
    public PhotonView pv;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Team7_Other") && photonView.IsMine)
        {
            Debug.Log("충돌 인식. 대상 함수 호출");
            other.GetComponent<Team7_Player>().Team7_Die();

            //pv.RPC("Team7_Die", RpcTarget.All);

            //photonView.RPC("Kill", RpcTarget.All, other.gameObject);
        }
    }




    
}
