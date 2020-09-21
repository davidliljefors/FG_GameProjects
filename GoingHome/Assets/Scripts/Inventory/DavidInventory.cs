using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


/// <summary>
/// Prototype inventory
/// </summary>
public class DavidInventory : MonoBehaviour
{
	private const int k_InventorySize = 12;

	[SerializeField] private Transform m_InventoryPanel = null;

	private List<Item> m_InventoryItems;
	private List<Image> m_Images;
	private List<GameObject> m_Slots;
	private List<Button> m_Buttons;

	private void Awake()
	{
		m_InventoryItems = new List<Item>();
		m_Slots = new List<GameObject>();
		m_Images = new List<Image>();
		m_Buttons = new List<Button>();
		Assert.IsNotNull(m_InventoryPanel, "Assign inventory panel");

		foreach (Transform child in m_InventoryPanel)
		{
			m_Buttons.Add(child.GetComponent<Button>());
			m_Slots.Add(child.gameObject);
			m_Images.Add(child.GetComponentsInChildren<Image>()[1]);
			child.gameObject.SetActive(false);
		}
		m_InventoryPanel.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			ToggleInventory();
		}
	}
	/// <summary>
	/// Add item to the inventory, increase count if its already there 
	/// Returns false if full
	/// </summary>
	/// <param name="item"> The item to be added</param>
	/// <returns> 
	/// false if the inventory is full 
	/// </returns>
	public bool AddItem(Item item)
	{
		int indexOfItem = m_InventoryItems.FindIndex(x => x.Icon == item.Icon);

		if (indexOfItem >= 0) // IF not found index is -1 (C# standard)
		{
			m_InventoryItems[indexOfItem].Count += item.Count;
			UpdateInventoryIcon(indexOfItem);
			return true;
		}

		if (m_InventoryItems.Count < k_InventorySize)
		{
			m_InventoryItems.Add(item);
			int newIndex = m_InventoryItems.Count - 1;
			RefreshInventoryButtons();
			return true;
		}
		return false;
	}
	/// <summary>
	/// Remove item from from inventory.
	/// Returns false if not enough items
	/// </summary>
	public bool RemoveItem(Item item)
	{
		int indexOfItem = m_InventoryItems.FindIndex(x => x.Icon == item.Icon);
		if (indexOfItem >= 0) // IF not found index is -1 (C# standard)
		{
			m_InventoryItems[indexOfItem] = null;
			for (int i = indexOfItem; i < m_InventoryItems.Count - 1; i++)
			{
				m_InventoryItems[i] = m_InventoryItems[i + 1];
			}
			m_Slots[m_InventoryItems.Count - 1].GetComponent<Button>().onClick.RemoveAllListeners();
			m_InventoryItems.RemoveAt(m_InventoryItems.Count - 1);
			RefreshInventoryButtons();
			return true;
		}
		return false;
	}
	/// <summary>
	/// Remove item from from inventory.
	/// Returns false if not enough items
	/// </summary>
	public bool RemoveItem(Item item, int amount)
	{
		int indexOfItem = m_InventoryItems.FindIndex(x => x.Icon == item.Icon);
		if (indexOfItem >= 0) // IF not found index is -1 (C# standard)
		{
			if(m_InventoryItems[indexOfItem].Count > amount)
			{
				m_InventoryItems[indexOfItem].Count -= amount;
				UpdateInventoryIcon(indexOfItem);
				return true;
			}

			if (m_InventoryItems[indexOfItem].Count == amount)
			{
				return RemoveItem(item);
			}
		}
		return false;
	}

	private void RefreshInventoryButtons()
	{
		if(m_InventoryPanel.gameObject.activeSelf)
		{
			foreach (GameObject go in m_Slots)
			{

				go.SetActive(false);
			}
			foreach (Button b in m_Buttons)
			{
				b.onClick.RemoveAllListeners();
			}

			for (int i = 0; i < m_InventoryItems.Count; i++)
			{
				//m_Buttons[i].onClick.AddListener(m_InventoryItems[i].OnClicked);
				UpdateInventoryIcon(i);
			};
		}
	}

	private void UpdateInventoryIcon(int index)
	{
		m_Images[index].sprite = m_InventoryItems[index].Icon;
		m_Slots[index].GetComponentInChildren<Text>().text = "x" + m_InventoryItems[index].Count;
		m_Slots[index].SetActive(true);
	}

	private void ToggleInventory()
	{
		m_InventoryPanel.gameObject.SetActive(!m_InventoryPanel.gameObject.activeInHierarchy);
		RefreshInventoryButtons();
	}
}
