using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Team7_ExpBar : MonoBehaviourPun
{
    void Start()
    {
        BarUpdate();
        
    }

    void BarUpdate()
    {
        if (!photonView.IsMine) gameObject.SetActive(false);
    }

}
