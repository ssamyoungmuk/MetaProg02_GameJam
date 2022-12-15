using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOO
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] PlayerData playerData = null;

        private void OnCollisionEnter(Collision collision)
        {
            BasePlayer targetPlayer =null;

            if (collision.gameObject!=this.gameObject&&
                collision.gameObject.TryGetComponent<BasePlayer>(out targetPlayer))
            {
                targetPlayer.TransferDamage(playerData.info.damage);
            }
        }

    }
}

