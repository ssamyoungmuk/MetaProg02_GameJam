using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MafiaGame;

public class PlayerInfo : MonoBehaviour
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
        isDie=true;
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
