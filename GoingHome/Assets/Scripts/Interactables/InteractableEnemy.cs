using UnityEngine;

public class InteractableEnemy : InteractableBase
{
	private IDamagable m_Damagable = null;

	private void Start()
	{
		m_Damagable = GetComponent<IDamagable>();
	}

	// Attack enemy
	protected override void Button1Clicked()
	{
		m_Interactor.GetComponent<Combat>().Attack(m_Damagable, transform.position);
		//AudioManager.Instance?.Play("EnemyHit"); //play SFX from AudioManager
		//AudioManager.Instance?.Play("EnemyHitScream"); //play SFX from AudioManager
		Deselected();
	}

	//Close menu
	protected override void Button2Clicked()
	{
		Deselected();
	}
}
