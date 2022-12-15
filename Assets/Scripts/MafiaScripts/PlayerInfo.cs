using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : MonoBehaviour
{
    int player_Num = 0;
    int player_JobNum = 0;
    string player_JobName;

    bool countChack = false;     // �÷��̾��� ���� ���� ������ �����Ҷ� ���

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
            // ���� 0, �ǻ� 1, ���Ǿ� 2

        }
        else
        {
            countChack = true;
            // ���� 0, �ǻ� 1, ���Ǿ� 2 ~ 3
        }
    }

    public void JobSerch(int _job)
    {
        if (_job <= 3)
        {
            switch (_job)
            {
                case 0:
                    player_JobName = "����";
                    break;
                case 1:
                    player_JobName = "�ǻ�";
                    break;
                case 2:
                    player_JobName = "���Ǿ�";
                    break;
                case 3:
                    if (countChack) player_JobName = "���Ǿ�";
                    else player_JobName = "�ù�";
                    break;
            } 
        }
        else player_JobName = "�ù�";
    }
}
