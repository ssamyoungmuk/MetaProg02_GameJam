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
        [SerializeField] TextMeshProUGUI Timer;

        PlayerCard[] players = null;

        List<byte> CardList = null;
        List<Color> ColorList = null;
        Color newColor1; // 색상은 4종류
        Color newColor2; // 색상은 4종류
        Color newColor3; // 색상은 4종류
        Color newColor4; // 색상은 4종류
        byte number; // 숫자는 1 ~ 5
        Color color;

        public bool isGaming { get; set; } = true;

        void Awake()
        {
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
            bell.cardSetting();
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

            yield return new WaitForSeconds(0.2f);
            gameStart.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            gameStart.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);

            if (PhotonNetwork.IsMasterClient) players = this.transform.GetComponentsInChildren<PlayerCard>();
            bell.cardSetting();
            while (isGaming)
            {
                for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
                {
                    if (PhotonNetwork.IsMasterClient) players[i].UnFlip();
                    if (PhotonNetwork.IsMasterClient) SetCard(i);
                    yield return StartCoroutine(OneTurn(i));
                }
                yield return new WaitForSeconds(1f);
            }
            HalliGalliMgr.Inst.GameOver();
        }
        IEnumerator OneTurn(int player)
        {
            bell.BellSetting();
            float time = 30f;
            while (time > 0f)
            {
                Timer.text = $"{(int)time} Sec";
                if (player == HalliGalliMgr.Inst.MyNumber - 1 && Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("스페이스바 누름");
                    photonView.RPC(nameof(RPC_RequestFlip), RpcTarget.MasterClient, player);
                    break;
                }
                time -= Time.deltaTime;
                yield return null;
            }
            Timer.text = "0 Sec";
            if (PhotonNetwork.IsMasterClient) players[player].Flip();
        }
        void SetCard(int player)
        {
            color = ColorList[Random.Range(0, 3)];
            number = (byte)CardList[Random.Range(0, CardList.Count)];
            players[player].SetCard(number, color);
        }

        [PunRPC]
        void RPC_RequestFlip(int player)
        {
            players[player].Flip();
        }
    }
}

