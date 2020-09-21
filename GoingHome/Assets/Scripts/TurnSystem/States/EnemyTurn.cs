using System;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyTurn : ITurn
{
	/// <summary>
	/// Invoke from enemy when finished with its turn
	/// </summary>
	public static Action EnemyFinished { get; private set; } = delegate { };

	private EnemySpawn m_EnemySpawner = null;
	private Transform m_EnemyParent = null;
	private EnemyController[] m_Enemies = null;
	private int m_CurrentEnemyIndex = 0;


	public Action OnStateFinished { get; set; } = delegate { };

	public EnemyTurn(Transform enemyParent)
	{
		m_EnemySpawner = GameObject.FindObjectOfType<EnemySpawn>();
		Assert.IsNotNull(m_EnemySpawner, "No Enemy spawner found");
		EnemyFinished = HandleNextEnemy;
		m_EnemyParent = enemyParent;
		m_Enemies = new EnemyController[0];
	}

	private void HandleNextEnemy()
	{
		if (m_CurrentEnemyIndex >= m_Enemies.Length)
		{
			OnStateFinished.Invoke();
		}
		else
		{
			EnemyController Enemy = m_Enemies[m_CurrentEnemyIndex];
			m_CurrentEnemyIndex++;
			//Enemy.DoEnemyTurn();
		}
	}

	public void Enter()
	{
		m_Enemies = m_EnemyParent.GetComponentsInChildren<EnemyController>();
		if(m_Enemies.Length == 0)
		{
			m_EnemySpawner.SpawnEnemies();
			m_Enemies = m_EnemyParent.GetComponentsInChildren<EnemyController>();
			if(m_Enemies.Length == 0)
			{
				Debug.LogError("unhandled situation, no enemies after spawn was called");
			}
		}
		m_CurrentEnemyIndex = 0;
		HandleNextEnemy();
	}

	public void Tick()
	{
		
	}

	public void Exit()
	{
		Debug.Log("EnemyTurn::Exit");
	}
}