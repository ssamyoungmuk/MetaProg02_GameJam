using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PicoPark
{
    public class GameMgr : MonoBehaviour
    {
        static private GameMgr _instance=null;
        static public GameMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<GameMgr>();
                    if (_instance == null)
                    {
                        _instance = new GameObject().GetComponent<GameMgr>();
                    }
                }
                return _instance;
            }
        }

        [SerializeField] public UIMgr uiMgr;
        // Start is called before the first frame update
        void Start()
        {
            uiMgr = FindObjectOfType<UIMgr>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
