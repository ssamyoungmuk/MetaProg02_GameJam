using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PicoPark
{
    public class GameMgr : MonoBehaviour
    {
        static private GameMgr _instance;
        static public GameMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameMgr();
                }
                return _instance;
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
