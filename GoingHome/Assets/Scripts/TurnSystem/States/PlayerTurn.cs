using System;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerTurn : ITurn
{
	private const int k_MaxCharacters = 4;

	public static Action CharacterFinished { get; private set; } = delegate { };
	public Action OnStateFinished { get; set; }

	private Transform m_PlayerParent;
	private GameObject[] m_Characters;
	private int m_CurrentCharacterIndex = 0;

	public PlayerTurn(Transform playerParent)
	{
		CharacterFinished = HandleNextCharacter;
		m_PlayerParent = playerParent;
		m_Characters = new GameObject[k_MaxCharacters];

		Assert.IsNotNull(m_PlayerParent, "Cant find player root object, check name");
	}

	private void HandleNextCharacter()
	{
		if (m_CurrentCharacterIndex >= m_PlayerParent.childCount)
		{
			PlayerInput.OnBecomeCharactersTurn(null);
			OnStateFinished.Invoke();
		}
		else
		{
			//m_Characters[m_CurrentCharacterIndex].GetComponent<ICharacter>().MovesLeft = 2;
			PlayerInput.OnBecomeCharactersTurn(m_Characters[m_CurrentCharacterIndex]);
			m_CurrentCharacterIndex++;
		}
	}

	public void Enter()
	{
		if (m_PlayerParent.childCount <= 0)
		{
			TurnManager.OnGameOver.Invoke();
		}

		{
			int i = 0;
			foreach (Transform child in m_PlayerParent.transform)
			{
				m_Characters[i] = child.gameObject;
				i++;
			}
			
		}
		m_CurrentCharacterIndex = 0;

		HandleNextCharacter();

		Debug.Log("PlayerTurn::Enter");
	}

	public void Tick()
	{

	}

	public void Exit()
	{
		Debug.Log("PlayerTurn::Exit");
	}
}