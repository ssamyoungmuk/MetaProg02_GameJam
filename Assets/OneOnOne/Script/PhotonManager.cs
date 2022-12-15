using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace OOO
{
    public class PhotonManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] string ApplicationID = null;
        [SerializeField] List<GameObject> players = new List<GameObject>();

        RoomOptions roomOptions = new RoomOptions();
        byte maxPlayers = byte.Parse(2.ToString()); // 드롭다운에서 값 얻어오기.
        byte maxTime = byte.Parse(60.ToString());

        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties;

        //flag
        private static bool InServer = false;
        private static bool InLobby = false;
        private static bool InRoom = false;

        //cache
        readonly WaitUntil waitServer = new(() => InServer);
        readonly WaitUntil waitLobby = new(() => InLobby);
        readonly WaitUntil waitRoom = new(() => InRoom);

        private void Awake()
        {
            InitSetting();
        }

        private void Start()
        {
            StartCoroutine(nameof(JoinRoomProcess));
        }

        #region Initailizing
        private void InitSetting()
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = ApplicationID;

            //set flag
            InServer = false;
            InLobby = false;
            InRoom = false;
        }
        #endregion

        IEnumerator JoinRoomProcess()
        {
            PhotonNetwork.ConnectUsingSettings();

            yield return waitServer;
            PhotonNetwork.JoinLobby();

            yield return waitLobby;
            PhotonNetwork.JoinOrCreateRoom("main", new RoomOptions { MaxPlayers = 32 }, null);
        }

        #region Callback
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            InServer = true;
            Debug.Log("PhotonManager## Server Connected!");
        }
        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            InLobby = true;
            Debug.Log("PhotonManager## Lobby Joined!");
        }
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            InRoom = true;
            Debug.Log("PhotonManager## Room Joined!");

            int randIndex = Random.Range(0, players.Count);

            PhotonNetwork.Instantiate(players[randIndex].name, this.transform.position, Quaternion.identity);
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        }
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(this.transform.position,this.transform.localScale);   
        }
    }
}
