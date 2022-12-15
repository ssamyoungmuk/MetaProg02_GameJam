using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace PicoPark
{
    public class GameSceneManager : MonoBehaviourPunCallbacks
    {
        private void Awake()
        {
            PhotonNetwork.Instantiate("PicoPlayer", Vector3.zero,Quaternion.identity);
        }
    }
}
