using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Team7_CandyAttack : MonoBehaviourPunCallbacks
{
    public PhotonView pv;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Team7_Other"))
        {
            //other.GetComponent<Team7_Player>().Team7_Die();

            pv.RPC("Team7_Die", RpcTarget.OthersBuffered);

            //photonView.RPC("Kill", RpcTarget.All, other.gameObject);
        }
    }




    
}
