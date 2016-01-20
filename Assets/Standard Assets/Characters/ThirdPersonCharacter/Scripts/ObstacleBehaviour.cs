using UnityEngine;
using System.Collections;

public class ObstacleBehaviour : MonoBehaviour {

	bool m_Active = false;
	public GameObject m_explosion;
	GameManager m_gameManager;
	// Use this for initialization
	void Start () {
		m_gameManager = GameObject.FindWithTag ("GameController").GetComponent<GameManager>();
	}

	public void Spawn(Transform parent, Vector3 pos)
	{
		transform.parent = parent;
		transform.localPosition = pos;
		m_Active = true;
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.collider.CompareTag ("Player")) {
			UnityEngine.EventSystems.ExecuteEvents.Execute<GameManager>(m_gameManager.gameObject, null, (x,y)=>x.OnCollisionWithObstacle());
			Explode();
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) {
			UnityEngine.EventSystems.ExecuteEvents.Execute<GameManager>(m_gameManager.gameObject, null, (x,y)=>x.OnBlockPass());
		}
	}
	void Explode()
	{
		GameObject.Instantiate (m_explosion, transform.position, Quaternion.identity);
	}
	public void DeSpawn(Transform parent)
	{
		if (transform.parent == parent) {
			m_Active = false;
		}
	}
	public bool CanSpawn() { return !m_Active;}
}
