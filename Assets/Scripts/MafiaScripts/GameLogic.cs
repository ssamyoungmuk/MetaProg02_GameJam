using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using ExitGames.Client.Photon.StructWrapping;

namespace MafiaGame
{
    public class GameLogic : Singleton<GameLogic>
    {
        [Header("GameUI")]
        [SerializeField] GameObject GameStartUI;
        [SerializeField] TextMeshProUGUI DayText;
        [SerializeField] TextMeshProUGUI debate_Text;
        [SerializeField] TextMeshProUGUI debateTime_Text;
        [SerializeField] TextMeshProUGUI Vote_Text;
        [SerializeField] GameObject voteVote;
        [SerializeField] GameObject chat;
        [SerializeField] GameObject mafiaWin;
        [SerializeField] GameObject peopleWin;
        [SerializeField] GameObject jobUI;
        [SerializeField] GameObject jobUI2;
        [SerializeField] TextMeshProUGUI myJobText;
        [SerializeField] TextMeshProUGUI myTeam;
        [SerializeField] TextMeshProUGUI skillClick;

        public GameObject[] voteButton;
        [Header("scripts")]
        public CharacterJob characterJob;
        public PlayerInfo myInfo;
        public UIChatManager uIChatManager;
        public void MyInfo(GameObject obj)
        {
            myInfo = obj.GetComponent<PlayerInfo>();
        }
        PlayerInfo[] playerInfos;
        void YouDie(int num)
        {
            GameObject obj = null;
            for (int i = 0; i < playerInfos.Length; i++)
                if (playerInfos[i].player_Num == num) obj = playerInfos[i].gameObject;
            obj.gameObject.GetPhotonView().RPC("Die", RpcTarget.All);

        }
        int day = 0;
        int time = 0;
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
            yield return new WaitForSeconds(1f);
            if (myInfo.jobName == jobList.Doctor) myJobText.text = "의사";
            else if (myInfo.jobName == jobList.Police) myJobText.text = "경찰";
            else if (myInfo.jobName == jobList.Mafia) myJobText.text = "마피아";
            else if (myInfo.jobName == jobList.People) myJobText.text = "시민";
            jobUI2.SetActive(true);
            List<PlayerInfo> list = new List<PlayerInfo>();
            PlayerInfo[] play = FindObjectsOfType<PlayerInfo>();
            playerInfos = FindObjectsOfType<PlayerInfo>();
            for (int i = 0; i < play.Length; i++)
            {
                if (play[i].jobName == jobList.Mafia) list.Add(play[i]);
            }
            if (characterJob.mafiaNum > 1)
            {
                if (myInfo.jobName == jobList.Mafia)
                {
                    myTeam.gameObject.SetActive(true);
                    if (list[0].player_Num != myInfo.player_Num) myTeam.text = $"{PhotonNetwork.PlayerList[list[0].player_Num].NickName}";
                    else if (list[1].player_Num != myInfo.player_Num) myTeam.text = $"{PhotonNetwork.PlayerList[list[1].player_Num].NickName}";
                }
            }
            voteCount = new int[PhotonNetwork.PlayerList.Length];
            yield return new WaitForSeconds(1f);
            morning = StartCoroutine(Day_Morning());
        }
        Coroutine morning;
        Coroutine debate;
        IEnumerator Day_Morning()
        {
            day++;
            Monning = true;
            Night = false;
            DayText.text = day + "Day Morning";
            Fade(DayText.gameObject, fade.All);
            yield return new WaitForSeconds(2f);
            debate = StartCoroutine(StartDebate());
        }
        [PunRPC]
        void SetTime(int tm)
        {
            time = tm;
            timeSet = true;
        }
        bool voteEndSet;
        bool timeSet;
        IEnumerator StartDebate()
        {
            if (killPlayerNum != -1)
            {
                if (PhotonNetwork.IsMasterClient) YouDie(killPlayerNum);
            }
            for (int i = 0; i < voteButton.Length; i++) if (voteButton[i].GetComponent<Image>().color == Color.yellow) voteButton[i].GetComponent<Image>().color = Color.white;
            chat.SetActive(true);
            Fade(chat.gameObject, fade.In);
            isSkill = false;
            if (PhotonNetwork.IsMasterClient)
            {
                time = 120;
                gameObject.GetPhotonView().RPC("SetTime", RpcTarget.AllBufferedViaServer, time);
            }
            maxVote = 0;
            maxVotePlayer = -1;
            debateTime_Text.gameObject.SetActive(true);
            debateTime_Text.text = time.ToString();
            timeSet = false;
            yield return new WaitUntil(() => timeSet);
            timeSet = false;
            killPlayerNum = -1;
            if (PhotonNetwork.IsMasterClient) gameObject.GetPhotonView().RPC("GameEnd", RpcTarget.AllBufferedViaServer);
            myInfo.Heal(false);
            while (time > 0)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    time--;
                    gameObject.GetPhotonView().RPC("SetTime", RpcTarget.AllBufferedViaServer, time);
                }
                yield return new WaitForSeconds(1f);
                debateTime_Text.text = time.ToString();
            }

            for (int i = 0; i < voteCount.Length; i++)
                voteCount[i] = 0;
            debate_Text.text = "투표 시작";

            Fade(debate_Text.gameObject, fade.All);
            //투표시작
            isVoteVoteChack = false;
            voteChack = false;
            isVoteOn = true;
            if (PhotonNetwork.IsMasterClient)
            {
                time = 10;
                gameObject.GetPhotonView().RPC("SetTime", RpcTarget.AllBufferedViaServer, time);
            }
            timeSet = false;
            yield return new WaitUntil(() => timeSet);
            timeSet = false;
            while (time > 0)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    time--;
                    gameObject.GetPhotonView().RPC("SetTime", RpcTarget.AllBufferedViaServer, time);
                }
                yield return new WaitForSeconds(1f);
                debateTime_Text.text = time.ToString();
            }
            isVoteOn = false;
            //반론
            if (PhotonNetwork.IsMasterClient) gameObject.GetPhotonView().RPC("VoteEnd", RpcTarget.All);
            yield return new WaitUntil(() => voteEndSet);
            voteEndSet = false;
            if (Night == false)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    time = 20;
                    gameObject.GetPhotonView().RPC("SetTime", RpcTarget.AllBufferedViaServer, time);
                }
                timeSet = false;
                yield return new WaitUntil(() => timeSet);
                timeSet = false;
                while (time > 0)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        time--;
                        gameObject.GetPhotonView().RPC("SetTime", RpcTarget.AllBufferedViaServer, time);
                    }
                    yield return new WaitForSeconds(1f);
                    debateTime_Text.text = time.ToString();
                }
                //찬반
                if (PhotonNetwork.IsMasterClient) gameObject.GetPhotonView().RPC("VoteVote", RpcTarget.All);
                if (PhotonNetwork.IsMasterClient)
                {
                    time = 10;
                    gameObject.GetPhotonView().RPC("SetTime", RpcTarget.AllBufferedViaServer, time);
                }
                timeSet = false;
                yield return new WaitUntil(() => timeSet);
                timeSet = false;
                while (time > 0)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        time--;
                        gameObject.GetPhotonView().RPC("SetTime", RpcTarget.AllBufferedViaServer, time);
                    }
                    yield return new WaitForSeconds(1f);
                    debateTime_Text.text = time.ToString();
                }
                if (PhotonNetwork.IsMasterClient) gameObject.GetPhotonView().RPC("VoteVoteEnd", RpcTarget.All);
                voteVote.SetActive(false);
            }
            yield return new WaitForSeconds(1f);
            //사망후 밤
            chat.SetActive(true);
            Fade(chat.gameObject, fade.Out);
            for (int i = 0; i < voteButton.Length; i++) if (voteButton[i].GetComponent<Image>().color == Color.yellow) voteButton[i].GetComponent<Image>().color = Color.white;

            yield return new WaitForSeconds(1f);
            Vote_Text.gameObject.SetActive(false);

            if (PhotonNetwork.IsMasterClient) gameObject.GetPhotonView().RPC("GameEnd", RpcTarget.All);
            Monning = false;
            Night = true;
            DayText.text = day + "Day Night";
            Fade(DayText.gameObject, fade.All);
            if (PhotonNetwork.IsMasterClient)
            {
                time = 15;
                gameObject.GetPhotonView().RPC("SetTime", RpcTarget.AllBufferedViaServer, time);
            }
            timeSet = false;
            yield return new WaitUntil(() => timeSet);
            timeSet = false;
            while (time > 0)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    time--;
                    gameObject.GetPhotonView().RPC("SetTime", RpcTarget.AllBufferedViaServer, time);
                }
                yield return new WaitForSeconds(1f);
                debateTime_Text.text = time.ToString();
            }
            debateTime_Text.gameObject.SetActive(false);
            morning = StartCoroutine(Day_Morning());


        }
        bool isVoteOn;//투표시간인지
        bool Monning;//낮인지
        bool Night;//밤인지
        int[] voteCount;//플레이어 투표수
        bool voteChack;//투표했는지체크
        bool voteSame;//투표수같을때
        int maxVote;//젤높은투표수
        int maxVotePlayer;//젤높은플레이어번호


        string jobString;
        List<jobList> job = new List<jobList>(0);
        [PunRPC]
        void PlayerJobList(jobList a)
        {
            job.Add(a);
        }
        bool isGameEnd;
        [PunRPC]
        void GameEnd()
        {
            if (characterJob.mafiaNum == characterJob.peopleNum)
            {
                isGameEnd = true;
                mafiaWin.SetActive(true);
                if (morning != null) StopCoroutine(morning);
                if (debate != null) StopCoroutine(debate);
                chat.SetActive(true);
            }
            else if (characterJob.mafiaNum == 0)
            {
                isGameEnd = true;
                peopleWin.SetActive(true);
                if (morning != null) StopCoroutine(morning);
                if (debate != null) StopCoroutine(debate);
                chat.SetActive(true);
            }
        }
        [PunRPC]
        void VoteCount(int count)
        {
            voteCount[count]++;
            if (maxVote < voteCount[count])
            {
                voteSame = false;//투표수같음 해제
                maxVote = voteCount[count];
                maxVotePlayer = count;
            }
            else if (maxVote <= voteCount[count]) voteSame = true;//젤높은투표수와 같아질시 투표수같음 ture해서 플레이어 죽임 방지
        }
        bool isSkill;
        int killPlayerNum = -1;
        void MafiaClick(int num)
        {
            gameObject.GetPhotonView().RPC("MafiaNight", RpcTarget.All, num);
        }
        [PunRPC]
        void MafiaNight(int num)
        {
            if (myInfo.jobName == jobList.Mafia)
            {
                if (killPlayerNum != -1) voteButton[killPlayerNum].GetComponent<Image>().color = Color.white;
                voteButton[num].GetComponent<Image>().color = Color.yellow;
            }
            killPlayerNum = num;
        }
        void PoliceClick(int num)
        {
            if (isSkill == true) return;
            isSkill = true;
            skillClick.gameObject.SetActive(true);
            if (job[num] == jobList.Mafia) skillClick.text = "마피아 입니다.";
            else skillClick.text = "마피아가 아닙니다.";
            Fade(skillClick.gameObject, fade.All);
        }
        void DoctorClick(int num)
        {
            if (isSkill == true) return;
            isSkill = true;
            gameObject.GetPhotonView().RPC("YouHeal", RpcTarget.All, num);
        }
        [PunRPC]
        void YouHeal(int num)
        {
            for (int i = 0; i < playerInfos.Length; i++)
                if (playerInfos[i].player_Num == num) playerInfos[i].Heal(true);

        }
        #region 투표버튼
        public void VoteButton0()
        {
            if (myInfo.isDie) return;
            if (voteChack && Night != true) return;
            if (isVoteOn)
            {
                voteButton[0].GetComponent<Image>().color = Color.yellow;
                gameObject.GetPhotonView().RPC("VoteCount", RpcTarget.All, 0);
            }
            else if (Night && myInfo.jobName == jobList.Mafia && isSkill == false) MafiaClick(0);
            else if (Night && myInfo.jobName == jobList.Doctor && isSkill == false)
            {
                voteButton[0].GetComponent<Image>().color = Color.yellow;
                DoctorClick(0);
            }
            else if (Night && myInfo.jobName == jobList.Police && isSkill == false)
            {
                voteButton[0].GetComponent<Image>().color = Color.yellow;
                PoliceClick(0);
            }
            voteChack = true;
        }
        public void VoteButton1()
        {
            if (myInfo.isDie) return;
            if (voteChack && Night != true) return;
            if (isVoteOn)
            {
                voteButton[1].GetComponent<Image>().color = Color.yellow;
                gameObject.GetPhotonView().RPC("VoteCount", RpcTarget.All, 1);
            }
            else if (Night && myInfo.jobName == jobList.Mafia && isSkill == false) MafiaClick(1);
            else if (Night && myInfo.jobName == jobList.Doctor && isSkill == false)
            {
                voteButton[1].GetComponent<Image>().color = Color.yellow;
                DoctorClick(1);
            }
            else if (Night && myInfo.jobName == jobList.Police && isSkill == false)
            {
                voteButton[1].GetComponent<Image>().color = Color.yellow;
                PoliceClick(1);
            }
            voteChack = true;
        }
        public void VoteButton2()
        {
            if (myInfo.isDie) return;
            if (voteChack && Night != true) return;
            if (isVoteOn)
            {
                voteButton[2].GetComponent<Image>().color = Color.yellow;
                gameObject.GetPhotonView().RPC("VoteCount", RpcTarget.All, 2);
            }
            else if (Night && myInfo.jobName == jobList.Mafia && isSkill == false) MafiaClick(2);
            else if (Night && myInfo.jobName == jobList.Doctor && isSkill == false)
            {
                voteButton[2].GetComponent<Image>().color = Color.yellow;
                DoctorClick(2);
            }
            else if (Night && myInfo.jobName == jobList.Police && isSkill == false)
            {
                voteButton[2].GetComponent<Image>().color = Color.yellow;
                PoliceClick(2);
            }
            voteChack = true;
        }
        public void VoteButton3()
        {
            if (myInfo.isDie) return;
            if (voteChack && Night != true) return;
            if (isVoteOn)
            {
                voteButton[3].GetComponent<Image>().color = Color.yellow;
                gameObject.GetPhotonView().RPC("VoteCount", RpcTarget.All, 3);
            }
            else if (Night && myInfo.jobName == jobList.Mafia && isSkill == false) MafiaClick(3);
            else if (Night && myInfo.jobName == jobList.Doctor && isSkill == false)
            {
                voteButton[3].GetComponent<Image>().color = Color.yellow;
                DoctorClick(3);
            }
            else if (Night && myInfo.jobName == jobList.Police && isSkill == false)
            {
                voteButton[3].GetComponent<Image>().color = Color.yellow;
                PoliceClick(3);
            }
            voteChack = true;
        }
        public void VoteButton4()
        {
            if (myInfo.isDie) return;
            if (voteChack && Night != true) return;
            if (isVoteOn)
            {
                voteButton[4].GetComponent<Image>().color = Color.yellow;
                gameObject.GetPhotonView().RPC("VoteCount", RpcTarget.All, 4);
            }
            else if (Night && myInfo.jobName == jobList.Mafia && isSkill == false) MafiaClick(4);
            else if (Night && myInfo.jobName == jobList.Doctor && isSkill == false)
            {
                voteButton[4].GetComponent<Image>().color = Color.yellow;
                DoctorClick(4);
            }
            else if (Night && myInfo.jobName == jobList.Police && isSkill == false)
            {
                voteButton[4].GetComponent<Image>().color = Color.yellow;
                PoliceClick(4);
            }
            voteChack = true;
        }
        public void VoteButton5()
        {
            if (myInfo.isDie) return;
            if (voteChack && Night != true) return;
            if (isVoteOn)
            {
                voteButton[5].GetComponent<Image>().color = Color.yellow;
                gameObject.GetPhotonView().RPC("VoteCount", RpcTarget.All, 5);
            }
            else if (Night && myInfo.jobName == jobList.Mafia && isSkill == false) MafiaClick(5);
            else if (Night && myInfo.jobName == jobList.Doctor && isSkill == false)
            {
                voteButton[5].GetComponent<Image>().color = Color.yellow;
                DoctorClick(5);
            }
            else if (Night && myInfo.jobName == jobList.Police && isSkill == false)
            {
                voteButton[5].GetComponent<Image>().color = Color.yellow;
                PoliceClick(5);
            }
            voteChack = true;
        }
        public void VoteButton6()
        {
            if (myInfo.isDie) return;
            if (voteChack && Night != true) return;
            if (isVoteOn)
            {
                voteButton[6].GetComponent<Image>().color = Color.yellow;
                gameObject.GetPhotonView().RPC("VoteCount", RpcTarget.All, 6);
            }
            else if (Night && myInfo.jobName == jobList.Mafia && isSkill == false) MafiaClick(6);
            else if (Night && myInfo.jobName == jobList.Doctor && isSkill == false)
            {
                voteButton[6].GetComponent<Image>().color = Color.yellow;
                DoctorClick(6);
            }
            else if (Night && myInfo.jobName == jobList.Police && isSkill == false)
            {
                voteButton[6].GetComponent<Image>().color = Color.yellow;
                PoliceClick(6);
            }
            voteChack = true;
        }
        public void VoteButton7()
        {
            if (myInfo.isDie) return;
            if (voteChack && Night != true) return;
            if (isVoteOn)
            {
                voteButton[7].GetComponent<Image>().color = Color.yellow;
                gameObject.GetPhotonView().RPC("VoteCount", RpcTarget.All, 7);
            }
            else if (Night && myInfo.jobName == jobList.Mafia && isSkill == false) MafiaClick(7);
            else if (Night && myInfo.jobName == jobList.Doctor && isSkill == false)
            {
                voteButton[7].GetComponent<Image>().color = Color.yellow;
                DoctorClick(7);
            }
            else if (Night && myInfo.jobName == jobList.Police && isSkill == false)
            {
                voteButton[7].GetComponent<Image>().color = Color.yellow;
                PoliceClick(7);
            }
            voteChack = true;
        }
        #endregion
        [PunRPC]
        void VoteEnd()
        {
            if (voteSame || maxVotePlayer == -1)
            {
                Night = true;
                Vote_Text.text = "투표 무효";
                Vote_Text.gameObject.SetActive(true);
                Fade(Vote_Text.gameObject, fade.All);
            }
            else
            {
                Vote_Text.text = $"{PhotonNetwork.PlayerList[maxVotePlayer].NickName}의 반론";
                Vote_Text.gameObject.SetActive(true);
                Fade(Vote_Text.gameObject, fade.All);
            }
            voteEndSet = true;
        }
        bool isVoteVoteChack;//찬반투표 했는지
        [PunRPC]
        void VoteVote()
        {
            voteVote.SetActive(true);
        }
        public void LiveButton()
        {
            if (myInfo.isDie) return;
            if (isVoteVoteChack == true) return;
            isVoteVoteChack = true;
            gameObject.GetPhotonView().RPC("LiveCount", RpcTarget.All);
        }
        public void DieButton()
        {
            if (myInfo.isDie) return;
            if (isVoteVoteChack == true) return;
            isVoteVoteChack = true;
            gameObject.GetPhotonView().RPC("DieCount", RpcTarget.All);
        }
        [PunRPC]
        void LiveCount()
        {
            voteLive++;
        }
        [PunRPC]
        void DieCount()
        {
            voteDie++;
        }
        int voteLive;
        int voteDie;
        [PunRPC]
        void VoteVoteEnd()
        {
            if (voteDie <= voteLive)
            {
                Vote_Text.text = "투표 무효";
                Vote_Text.gameObject.SetActive(true);

                Fade(Vote_Text.gameObject, fade.All);
            }
            else
            {
                Vote_Text.text = $"{PhotonNetwork.PlayerList[maxVotePlayer].NickName}님이 죽었습니다.";
                Vote_Text.gameObject.SetActive(true);

                YouDie(maxVotePlayer);
                Fade(Vote_Text.gameObject, fade.All);
            }
        }

        public void MyJobClick()
        {
            if (jobUI.activeSelf) jobUI.SetActive(false);
            else jobUI.SetActive(true);
        }

        ////////////////////////////////////////////////////////////////////
        public void Fade(GameObject fadeIn, fade fd)
        {
            StartCoroutine(Fade_Delay(fadeIn, fd));
        }
        IEnumerator Fade_Delay(GameObject fadeIn, fade fd)
        {
            CanvasGroup canvasGroup = fadeIn.GetComponent<CanvasGroup>();
            if (fd == fade.In || fd == fade.All)
            {
                canvasGroup.alpha = 0;
                fadeIn.SetActive(true);
                for (int i = 0; i < 50; i++)
                {
                    canvasGroup.alpha += 0.02f;
                    yield return new WaitForSeconds(0.01f);
                }
            }
            if (fd == fade.Out || fd == fade.All)
            {

                canvasGroup.alpha = 1;
                for (int i = 0; i < 50; i++)
                {
                    canvasGroup.alpha -= 0.02f;
                    yield return new WaitForSeconds(0.01f);
                }
                fadeIn.SetActive(false);
            }
        }

    }
}

