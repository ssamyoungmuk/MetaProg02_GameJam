using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOO
{
    public class PlayerData : MonoBehaviour
    {
        [System.Serializable]
        public struct Info
        {
            public float damage;
            public float attackInterval;
            public float maxHp;
            public float speed;
            public Transform rightArm;
            public Transform leftArm;
            public float rotationSensetive;
            [HideInInspector] public float curHp;
        }
        [SerializeField] public Info info;
    }
}

