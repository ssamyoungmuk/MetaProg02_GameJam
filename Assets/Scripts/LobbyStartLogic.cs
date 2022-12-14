using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyStartLogic : MonoBehaviour
{
    [SerializeField] Transform listFrames = null;

    GameFrame[] gameFrames = null;
    // Start is called before the first frame update
    void Start()
    {
        GameInfoList gii = Resources.Load<GameInfoList>("GameInfoList");
        // 하위 GameFrame 들을 모두 읽어 오자.
		gameFrames = listFrames.GetComponentsInChildren<GameFrame>();

        // GameFrame 에 게임이름과 간판이미지를 보여주자.
        GameInfo gi = null;
        for(int i = 0; i < gameFrames.Length; ++i)
        {
            gi = gii.GetGameInfoAt(i + 1);
            gameFrames[i].ShowInfo(gi.GameName, gi.GameImage);

		}

	}
}
