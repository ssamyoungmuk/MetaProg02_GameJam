using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameInfo
{
	public string GameName;
	public Sprite GameImage;
	public string GameSceneName;
}

[CreateAssetMenu(fileName = "New GameInfoList", menuName = "ScriptableObjects/GameInfoList", order = 1)]
public class GameInfoList : ScriptableObject
{
    
    [SerializeField] GameInfo[] gameInfos = null;

	public int GetGameCount() {  return gameInfos.Length; }
	public GameInfo GetGameInfoAt(int idx)
	{
		return gameInfos[idx];
	}
}
