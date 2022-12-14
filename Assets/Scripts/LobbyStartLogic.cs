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
        // ���� GameFrame ���� ��� �о� ����.
		gameFrames = listFrames.GetComponentsInChildren<GameFrame>();

        // GameFrame �� �����̸��� �����̹����� ��������.
        GameInfo gi = null;
        for(int i = 0; i < gameFrames.Length; ++i)
        {
            gi = gii.GetGameInfoAt(i + 1);
            gameFrames[i].ShowInfo(gi.GameName, gi.GameImage);

		}

	}
}
