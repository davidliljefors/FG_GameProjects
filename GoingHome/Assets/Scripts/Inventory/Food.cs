using System;
using UnityEngine;

[System.Serializable]
public class Food  //IItem
{
	[SerializeField] private Sprite m_Icon = null;
	[SerializeField] private string m_Name = "NoName";
	[SerializeField] private int m_Nutrition = 1;

	public ItemType Type { get; private set; } = ItemType.Food;
	public Sprite Icon { get => m_Icon; }
	public int Count { get; set; } = 4;
	public string Name { get => m_Name; }

	public void OnClicked()
	{
		Debug.Log("Crunched a " + Name + " giving " + m_Nutrition + " nutrition");
		//Component.FindObjectOfType<DavidInventory>().RemoveItem(this, 1);
	}
}
