using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace MafiaGamne
{
    public class CharacterJob : MonoBehaviourPunCallbacks
    {
        bool police = false;
        bool doctor = false;

        List<GameObject> player = new List<GameObject>();

        // ¸¶ÇÇ¾Æ 0, °æÂû 1, ÀÇ»ç 2, ½Ã¹Î 3
        List<int> job = new List<int>(0);
        int saveNum = 0;

        private void Awake()
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
                {
                    GameObject _player = PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);
                    _player.AddComponent<PlayerInfo>();
                    _player.GetPhotonView().RPC("PlayerNum", RpcTarget.All, i);
                    player.Add(_player);
                }
            }
        }
        
        public void JobSeting(int _playerNum, int _min, int _max)
        {
            if (job.Count >= _playerNum) return;

            for (int i = 0; i < _playerNum; i++)
            {
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

        private void Start()
        {
            int playerNum = PhotonNetwork.PlayerList.Length;
            if (PhotonNetwork.IsMasterClient)
            {
                JobSeting(playerNum, 0, playerNum);
                for (int i = 0; i < playerNum; i++)
                {
                    player[i].GetComponent<PlayerInfo>().Player_JobSeting(job[i]);
                }
            }
        }
    } 
}
