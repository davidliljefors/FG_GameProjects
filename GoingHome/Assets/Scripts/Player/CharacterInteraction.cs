using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

/// <summary>
/// Handles the character interaction, with itself and the interactables withing its reach
/// </summary>
public class CharacterInteraction : MonoBehaviour, ICharacter
{
	private const string k_EnemyTag = "Enemy";

	[SerializeField] private float m_AttackTime = 1.2f;
	[SerializeField] private float m_InteractTime = 0.7f;
	[SerializeField] private GameObject m_WeaponMesh = null;
	private Item m_Item = null;
	private PlayerController m_PController = null;
	private GameObject m_SelectedInteractable = null;

	public bool HasFocus { get => m_SelectedInteractable != null; }

	#region ICharacter Implementation

	public int MovesLeft { get; set; } = 0;

	public Item CarriedItem
	{
		get => m_Item;
		set
		{
			if (value.Type == ItemType.Weapon)
			{
				m_WeaponMesh.SetActive(true);
				m_Item = value;
			}
			//Todo drop or something with old item!!
		}
	}

	public void PerformInteraction(Action action, int cost)
	{
		if(cost > 0)
		{
			if(m_SelectedInteractable.CompareTag(k_EnemyTag))
			{
				GetComponent<PlayerController>().Suspend(m_AttackTime);
			}
			else
			{
				GetComponent<PlayerController>().Suspend(m_InteractTime);
			}
		}

		GetComponent<PlayerController>().MovesLeft -= cost;
		action.Invoke();
		Close();
	}

	public void Clicked(GameObject clicked)
    {
		m_SelectedInteractable = clicked;
        m_SelectedInteractable.GetComponent<IInteractable>().Clicked(gameObject);
    }

	public void Close()
	{
		if(m_SelectedInteractable != null)
		{
			m_SelectedInteractable.GetComponent<IInteractable>().Deselected();
		}
		m_SelectedInteractable = null;
	}
	#endregion ICharacter Implementation


	private void Awake()
	{
		m_PController = GetComponent<PlayerController>();
		Assert.IsNotNull(m_PController, "PlayerController not found on player");
	}
}
