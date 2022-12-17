using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;

namespace HalliGalli
{
    public class PlayerNickName : MonoBehaviourPun
    {
        [SerializeField] TextMeshProUGUI myNickName = null;

        GameObject joinroomPanel = null;
        Transform parent = null;
        private void Awake()
        {
            joinroomPanel = GameObject.FindGameObjectWithTag("joinroomPanel");
            if (photonView.IsMine)
                photonView.RPC(nameof(RPC_SetParent), RpcTarget.AllBuffered, HalliGalliMgr.Inst.MyNumber);
        }

        // Start is called before the first frame update
        void Start()
        {
            if (photonView.IsMine)
                photonView.RPC(nameof(RPC_SettingNickName), RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
        }

        public void SetMyNumber(byte myNumber)
        {
            if (photonView.IsMine)
                photonView.RPC(nameof(RPC_SetParent), RpcTarget.AllBuffered, myNumber);
        }
        [PunRPC]
        void RPC_SetParent(byte parentindex)
        {
            parent = joinroomPanel.transform.GetChild(parentindex);
            this.transform.SetParent(parent, false);
        }

        [PunRPC]
        void RPC_SettingNickName(string Name)
        {
            myNickName.text = Name;
        }
    }
}


