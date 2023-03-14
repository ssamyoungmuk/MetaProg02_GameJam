using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HalliGalli
{
    public class PlayerCard : MonoBehaviourPun
    {
        private ShuffleCard cardMaster = null;

        [SerializeField] TextMeshProUGUI nickName = null;
        [SerializeField] TextMeshProUGUI cardNumber = null;
        
        [SerializeField] Image[] playerHP = null;

        GameObject inGamePanel = null;
        Button Bell = null;

        public bool Alive { get; private set; } = true;
        public byte MyringCount = 0;
        public byte mycardNumber = 0;
        public Color cardColor = Color.white;

        byte HP = 5;
        Color dieColor = new Color(100, 0, 0, 200);

        private void Awake()
        {
            inGamePanel = GameObject.Find("InGamePanel");
            cardNumber.gameObject.SetActive(false);
            if (photonView.IsMine)
            {
                photonView.RPC(nameof(RPC_SetParent), RpcTarget.AllBuffered, HalliGalliMgr.Inst.MyNumber);
                photonView.RPC(nameof(RPC_SetNickName), RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
                HalliGalliMgr.Inst.editMyNumber = SetMyNumber;
                inGamePanel.TryGetComponent<ShuffleCard>(out cardMaster);
            }
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                Bell = inGamePanel.GetComponentInChildren<Button>();
                Bell.onClick.AddListener(delegate { BellJudge(); });
            }
        }

        public void SetMyNumber(byte myNumber)
        {
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

        public void UnFlip()
        {
            if (Alive == false) return;
            photonView.RPC(nameof(RPC_UnFlip), RpcTarget.All);
        }
        [PunRPC]
        void RPC_UnFlip()
        {
            cardNumber.gameObject.SetActive(false);
        }

        public void Init()
        {
            if (Alive == false) return;
            photonView.RPC(nameof(RPC_Init), RpcTarget.All);
        }
        [PunRPC]
        void RPC_Init()
        {
            MyringCount = 0;
        }

        public void SetCard(byte Number, Color color)
        {
            if (Alive == false) return;
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
            if (Alive == false) return;
            photonView.RPC(nameof(RPC_Flip), RpcTarget.All);
        }

        [PunRPC]
        void RPC_Flip()
        {
            cardNumber.text = mycardNumber.ToString();
            cardNumber.color = cardColor;
            cardNumber.gameObject.SetActive(true);
        }

        void BellJudge()
        {
            photonView.RPC(nameof(RPC_RingBell), RpcTarget.All);
        }

        [PunRPC]
        void RPC_RingBell()
        {
            MyringCount = 1;
            cardMaster.BellJudge();
        }

        public void DownHeart()
        {
            if (Alive == false) return;
            photonView.RPC(nameof(RPC_DownHeart), RpcTarget.All);
        }

        [PunRPC]
        void RPC_DownHeart()
        {
            HP--;
            playerHP[HP].color = dieColor;
            if (HP == 0)
            {
                Alive = false;
                MyringCount = 0;
                mycardNumber = 0;
                this.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                cardMaster.AliveCount--;
            }
        }

        public void GameOver()
        {
            photonView.RPC(nameof(RPC_GameOver), RpcTarget.All);
        }
        [PunRPC]
        void RPC_GameOver()
        {
            if (photonView.IsMine)
            {
                if (Alive) cardMaster.GameOver(true);
                else cardMaster.GameOver(false);
            }
        }
    }
}

