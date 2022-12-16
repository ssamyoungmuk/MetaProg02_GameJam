using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MafiaGame;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviourPun
{
    public int player_Num { get; private set; } = 0;
    [field:SerializeField] public jobList jobName { get; private set; }
    [field: SerializeField] public bool isDie { get; private set; }
    [field: SerializeField] public bool isHeal { get; private set; }

    public enum Job
    {
        mafia = 0,
        police = 1,
        doctor = 2,
        citizen = 3
    }

    [PunRPC]
    void Die()
    {
        if (isHeal)
        {
            if (photonView.IsMine)
            {
                string st = $"{GameLogic.Instance.photonNick[player_Num]}님이 의사의 도음으로 살았습니다.";
                GameLogic.Instance.uIChatManager.gameObject.GetPhotonView().RPC("SystemMessge", RpcTarget.All, st);
            }

        }
        else
        {
            isDie = true;
            if (jobName == jobList.Mafia) GameLogic.Instance.characterJob.mafiaNum--;
            else GameLogic.Instance.characterJob.peopleNum--;
            if (photonView.IsMine)
            {
            string st = $"{GameLogic.Instance.photonNick[player_Num]}님이 죽었습니다.";
                GameLogic.Instance.uIChatManager.gameObject.GetPhotonView().RPC("SystemMessge", RpcTarget.All, st);
            }
            GameLogic.Instance.voteButton[player_Num].GetComponent<Image>().color = Color.red;
            GameLogic.Instance.voteButton[player_Num].GetComponent<Button>().interactable = false;
        }
    }
    [PunRPC]
    void PlayerNum(int num)
    {
        player_Num = num;
        GameLogic.Instance.gameObject.GetPhotonView().RPC("CreateMafiaInfo", RpcTarget.All);
    }
    public void Heal(bool bl)
    {
        isHeal = bl;
    }
    [PunRPC]
    public void Player_JobSeting(jobList _job)
    {
        jobName = _job;
    }
}
