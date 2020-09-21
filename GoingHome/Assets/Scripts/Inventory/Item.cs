using UnityEngine;

public class Item
{
	private ItemSO m_ItemSO = null;
	public int Count { get; set; }

	public Sprite Icon { get => m_ItemSO.Icon; }
	public GameObject Prefab{ get => m_ItemSO.Prefab; }
	public ItemType Type { get => m_ItemSO.Type; }
	public WeaponAttackStyle AttackStyle { get => m_ItemSO.AttackStyle; }
	public int Damage { get
		{
			if (Type == ItemType.Weapon)
			{
				return m_ItemSO.Damage;
			}
			else
			{
				Debug.LogError("Cant attack with a non weapon");
				return 0;
			}
		}
	}


	public Item(ItemSO itemSO)
	{
		m_ItemSO = itemSO;
		Count = 1;
	}

	public Item(ItemSO itemSO, int count)
	{
		m_ItemSO = itemSO;
		Count = count;
	}

}
