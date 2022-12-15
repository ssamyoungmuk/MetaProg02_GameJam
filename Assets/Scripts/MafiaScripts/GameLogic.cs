using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

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

        public GameObject[] voteButton;
        [Header("scripts")]
        public CharacterJob characterJob;
        public PlayerInfo myInfo;
        [PunRPC]
        void MyInfo(int viewID)
        {
            if (photonView.IsMine)
                myInfo = PunFindObject(viewID).GetComponent<PlayerInfo>();
        }
        public GameObject PunFindObject(int viewID3)//����̵� �Ѱܹ޾� ������� ������Ʈ�� ã�´�.
        {
            GameObject find = null;
            PhotonView[] viewObject = FindObjectsOfType<PhotonView>();
            for (int i = 0; i < viewObject.Length; i++)
            {
                if (viewObject[i].ViewID == viewID3) find = viewObject[i].gameObject;
            }
            if (find != null) return find;
            else return null;
        }
        [PunRPC]
        void YouDie(int num)
        {
            if (PhotonNetwork.PlayerList[num].NickName==PhotonNetwork.NickName)
            {
                myInfo.gameObject.GetPhotonView().RPC("Die", RpcTarget.All);
            }
            voteButton[num].GetComponent<Image>().color = Color.red;
        }
        int day = 0;
        float time = 0;

        public void GameStart()
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                voteButton[i].SetActive(true);
            GameStartUI.SetActive(true);
            StartCoroutine(GameStart_Delay());
            myJobText.text = myInfo.jobName.ToString();
            jobUI2.SetActive(true);
            voteCount = new int[PhotonNetwork.PlayerList.Length];
        }

        IEnumerator GameStart_Delay()
        {
            Fade(GameStartUI, fade.All);
            yield return new WaitForSeconds(2f);
            morning=StartCoroutine(Day_Morning());
        }
        Coroutine morning;
        Coroutine debate;
        IEnumerator Day_Morning()
        {
            GameEnd();
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
                chat.SetActive(true);
            Fade(chat.gameObject, fade.In);
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
                debate_Text.text = "��ǥ ����";

                Fade(debate_Text.gameObject, fade.All);
                //��ǥ����
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
                //�ݷ�
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
                //����
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
            //����� ��chat.SetActive(true);
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
        bool isVoteOn;//��ǥ�ð�����
        bool Monning;//������
        bool Night;//������
        int[] voteCount;//�÷��̾� ��ǥ��
        bool voteChack;//��ǥ�ߴ���üũ
        bool voteSame;//��ǥ��������
        int maxVote;//��������ǥ��
        int maxVotePlayer;//�������÷��̾��ȣ


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
                voteSame = false;//��ǥ������ ����
                maxVote = voteCount[count];
                maxVotePlayer = count;
            }
            else if (maxVote <= voteCount[count]) voteSame = true;//��������ǥ���� �������� ��ǥ������ ture�ؼ� �÷��̾� ���� ����
        }
        public void VoteButton0()
        {
            if (myInfo.isDie) return;
            if (voteChack) return;
            if (isVoteOn) gameObject.GetPhotonView().RPC("VoteCount", RpcTarget.All, 0);
            else if (Night && myInfo.jobName == jobList.Mafia)
            {

            }
            else if (Night && myInfo.jobName == jobList.Doctor)
            {

            }
            else if (Night && myInfo.jobName == jobList.Police)
            {

            }

            voteChack = true;
        }
        [PunRPC]
        void VoteEnd()
        {
            if(voteSame||maxVotePlayer==-1)
            {
                Night = true;
                Vote_Text.text = "��ǥ ��ȿ";
                Vote_Text.gameObject.SetActive(true);
                Fade(Vote_Text.gameObject, fade.All);
            }
            else
            {
                Vote_Text.text = $"{PhotonNetwork.PlayerList[maxVotePlayer].NickName}�� �ݷ�";
                Vote_Text.gameObject.SetActive(true);
                Fade(Vote_Text.gameObject, fade.All);
            }
        }
        bool isVoteVoteChack;//������ǥ �ߴ���
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
                Vote_Text.text = "��ǥ ��ȿ";
                Vote_Text.gameObject.SetActive(true);

                Fade(Vote_Text.gameObject, fade.Out);
            }
            else
            {
                Vote_Text.text = $"{PhotonNetwork.PlayerList[maxVotePlayer].NickName}���� �׾����ϴ�.";
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

