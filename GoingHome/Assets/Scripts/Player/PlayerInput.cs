using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

/// <summary>
/// Class that handles all the player input during the game scene
/// </summary>
public class PlayerInput : MonoBehaviour
{
	private const string k_InteractableTag = "Interactable";
	private const string k_PlayerTag = "Player";
	private const string k_GroundTag = "Ground";
	private const string k_EnemyTag = "Enemy";

	[SerializeField] private LayerMask m_NonInteractableLayer = default;
	[SerializeField] private GameObject m_UnderPlayer;
	[SerializeField] private Button m_EndTurnButton = null;

	[Header("Cursors")]
	[SerializeField] private Texture2D m_DefaultCursor;
	[SerializeField] private Texture2D m_DefaultCursorPressed;
	[SerializeField] private Texture2D m_WalkCursor;
	[SerializeField] private Texture2D m_GrabCursor;
	[SerializeField] private Texture2D m_HandCursor;
	[SerializeField] private Texture2D m_AttackCursor;

	private IInteractable m_SelectedInteractable = null;
	private Ray m_CameraToMouseRay;
	private Camera m_Camera;

	private Highlightable m_LastHighlight = null;
	private Quaternion m_UnderPlayerRotation = Quaternion.identity;

	public GameObject SelectedCharacter { get; set; }
	private PlayerController m_PController = null;

	// DoubleClick fields
	[Header("")]
	[SerializeField] private bool m_UseDoubleClick = false;
	private Vector3 m_LastClickPos = default;
	private float m_LastClickTime = 0f;
	private float m_TimeBetweenClicks = 0.2f;

	/// <summary>
	/// Invoked from PlayerTurn
	/// </summary>
	public static Action<GameObject> OnBecomeCharactersTurn = delegate { };

	private void Awake()
	{
		//m_UnderPlayer.SetActive(false);
		Assert.IsNotNull(m_EndTurnButton, "No reference set to EndTurnButton");
		m_UnderPlayerRotation = m_UnderPlayer.transform.rotation;
		m_UnderPlayer = Instantiate(m_UnderPlayer);
		m_UnderPlayer.SetActive(false);
		//
	}
	void Start()
	{
		m_Camera = Camera.main;
		OnBecomeCharactersTurn = OnCharactersTurn;
	}

	private void OnCharactersTurn(GameObject character)
	{
		if (character == null)
		{
			m_PController = null;
			m_UnderPlayer.SetActive(false);
			m_UnderPlayer.transform.parent = null;
			m_EndTurnButton.interactable = false;
		}

		if (SelectedCharacter != null)
		{
			SelectedCharacter.GetComponent<Highlightable>().Highlight(false);
		}

		SelectedCharacter = character;

		if (character != null)
		{
			m_EndTurnButton.interactable = true;
			m_UnderPlayer.SetActive(true);
			m_UnderPlayer.transform.parent = SelectedCharacter.transform;
			m_UnderPlayer.transform.localPosition = Vector3.zero + Vector3.up * 0.1f;
			m_PController = character.GetComponent<PlayerController>();
		}
	}

	private void LateUpdate()
	{
		if (m_UnderPlayer != null)
		{
			m_UnderPlayer.transform.rotation = m_UnderPlayerRotation;
		}
	}

	public void ForceEndTurn()
	{
		if (m_PController != null)
		{
			m_PController.MovesLeft = int.MinValue;
		}
		else
		{
			Debug.LogError("Tried to end turn but had nothing to end turn on");
		}
	}


	void Update()
	{
		m_CameraToMouseRay = m_Camera.ScreenPointToRay(Input.mousePosition);

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			m_SelectedInteractable?.Deselected();
			m_SelectedInteractable = null;
		}

		if (m_LastHighlight)
		{ m_LastHighlight.Highlight(false); }
		m_LastHighlight = null;


		if (Physics.Raycast(m_CameraToMouseRay, out RaycastHit hit1, 1000f))
		{
			UpdateCursor(hit1.collider.gameObject);
			if (SelectedCharacter != null && m_PController != null)
			{
				m_PController.MouseOver(hit1.point, hit1.transform.gameObject);
			}

			if ((hit1.collider.CompareTag(k_InteractableTag) || hit1.collider.CompareTag(k_EnemyTag)) && hit1.transform.gameObject.layer != m_NonInteractableLayer)
			{
				m_LastHighlight = hit1.collider.GetComponent<Highlightable>();
				if (m_LastHighlight)
				{
					m_LastHighlight.Highlight(true);
				}
			}

		}


		if (Input.GetMouseButtonDown(0))
		{
			if (m_UseDoubleClick)
			{
				if (m_LastClickTime + m_TimeBetweenClicks >= Time.time && Input.mousePosition == m_LastClickPos)
				{
					if (SelectedCharacter != null && !EventSystem.current.IsPointerOverGameObject())
					{
						if (Physics.Raycast(m_CameraToMouseRay, out RaycastHit hit2, 1000f))
						{
							if (m_PController)
							{
								m_PController.MouseClicked(hit2.point, hit2.transform.gameObject);
							}
						}
					}
				}
				else
				{
					m_LastClickTime = Time.time;
					m_LastClickPos = Input.mousePosition;
				}
			}
			else
			{
				if (SelectedCharacter != null && !EventSystem.current.IsPointerOverGameObject())
				{
					if (Physics.Raycast(m_CameraToMouseRay, out RaycastHit hit2, 1000f))
					{
						if (m_PController)
						{
							m_PController.MouseClicked(hit2.point, hit2.transform.gameObject);
						}
					}
				}
			}
		}
	}

	private void UpdateCursor(GameObject mouseOver)
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			if (Input.GetMouseButton(0))
			{
				Cursor.SetCursor(m_DefaultCursorPressed, new Vector2(20, 20), CursorMode.ForceSoftware);
				return;
			}
			else
			{
				Cursor.SetCursor(m_DefaultCursor, Vector2.zero, CursorMode.ForceSoftware);
				return;
			}
		}

		if (mouseOver.CompareTag(k_InteractableTag))
		{
			if (Input.GetMouseButton(0))
			{
				Cursor.SetCursor(m_GrabCursor, new Vector2(20, 20), CursorMode.ForceSoftware);
			}
			else
			{
				Cursor.SetCursor(m_HandCursor, new Vector2(20, 20), CursorMode.ForceSoftware);
			}
			return;
		}
		if (mouseOver.CompareTag(k_EnemyTag))
		{
			Cursor.SetCursor(m_AttackCursor, new Vector2(20, 20), CursorMode.ForceSoftware);
			return;
		}

		if (SelectedCharacter != null)
		{
			if (mouseOver.CompareTag(k_GroundTag))
			{
				Cursor.SetCursor(m_WalkCursor, Vector2.zero, CursorMode.ForceSoftware);
				return;
			}
		}


		if (Input.GetMouseButton(0))
		{
			Cursor.SetCursor(m_DefaultCursorPressed, new Vector2(20, 20), CursorMode.ForceSoftware);
			return;
		}
		else
		{
			Cursor.SetCursor(m_DefaultCursor, Vector2.zero, CursorMode.ForceSoftware);
			return;
		}

	}
}

