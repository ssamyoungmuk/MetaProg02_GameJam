using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Team7_CandyAttack : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Team7_Other"))
        {
            other.GetComponent<Team7_Player>().Team7_Die();

            //photonView.RPC("Kill", RpcTarget.All, other.gameObject);
        }
    }




    
}
