using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace PicoPark
{
    public class FollowCam : MonoBehaviourPun
    {
        PlayerMove[] playerMove;
        Transform myPlayer;
        Vector3 followPos;

        void Start()
        {
            playerMove = FindObjectsOfType<PlayerMove>();
            for (int i = 0; i < playerMove.Length; i++)
            {
                if (playerMove[i].gameObject.GetPhotonView().IsMine)
                {
                    myPlayer = playerMove[i].transform;
                    break;
                }
            }
            followPos =gameObject.transform.position;
        }
        void Update()
        {
            followPos.x = myPlayer.transform.position.x;
            gameObject.transform.position = followPos;
        }
    }
}

