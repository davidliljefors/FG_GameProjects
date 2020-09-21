using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{

    [SerializeField] private GameObject m_PlayerParent = null;

	[SerializeField] private GameObject[] m_CharactersUI = null;

	private TurnManager m_TurnManager = null;

	[SerializeField] private GameObject buttonPress;

    private void Start()
    {
		Assert.IsNotNull(m_PlayerParent, "Assign PlayerCharacters reference to PlayerHUD");
		Assert.IsNotNull(m_CharactersUI, "Assign PlayerHUD objects to PlayerHUD script");
		m_TurnManager = FindObjectOfType<TurnManager>();
		m_TurnManager.OnTurnDataChanged += Redraw;
		Redraw();
	}
	private void OnDisable()
	{
		m_TurnManager.OnTurnDataChanged -= Redraw;
	}

	private void Redraw()
    {
		//foreach (var go in m_CharactersUI)
		//{
		//	go.SetActive(false);
		//}

		for (int i = 0; i < m_PlayerParent.transform.childCount; i++)
		{
			m_CharactersUI[i].SetActive(true);
		}
	}

	public void OnButtonPress()
	{
		buttonPress.SetActive(true);
	}

	public void OnButtonRelease()
	{
		buttonPress.SetActive(false);
	}
}
