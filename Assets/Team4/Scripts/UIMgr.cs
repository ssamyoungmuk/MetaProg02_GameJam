using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
namespace PicoPark
{
    public class UIMgr : MonoBehaviourPun
    {
        [SerializeField] TextMeshProUGUI[] peopleCount;
        [SerializeField] float peopleNum;

        [PunRPC]
        public void CountPeopleNum()
        {
            peopleNum += 1;
            peopleCount[0].text = "NowNumber : " + peopleNum + " / " + PhotonNetwork.PlayerList.Length;
            if (peopleNum >= PhotonNetwork.PlayerList.Length) Win(); 
        }
        [PunRPC]
        public void MinusPeopleNum() { peopleNum -= 1; peopleCount[0].text = "NowNumber : " + peopleNum + " / " +PhotonNetwork.PlayerList.Length; }
        public void Win()
        {
            GameMgr.Instance.EndGame();
            peopleCount[0].text = "Win!! ByeBye";
        }
    }
}
