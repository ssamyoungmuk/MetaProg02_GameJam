using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

namespace PicoPark
{
    public class LodingManager : MonoBehaviourPunCallbacks
    {
        public RawImage [] players;
        [SerializeField] int myNum =-1;
        [SerializeField] int playerNum = 0;
        [SerializeField] int readyNum =0;
        private void Start()
        {
            OnJoinedRoom();
            PhotonNetwork.AutomaticallySyncScene=true;
        }

        public override void OnJoinedRoom()
        {
            myNum = PhotonNetwork.PlayerList.Length-1;
        }
        public void OnClick_ReadyButton()
        {
            gameObject.GetPhotonView().RPC("ImageOn", RpcTarget.All);
        }

        [PunRPC]
        public void ImageOn()
        {
            players[myNum].gameObject.SetActive(true);
            readyNum++;
            if (PhotonNetwork.IsMasterClient)
            {
                if (readyNum == PhotonNetwork.PlayerList.Length) PhotonNetwork.LoadLevel("PicoParkInGame");
            }
        }

    }
}
