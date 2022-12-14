using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameFrame : MonoBehaviour
{
    [SerializeField] Image imgGame;
	[SerializeField] TextMeshProUGUI txtGameName;

	public int GameIndex { get; set; }
	public Action<int> callbackExecGame = null;

	public void ShowInfo(string gameName, Sprite gameImage)
	{
		if(gameImage != null)
		{
			imgGame.sprite = gameImage;
			txtGameName.text = gameName;
		}
	}


	// 게임 실행
	public void OnClick_Game()
	{
		if (callbackExecGame != null)
			callbackExecGame(GameIndex);
	}
}
