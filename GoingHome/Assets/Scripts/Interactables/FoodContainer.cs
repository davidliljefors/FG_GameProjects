using UnityEngine;
using UnityEngine.Assertions;

public class FoodContainer : InteractableBase
{
	public int foodValue;
	[SerializeField] private GameObject m_ObjectToDestroy;

	private GameObject m_playerCharacters;
	private FoodSupplyScript m_inventory;

	private void Start()
	{
		m_playerCharacters = GameObject.Find("PlayerCharacters");
		m_inventory = m_playerCharacters.GetComponent<FoodSupplyScript>();
		Assert.IsNotNull(m_ObjectToDestroy);
	}

	protected override void Button1Clicked()
	{
		//If space in food inventory
		if (m_inventory.currentFood < m_inventory.maxFood)
		{
			AudioManager.Instance?.Play("PickupFood"); //play SFX from AudioManager
			m_inventory.AddFood(foodValue);
			Destroy(m_ObjectToDestroy);
			Deselected();
			gameObject.tag = "Untagged";
		}
		else
		{
			//Inactivate button?
		}
	}

	protected override void Button2Clicked()
	{
		Deselected();
	}
}
