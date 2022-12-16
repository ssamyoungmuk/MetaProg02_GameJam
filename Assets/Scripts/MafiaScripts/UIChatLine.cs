using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIChatLine : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtNickName;
	[SerializeField] TextMeshProUGUI txtChat;



	// Start is called before the first frame update
	void Start()
    {
        
    }

    public void SetChat(string name, string chat)
	{
		txtNickName.text = name;
		txtChat.text = chat;
	}
}
