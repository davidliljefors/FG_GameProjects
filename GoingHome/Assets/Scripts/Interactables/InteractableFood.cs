using UnityEngine;

public class InteractableFood : InteractableBase
{
	public int foodValue;
	//private GameObject m_player;
	private GameObject m_playerCharacters;
	private FoodSupplyScript m_inventory;

	private void Start()
	{
		//m_player = GameObject.FindGameObjectWithTag("Player");

		m_playerCharacters = GameObject.Find("PlayerCharacters");
		m_inventory = m_playerCharacters.GetComponent<FoodSupplyScript>();
	}

	protected override void Button1Clicked()
	{
		//If space in food inventory
		if(m_inventory.currentFood < m_inventory.maxFood)
		{
			AudioManager.Instance?.Play("PickupFood"); //play SFX from AudioManager
			m_inventory.AddFood(foodValue);
			Destroy(gameObject);
			Deselected();
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
