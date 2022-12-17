using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;

namespace HalliGalli
{
    public class PlayerCard : MonoBehaviourPun
    {
        public CountAlive countAlive = null;

        [SerializeField] TextMeshProUGUI cardNumber = null;
        [SerializeField] TextMeshProUGUI nickName = null;
        [SerializeField] Image[] playerHP = null;

        GameObject inGamePanel = null;
        Button Bell = null;

        public bool alive { get; private set; } = true;
        public byte MyringCount = 0;
        public byte mycardNumber = 0;
        public Color cardColor = Color.black;

        byte HP = 5;
        Color dieColor = new Color(100, 0, 0, 200);

        private void Awake()
        {
            inGamePanel = GameObject.FindGameObjectWithTag("inGmaePanel");
            cardNumber.gameObject.SetActive(false);
            if (photonView.IsMine)
            {
                photonView.RPC(nameof(RPC_SetParent), RpcTarget.AllBuffered, HalliGalliMgr.Inst.MyNumber);
                photonView.RPC(nameof(RPC_SetNickName), RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
                Bell = inGamePanel.transform.GetComponentInChildren<Button>();
                Bell.onClick.AddListener(delegate { BellJudge(); });
            }
        }

        void BellJudge()
        {
            MyringCount = 1;
        }
        public void SetMyNumber(byte myNumber)
        {
            if (photonView.IsMine)
                photonView.RPC(nameof(RPC_SetParent), RpcTarget.AllBuffered, myNumber);
        }

        [PunRPC]
        void RPC_SetParent(byte parentindex)
        {
            Transform parent = inGamePanel.transform.GetChild(parentindex);
            this.transform.SetParent(parent, false);
        }
        [PunRPC]
        void RPC_SetNickName(string MyNickName)
        {
            nickName.text = MyNickName;
        }

        public void SetCard(byte Number, Color color)
        {
            if (alive == false) return;
            photonView.RPC(nameof(RPC_SetCard), RpcTarget.All, Number, color.r, color.g, color.b, color.a);
        }
        [PunRPC]
        void RPC_SetCard(byte Number, float R, float G, float B, float A)
        {
            mycardNumber = Number;
            cardColor.r = R;
            cardColor.g = G;
            cardColor.b = B;
            cardColor.a = A;
        }

        public void Flip()
        {
            if (alive == false) return;
            photonView.RPC(nameof(RPC_Flip), RpcTarget.All);
        }

        [PunRPC]
        void RPC_Flip()
        {
            MyringCount = 0;
            cardNumber.text = mycardNumber.ToString();
            cardNumber.color = cardColor;
            cardNumber.gameObject.SetActive(true);
        }

        public void UnFlip()
        {
            if (alive == false) return;
            photonView.RPC(nameof(RPC_UnFlip), RpcTarget.All);
        }

        [PunRPC]
        void RPC_UnFlip()
        {
            mycardNumber = 0;
            cardNumber.gameObject.SetActive(false);
        }


        public void DownHeart()
        {
            if (alive == false) return;
            photonView.RPC(nameof(RPC_DownHeart), RpcTarget.All);
        }

        [PunRPC]
        void RPC_DownHeart()
        {
            HP--;
            playerHP[HP].color = dieColor;
            if (HP == 0)
            {
                alive = false;
                mycardNumber = 0;
                this.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                if (countAlive != null) countAlive();
            }
        }
    }
}

