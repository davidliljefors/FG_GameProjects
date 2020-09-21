using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class Combat : MonoBehaviour
{
	[SerializeField] private int m_Damage = 1;
	[SerializeField] private AudioClip m_AttackSfx;
	private Animator m_Animator;

	private void Awake()
	{
		m_Animator = GetComponentInChildren<Animator>();
		Assert.IsNotNull(m_Animator, "No animator attachted to combat");
	}

	public void Attack(IDamagable target)
	{
		if (m_AttackSfx != null)
		{ 
			//AudioManager.Instance.PlaySfx(m_AttackSfx, transform); 
		}
		m_Animator.SetTrigger("Attack");

		StartCoroutine(DelayedDamage(target, m_Damage, 0.75f));
	}

	public void Attack(IDamagable target, Vector3 location)
	{
		if (m_AttackSfx != null)
		{
			//AudioManager.Instance.PlaySfx(m_AttackSfx, transform); 
		}
		m_Animator.SetTrigger("Attack");

		StartCoroutine(RotateAndDamage(target, location, m_Damage, 0.2f));
	}

	private IEnumerator DelayedDamage(IDamagable target, int damage, float delay)
	{
		yield return new WaitForSeconds(delay);
		target.ApplyDamage(damage);
	}

	private IEnumerator RotateAndDamage(IDamagable target, Vector3 location, int damage, float delay)
	{
		Quaternion startRot = transform.rotation;
		Quaternion targetrot = Quaternion.LookRotation(location - transform.position);
		float endTime = Time.time + delay;

		while(Time.time < endTime)
		{
			float t = 1 - (endTime - Time.time) / 0.2f;
			transform.rotation = Quaternion.Lerp(startRot, targetrot, t);
			yield return new WaitForFixedUpdate();
		}
		target.ApplyDamage(damage);
	}
}

