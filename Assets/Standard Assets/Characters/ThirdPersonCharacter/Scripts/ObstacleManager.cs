using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour {

	List<ObstacleBehaviour> m_obstacleList = new List<ObstacleBehaviour>();
	public ObstacleBehaviour m_obstaclePrefab;
	public void Spawn(Transform parent, Vector3 pos)
	{
		List<int> active_list = new List<int>();
		for (int i = 0; i < m_obstacleList.Count; i++) {
			if (m_obstacleList[i].CanSpawn())
				active_list.Add(i);
		}

		if (active_list.Count <= 0) {
			ObstacleBehaviour obstacle =  Instantiate(m_obstaclePrefab) as ObstacleBehaviour;
			active_list.Add (m_obstacleList.Count);
			m_obstacleList.Add(obstacle);
		}
		m_obstacleList [active_list [Random.Range (0, active_list.Count)]].Spawn (parent, pos);
	}
	public void DeSpawn(Transform parent)
	{
		for (int i = 0; i < m_obstacleList.Count; i++) {
				m_obstacleList[i].DeSpawn(parent);
		}
	}
}
