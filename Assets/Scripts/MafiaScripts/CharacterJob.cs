using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Runtime.InteropServices;

namespace MafiaGame
{
    public class CharacterJob : MonoBehaviourPunCallbacks
    {
        List<GameObject> player = new List<GameObject>();

        // 마피아 0, 경찰 1, 의사 2, 시민 3
        List<int> job = new List<int>(0);
        int saveNum = 0;

        private void Awake()
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
                {
                    GameObject _player = PhotonNetwork.Instantiate("MafiaInfo", new Vector3(0, 0, 0), Quaternion.identity);
                    _player.GetPhotonView().RPC("PlayerNum", RpcTarget.All, i);
                }
            }

        }

        public void JobSeting(int _playerNum, int _min, int _max)
        {
            if (job.Count >= _playerNum) return;
            Debug.Log("리턴안됨");
            for (int i = 0; i < _playerNum; i++)
            {
                Debug.Log("????"+i);
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
            count++;
            int playerNum = PhotonNetwork.PlayerList.Length;
            if (count < playerNum) return;
                JobSeting(playerNum, 0, playerNum);
            if (PhotonNetwork.IsMasterClient==true)
            {
                PlayerInfo[] playerList = FindObjectsOfType<PlayerInfo>();
                for (int i = 0; i < playerList.Length; i++)
                {
                    Debug.Log("리스트"+i);
                    playerList[i].gameObject.GetPhotonView().RPC("Player_JobSeting",RpcTarget.All,job[i]);
                }
            }
        }
    } 
}
