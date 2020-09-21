using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMenu : MonoBehaviour
{

	[SerializeField] private KeyCode m_DebugKey = default;
	[SerializeField] private GameObject m_DebugMenu = null;

	private void Awake()
	{
		DontDestroyOnLoad(this);
	}

	private void Update()
	{
		if(Input.GetKeyDown(m_DebugKey))
		{
			m_DebugMenu.SetActive(!m_DebugMenu.activeSelf);
		}
	}

	public void LoadLevel1()
	{
		SceneManager.LoadScene("Level_1");
	}

	public void LoadLevel2()
	{
		SceneManager.LoadScene("Level_2");
	}
	public void LoadLevel3()
	{
		SceneManager.LoadScene("Level_3");
	}

	public void LoadLevel4()
	{
		SceneManager.LoadScene("Level_4A");
	}

	public void LoadLevel5()
	{
		SceneManager.LoadScene("Level_5");
	}

	public void ForceTurn()
	{
		var obj = FindObjectOfType<TurnManager>();
		if(obj != null)
		{
			obj.ForceNextTurn();
		}
	}

	public void SetFreeMoves()
	{
		TurnManager.s_FreeMoveEnabled = true;
	}

}
