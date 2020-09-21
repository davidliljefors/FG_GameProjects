using UnityEngine.Assertions;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Configuration;
using UnityEditor;

[System.Serializable]
public struct ContextMenuTask
{
	[SerializeField] private int m_Cost;
	[SerializeField] private Sprite m_Icon;
	[SerializeField] private Sprite m_IconHover;
	[SerializeField] private Sprite m_IconPressed;

	public Sprite Icon { get => m_Icon; set => m_Icon = value; }
	public Sprite IconHover { get => m_IconHover; set => m_IconHover = value; }
	public Sprite IconPressed { get => m_IconPressed; set => m_IconPressed = value; }
	public Action Action { get; set; }
	public bool Enabled { get ; set ; }
	public int Cost { get => m_Cost; private set => m_Cost = value; }
}

/// <summary>
/// Context menu handles the rendering of clickable buttons around interactables in the world
/// </summary>

public class ContextMenu : MonoBehaviour
{
	private Animator m_Animator;
	private Transform m_CameraTransform = default;

	private Button[] m_Buttons;
	private Image[] m_ButtonImages;
	private int m_AnimationStateHash = 0;

	private void Awake()
	{
		m_AnimationStateHash = Animator.StringToHash("ShowButtonMenu");
		m_CameraTransform = Camera.main.transform;
		m_Animator = GetComponent<Animator>();
		m_Buttons = GetComponentsInChildren<Button>();
		m_ButtonImages = new Image[m_Buttons.Length];
		for(int i = 0; i<m_Buttons.Length; i++)
		{
			m_ButtonImages[i] = m_Buttons[i].GetComponent<Image>();
			m_Buttons[i].gameObject.SetActive(false);
		}
		foreach(Button b in m_Buttons)
		{ 
			b.gameObject.SetActive(false);
			
		}

		Assert.IsNotNull(m_CameraTransform, "Cant find main camera");
		Assert.IsNotNull(m_Buttons, "Cant find any buttons");
		Assert.IsNotNull(m_Animator, "Cant find animation");
	}

	private void Update()
	{
		transform.rotation = Quaternion.LookRotation(transform.position - m_CameraTransform.transform.position);
	}
	/// <summary>
	/// Opens the menu with the provided ContextMenuItems
	/// </summary>
	public void OpenMenu(ContextMenuTask[] menuItems, Vector3 position, ICharacter openedBy)
	{
		for(int i = 0; i< menuItems.Length; i++)
		{
			if(menuItems[i].Enabled)
			{
				m_ButtonImages[i].sprite = menuItems[i].Icon;
				SetSprites(m_Buttons[i], menuItems[i].Icon, menuItems[i].IconPressed, menuItems[i].IconHover);
				m_Buttons[i].image.SetNativeSize();
				var index = i;
				m_Buttons[i].onClick.AddListener(() =>
				{
					openedBy.PerformInteraction(menuItems[index].Action, menuItems[index].Cost);
				});
				m_Buttons[i].gameObject.SetActive(true);
			}
		}
		m_Animator.Play(m_AnimationStateHash, 0, 0.0f);
		transform.position = position;
		//Invoke("EnableButtons", 0.2f);
	}

	private void SetSprites(Button b, Sprite normal, Sprite pressed, Sprite hover)
	{
		var state = b.spriteState;
		b.image.sprite = normal;
		state.highlightedSprite = hover;
		state.pressedSprite = pressed;
		b.spriteState = state;
	}

	private void EnableButtons()
    {
		foreach(Button b in m_Buttons)
        {
			b.interactable = true;
        }
    }

	/// <summary>
	/// Close the menu
	/// </summary>
	public void CloseMenu()
	{
		foreach (Button b in m_Buttons)
		{
			b.onClick.RemoveAllListeners();
			b.gameObject.SetActive(false);
			//b.interactable = false;
		}
	}

	/// <summary>
	/// Enable a button by ID
	/// </summary>
	public void EnableButton(int id)
	{
		if (id < m_Buttons.Length && id >= 0)
		{
			m_Buttons[id].gameObject.SetActive(true);
		}
	}

	/// <summary>
	/// Disable a button by ID
	/// </summary>
	public void DisableButton(int id)
	{ 
		if(id < m_Buttons.Length && id >= 0)
		{
			m_Buttons[id].gameObject.SetActive(false);
		}
	}
}
