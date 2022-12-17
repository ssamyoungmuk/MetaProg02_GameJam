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

        private void Awake()
        {
            joinroomPanel = GameObject.FindGameObjectWithTag("joinroomPanel");
            if (photonView.IsMine)
            {
                photonView.RPC(nameof(RPC_SetParent), RpcTarget.AllBuffered, HalliGalliMgr.Inst.MyNumber);
                photonView.RPC(nameof(RPC_SettingNickName), RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
            }
        }

        public void SetMyNumber(byte myNumber)
        {
            if (photonView.IsMine)
                photonView.RPC(nameof(RPC_SetParent), RpcTarget.AllBuffered, myNumber);
        }

        [PunRPC]
        void RPC_SetParent(byte parentindex)
        {
            Transform parent = joinroomPanel.transform.GetChild(parentindex);
            this.transform.SetParent(parent, false);
        }

        [PunRPC]
        void RPC_SettingNickName(string Name)
        {
            myNickName.text = Name;
        }
    }
}


