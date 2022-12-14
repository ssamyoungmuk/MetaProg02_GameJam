using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;
using System.Text.RegularExpressions;
using Photon.Realtime;
using System.Xml;
using UnityEngine.UI;
using TMPro;
using System;

namespace MafiaGame
{
    public enum fade
    {
        In,
        Out,
        All,
    }
    public class NetworkMGR : MonoBehaviourPunCallbacks
    {
        [Header("FadeUI")]
        [SerializeField] GameObject nameElseUI;
        [SerializeField] GameObject blackUI;
        [Header("LoginPanel")]
        public GameObject nickNamePanel;
        [Header("LobbyPanel")]
        public GameObject lobbyPanel;
        [Header("playerUI")]
        public TextMeshProUGUI[] playerName;
        public GameObject[] readyButton;

        bool isPlay;

        private int readyCount = 0;
        private int myButtonNum = 0;
        [Header("내상태")]
        public ReadyState myReadyState = ReadyState.None;
        //This is 
        public enum ReadyState
        {
            None,
            Ready,
            UnReady,
        }

        bool isNameElseUIFadeout;
        // Start is called before the first frame update
        private void Awake()
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "1adfdb50-72b8-47e6-af98-608eb7519a4d\n";
            PhotonNetwork.SendRate = 300;
            PhotonNetwork.SerializationRate = 150;
            Application.targetFrameRate = 60;

        }
        void Start()
        {
            GameLogic.Instance.Fade(blackUI, fade.Out);
            PhotonNetwork.ConnectUsingSettings(); //Photon.Pun 내부 클래스
            Debug.Log(PhotonNetwork.NetworkClientState + "*********************");

        }

        public override void OnConnected()
        {
            base.OnConnected();
            
        }
        //입장할 방이 없으면 새로운 방 생성
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("조인 실패");
            //맥스 인원과 방 상태 표현 (시작인지 아닌지)
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 8, IsOpen = true });
        }

        public void OnEndEdit(string instr)
        {
            if (Regex.IsMatch(instr, @"^(?=.*[가-힣])[가-힣]{2,16}$") !=true)
            {
                PhotonNetwork.NickName = instr;
                return;
            }
            Debug.Log("!!!!!");
            PhotonNetwork.NickName = instr; //닉네임 할당
        }

        // 닉네임 밑에 커넥트 버튼 클릭시 
        public void OnClick_Connected()
        {
            if (isNameElseUIFadeout == true) return;
            if (Regex.IsMatch(PhotonNetwork.NickName, @"^(?=.*[가-힣])[가-힣]{2,16}$") != true)
            {
                Debug.Log(PhotonNetwork.NickName);
                GameLogic.Instance.Fade(nameElseUI, fade.All);
                StartCoroutine(Delay());
                return;
             }
            else
            {
                StartCoroutine(Delay());
                Debug.Log("입장");
                PhotonNetwork.JoinRandomRoom();
                GameLogic.Instance.Fade(nickNamePanel, fade.Out);
                lobbyPanel.SetActive(true);
            }
        }
        IEnumerator Delay()
        {
            isNameElseUIFadeout = true;
            yield return new WaitForSeconds(2f);
            isNameElseUIFadeout = false;
        }


        public override void OnJoinedRoom()
        {
            Debug.Log("새로운 플레이어가 참가하셨습니다");

            myReadyState = ReadyState.UnReady;
            SortedPlayer();
        }
        //타인이 들어올때
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("새로운 플레이어가 참가하셨습니다");
            SortedPlayer();
        }

        //플레이어가 나갈때
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if(isPlay) return;
            ClearLobby();
            SortedPlayer();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            Debug.Log("마스터 클라이언트 변경:" + newMasterClient.ToString());
        }
        #region 플레이어 정렬
        public void SortedPlayer()
        {
            gameObject.GetPhotonView().RPC("ZeroCounT", RpcTarget.MasterClient);
            Player[] sortedPlayers = PhotonNetwork.PlayerList;

            for (int i = 0; i < sortedPlayers.Length; i++)
            {
                playerName[i].text = sortedPlayers[i].NickName;
                playerName[i].gameObject.SetActive(true);
                readyButton[i].gameObject.SetActive(true);
                //자신의 버튼만 활성화 하기 
                if (sortedPlayers[i].NickName == PhotonNetwork.NickName)
                {
                    Debug.Log("i : " + i);
                    myButtonNum = i;
                    playerName[i].color = Color.green;
                    readyButton[myButtonNum].GetComponent<Button>().interactable = true; //나만 누르기 위해 활성화

                    //내 상태가 레디면 노란색 -->그런데 이건 서버에서 표현 해줘야 하기 때문에 RPC함수 사용
                    gameObject.GetPhotonView().RPC("ButtonColor", RpcTarget.All, myReadyState, myButtonNum);
                }

                if (readyButton[i].GetComponent<Image>().color == Color.yellow)
                {
                    gameObject.GetPhotonView().RPC("ReadyCounT", RpcTarget.MasterClient);
                }
            }

        }
        #endregion
        //각각의 플레이어 상태에 따른 색 표현 
        [PunRPC]
        public void ButtonColor(ReadyState readyState, int buttonNum)
        {
            if (readyState == ReadyState.Ready)
                readyButton[buttonNum].GetComponent<Image>().color = Color.yellow;
            else
                readyButton[buttonNum].GetComponent<Image>().color = Color.grey;
        }
        Coroutine startC;
        public void LoadScene()
        {
            // 마스터일때만 해당 함수 실행 가능
            if (PhotonNetwork.IsMasterClient)
            {
                if (readyCount == PhotonNetwork.PlayerList.Length && readyCount >3)
                {
                    Debug.Log("시작");
                    //5명 레디 완료시 2초후 게임 실행 코루틴 
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    if (startC != null) StopCoroutine(startC);
                    startC = StartCoroutine(GameStartUI_Delay());
                    
                }
            }
        }
        IEnumerator GameStartUI_Delay()
        {
            yield return new WaitForSeconds(2f);
            if (readyCount == PhotonNetwork.PlayerList.Length && readyCount >3)
            {
                gameObject.GetPhotonView().RPC("GameStartUI", RpcTarget.All);
            }
            else
            {
                Debug.Log("누군가 레디 취소함");
                PhotonNetwork.CurrentRoom.IsOpen = true;
            }
        }
        [PunRPC]
        void GameStartUI()
        {
            for(int i=0;i<readyButton.Length;i++)
            {
                Destroy(readyButton[i]);
            }
            isPlay = true;
            GameLogic.Instance.characterJob =  gameObject.AddComponent<CharacterJob>();
                GameLogic.Instance.GameStart();
        }
        [PunRPC]
        void ReadyCounT()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                readyCount++;
                LoadScene();
                Debug.Log("레디 숫자 : " + readyCount);
            }
        }
        [PunRPC]
        void ZeroCounT()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                readyCount = 0;
                Debug.Log("레디 숫자 : " + readyCount);
            }
        }
        #region 플레이어 자리 초기화
        public void ClearLobby()
        {
            //대기창 초기화 
            for (int i = 0; i < playerName.Length; i++)
            {
                playerName[i].text = " ";
                playerName[i].gameObject.SetActive(false);

                readyButton[i].gameObject.SetActive(false);
                //soulEff[i].SetActive(false);
                readyButton[i].GetComponent<Image>().color = Color.gray;
                readyButton[i].GetComponent<Button>().interactable = false;
            }
        }
        #endregion
        public void ButtonClick()
        {
            gameObject.GetPhotonView().RPC("ZeroCounT", RpcTarget.MasterClient);
            if (myReadyState == ReadyState.Ready)
            {
                myReadyState = ReadyState.UnReady;
                SortedPlayer();
            }
            else
            {
                myReadyState = ReadyState.Ready;
                SortedPlayer();

            }
        }
    }
}
