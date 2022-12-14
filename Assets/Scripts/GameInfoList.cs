using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameInfo
{
	public string Name;
	public Image GameImage;
}

public class GameInfoList : ScriptableObject
{
    
    [SerializeField] GameInfo[] gameInfos = null;

	public GameInfo GetGameInfoAt(int idx)
	{
		return gameInfos[idx];
	}
}
