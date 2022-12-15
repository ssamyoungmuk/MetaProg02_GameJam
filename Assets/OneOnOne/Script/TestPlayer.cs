using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace OOO
{
    public class TestPlayer : BasePlayer
    {
        protected override void InputKey()
        {
            if (!photonView.IsMine) return;
            base.InputKey();
        }

        protected override void PlayerMoveAndRotation()
        {
            if (!photonView.IsMine) return;
            base.PlayerMoveAndRotation();
        }
    }
}

