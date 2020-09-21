using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
	Unspecified,
	Food,
	Weapon
}

public enum WeaponAttackStyle
{
	Unspecified,
	Lunge,
	Slash,
	Crush
}

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemSO : ScriptableObject
{
	public GameObject Prefab;
	public string Name;
	public Sprite Icon;
	public ItemType Type;
	public WeaponAttackStyle AttackStyle;
	public int Damage;

	public Item CreateItem()
	{
		return new Item(this);
	}
}

public static class ItemTools
{
	public static readonly Dictionary<WeaponAttackStyle, int> AnimationHashByType =
		new Dictionary<WeaponAttackStyle, int>
	{
		{WeaponAttackStyle.Lunge , Animator.StringToHash("Lunge") },
		{WeaponAttackStyle.Slash , Animator.StringToHash("Slash") },
		{WeaponAttackStyle.Crush , Animator.StringToHash("Crush") }
	};
}
