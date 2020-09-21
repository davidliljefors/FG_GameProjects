using UnityEngine.Assertions;
using UnityEngine;

/// <summary>
/// Base class for interactables,
/// Add Sprites in inspector (max 4)
/// For each button sprite, override ButtonNClicked where N is the button 1-4
/// 
/// </summary>
[RequireComponent(typeof(Highlightable))]
public abstract class InteractableBase : MonoBehaviour, IInteractable
{
	[SerializeField] protected ContextMenuTask[] m_MenuTasks = null;
	protected ContextMenu m_ContextMenu;
	protected GameObject m_Interactor = null;

	public bool InRange { get; set; }


	private const int k_MaxButtons = 4;
	protected virtual void Button1Clicked() { Debug.LogError("Button1Clicked() not implemented"); }
	protected virtual void Button2Clicked() { Debug.LogError("Button2Clicked() not implemented"); }
	protected virtual void Button3Clicked() { Debug.LogError("Button3Clicked() not implemented"); }
	protected virtual void Button4Clicked() { Debug.LogError("Button4Clicked() not implemented"); }


	protected virtual void Awake()
	{
		Assert.IsTrue(k_MaxButtons >= m_MenuTasks.Length, "Current max is 4 buttons, remove a few sprites or increase increase max buttons");
		m_ContextMenu = FindObjectOfType<ContextMenu>();
		Assert.IsTrue(m_MenuTasks.Length > 0, "No actions found");
		for (int i = 0; i < m_MenuTasks.Length; i++)
		{
			m_MenuTasks[i].Enabled = true;
		}
		SetupInteractionButtons();
	}

	protected void SetupInteractionButtons()
	{
		int index = 0;
		if (m_MenuTasks.Length > 0)
		{ m_MenuTasks[index].Action = Button1Clicked; index++; }
		if (m_MenuTasks.Length > 1)
		{ m_MenuTasks[index].Action = Button2Clicked; index++; }
		if (m_MenuTasks.Length > 2)
		{ m_MenuTasks[index].Action = Button3Clicked; index++; }
		if (m_MenuTasks.Length > 3)
		{ m_MenuTasks[index].Action = Button4Clicked; index++; }
	}

	public virtual void Clicked(GameObject clickedBy)
	{
		m_Interactor = clickedBy;
		m_ContextMenu.OpenMenu(m_MenuTasks, transform.position, clickedBy.GetComponent<ICharacter>());
	}

	public virtual void Deselected()
	{
		m_Interactor = null;
		m_ContextMenu.CloseMenu();
	}

	protected void EnableButton(int id)
	{
		m_ContextMenu.EnableButton(id);
		m_MenuTasks[id].Enabled = true;
	}

	protected void DisableButton(int id)
	{
		m_ContextMenu.DisableButton(id);
		m_MenuTasks[id].Enabled = false;
	}
}
