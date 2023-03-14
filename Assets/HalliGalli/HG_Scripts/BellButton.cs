using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace HalliGalli
{
    public class BellButton : MonoBehaviourPun
    {
        [SerializeField] Button Bell = null;
        
        // Start is called before the first frame update
        void Awake()
        {
            Bell.onClick.AddListener(delegate { RingBell(); });
        }

        public void BellActive()
        {
            Bell.interactable = true;
        }

        void RingBell()
        {
            photonView.RPC(nameof(RPC_BellDisable), RpcTarget.All);
        }

        [PunRPC]
        void RPC_BellDisable()
        {
            Bell.interactable = false;
        }
    }
}

