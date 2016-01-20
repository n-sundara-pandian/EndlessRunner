using UnityEngine;
using System.Collections;
public class ScrollMsg : MonoBehaviour {

	public TextMesh m_message;
	public float m_startX;
	public float m_endX;

	public void ShowMesage(string msg)
	{
		m_message.text = msg;
		Vector3 pos = m_message.transform.position;
		pos.x = m_startX;
		m_message.transform.position = pos;
		LeanTween.moveX (m_message.gameObject, m_endX, 5.0f ).setEase( LeanTweenType.linear ).setDelay(0.1f);
	}

}
