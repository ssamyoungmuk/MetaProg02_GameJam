using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System;

namespace MafiaGame
{ 
    public class GameLogic : Singleton<GameLogic>
    {
        [Header("GameUI")]
        [SerializeField] GameObject GameStartUI;
        [SerializeField] TextMeshProUGUI DayText;
        [SerializeField] TextMeshProUGUI debate_Text;
        [SerializeField] TextMeshProUGUI debateTime_Text;
        public GameObject[] voteButton;

        int day = 0;
        float time = 0;

        public void GameStart()
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                voteButton[i].SetActive(true);
            GameStartUI.SetActive(true);
            StartCoroutine(GameStart_Delay());
        }

        IEnumerator GameStart_Delay()
        {
            Fade(GameStartUI, fade.All);
            yield return new WaitForSeconds(2f);
            StartCoroutine(Day_Morning());
        }

        IEnumerator Day_Morning()
        {
            day++;
            Monning = true;
            Night = false;
            DayText.text = day + DayText.text + " Morning";
            Fade(DayText.gameObject, fade.All);
            yield return new WaitForSeconds(2f);
            StartCoroutine(StartDebate());
        }

        IEnumerator StartDebate()
        {
            time = 180f;
            debateTime_Text.gameObject.SetActive(true);
            debateTime_Text.text = time.ToString();
            while (time >= 0)
            {
                time--;
                yield return new WaitForSeconds(1f);
                debateTime_Text.text = time.ToString();
            }
            debateTime_Text.gameObject.SetActive(false);

            debate_Text.text = "투표 시작";

            Fade(debate_Text.gameObject, fade.All);
            //투표시작
            voteChack = false;
            isVoteOn = true;
            yield return new WaitForSeconds(10f);
            isVoteOn = false;
            //반론
            yield return new WaitForSeconds(30f);
            //찬반
            yield return new WaitForSeconds(10f);
            //사망후 밤
            
            Monning = false;
            Night = true;
            
            //StartCoroutine();

        }
        bool isVoteOn;
        bool Monning;
        bool Night;
        int[] voteCount;
        bool voteChack;

        string jobString;
        PlayerInfo[] playerList = FindObjectsOfType<PlayerInfo>();
        Dictionary<string, int> list = new Dictionary<string, int>();
        [PunRPC]
        void VoteCount(int count)
        {
            voteCount[count]++;
        }
        void VoteButton0()
        {
            if (voteChack) return;
            if (isVoteOn) gameObject.GetPhotonView().RPC("VoteCount", RpcTarget.All, 0);
            jobString = playerList[0].player_JobName;
            if(Night) 

            voteChack = true;
        }
        void VoteButton1()
        {
            if (voteChack) return;
            if (isVoteOn) gameObject.GetPhotonView().RPC("VoteCount", RpcTarget.All, 1);
            voteChack = true;
        }
        void VoteButton2()
        {
            if (voteChack) return;
            if (isVoteOn) gameObject.GetPhotonView().RPC("VoteCount", RpcTarget.All, 2);
            voteChack = true;
        }
        void VoteSort()
        {
            
        }
        CanvasGroup canvasGroup;
        public void Fade(GameObject fadeIn, fade fd)
        {
            canvasGroup = fadeIn.GetComponent<CanvasGroup>();
            StartCoroutine(Fade_Delay(fadeIn, fd));
        }
        IEnumerator Fade_Delay(GameObject fadeIn, fade fd)
        {
            if (fd == fade.In || fd == fade.All)
            {
                canvasGroup.alpha = 0;
                fadeIn.SetActive(true);
                for (int i = 0; i < 100; i++)
                {
                    canvasGroup.alpha += 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }
            }
            if (fd == fade.Out || fd == fade.All)
            {

                canvasGroup.alpha = 1;
                for (int i = 0; i < 100; i++)
                {
                    canvasGroup.alpha -= 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }
                fadeIn.SetActive(false);
            }
        }

    }
}

