using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : MonoBehaviour
{
    int player_Num = 0;
    int player_JobNum = 0;
    string player_JobName;

    bool countChack = false;     // 플레이어의 수에 따라 직업을 결정할때 사용

    public enum Job
    {
        mafia = 0,
        police = 1,
        doctor = 2,
        citizen = 3
    }

    void PlayerNum(int num)
    {
        player_Num = num;
    }

    public void Player_JobSeting(int _job)
    {
        int playerCount = PhotonNetwork.PlayerList.Length;

        if (playerCount == 4 || playerCount == 5)
        {
            countChack = false;
            // 경찰 0, 의사 1, 마피아 2

        }
        else
        {
            countChack = true;
            // 경찰 0, 의사 1, 마피아 2 ~ 3
        }
    }

    public void JobSerch(int _job)
    {
        if (_job <= 3)
        {
            switch (_job)
            {
                case 0:
                    player_JobName = "경찰";
                    break;
                case 1:
                    player_JobName = "의사";
                    break;
                case 2:
                    player_JobName = "마피아";
                    break;
                case 3:
                    if (countChack) player_JobName = "마피아";
                    else player_JobName = "시민";
                    break;
            } 
        }
        else player_JobName = "시민";
    }
}
