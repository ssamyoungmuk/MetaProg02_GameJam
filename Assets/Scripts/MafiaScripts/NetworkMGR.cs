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
        [Header("������")]
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
            PhotonNetwork.ConnectUsingSettings(); //Photon.Pun ���� Ŭ����
            Debug.Log(PhotonNetwork.NetworkClientState + "*********************");

        }

        public override void OnConnected()
        {
            base.OnConnected();
            
        }
        //������ ���� ������ ���ο� �� ����
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("���� ����");
            //�ƽ� �ο��� �� ���� ǥ�� (�������� �ƴ���)
            PhotonNetwork.CreateRoom("MafiaGame", new RoomOptions { MaxPlayers = 8, IsOpen = true });
        }

        public void OnEndEdit(string instr)
        {
            if (Regex.IsMatch(instr, @"^(?=.*[a-z0-9��-�R])[a-z0-9��-�R]{2,16}$") !=true)
            {
                PhotonNetwork.NickName = instr;
                return;
            }
            Debug.Log("!!!!!");
            PhotonNetwork.NickName = instr; //�г��� �Ҵ�
        }

        // �г��� �ؿ� Ŀ��Ʈ ��ư Ŭ���� 
        public void OnClick_Connected()
        {
            if (isNameElseUIFadeout == true) return;
            if (Regex.IsMatch(PhotonNetwork.NickName, @"^(?=.*[a-z0-9��-�R])[a-z0-9��-�R]{2,16}$") != true)
            {
                Debug.Log(PhotonNetwork.NickName);
                GameLogic.Instance.Fade(nameElseUI, fade.All);
                return;
             }
            else
            {

                Debug.Log("����");
                PhotonNetwork.JoinRandomRoom();
                GameLogic.Instance.Fade(nickNamePanel, fade.Out);
                lobbyPanel.SetActive(true);
            }
        }


        public override void OnJoinedRoom()
        {
            Debug.Log("���ο� �÷��̾ �����ϼ̽��ϴ�");

            myReadyState = ReadyState.UnReady;
            SortedPlayer();
        }
        //Ÿ���� ���ö�
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("���ο� �÷��̾ �����ϼ̽��ϴ�");
            SortedPlayer();
        }

        //�÷��̾ ������
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            ClearLobby();
            SortedPlayer();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            Debug.Log("������ Ŭ���̾�Ʈ ����:" + newMasterClient.ToString());
        }
        #region �÷��̾� ����
        public void SortedPlayer()
        {
            gameObject.GetPhotonView().RPC("ZeroCounT", RpcTarget.MasterClient);
            Player[] sortedPlayers = PhotonNetwork.PlayerList;

            for (int i = 0; i < sortedPlayers.Length; i++)
            {
                playerName[i].text = sortedPlayers[i].NickName;
                playerName[i].gameObject.SetActive(true);
                readyButton[i].gameObject.SetActive(true);
                //�ڽ��� ��ư�� Ȱ��ȭ �ϱ� 
                if (sortedPlayers[i].NickName == PhotonNetwork.NickName)
                {
                    Debug.Log("i : " + i);
                    myButtonNum = i;
                    readyButton[myButtonNum].GetComponent<Button>().interactable = true; //���� ������ ���� Ȱ��ȭ

                    //�� ���°� ����� ����� -->�׷��� �̰� �������� ǥ�� ����� �ϱ� ������ RPC�Լ� ���
                    gameObject.GetPhotonView().RPC("ButtonColor", RpcTarget.All, myReadyState, myButtonNum);
                }

                if (readyButton[i].GetComponent<Image>().color == Color.yellow)
                {
                    gameObject.GetPhotonView().RPC("ReadyCounT", RpcTarget.MasterClient);
                }
            }

        }
        #endregion
        //������ �÷��̾� ���¿� ���� �� ǥ�� 
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
            // �������϶��� �ش� �Լ� ���� ����
            if (PhotonNetwork.IsMasterClient)
            {
                if (readyCount == PhotonNetwork.PlayerList.Length && readyCount > 3)
                {
                    Debug.Log("����");
                    //5�� ���� �Ϸ�� 2���� ���� ���� �ڷ�ƾ 
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
                Debug.Log("������ ���� �����");
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
                Debug.Log("���� ���� : " + readyCount);
            }
        }
        [PunRPC]
        void ZeroCounT()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                readyCount = 0;
                Debug.Log("���� ���� : " + readyCount);
            }
        }
        #region �÷��̾� �ڸ� �ʱ�ȭ
        public void ClearLobby()
        {
            //���â �ʱ�ȭ 
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
