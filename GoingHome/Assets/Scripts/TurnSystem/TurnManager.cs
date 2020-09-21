using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class TurnManager : MonoBehaviour
{
	private const string k_PlayerTag = "Player";
	private const string k_EnemyTag = "Enemy";

	[Header("GAME OVER")]
	[SerializeField] private GameObject gameOverScreen = null;
	[SerializeField] private Animator gameOverAnim = null;
	[Header("Buttons")]
	[SerializeField] private GameObject startRound = null;
	[SerializeField] private GameObject endTurn = null;
	[Header("Camera Animations")]
	[SerializeField] private Animator focalPointPrefab = null;

	[SerializeField] private GameObject turnOrderUiMainPrefab = null;

	private int m_AliveEnemies = 0;
	private bool m_PlayerDied = false;

	public static Action OnGameOver				= delegate { Debug.LogError("Turn Manager not active"); };
	public static Action<GameObject> OnSpawned	= delegate { Debug.LogError("Turn Manager not active"); };
	public static Action<GameObject> OnKilled	= delegate { Debug.LogError("Turn Manager not active"); };

	public event Action OnTurnDataChanged;
	public static bool s_FreeMoveEnabled = true;

	public int CurrentTurn { get; set; } = -1;
	public List<GameObject> AliveCharacters { get; set; }

    public int AliveEnemies { get => m_AliveEnemies; 
		set { m_AliveEnemies = value; s_FreeMoveEnabled = m_AliveEnemies == 0; } }

    public int AlivePlayers { get; set; } = 0;

    bool started = false;
	public void Begin()
	{
		focalPointPrefab.SetTrigger("ZoomOut");
		startRound.SetActive(false);
		turnOrderUiMainPrefab.SetActive(true);
		endTurn.SetActive(true);

		if (!started)
		{
			NextTurn();
			started = true;
		}
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Begin();
		}
        if (Input.GetKeyDown(KeyCode.F4))
        {
			s_FreeMoveEnabled = true;
        }
    }


	private void Awake()
	{
		focalPointPrefab.SetTrigger("Start");
		AliveCharacters = new List<GameObject>();
		OnSpawned = OnSpawnedImpl;
		OnKilled = OnKilledImpl;
		OnGameOver = OnGameOverImpl;
		m_PlayerDied = false;
	}

	public void EndTurn()
	{
		Begin();
	}

	public void ForceNextTurn()
	{
		if(AliveCharacters[CurrentTurn] != null)
		{
			AliveCharacters[CurrentTurn].GetComponent<ITurnPawn>().ForceEndTurn();
		}
		else
		{
			NextTurn();
		}
		
	}

	private void OnSpawnedImpl(GameObject spawned)
	{
		if (spawned.CompareTag(k_PlayerTag))
		{
			AlivePlayers++;
		}
        else if(spawned.CompareTag(k_EnemyTag))
        {
			AliveEnemies++;
		}

		if(AliveCharacters.Count == 0)
		{
			AliveCharacters.Add(spawned);
			return;
		}

		AliveCharacters.AddRandom(spawned);

		if (CurrentTurn >= AliveCharacters.FindIndex(x => x == spawned))
		{
			CurrentTurn += 1;
		}

		OnTurnDataChanged.Invoke();
	}

	private void OnKilledImpl(GameObject killed)
	{
		if(killed == AliveCharacters[CurrentTurn])
		{
			Debug.LogError("Current turn got killed!!!!, Lets hope it works :pray:");
			return;
		}

		if (CurrentTurn >= AliveCharacters.FindIndex(x => x == killed))
		{
			CurrentTurn -= 1;
		}
		AliveCharacters.Remove(killed);

		if(killed.CompareTag(k_PlayerTag))
		{
			AlivePlayers--;
			if(AlivePlayers <= 0)
			{
				OnGameOverImpl();
			}
		}
		else if(killed.CompareTag(k_EnemyTag))
        {
			AliveEnemies--;
        }
		OnTurnDataChanged.Invoke();
	}

	
	private void NextTurn()
	{
		CurrentTurn += 1;
		CurrentTurn = CurrentTurn % (AliveCharacters.Count);
		OnTurnDataChanged.Invoke();

		AliveCharacters[CurrentTurn].GetComponent<ITurnPawn>().OnFinished += OnTurnFinished;
		AliveCharacters[CurrentTurn].GetComponent<ITurnPawn>().BeginTurn();
	}

	private void OnTurnFinished()
	{
		AliveCharacters[CurrentTurn].GetComponent<ITurnPawn>().OnFinished -= OnTurnFinished;
		Debug.Log("Turn finished");
		if(!m_PlayerDied)
		{
			Invoke("NextTurn", 0.5f);
		}
	}


	private void OnGameOverImpl()
	{
		m_PlayerDied = true;
		gameOverScreen.SetActive(true);
		gameOverAnim.SetTrigger("Fade");
	}
}

public static class IListExtensions
{
	/// <summary>
	/// Add element at random position
	/// </summary>
	public static void AddRandom<T>(this IList<T> ts, T item, int start = 0)
	{
		if (ts.Count == 0)
		{
			ts.Add(item);
			return;
		}
		var r = UnityEngine.Random.Range(0, ts.Count + 1);
		ts.Insert(r, item);
	}

	/// <summary>
	/// Shuffles the element order of the specified list.
	/// </summary>
	public static void Shuffle<T>(this IList<T> ts)
	{
		var count = ts.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i)
		{
			var r = UnityEngine.Random.Range(i, count);
			var tmp = ts[i];
			ts[i] = ts[r];
			ts[r] = tmp;
		}
	}
}
