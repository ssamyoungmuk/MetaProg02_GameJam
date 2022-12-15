using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Team7_CandyAttack : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Team7_Me") && other.CompareTag("Team7_Other"))
        {
            Kill(other.gameObject);
        }
    }

    [PunRPC]
    private void Kill(GameObject target)
    {
        target.GetComponent<Team7_Player>().Team7_Die();
    }

    
}
