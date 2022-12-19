using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Runtime.InteropServices;

namespace MafiaGame
{
    public enum jobList
    {
        Mafia,
        Doctor,
        Police,
        People,

    }
    public class CharacterJob : MonoBehaviourPunCallbacks
    {
        // ¸¶ÇÇ¾Æ 0, °æÂû 1, ÀÇ»ç 2, ½Ã¹Î 3
        List<int> job = new List<int>(0);
        int saveNum = 0;
        public int peopleNum;
        public int mafiaNum;
        private void Awake()
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
                {
                    GameObject _player = PhotonNetwork.Instantiate("MafiaInfo", new Vector3(0, 0, 0), Quaternion.identity);
                    _player.GetPhotonView().RPC("PlayerNum", RpcTarget.All, i);
                    GameLogic.Instance.MyInfo(_player);
                }
            }
            if(PhotonNetwork.PlayerList.Length == 4 || PhotonNetwork.PlayerList.Length == 5)
            {
                mafiaNum = 1;
            }
            else mafiaNum = 2;
            peopleNum = PhotonNetwork.PlayerList.Length - mafiaNum;

        }

        public void JobSeting(int _playerNum, int _min, int _max)
        {
            if (job.Count >= _playerNum) return;
            Debug.Log("¸®ÅÏ¾ÈµÊ");
            for (int i = 0; i < _playerNum; i++)
            {
                Debug.Log("????" + i);
                saveNum = Random.Range(_min, _max);
                if (job.Contains(saveNum))
                {
                    JobSeting(_playerNum, _min, _max);
                    break;
                }
                else
                {
                    job.Add(saveNum);
                    Debug.Log(saveNum);
                }
            }
        }
        int count;
        [PunRPC]
        void CreateMafiaInfo()
        {
            jobList jb = jobList.People;
            count++;
            int playerNum = PhotonNetwork.PlayerList.Length;
            if (count < playerNum) return;
            JobSeting(playerNum, 0, playerNum);
            if (PhotonNetwork.IsMasterClient == true)
            {
                PlayerInfo[] playerList = FindObjectsOfType<PlayerInfo>();
                for (int i = 0; i < playerList.Length; i++)
                {
                    switch (job[i])
                    {
                        case 0:
                            jb = jobList.Police;
                            break;
                        case 1:
                            jb = jobList.Doctor;
                            break;
                        case 2:
                            jb = jobList.Mafia;
                            break;
                        case 3:
                            if (PhotonNetwork.PlayerList.Length > 5) jb = jobList.Mafia;
                            else jb = jobList.People;
                            break;
                        case 4:
                            jb = jobList.People;
                            break;
                        case 5:
                            jb = jobList.People;
                            break;
                        case 6:
                            jb = jobList.People;
                            break;
                        case 7:
                            jb = jobList.People;
                            break;
                    }

                    playerList[i].gameObject.GetPhotonView().RPC("Player_JobSeting", RpcTarget.All, jb);
                    gameObject.GetPhotonView().RPC("PlayerJobList", RpcTarget.All, jb, playerList[i].gameObject.GetPhotonView().ViewID);
                }
            }
        }
    }
}
