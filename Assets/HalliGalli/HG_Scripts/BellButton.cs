using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;

namespace HalliGalli
{
    public class BellButton : MonoBehaviourPun
    {
        [SerializeField] ShuffleCard shuffleCard = null;
        PlayerCard[] players = null;
        Button Bell = null;
        byte ringCount = 0;
        int aliveCount;
        public byte MyringCount = 0;

        // Start is called before the first frame update
        void Start()
        {
            Bell = GetComponent<Button>();
            Bell.onClick.AddListener(ringBell);
        }
        public void cardSetting()
        {
            players = this.transform.GetComponentsInChildren<PlayerCard>();
        }
        public void BellSetting()
        {
            Bell.interactable = true;
        }

        void ringBell()
        {
            MyringCount += ringCount; //1¼øÀ§´Â 0
            Bell.interactable = false;
            photonView.RPC(nameof(RPC_AnnounceRingCount), RpcTarget.All);
            if (MyringCount == 0)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    bool ok = false;
                    if (players[i].mycardNumber == 5) break;

                    for (int j = i + 1; i < players.Length; i++)
                    {
                        if (players[i].cardColor == players[j].cardColor)
                        {
                            if (players[i].mycardNumber + players[j].mycardNumber != 5)
                            {
                                if (players[i].photonView.IsMine)
                                {
                                    players[i].DownHeart();
                                    ok = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (ok) break;
                }
            }
            else
            {
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].photonView.IsMine) players[i].DownHeart();
                }
            }

            aliveCount = players.Length;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].alive == false) aliveCount--;
            }
            if (aliveCount == 1)
            {
                shuffleCard.isGaming = false;
            }
        }
        [PunRPC]
        void RPC_AnnounceRingCount()
        {
            ringCount++;
        }
    }
}

