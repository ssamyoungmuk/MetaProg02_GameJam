using Photon.Pun;
using TMPro;
using UnityEngine;

namespace HalliGalli
{
    public class PlayerNickName : MonoBehaviourPun
    {
        [SerializeField] TextMeshProUGUI myNickName = null;

        GameObject joinroomPanel = null;

        private void Awake()
        {
            joinroomPanel = GameObject.Find("JoinRoomPanel");
            if (photonView.IsMine)
            {
                photonView.RPC(nameof(RPC_SetParent), RpcTarget.AllBuffered, HalliGalliMgr.Inst.MyNumber);
                photonView.RPC(nameof(RPC_SettingNickName), RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
                HalliGalliMgr.Inst.editMyNumber = SetMyNumber;
            }
        }

        public void SetMyNumber(byte myNumber)
        {
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


