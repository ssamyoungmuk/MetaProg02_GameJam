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
	public Action<int, bool> callbackNormalMsg = null;
	public Action<int> callbackErrorMsg = null;

	public void ShowInfo(string gameName, Sprite gameImage)
	{
		if(gameImage != null)
		{
			imgGame.sprite = gameImage;
			txtGameName.text = gameName;
		}
	}


	// ���� ����
	public void OnClick_Game()
	{
		if (callbackExecGame != null)
			callbackExecGame(GameIndex);
	}

	// ���ƿ�
	public void OnClick_Good()
	{
		string keyName = SystemInfo.deviceName + "_good";
		int goodValue = PlayerPrefs.GetInt(keyName, 0);
		if(goodValue == 0)
		{
			Debug.Log("## ���ƿ�~~~");
			PlayerPrefs.SetInt(keyName, GameIndex);
			if (callbackNormalMsg != null)
				callbackNormalMsg(GameIndex, true);
		}
		else
		{
			if (goodValue == GameIndex)
			{
				Debug.Log("## ���� �Ⱦ��~~~");
				PlayerPrefs.SetInt(keyName, 0);
				if (callbackNormalMsg != null)
					callbackNormalMsg(GameIndex, false);
			}
			else
			{
				Debug.Log("## �̹� �ٸ� ���ӿ� ����ߴ�~~~~");
				if (callbackErrorMsg != null)
					callbackErrorMsg(goodValue);
			}
		}




	}
}
