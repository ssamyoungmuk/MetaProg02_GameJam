using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] int player_Num = 0;
    [SerializeField] int player_JobNum = 0;
    [SerializeField] string player_JobName;

    [SerializeField] bool countChack = false;     // �÷��̾��� ���� ���� ������ �����Ҷ� ���

    public enum Job
    {
        mafia = 0,
        police = 1,
        doctor = 2,
        citizen = 3
    }

    [PunRPC]
    void PlayerNum(int num)
    {
        player_Num = num;
    }

    [PunRPC]
    public void Player_JobSeting(int _job)
    {
        int playerCount = PhotonNetwork.PlayerList.Length;

        if (playerCount == 4 || playerCount == 5)
        {
            // ���� 0, �ǻ� 1, ���Ǿ� 2
            JobSerch(_job, false);
            Debug.Log(player_Num + " , " + player_JobName);
        }
        else
        {
            // ���� 0, �ǻ� 1, ���Ǿ� 2 ~ 3 a 4
            JobSerch(_job, true);
            Debug.Log(player_Num + " , " + player_JobName);
        }
    }

    public void JobSerch(int _job, bool countChack)
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
