using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameFrame : MonoBehaviour
{
    [SerializeField] Image imgGame;
	[SerializeField] TextMeshProUGUI txtGameName;

	public void ShowInfo(string gameName, Sprite gameImage)
	{
		if(gameImage != null)
		{
			imgGame.sprite = gameImage;
			txtGameName.text = gameName;
		}
	}
}
