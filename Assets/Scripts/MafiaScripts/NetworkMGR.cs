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
        public GameObject[] voteButton;


        private int readyCount = 0;
        private int myButtonNum = 0;
        [Header("³»»óÅÂ")]
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
        void Start()
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "1adfdb50-72b8-47e6-af98-608eb7519a4d\n";
            GameLogic.Instance.Fade(blackUI, fade.Out);
            PhotonNetwork.ConnectUsingSettings(); //Photon.Pun ³»ºÎ Å¬·¡½º
            Debug.Log(PhotonNetwork.NetworkClientState + "*********************");

        }

        public override void OnConnected()
        {
            base.OnConnected();
            
        }
        //ÀÔÀåÇÒ ¹æÀÌ ¾øÀ¸¸é »õ·Î¿î ¹æ »ý¼º
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Á¶ÀÎ ½ÇÆÐ");
            //¸Æ½º ÀÎ¿ø°ú ¹æ »óÅÂ Ç¥Çö (½ÃÀÛÀÎÁö ¾Æ´ÑÁö)
            PhotonNetwork.CreateRoom("MafiaGame", new RoomOptions { MaxPlayers = 8, IsOpen = true });
        }

        public void OnEndEdit(string instr)
        {
            if (Regex.IsMatch(instr, @"^(?=.*[a-z0-9°¡-ÆR])[a-z0-9°¡-ÆR]{2,16}$") !=true)
            {
                PhotonNetwork.NickName = instr;
                return;
            }
            Debug.Log("!!!!!");
            PhotonNetwork.NickName = instr; //´Ð³×ÀÓ ÇÒ´ç
        }

        // ´Ð³×ÀÓ ¹Ø¿¡ Ä¿³ØÆ® ¹öÆ° Å¬¸¯½Ã 
        public void OnClick_Connected()
        {
            if (isNameElseUIFadeout == true) return;
            if (Regex.IsMatch(PhotonNetwork.NickName, @"^(?=.*[a-z0-9°¡-ÆR])[a-z0-9°¡-ÆR]{2,16}$") != true)
            {
                Debug.Log(PhotonNetwork.NickName);
                GameLogic.Instance.Fade(nameElseUI, fade.All);
                return;
             }
            else
            {

                Debug.Log("ÀÔÀå");
                PhotonNetwork.JoinRandomRoom();
                GameLogic.Instance.Fade(nickNamePanel, fade.Out);
                lobbyPanel.SetActive(true);
            }
        }


        public override void OnJoinedRoom()
        {
            Debug.Log("»õ·Î¿î ÇÃ·¹ÀÌ¾î°¡ Âü°¡ÇÏ¼Ì½À´Ï´Ù");

            myReadyState = ReadyState.UnReady;
            SortedPlayer();
        }
        //Å¸ÀÎÀÌ µé¾î¿Ã¶§
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("»õ·Î¿î ÇÃ·¹ÀÌ¾î°¡ Âü°¡ÇÏ¼Ì½À´Ï´Ù");
            SortedPlayer();
        }

        //ÇÃ·¹ÀÌ¾î°¡ ³ª°¥¶§
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            ClearLobby();
            SortedPlayer();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            Debug.Log("¸¶½ºÅÍ Å¬¶óÀÌ¾ðÆ® º¯°æ:" + newMasterClient.ToString());
        }
        #region ÇÃ·¹ÀÌ¾î Á¤·Ä
        public void SortedPlayer()
        {
            gameObject.GetPhotonView().RPC("ZeroCounT", RpcTarget.MasterClient);
            Player[] sortedPlayers = PhotonNetwork.PlayerList;

            for (int i = 0; i < sortedPlayers.Length; i++)
            {
                playerName[i].text = sortedPlayers[i].NickName;
                playerName[i].gameObject.SetActive(true);
                readyButton[i].gameObject.SetActive(true);
                //ÀÚ½ÅÀÇ ¹öÆ°¸¸ È°¼ºÈ­ ÇÏ±â 
                if (sortedPlayers[i].NickName == PhotonNetwork.NickName)
                {
                    Debug.Log("i : " + i);
                    myButtonNum = i;
                    readyButton[myButtonNum].GetComponent<Button>().interactable = true; //³ª¸¸ ´©¸£±â À§ÇØ È°¼ºÈ­

                    //³» »óÅÂ°¡ ·¹µð¸é ³ë¶õ»ö -->±×·±µ¥ ÀÌ°Ç ¼­¹ö¿¡¼­ Ç¥Çö ÇØÁà¾ß ÇÏ±â ¶§¹®¿¡ RPCÇÔ¼ö »ç¿ë
                    gameObject.GetPhotonView().RPC("ButtonColor", RpcTarget.All, myReadyState, myButtonNum);
                }

                if (readyButton[i].GetComponent<Image>().color == Color.yellow)
                {
                    gameObject.GetPhotonView().RPC("ReadyCounT", RpcTarget.MasterClient);
                }
            }

        }
        #endregion
        //°¢°¢ÀÇ ÇÃ·¹ÀÌ¾î »óÅÂ¿¡ µû¸¥ »ö Ç¥Çö 
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
            // ¸¶½ºÅÍÀÏ¶§¸¸ ÇØ´ç ÇÔ¼ö ½ÇÇà °¡´É
            if (PhotonNetwork.IsMasterClient)
            {
                if (readyCount == PhotonNetwork.PlayerList.Length && readyCount > 3)
                {
                    Debug.Log("½ÃÀÛ");
                    //5¸í ·¹µð ¿Ï·á½Ã 2ÃÊÈÄ °ÔÀÓ ½ÇÇà ÄÚ·çÆ¾ 
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    if (startC != null) StopCoroutine(startC);
                    startC = StartCoroutine(GameStartUI_Delay());
                    
                }
            }
        }
        IEnumerator GameStartUI_Delay()
        {
            yield return new WaitForSeconds(2f);
            if (readyCount == PhotonNetwork.PlayerList.Length && readyCount > 3)
            {
                gameObject.GetPhotonView().RPC("GameStartUI", RpcTarget.All);
            }
            else
            {
                Debug.Log("´©±º°¡ ·¹µð Ãë¼ÒÇÔ");
                PhotonNetwork.CurrentRoom.IsOpen = true;
            }
        }
        [PunRPC]
        void GameStartUI()
        {
            for(int i=0;i<readyButton.Length;i++)
            {
                readyButton[i].SetActive(false);
            }
            for(int i=0;i<PhotonNetwork.PlayerList.Length;i++)
                voteButton[i].SetActive(true);
            gameObject.AddComponent<CharacterJob>();
                GameLogic.Instance.GameStart();
        }
        [PunRPC]
        void ReadyCounT()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                readyCount++;
                LoadScene();
                Debug.Log("·¹µð ¼ýÀÚ : " + readyCount);
            }
        }
        [PunRPC]
        void ZeroCounT()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                readyCount = 0;
                Debug.Log("·¹µð ¼ýÀÚ : " + readyCount);
            }
        }
        #region ÇÃ·¹ÀÌ¾î ÀÚ¸® ÃÊ±âÈ­
        public void ClearLobby()
        {
            //´ë±âÃ¢ ÃÊ±âÈ­ 
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
