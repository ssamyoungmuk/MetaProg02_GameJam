using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIAlertMsg : MonoBehaviour
{
	[SerializeField] GameObject m_goNormal = null;
	[SerializeField] TextMeshProUGUI m_msg = null;
	
	[Header("")]
	[SerializeField] float m_fadeInDuration = 2.0f;
	[Header("[Animation]")]
	[SerializeField] Animation m_animComp = null;

	Coroutine m_coroutine = null;

	// msgType : 0 일반, 1 경고, 2 에러
	public void ShowMessage(string msg, float addFadeInDuration = 0f)
	{
		// play animation
		m_animComp.clip.legacy = true;
		m_animComp.Stop();
		m_animComp.Play();

		// msg
		m_msg.text = msg;
		gameObject.SetActive(true);


		if (m_coroutine != null)
			StopCoroutine(m_coroutine);
		m_coroutine = StartCoroutine(processShow(addFadeInDuration));
	}

	public void HideMessage()
	{
		gameObject.SetActive(false);
	}

	IEnumerator processShow(float addFadeInDuration)
	{
		yield return new WaitForSeconds(m_fadeInDuration + addFadeInDuration);
		HideMessage();
	}
}
