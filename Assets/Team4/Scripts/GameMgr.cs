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

        [SerializeField] bool gameOver;

        [SerializeField] public UIMgr uiMgr;
        [SerializeField] public GameSceneManager gameSceneManager;
        // Start is called before the first frame update
        void Start()
        {
            uiMgr = FindObjectOfType<UIMgr>();
            gameSceneManager = FindObjectOfType<GameSceneManager>();
        }
        public void EndGame()
        {
            StartCoroutine(Co_ToLobby());
        }
        IEnumerator Co_ToLobby()
        {
            yield return new WaitForSeconds(3);
            gameOver = true;
            gameSceneManager.GameToLobby();
        }

        public bool GetOverState { get{ return gameOver; } }
    }
}
