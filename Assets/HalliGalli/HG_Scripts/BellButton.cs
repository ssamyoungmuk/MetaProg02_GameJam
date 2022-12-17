using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;

namespace HalliGalli
{
    public class BellButton : MonoBehaviourPun
    {
        [SerializeField] ShuffleCard shuffleCard = null;

        Button Bell = null;
        
        // Start is called before the first frame update
        void Start()
        {
            Bell = GetComponent<Button>();
            Bell.onClick.AddListener(delegate { RingBell(); });
        }

        public void BellActive()
        {
            Bell.interactable = true;
        }

        void RingBell()
        {
            photonView.RPC(nameof(RPC_BellDisable), RpcTarget.All);
            shuffleCard.BellJudge();
        }

        [PunRPC]
        void RPC_BellDisable()
        {
            Bell.interactable = false;
        }
    }
}

