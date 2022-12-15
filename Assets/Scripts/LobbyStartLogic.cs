using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyStartLogic : MonoBehaviour
{
    [Header("[�ε� ȭ��]")]
	[SerializeField] GameObject objLoadingUI = null;
	[SerializeField] TextMeshProUGUI textGameName = null; // �ε� �� ǥ���ϱ� ���� �����̸�
    [Header("[���������ӵ�..]")]
	[SerializeField] Transform listFrames = null;

    GameFrame[] gameFrames = null;
	GameInfoList resGameInfoList = null;
	// Start is called before the first frame update
	void Start()
    {
        // ó���� �ε�ȭ�� ����
        objLoadingUI.SetActive(false);

        // ������������ ��������..
		if (resGameInfoList == null)
			resGameInfoList = Resources.Load<GameInfoList>("GameInfoList");
        // ���� GameFrame ���� ��� �о� ����.
		gameFrames = listFrames.GetComponentsInChildren<GameFrame>();

        // GameFrame �� �����̸��� �����̹����� ��������.
        GameInfo gi = null;
        for(int i = 0; i < gameFrames.Length; ++i)
        {
            gi = resGameInfoList.GetGameInfoAt(i + 1);
            gameFrames[i].ShowInfo(gi.GameName, gi.GameImage);
            gameFrames[i].GameIndex = i + 1;
            if(gi.GameName.Length > 0) // �����̸��� �ִٸ� ���ӽ��డ���ϹǷ�..
                gameFrames[i].callbackExecGame = startGameAt;
		}

	}

    // ���� ����
    void startGameAt(int gameIndex)
    {
		GameInfo gi = resGameInfoList.GetGameInfoAt(gameIndex);
        StartCoroutine(processGameStart(gi.GameName, gi.GameSceneName));
	}

    // ���� ���� - �ε�ȭ�� ���� �����ְ� ���ε��� �Ϸ�Ǹ� ������ �ѱ��
    IEnumerator processGameStart(string gameName, string gameSceneName)
    {
        // �����̸��� �ε�ȭ�鿡 ��������.
        textGameName.text = gameName;
        // �ε�ȭ��..
		objLoadingUI.SetActive(true);
		yield return new WaitForSeconds(1f);

        // ���ε�
        AsyncOperation op = SceneManager.LoadSceneAsync(gameSceneName);
        while(op.isDone == false)
        {
            yield return null;
        }
    }
}
