using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
	public Vector3 spawnArea;
	public Vector3 spawnOffset;
	private float m_spawnHeight;
	private int m_enemiesInNextWave;
	public GameObject testEnemy;
	public LayerMask AllButGround;

	private Collider[] m_colliders;
	public float radius;
	public bool shouldSpawn;

	private int m_failSafe = 25;
	private int m_SpawnAttempts = 0;

	private void Start()
	{
		m_enemiesInNextWave = transform.childCount + 1;
		Debug.Log(m_enemiesInNextWave);
		if(transform.childCount != 0)
		{
			m_spawnHeight = gameObject.transform.GetChild(0).transform.position.y;
		}
		else
		{
			m_spawnHeight = 1f;
		}
		

	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(255, 0, 0, 0.4f);
		Gizmos.DrawCube(spawnOffset, spawnArea);
	}

	//Initiate spawn.
	public void SpawnEnemies()
	{
		Debug.Log("Spawning");
		if (transform.childCount == 0 && shouldSpawn)
		{
			for (int i = 0; i < m_enemiesInNextWave; i++)
			{
				m_SpawnAttempts = 0;
				TrySpawnInRandomArea();
			}
			m_enemiesInNextWave++;
		}
	}


	private void TrySpawnInRandomArea()
	{
		if (++m_SpawnAttempts < m_failSafe)
		{
			Vector3 randomSpawnPos = new Vector3
				(Random.Range(-spawnArea.x / 2, spawnArea.x / 2) + spawnOffset.x,
				m_spawnHeight,
				Random.Range(-spawnArea.z / 2, spawnArea.z / 2) + spawnOffset.z);

			m_colliders = Physics.OverlapSphere(randomSpawnPos, radius, AllButGround);
			if (m_colliders.Length > 0)
			{
				TrySpawnInRandomArea();
				return;
			}
			else
			{
				SpawnEnemy(randomSpawnPos);
				return;
			}
		}
		else
		{
			Debug.Log("Aborted due to infinite loop");
			return;
		}

	}

	//Spawns enemies
	private void SpawnEnemy(Vector3 finalPosition)
	{
		Instantiate(testEnemy, finalPosition, Quaternion.identity, transform);
	}
}
