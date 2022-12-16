using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;
using MafiaGame;

// 채팅옵션을 가져오기 위한 확장클래스
public static class AppSettingsExtensions
{
	public static ChatAppSettings GetChatSettings(this Photon.Realtime.AppSettings appSettings)
	{
		return new ChatAppSettings
		{
			AppIdChat = appSettings.AppIdChat,
			AppVersion = appSettings.AppVersion,
			FixedRegion = appSettings.IsBestRegion ? null : appSettings.FixedRegion,
			NetworkLogging = appSettings.NetworkLogging,
			Protocol = appSettings.Protocol,
			EnableProtocolFallback = appSettings.EnableProtocolFallback,
			Server = appSettings.IsDefaultNameServer ? null : appSettings.Server,
			Port = (ushort)appSettings.Port,
			ProxyServer = appSettings.ProxyServer
			// values not copied from AppSettings class: AuthMode
			// values not needed from AppSettings class: EnableLobbyStatistics 
		};
	}
}
public class UIChatManager : MonoBehaviour, IChatClientListener
{
	[Header("[대화내용]")]
	[SerializeField] ScrollRect srChat = null;		// 대화 목록
	[SerializeField] TextMeshProUGUI txtChat = null; // 대화 내용
	[SerializeField] TMP_InputField myChat = null;	// 대화 입력창


	List<UIChatLine> listCurLines = new List<UIChatLine>();		// 현재 입력된 대화 목록
	ChatClient myChatClient = null;
	UIChatLine prefChatLine = null; // 채팅 프리팹
	// Start is called before the first frame update
	void Start()
    {
		// 채팅UI 프리팹을 미리 생성
		prefChatLine = Resources.Load<UIChatLine>("chatLine");

		myChat.text = "";
        if(myChatClient == null)
		{
			myChatClient = new ChatClient(this);
			myChatClient.UseBackgroundWorkerForSending = true;
			myChatClient.AuthValues = new AuthenticationValues(PhotonNetwork.NickName);
			myChatClient.ConnectUsingSettings(PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings());
		}
    }

	public void OnDestroy()
	{
		if (myChatClient != null)
			myChatClient.Disconnect();
	}


	// Update is called once per frame
	void Update()
    {
		if (myChatClient != null)
			myChatClient.Service();


		if(Input.GetKeyDown(KeyCode.Return))
		{
			myChat.ActivateInputField();
		}
    }


	// 채팅창 입력
	public void OnEndEdit(string inStr)
	{
		if (GameLogic.Instance.myInfo.isDie) return;
		// 채팅 입력이 아무것도 없으면 리턴
		if (inStr.Length <= 0)
			return;

		// 포톤채팅으로 내가 입력한 내용을 보내기
		myChatClient.PublishMessage("public", inStr);
		myChat.text = "";

		srChat.verticalNormalizedPosition = 0;	// 스크롤뷰를 맨 마지막으로 스크롤 시킨다. (1이면 맨 위로 이동)

		// inStr 을 채팅목록창에 추가해야 한다.
		//addChatLine(PhotonNetwork.NickName, inStr);
	}

    void addChatLine(string userName, string chatLine)
	{
		//txtChat.text += $"[{userName}] : {chatLine}\n";	// [내이름] : 대화내용

		// UI 프리팹을 기준으로 UI 객체 하나 생성해서
		UIChatLine instObj = Instantiate<UIChatLine>(prefChatLine, srChat.content.transform);
		instObj.SetChat(userName, chatLine);
		listCurLines.Add(instObj);

		if(listCurLines.Count > 10)
		{
			// 제일 처음에 입력된 채팅컨트롤은 삭제
			instObj = listCurLines[0];
			Destroy(instObj.gameObject);
			// 리스트에서도 삭제
			listCurLines.RemoveAt(0);
		}

	}
    public void SystemMessge(string chatLine)
    {
        //txtChat.text += $"[{userName}] : {chatLine}\n";	// [내이름] : 대화내용

        // UI 프리팹을 기준으로 UI 객체 하나 생성해서
        UIChatLine instObj = Instantiate<UIChatLine>(prefChatLine, srChat.content.transform);
        instObj.SetChat("[System]", chatLine);
        listCurLines.Add(instObj);

        if (listCurLines.Count > 10)
        {
            // 제일 처음에 입력된 채팅컨트롤은 삭제
            instObj = listCurLines[0];
            Destroy(instObj.gameObject);
            // 리스트에서도 삭제
            listCurLines.RemoveAt(0);
        }

    }



    //----------------------------------------------------------------------------
    // 포톤채팅 인터페이스 구현 함수
    #region 포톤채팅 인터페이스 구현 함수
    public void DebugReturn(DebugLevel level, string message)
	{
		//throw new System.NotImplementedException();
	}

	public void OnChatStateChange(ChatState state)
	{
		//throw new System.NotImplementedException();
		//addChatLine("[시스템]", "OnChatStateChange : " + state);
	}

	public void OnConnected()
	{
		//throw new System.NotImplementedException();
		//addChatLine("[시스템]", "OnConnected !!!");

		// 채널 가입 필요
		myChatClient.Subscribe("public", 0);
	}

	public void OnDisconnected()
	{
		//throw new System.NotImplementedException();
		//addChatLine("[시스템]", "OnDisconnected !!!");
	}


	// 상대방이 보낸 채팅메세지를 받아오는 함수
	public void OnGetMessages(string channelName, string[] senders, object[] messages)
	{
		//throw new System.NotImplementedException();
		for(int i = 0; i < messages.Length; i++)
		{
			addChatLine(senders[i], messages[i].ToString());
		}
	}

	public void OnPrivateMessage(string sender, object message, string channelName)
	{
		//throw new System.NotImplementedException();
	}

	public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
	{
		//throw new System.NotImplementedException();
	}

	public void OnSubscribed(string[] channels, bool[] results)
	{
		//throw new System.NotImplementedException();
		//addChatLine("[시스템]", string.Format("OnSubscribed ({0}) < {1}>", string.Join(",", channels), string.Join(",", results)));
	}

	public void OnUnsubscribed(string[] channels)
	{
		//throw new System.NotImplementedException();
		//addChatLine("[시스템]", string.Format("OnUnsubscribed ({0})", string.Join(",", channels)));
	}

	public void OnUserSubscribed(string channel, string user)
	{
		//throw new System.NotImplementedException();
	}

	public void OnUserUnsubscribed(string channel, string user)
	{
		//throw new System.NotImplementedException();
	}
	#endregion // 포톤채팅 인터페이스 구현 함수
	//------------------------------------------------------------------------------------------
}
