using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;
using MafiaGame;

// ä�ÿɼ��� �������� ���� Ȯ��Ŭ����
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
	[Header("[��ȭ����]")]
	[SerializeField] ScrollRect srChat = null;		// ��ȭ ���
	[SerializeField] TextMeshProUGUI txtChat = null; // ��ȭ ����
	[SerializeField] TMP_InputField myChat = null;	// ��ȭ �Է�â


	List<UIChatLine> listCurLines = new List<UIChatLine>();		// ���� �Էµ� ��ȭ ���
	ChatClient myChatClient = null;
	UIChatLine prefChatLine = null; // ä�� ������
	// Start is called before the first frame update
	void Start()
    {
		// ä��UI �������� �̸� ����
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


	// ä��â �Է�
	public void OnEndEdit(string inStr)
	{
		if (GameLogic.Instance.myInfo.isDie) return;
		// ä�� �Է��� �ƹ��͵� ������ ����
		if (inStr.Length <= 0)
			return;

		// ����ä������ ���� �Է��� ������ ������
		myChatClient.PublishMessage("public", inStr);
		myChat.text = "";

		srChat.verticalNormalizedPosition = 0;	// ��ũ�Ѻ並 �� ���������� ��ũ�� ��Ų��. (1�̸� �� ���� �̵�)

		// inStr �� ä�ø��â�� �߰��ؾ� �Ѵ�.
		//addChatLine(PhotonNetwork.NickName, inStr);
	}

    void addChatLine(string userName, string chatLine)
	{
		//txtChat.text += $"[{userName}] : {chatLine}\n";	// [���̸�] : ��ȭ����

		// UI �������� �������� UI ��ü �ϳ� �����ؼ�
		UIChatLine instObj = Instantiate<UIChatLine>(prefChatLine, srChat.content.transform);
		instObj.SetChat(userName, chatLine);
		listCurLines.Add(instObj);

		if(listCurLines.Count > 10)
		{
			// ���� ó���� �Էµ� ä����Ʈ���� ����
			instObj = listCurLines[0];
			Destroy(instObj.gameObject);
			// ����Ʈ������ ����
			listCurLines.RemoveAt(0);
		}

	}
    public void SystemMessge(string chatLine)
    {
        //txtChat.text += $"[{userName}] : {chatLine}\n";	// [���̸�] : ��ȭ����

        // UI �������� �������� UI ��ü �ϳ� �����ؼ�
        UIChatLine instObj = Instantiate<UIChatLine>(prefChatLine, srChat.content.transform);
        instObj.SetChat("[System]", chatLine);
        listCurLines.Add(instObj);

        if (listCurLines.Count > 10)
        {
            // ���� ó���� �Էµ� ä����Ʈ���� ����
            instObj = listCurLines[0];
            Destroy(instObj.gameObject);
            // ����Ʈ������ ����
            listCurLines.RemoveAt(0);
        }

    }



    //----------------------------------------------------------------------------
    // ����ä�� �������̽� ���� �Լ�
    #region ����ä�� �������̽� ���� �Լ�
    public void DebugReturn(DebugLevel level, string message)
	{
		//throw new System.NotImplementedException();
	}

	public void OnChatStateChange(ChatState state)
	{
		//throw new System.NotImplementedException();
		//addChatLine("[�ý���]", "OnChatStateChange : " + state);
	}

	public void OnConnected()
	{
		//throw new System.NotImplementedException();
		//addChatLine("[�ý���]", "OnConnected !!!");

		// ä�� ���� �ʿ�
		myChatClient.Subscribe("public", 0);
	}

	public void OnDisconnected()
	{
		//throw new System.NotImplementedException();
		//addChatLine("[�ý���]", "OnDisconnected !!!");
	}


	// ������ ���� ä�ø޼����� �޾ƿ��� �Լ�
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
		//addChatLine("[�ý���]", string.Format("OnSubscribed ({0}) < {1}>", string.Join(",", channels), string.Join(",", results)));
	}

	public void OnUnsubscribed(string[] channels)
	{
		//throw new System.NotImplementedException();
		//addChatLine("[�ý���]", string.Format("OnUnsubscribed ({0})", string.Join(",", channels)));
	}

	public void OnUserSubscribed(string channel, string user)
	{
		//throw new System.NotImplementedException();
	}

	public void OnUserUnsubscribed(string channel, string user)
	{
		//throw new System.NotImplementedException();
	}
	#endregion // ����ä�� �������̽� ���� �Լ�
	//------------------------------------------------------------------------------------------
}
