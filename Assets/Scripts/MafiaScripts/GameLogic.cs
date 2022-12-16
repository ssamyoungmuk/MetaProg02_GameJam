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
        [PunRPC]
        void YouDie(int num)
        {
            if (PhotonNetwork.PlayerList[num].NickName==PhotonNetwork.NickName)
            {
                myInfo.gameObject.GetPhotonView().RPC("Die", RpcTarget.All);
            }
        }
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
            yield return new WaitForSeconds(1f);
            if (myInfo.jobName == jobList.Doctor) myJobText.text = "의사";
            else if (myInfo.jobName == jobList.Police) myJobText.text = "경찰";
            else if (myInfo.jobName == jobList.Mafia) myJobText.text = "마피아";
            else if (myInfo.jobName == jobList.People) myJobText.text = "시민";
            jobUI2.SetActive(true);
            List<PlayerInfo> list = new List<PlayerInfo>();
            PlayerInfo[] play = FindObjectsOfType<PlayerInfo>();
            for(int i=0;i<play.Length;i++)
            {
                if (play[i].jobName == jobList.Mafia) list.Add(play[i]);
            }
            if (characterJob.mafiaNum > 1)
            {
                if (myInfo.jobName == jobList.Mafia)
                {
                    if(play[0].player_Num!= myInfo.player_Num) myTeam.text = $"{PhotonNetwork.PlayerList[play[0].player_Num].NickName}";
                    else if(play[1].player_Num!= myInfo.player_Num) myTeam.text = $"{PhotonNetwork.PlayerList[play[1].player_Num].NickName}";
                }
            }
            voteCount = new int[PhotonNetwork.PlayerList.Length];
            yield return new WaitForSeconds(1f);
            morning =StartCoroutine(Day_Morning());
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
            debate= StartCoroutine(StartDebate());
        }

        IEnumerator StartDebate()
        {
            if(killPlayerNum!=-1)
            {

                if(PhotonNetwork.IsMasterClient) gameObject.GetPhotonView().RPC("YouDie", RpcTarget.All, killPlayerNum);
                GameEnd();
            }
            killPlayerNum = -1;
                chat.SetActive(true);
            Fade(chat.gameObject, fade.In);
            isSkill = false;
            myInfo.Heal(false);
                time = 5f;
                maxVote = 0;
                maxVotePlayer = -1;
                debateTime_Text.gameObject.SetActive(true);
                debateTime_Text.text = time.ToString();
                while (time > 0)
                {
                    time--;
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
                time = 5f;
                while (time > 0)
                {
                    time--;
                    yield return new WaitForSeconds(1f);
                    debateTime_Text.text = time.ToString();
                }
                isVoteOn = false;
                //반론
                if (PhotonNetwork.IsMasterClient) gameObject.GetPhotonView().RPC("VoteEnd", RpcTarget.All);
            if (Night == false)
            {
                time = 5f;
                while (time > 0)
                {
                    time--;
                    yield return new WaitForSeconds(1f);
                    debateTime_Text.text = time.ToString();
                }
                //찬반
                    if (PhotonNetwork.IsMasterClient) gameObject.GetPhotonView().RPC("VoteVote", RpcTarget.All);
                    time = 5f;
                    while (time > 0)
                    {
                        time--;
                        yield return new WaitForSeconds(1f);
                        debateTime_Text.text = time.ToString();
                    }
                if (PhotonNetwork.IsMasterClient) gameObject.GetPhotonView().RPC("VoteVoteEnd", RpcTarget.All);
                voteVote.SetActive(false);
                }
            //사망후 밤chat.SetActive(true);
            Fade(chat.gameObject, fade.Out);

            yield return new WaitForSeconds(2f);

                Vote_Text.gameObject.SetActive(false);

            GameEnd();
                Monning = false;
                Night = true;
            DayText.text = day + "Day Night";
            Fade(DayText.gameObject, fade.All);
            time = 20f;
                while (time > 0)
                {
                    time--;
                    yield return new WaitForSeconds(1f);
                    debateTime_Text.text = time.ToString();
                }
            debateTime_Text.gameObject.SetActive(false);
            morning=StartCoroutine(Day_Morning());


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
        void GameEnd()
        {
            if (characterJob.mafiaNum == characterJob.peopleNum)
            {
                isGameEnd = true;
                mafiaWin.SetActive(true);
                if(morning!=null) StopCoroutine(morning);
                if (debate != null) StopCoroutine(debate);
                chat.SetActive(true);
            }
            else if(characterJob.mafiaNum==0)
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
        int killPlayerNum=-1;
        void MafiaClick(int num)
        {
            gameObject.GetPhotonView().RPC("MafiaNight", RpcTarget.All, num);
        }
        [PunRPC]
        void MafiaNight(int num)
        {
            if (myInfo.jobName == jobList.Mafia)
            {
                if(killPlayerNum!=-1) voteButton[num].GetComponent<Image>().color = Color.white;
                voteButton[num].GetComponent<Image>().color = Color.yellow;
            }
            killPlayerNum = num;
        }
        void PoliceClick(int num)
        {
            if (isSkill == true) return;
            isSkill = true;
            skillClick.gameObject.SetActive(true);
            if (job[num]==jobList.Mafia) skillClick.text = "마피아 입니다.";
            else skillClick.text = "마피아가 아닙니다.";
            Fade(skillClick.gameObject, fade.All);
        }
        void DoctorClick(int num)
        {
            if (isSkill == true) return;
            isSkill = true;
            gameObject.GetPhotonView().RPC("YouHeal",RpcTarget.All,num);
        }
        [PunRPC]
        void YouHeal(int num)
        {
            if(myInfo.player_Num==num)
            myInfo.Heal(true);
        }
        public void VoteButton0()
        {
            if (myInfo.isDie) return;
            if (voteChack&&Night!=true) return;
            if (isVoteOn) gameObject.GetPhotonView().RPC("VoteCount", RpcTarget.All, 0);
            else if (Night && myInfo.jobName == jobList.Mafia&&isSkill==false)
            {
                MafiaClick(0);
            }
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
        [PunRPC]
        void VoteEnd()
        {
            if(voteSame||maxVotePlayer==-1)
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
            if(voteDie<=voteLive)
            {
                Vote_Text.text = "투표 무효";
                Vote_Text.gameObject.SetActive(true);

                Fade(Vote_Text.gameObject, fade.Out);
            }
            else
            {
                Vote_Text.text = $"{PhotonNetwork.PlayerList[maxVotePlayer].NickName}님이 죽었습니다.";
                Vote_Text.gameObject.SetActive(true);

                gameObject.GetPhotonView().RPC("YouDie", RpcTarget.All, maxVotePlayer);
                Fade(Vote_Text.gameObject, fade.Out);
            }
        }
        
        public void MyJobClick()
        {
            if(jobUI.activeSelf) jobUI.SetActive(false);
            else jobUI.SetActive(true);
        }

        ////////////////////////////////////////////////////////////////////
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

