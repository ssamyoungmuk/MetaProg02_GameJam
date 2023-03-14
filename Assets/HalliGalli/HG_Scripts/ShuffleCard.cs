using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

namespace HalliGalli
{
    public class ShuffleCard : MonoBehaviourPunCallbacks
    {
        [SerializeField] BellButton bell = null;
        [SerializeField] TextMeshProUGUI gameStart = null;
        [SerializeField] TextMeshProUGUI Timer = null;

        PlayerCard[] players = null;
        public byte AliveCount { get; set; } = 0;

        List<byte> CardList = null;
        List<Color> ColorList = null;
        Color newColor1 = Color.red;
        Color newColor2 = Color.yellow;
        Color newColor3 = Color.green;
        Color newColor4 = Color.blue;

        float time = 0;
        bool flip = false;
        bool ring = false;

        WaitForSeconds oneS = new WaitForSeconds(1f);
        WaitForSeconds halfS = new WaitForSeconds(0.5f);
        WaitUntil RingBell = null;

        void Awake()
        {
            AliveCount = PhotonNetwork.CurrentRoom.PlayerCount;
            RingBell = new WaitUntil(() => ring == false);

            if (PhotonNetwork.IsMasterClient == false) return;

            CardList = new List<byte>();
            ColorList = new List<Color>();

            // 리스트에 카드 14장 받기
            for (byte i = 1; i < 6; i++)
            {
                switch (i)
                {
                    case 1:
                        for (byte j = 0; j < 5; j++)
                        {
                            CardList.Add(i);
                        }
                        break;
                    case 2:
                    case 3:
                        for (byte j = 0; j < 3; j++)
                        {
                            CardList.Add(i);
                        }
                        break;
                    case 4:
                        for (byte j = 0; j < 2; j++)
                        {
                            CardList.Add(i);
                        }
                        break;
                    case 5:
                        CardList.Add(i);
                        break;
                }
            }

            // 리스트에 색상 4가지 받기
            ColorList.Add(newColor1);
            ColorList.Add(newColor2);
            ColorList.Add(newColor3);
            ColorList.Add(newColor4);
        }

        public void GameStart()
        {
            StartCoroutine(nameof(MainGame));
        }

        IEnumerator MainGame()
        {
            PhotonNetwork.Instantiate("HG_PlayerCard", new Vector3(0, 45, 0), Quaternion.identity);

            yield return halfS;
            gameStart.gameObject.SetActive(true);
            yield return oneS;
            gameStart.gameObject.SetActive(false);
            yield return halfS;

            if (PhotonNetwork.IsMasterClient) players = this.transform.GetComponentsInChildren<PlayerCard>();
            while (AliveCount > 1)
            {
                for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
                {
                    if (PhotonNetwork.IsMasterClient) SetCard(i);
                    yield return StartCoroutine(OneTurn(i));
                    
                    yield return RingBell;
                    
                }
                yield return null;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
                {
                    players[i].GameOver();
                }
            }
            HalliGalliMgr.Inst.GameOver();
        }

        void SetCard(int player)
        {
            players[player].UnFlip();
            for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                players[i].Init();
            }
            players[player].SetCard(CardList[Random.Range(0, CardList.Count)], ColorList[Random.Range(0, ColorList.Count)]);
        }

        IEnumerator OneTurn(int player)
        {
            time = 30f;
            flip = false;

            while (time > 0f)
            {
                Timer.text = $"{(int)time} Sec";
                if (player == HalliGalliMgr.Inst.MyNumber - 1 && Input.GetKeyDown(KeyCode.Space))
                {
                    photonView.RPC(nameof(RPC_RequestFlip), RpcTarget.MasterClient, player);
                    photonView.RPC(nameof(RPC_BellActive), RpcTarget.All);
                    photonView.RPC(nameof(RPC_TimeReset), RpcTarget.All);
                }
                time -= Time.deltaTime;
                yield return null;
            }

            Timer.text = "0 Sec";
            if (PhotonNetwork.IsMasterClient && flip == false) players[player].Flip();

            yield return oneS;
            Timer.text = "Time";
            yield return oneS;
        }
        

        public void BellJudge()
        {
            if (PhotonNetwork.IsMasterClient == false) return;

            photonView.RPC(nameof(RPC_RingBell), RpcTarget.All, true);


            byte C1 = 0; byte C2 = 0; byte C3 = 0; byte C4 = 0;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].cardColor == newColor1) C1 += players[i].mycardNumber;
                else if (players[i].cardColor == newColor2) C2 += players[i].mycardNumber;
                else if (players[i].cardColor == newColor3) C3 += players[i].mycardNumber;
                else if (players[i].cardColor == newColor4) C4 += players[i].mycardNumber;
            }

            if(C1 == 5 || C2 == 5 || C3 == 5 || C4 == 5) //해당하면 벨누르는거 ok
            {
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].MyringCount != 1) players[i].DownHeart();
                }
            }
            else //누른 플레이어 HP--
            {
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].MyringCount == 1) players[i].DownHeart();
                }
            }

            photonView.RPC(nameof(RPC_RingBell), RpcTarget.All, false);
        }

        public void GameOver(bool win)
        {
            if (win) gameStart.text = "Win";
            else gameStart.text = "Lose";

            gameStart.gameObject.SetActive(true);
        }

        #region Flip
        [PunRPC]
        void RPC_RequestFlip(int player)
        {
            flip = true;
            players[player].Flip();
        }

        [PunRPC]
        void RPC_BellActive()
        {
            bell.BellActive();
        }

        [PunRPC]
        void RPC_TimeReset()
        {
            time = 0;
        }
        #endregion

        [PunRPC]
        void RPC_RingBell(bool ringBell)
        {
            ring = ringBell;
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                CardList = new List<byte>();
                ColorList = new List<Color>();

                // 리스트에 카드 14장 받기
                for (byte i = 1; i < 6; i++)
                {
                    switch (i)
                    {
                        case 1:
                            for (byte j = 0; j < 5; j++)
                            {
                                CardList.Add(i);
                            }
                            break;
                        case 2:
                        case 3:
                            for (byte j = 0; j < 3; j++)
                            {
                                CardList.Add(i);
                            }
                            break;
                        case 4:
                            for (byte j = 0; j < 2; j++)
                            {
                                CardList.Add(i);
                            }
                            break;
                        case 5:
                            CardList.Add(i);
                            break;
                    }
                }

                // 리스트에 색상 4가지 받기
                ColorList.Add(newColor1);
                ColorList.Add(newColor2);
                ColorList.Add(newColor3);
                ColorList.Add(newColor4);

                players = this.transform.GetComponentsInChildren<PlayerCard>();
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            AliveCount--;
            if (PhotonNetwork.IsMasterClient)
                players = this.transform.GetComponentsInChildren<PlayerCard>();
        }
    }
}

