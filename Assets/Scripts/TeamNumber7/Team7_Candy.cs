using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Team7_Candy : MonoBehaviourPun
{
    [SerializeField] int expPoint;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Team7_Player>() != null)
        {
            other.gameObject.GetComponent<Team7_Player>().GetExp(expPoint);

            Destroy(gameObject);
            //Team7_GameManager.Inst.DestroyCandy(this.gameObject);
        }
    }
}
