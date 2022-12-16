using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyStartLogic : MonoBehaviour
{
    [Header("[로딩 화면]")]
	[SerializeField] GameObject objLoadingUI = null;
	[SerializeField] TextMeshProUGUI textGameName = null; // 로딩 중 표시하기 위한 게임이름
    [Header("[게임프레임들..]")]
	[SerializeField] Transform listFrames = null;

    GameFrame[] gameFrames = null;
	GameInfoList resGameInfoList = null;
	// Start is called before the first frame update
	void Start()
    {
        // 처음엔 로딩화면 끄기
        objLoadingUI.SetActive(false);

        // 게임정보들을 가져오자..
		if (resGameInfoList == null)
			resGameInfoList = Resources.Load<GameInfoList>("GameInfoList");
        // 하위 GameFrame 들을 모두 읽어 오자.
		gameFrames = listFrames.GetComponentsInChildren<GameFrame>();

        // GameFrame 에 게임이름과 간판이미지를 보여주자.
        GameInfo gi = null;
        for(int i = 0; i < gameFrames.Length; ++i)
        {
            gi = resGameInfoList.GetGameInfoAt(i + 1);
            gameFrames[i].ShowInfo(gi.GameName, gi.GameImage);
            gameFrames[i].GameIndex = i + 1;
            if(gi.GameName.Length > 0) // 게임이름이 있다면 게임실행가능하므로..
                gameFrames[i].callbackExecGame = startGameAt;
		}

	}

    // 게임 실행
    void startGameAt(int gameIndex)
    {
		GameInfo gi = resGameInfoList.GetGameInfoAt(gameIndex);
        StartCoroutine(processGameStart(gi.GameName, gi.GameSceneName));
	}

    // 게임 시작 - 로딩화면 먼저 보여주고 씬로딩이 완료되면 씬으로 넘기기
    IEnumerator processGameStart(string gameName, string gameSceneName)
    {
        // 게임이름을 로딩화면에 보여주자.
        textGameName.text = gameName;
        // 로딩화면..
		objLoadingUI.SetActive(true);
		yield return new WaitForSeconds(1f);

        // 씬로딩
        AsyncOperation op = SceneManager.LoadSceneAsync(gameSceneName);
        while(op.isDone == false)
        {
            yield return null;
        }
    }
}
