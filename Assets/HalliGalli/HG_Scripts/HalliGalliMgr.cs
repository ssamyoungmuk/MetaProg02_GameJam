using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

namespace HalliGalli
{
    public delegate void EditMyNumber(byte myNumber);
    public class HalliGalliMgr : MonoBehaviourPunCallbacks
    {
        #region 싱글턴
        private static HalliGalliMgr instance;
        public static HalliGalliMgr Inst
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<HalliGalliMgr>();
                    if (instance == null)
                    {
                        instance = new GameObject(nameof(HalliGalliMgr), typeof(HalliGalliMgr)).GetComponent<HalliGalliMgr>();
                    }
                }
                return instance;
            }
        }

        #endregion
        [Tooltip("if u want setting another app string id put is this line" +
            "may u stay this line null could be app id set default : 4f95d2c7-69cd-48bb-b609-1e239bed8c50")]
        [SerializeField] private string CustomAppId = null;

        public EditMyNumber editMyNumber = null;

        [Header("StartPanel")]
        [SerializeField] GameObject startPanel = null;
        [SerializeField] GameObject joinRoomPanel = null;
        [SerializeField] GameObject inGamePanel = null;

        [SerializeField] TMP_InputField nickName = null;
        [SerializeField] Button joinRoomButton = null;

        [Header("JoinRoomPanel")]
        [SerializeField] Button gamestartButton = null;

        [Header("InGamePanel")]
        [SerializeField] ShuffleCard shuffleCard = null;

        public byte MyNumber { get; set; } = 0;

        private void Awake()
        {
            // Set Screen Size 16 : 9 fullscreen false
            Screen.SetResolution(1920, 1080, true);
            // Improves the performance of the Photon network
            // Defines how many times per second PhotonNetwork should send packages
            PhotonNetwork.SendRate = 60;
            // PhotonView 들이 OnPhotonSerialize를 초당 몇회 호출할지
            // How many times per second PhotonViews call OnPhotonSerialize
            PhotonNetwork.SerializationRate = 30;
            // Automatically synchronized scenes from other clients when switching scenes from the master server.
            // 마스터 서버에서 장면을 전환할 때 다른 클라이언트의 장면을 자동으로 동기화합니다.
            PhotonNetwork.AutomaticallySyncScene = true;
            // Don't get a photonmessage when access the room.
            PhotonNetwork.IsMessageQueueRunning = false;

            if (CustomAppId != null)
            {
                PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = CustomAppId;
            }
            else
            {
                //default server app id for halligalli
                PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "4f95d2c7-69cd-48bb-b609-1e239bed8c50";
            }

            startPanel.SetActive(true);
            joinRoomPanel.SetActive(false);
            inGamePanel.SetActive(false);
        }

        void Start()
        {
            joinRoomButton.interactable = false;
            PhotonNetwork.ConnectUsingSettings(); // 접속 시도
            nickName.onValueChanged.AddListener(delegate { SettingjoinRoomButton(); });
            joinRoomButton.onClick.AddListener(OnClick_JoinRoomButton);
            gamestartButton.onClick.AddListener(OnClick_startbutton);
        }
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
        }
        void SettingjoinRoomButton() //닉네임->joinroombutton 활성화
        {
            if (nickName.text.Length > 3 && nickName.text.Length < 8)
            {
                joinRoomButton.interactable = true;
            }
            else
            {
                joinRoomButton.interactable = false;
            }
        }
        void OnClick_JoinRoomButton()
        {
            PhotonNetwork.NickName = nickName.text;

            if (PhotonNetwork.IsConnectedAndReady == false)
            {
                PhotonNetwork.Reconnect();
            }
            else
            {
                PhotonNetwork.JoinRandomRoom();
            }
        } //joinroom요청

        // (빈 방이 없으면) 랜덤 룸 참가에 실패한 경우 자동 실행 
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 }, null);
        }

        public override void OnDisconnected(DisconnectCause cause)  // Call when the master server is not connected
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        /// <summary>
        /// back to main scene
        /// </summary>
        public void OnClick_ExitButton() // 실행 종료
        {
            //Application.Quit(); 교수님 씬으로 돌아가게해
        }

        #region PlayerMatching

        // 룸에 참가 완료된 경우 자동 실행
        public override void OnJoinedRoom()
        {
            startPanel.SetActive(false);
            joinRoomPanel.SetActive(true);

            MyNumber = PhotonNetwork.CurrentRoom.PlayerCount;

            // 마스터가 아니면 게임 시작 버튼 비활성화
            if (PhotonNetwork.IsMasterClient) gamestartButton.interactable = true;
            else gamestartButton.interactable = false;

            GameObject player = PhotonNetwork.Instantiate("HG_Player_Pos", Vector3.zero, Quaternion.identity);
            editMyNumber += player.GetComponent<PlayerNickName>().SetMyNumber;

            #region 눈물의 과정 못지워요
            //playerInfo = PhotonNetwork.Instantiate("Player_Pos", Vector3.zero, Quaternion.identity);
            //playerInfo.transform.SetParent(joinRoomPanel.transform.GetChild(0).transform, false);

            //Debug.Log("## 붙었니 ?? playerInfo : " + playerInfo);

            //roomPlayerInfo.Add(playerInfo);

            //TextMeshProUGUI nickNameMaster = null;
            //nickNameMaster = playerInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            //nickNameMaster.text = PhotonNetwork.NickName;


            //PlayerCount = PhotonNetwork.CountOfPlayersInRooms;
            //Debug.Log("## PhotonNetwork.CountOfPlayersInRooms" + PlayerCount);

            // 룸 안에 있는 플레이어 수 체크
            //checkcount = PhotonNetwork.LocalPlayer.ActorNumber;
            //Debug.Log("## PhotonNetwork.LocalPlayer.ActorNumber" + checkcount);

            //bool isJoin = false;

            //for (int i = 1; i <= checkcount; i++)
            //{
            //    //if (PhotonNetwork.IsMasterClient == false && photonView.IsMine == true)
            //    //{
            //    #region
            //    //playerInfo = Instantiate(lobbyPlayerInfo, roomPlayerinfoPos[i].transform);
            //    //playerInfo = PhotonNetwork.Instantiate("Player_Pos", roomPlayerinfoPos[i].transform.position, Quaternion.identity);

            //    //ObjroomPlayerinfoPos[i] = joinRoomPanel.transform.GetChild(i);

            //    //joinRoomPanel.transform.GetChild(i);
            //    //ObjroomPlayerinfoPos[0].transform.SetParent(joinRoomPanel.transform.GetChild(i), false);
            //    //playerInfo = PhotonNetwork.Instantiate("Player_Pos", ObjroomPlayerinfoPos[i].transform.position, Quaternion.identity);
            //    #endregion

            //    GameObject pos = joinRoomPanel;
            //    pos.transform.GetChild(i);
            //    Debug.Log("## pos.transform.GetChild: " + pos.transform.GetChild(i));

            //    //GameObject player = PhotonNetwork.Instantiate("Player_Pos", pos.transform.position, Quaternion.identity);

            //    playerInfo = PhotonNetwork.Instantiate("Player_Pos", pos.transform.position, Quaternion.identity);
            //    playerInfo.transform.SetParent(pos.transform.GetChild(i).transform, false);

            //    Debug.Log("## 붙었니 ?? playerInfo : " + playerInfo);

            //    roomPlayerInfo.Add(playerInfo);

            //    TextMeshProUGUI nickNames = null;
            //    nickNames = playerInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            //    nickNames.text = PhotonNetwork.NickName;
            //    //}
            //}
            //Debug.Log("## 방에 잘 들어감??");
            #endregion
        }

        void OnClick_startbutton() //마스터만 컨트롤
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount < 2) return;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            photonView.RPC(nameof(RPC_OnClick_gameStartButton), RpcTarget.All);
        }

        [PunRPC]
        public void RPC_OnClick_gameStartButton()
        {
            InGame();
        }
        private void InGame()
        {
            joinRoomPanel.SetActive(false);
            inGamePanel.SetActive(true);
            shuffleCard.GameStart();
        }

        public void Onclick_BackButton() // joinRoom Panel 에서의 뒤로 가기 버튼
        {
            PhotonNetwork.LeaveRoom();
        }
        #endregion

        public void GameOver()
        {
            PhotonNetwork.LeaveRoom();
        }
        public override void OnLeftRoom()
        {
            startPanel.SetActive(true);
            joinRoomPanel.SetActive(false);
            inGamePanel.SetActive(false);
        }
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.IsMasterClient) gamestartButton.interactable = true;
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (otherPlayer.ActorNumber < PhotonNetwork.LocalPlayer.ActorNumber)
            {
                MyNumber--;
                if (editMyNumber != null)
                    editMyNumber(MyNumber);
            }
        }

    }
}

