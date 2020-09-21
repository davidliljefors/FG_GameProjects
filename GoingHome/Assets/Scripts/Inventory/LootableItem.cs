using UnityEngine;

public class LootableItem : InteractableBase
{
	[SerializeField] private ItemSO m_ItemSO;

	protected override void Button1Clicked()
	{
		m_Interactor.GetComponent<ICharacter>().CarriedItem = m_ItemSO.CreateItem();
		AudioManager.Instance?.Play("EquipWeapon"); //play SFX from AudioManager
		Deselected();
		Destroy(gameObject);
	}

	protected override void Button2Clicked()
	{
		Deselected();
	}
}
