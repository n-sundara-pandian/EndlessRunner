using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using  UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.UI;
using System;

struct ObstacleData
{
	public enum ObstacleType
	{
		InVisible,
		Low,
		High
	}
	public Vector3 m_position;
	public ObstacleType m_type;
	public ObstacleData(int x, ObstacleType type)
	{
		float y = 0.5f;
		m_type = type;
		if (m_type == ObstacleType.High)
			y = 1.5f;
		m_position = new Vector3 (6 - x , y, 0);
	}
}
public class GameManager : MonoBehaviour, IEventsInterface {

	public Transform m_floor1;
	public Transform m_floor2;
	public ObstacleManager m_obstacleManager;
	public ThirdPersonCharacter m_player;
	public GameObject m_GameOverCamera;
	public GameObject m_fireworks;
	public GameObject m_fxRef;
	public Text m_scoreText;
	public ScrollMsg m_messageBox;
	int m_score;
	int m_prevScore;
	int m_highScore;
	int m_section;
	int m_obstacleRowSize;
	bool m_scorePassReward;
	bool m_highScorePassReward;
	List<ObstacleData> m_obstacleData = new List<ObstacleData>();
	public AudioClip m_jumpSfx;
	public AudioClip m_runSfx;
	public AudioClip m_crouchSfx;
	public AudioClip m_dieSfx;

	// Use this for initialization
	void Start () {
		char [] delimiter = { ',' };
		TextAsset obstaclesData = Resources.Load("obstacles") as TextAsset;
		string []lines = obstaclesData.text.Split("\n"[0]);
		for (int i = 0; i < lines.Length; i++) {
			string []words = lines[i].Split(delimiter);
			if (i == 0)
			{
				m_obstacleRowSize = words.Length;
			}
			if (words.Length != m_obstacleRowSize)
			{
				Debug.Log("Data Error : Check Resources/Obstacles.txt file"); 
			}
			for(int j = m_obstacleRowSize - 1; j >= 0; --j)
			{
				ObstacleData.ObstacleType type = (ObstacleData.ObstacleType) int.Parse(words[j]);
				m_obstacleData.Add(new ObstacleData(j, type));
			}
		}
		Reset ();
	}
	void SetupFloorsForSection(int section)
	{
		m_section = section;
		if (m_section % 2 == 0) {
			Vector3 pos =  m_floor1.position;
			pos.x += 160;
			m_floor1.position = pos;
			SetupObstacles (m_floor1);
		}
		else {
			Vector3 pos =  m_floor2.position;
			pos.x += 160;
			m_floor2.position = pos;
			SetupObstacles (m_floor2);
		}
	}
	void SetupObstacles(Transform parent)
	{
		m_obstacleManager.DeSpawn (parent);
		int random_index = UnityEngine.Random.Range (0, m_obstacleData.Count / m_obstacleRowSize);
		int start_index = random_index * m_obstacleRowSize;
		for (int i = start_index; i < start_index + m_obstacleRowSize; i++) {
			if (m_obstacleData[i].m_type == ObstacleData.ObstacleType.InVisible)
				continue;
			m_obstacleManager.Spawn(parent, m_obstacleData[i].m_position);
		}
	}
	public void Reset()
	{
		Vector3 pos =  m_floor1.position;
		pos.x = -160.0f;
		m_floor1.position = pos;
		pos =  m_floor2.position;
		pos.x = -80.0f;
		m_floor2.position = pos;
		m_section = 0;
		m_score = 0;
		m_prevScore = PlayerPrefs.GetInt ("Score");
		m_highScore = PlayerPrefs.GetInt ("HighScore");
		m_scoreText.text = GetFormattedScore ();
		m_scorePassReward = false;
		m_highScorePassReward = false;
		SetupFloorsForSection (0);
	}
	void Update()
	{
		if (Input.GetKey(KeyCode.Space)  && !m_player.IsAlive())
		{
			if (!Application.isLoadingLevel)
				Application.LoadLevel("Game");
		}
		if (Input.GetKey (KeyCode.R)) {
			m_prevScore = m_score + 1;
			m_highScore = m_prevScore;
		}
	}
	public void OnCreateNextSection ()
	{
		m_section++;
		SetupFloorsForSection (m_section);
	}
	public void OnCollisionWithObstacle()
	{
		if (!m_player.IsAlive())
			return;
		Rigidbody r = m_player.GetComponent<Rigidbody> ();	
		r.AddExplosionForce (100, m_player.transform.position, 50);
		m_player.Die (true);
		m_GameOverCamera.SetActive (true);
		PlayerPrefs.SetInt ("Score", m_score);
		if (m_score > m_highScore)
			PlayerPrefs.SetInt ("HighScore", m_score);
	}
	string GetFormattedScore()
	{
		return string.Format ("Score {0} {3}Previous Run {1} {3}High Score {2} ", m_score, m_prevScore, m_highScore, Environment.NewLine);
	}
	public void OnBlockPass()
	{
		m_score++;
		m_scoreText.text = GetFormattedScore ();
		if (!m_scorePassReward && m_score > m_prevScore) {
			GameObject fx = Instantiate(m_fireworks, Vector3.zero, Quaternion.identity) as GameObject;
			fx.GetComponent<ParticleSystem>().loop = false;
			fx.transform.SetParent (m_fxRef.transform);
			fx.transform.localPosition = Vector3.zero;
			m_scorePassReward = true;
			m_messageBox.ShowMesage(" You Have passed ur last run");
		}
		else if (!m_highScorePassReward && m_score > m_highScore) {
			GameObject fx = Instantiate(m_fireworks, m_fxRef.transform.position, Quaternion.identity) as GameObject;
			fx.GetComponent<ParticleSystem>().loop = true;
			fx.transform.SetParent (m_fxRef.transform);
			fx.transform.localPosition = Vector3.zero;
			m_highScorePassReward = true;
			m_messageBox.ShowMesage(" You Have passed ur High Score");
		}
	}
	public void PlayJumpSfx()
	{
		AudioSource src = GetComponent<AudioSource> ();
		src.loop = false;
		src.PlayOneShot (m_jumpSfx);
	}
	public void PlayCrouchSfx()
	{
		AudioSource src = GetComponent<AudioSource> ();
		src.loop = false;
		src.PlayOneShot (m_crouchSfx);
	}
	public void PlayDieSfx()
	{
		AudioSource src = GetComponent<AudioSource> ();
		src.loop = false;
		src.PlayOneShot (m_dieSfx);
	}
	public void PlayRunSfx()
	{
		AudioSource src = GetComponent<AudioSource> ();
		if (src.clip == m_runSfx && src.isPlaying)
			return;
		src.loop = true;
		src.clip = m_runSfx;
		src.Play();
	}
}
