using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractableExitScene : InteractableBase
{
	private Transform m_characterParent;
	private int m_foodToRemove;
	private bool m_enoughFood;
	//private bool m_allMiceNear;
	private Animator anim;

	public GameObject foodReqText;
	//public GameObject miceReqText;
	public float distanceForLeave = 3;
	public GameObject worldMap;
	public GameObject LeaveReqCanvas;
	private Transform m_CameraTransform = default;

	private void Start()
	{
		m_characterParent = GameObject.Find("PlayerCharacters").transform;
		m_CameraTransform = Camera.main.transform;
		anim = LeaveReqCanvas.GetComponent<Animator>();

		
		LeaveReqCanvas.SetActive(false);
	}

	private void Update()
	{
		LeaveReqCanvas.transform.rotation = Quaternion.LookRotation(transform.position - m_CameraTransform.transform.position);
	}

	protected override void Button1Clicked()
	{
		m_foodToRemove = 3;
		int currentFood = m_characterParent.gameObject.GetComponent<FoodSupplyScript>().currentFood;
		//for (int i = 0; i < m_characterParent.childCount; i++)
		//{
		//	if (m_characterParent.GetChild(i).gameObject.activeInHierarchy)
		//	{
		//		m_foodToRemove++;
		//	}
		//}
		//Checks if enough food
		if (m_foodToRemove <= currentFood)
		{

			//m_characterParent.GetComponent<FoodSupplyScript>().RemoveFood(m_foodToRemove);
			foodReqText.SetActive(false);
			m_enoughFood = true;
		}
		else
		{
			m_enoughFood = false;
			foodReqText.SetActive(true);
			foodReqText.GetComponent<TextMeshProUGUI>().text = "You haven't gathered enough food! (" + currentFood + "/" + m_foodToRemove + ")";
		}
		//int m_miceInRange = 0;
		//foreach (Transform mouse in m_characterParent)
		//{
		//	if (Vector3.Distance(transform.position, mouse.transform.position) < distanceForLeave)
		//	{
		//		m_miceInRange++;
		//	}
		//	else
		//	{
		//		m_miceInRange--;
		//	}
		//}

		//if(m_miceInRange == m_characterParent.childCount)
		//{
		//	m_allMiceNear = true;
		//	miceReqText.SetActive(false);
		//}
		//else
		//{
		//	m_allMiceNear = false;
		//	miceReqText.SetActive(true);
		//}


		if (m_enoughFood)
		{
			worldMap.SetActive(true);
			LeaveReqCanvas.SetActive(false);
		}
		else
		{
			LeaveReqCanvas.transform.GetChild(0).gameObject.SetActive(true);
			LeaveReqCanvas.SetActive(true);
			anim.SetTrigger("StartFade");
			StartCoroutine(SetInactive());
		}
		Deselected();
	}

	private IEnumerator SetInactive()
	{
		yield return new WaitForSeconds(3f);
		LeaveReqCanvas.SetActive(false);
	}

	protected override void Button2Clicked()
	{
		Deselected();
	}
}
