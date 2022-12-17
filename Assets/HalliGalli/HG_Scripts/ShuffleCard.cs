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

        List<byte> CardList = null;
        List<Color> ColorList = null;
        Color newColor1; // 색상은 4종류
        Color newColor2; // 색상은 4종류
        Color newColor3; // 색상은 4종류
        Color newColor4; // 색상은 4종류
        byte number; // 숫자는 1 ~ 5
        Color color;

        float time = 0;
        bool flip = false;
        byte aliveCount = 0;
        WaitForSeconds oneS = null;
        WaitForSeconds halfS = null;

        void Awake()
        {
            oneS = new WaitForSeconds(1f);
            halfS = new WaitForSeconds(0.5f);
            aliveCount = PhotonNetwork.CurrentRoom.PlayerCount;

            if (PhotonNetwork.IsMasterClient == false) return;

            CardList = new List<byte>();
            ColorList = new List<Color>();

            ColorUtility.TryParseHtmlString("#E97A7B", out newColor1);
            ColorUtility.TryParseHtmlString("#85EFFE", out newColor2);
            ColorUtility.TryParseHtmlString("#C5F782", out newColor3);
            ColorUtility.TryParseHtmlString("#B195DS", out newColor4);

            // 리스트에 색상 4가지 받기
            ColorList.Add(newColor1);
            ColorList.Add(newColor2);
            ColorList.Add(newColor3);
            ColorList.Add(newColor4);

            // 리스트에 카드 14장 받기
            for (byte i = 1; i < 6; i++)
            {
                if (i == 1)
                {
                    for (byte j = 0; j < 5; j++)
                    {
                        CardList.Add(i);
                    }
                }
                else if (i == 2)
                {
                    for (byte j = 0; j < 3; j++)
                    {
                        CardList.Add(i);
                    }
                }
                else if (i == 3)
                {
                    for (byte j = 0; j < 3; j++)
                    {
                        CardList.Add(i);
                    }
                }
                else if (i == 4)
                {
                    for (byte j = 0; j < 2; j++)
                    {
                        CardList.Add(i);
                    }
                }
                else if (i == 5)
                {
                    CardList.Add(i);
                }
            }
        }
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                CardList = new List<byte>();
                ColorList = new List<Color>();

                ColorUtility.TryParseHtmlString("#E97A7B", out newColor1);
                ColorUtility.TryParseHtmlString("#85EFFE", out newColor2);
                ColorUtility.TryParseHtmlString("#C5F782", out newColor3);
                ColorUtility.TryParseHtmlString("#B195DS", out newColor4);

                // 리스트에 색상 4가지 받기
                ColorList.Add(newColor1);
                ColorList.Add(newColor2);
                ColorList.Add(newColor3);
                ColorList.Add(newColor4);

                // 리스트에 카드 14장 받기
                for (byte i = 1; i < 6; i++)
                {
                    if (i == 1)
                    {
                        for (byte j = 0; j < 5; j++)
                        {
                            CardList.Add(i);
                        }
                    }
                    else if (i == 2)
                    {
                        for (byte j = 0; j < 3; j++)
                        {
                            CardList.Add(i);
                        }
                    }
                    else if (i == 3)
                    {
                        for (byte j = 0; j < 3; j++)
                        {
                            CardList.Add(i);
                        }
                    }
                    else if (i == 4)
                    {
                        for (byte j = 0; j < 2; j++)
                        {
                            CardList.Add(i);
                        }
                    }
                    else if (i == 5)
                    {
                        CardList.Add(i);
                    }
                }

                players = this.transform.GetComponentsInChildren<PlayerCard>();
            }
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            aliveCount--;
            if (PhotonNetwork.IsMasterClient)
                players = this.transform.GetComponentsInChildren<PlayerCard>();
        }
        public void GameStart()
        {
            StartCoroutine(nameof(MainGame));
        }

        IEnumerator MainGame()
        {
            GameObject player = PhotonNetwork.Instantiate("HG_PlayerCard", new Vector3(0, 45, 0), Quaternion.identity);
            HalliGalliMgr.Inst.editMyNumber += player.GetComponent<PlayerCard>().SetMyNumber;
            player.GetComponent<PlayerCard>().countAlive = CountAlivePlayers;

            yield return halfS;
            gameStart.gameObject.SetActive(true);
            yield return oneS;
            gameStart.gameObject.SetActive(false);
            yield return halfS;

            if (PhotonNetwork.IsMasterClient) players = this.transform.GetComponentsInChildren<PlayerCard>();
            while (aliveCount > 1)
            {
                for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
                {
                    if (PhotonNetwork.IsMasterClient) players[i].UnFlip();
                    if (PhotonNetwork.IsMasterClient) SetCard(i);
                    yield return StartCoroutine(OneTurn(i));
                }
                yield return oneS;
            }
            HalliGalliMgr.Inst.GameOver();
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
        }
        void SetCard(int player)
        {
            color = ColorList[Random.Range(0, 3)];
            number = (byte)CardList[Random.Range(0, CardList.Count)];
            players[player].SetCard(number, color);
        }
        public void BellJudge()
        {
            StopCoroutine(nameof(MainGame));

            if (PhotonNetwork.IsMasterClient == false) return;

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

            photonView.RPC(nameof(RPC_Restart), RpcTarget.All);
        }

        void CountAlivePlayers()
        {
            aliveCount--;
        }

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
            time = 0f;
        }
        [PunRPC]
        void RPC_Restart()
        {
            StartCoroutine(nameof(MainGame));
        }
    }
}

