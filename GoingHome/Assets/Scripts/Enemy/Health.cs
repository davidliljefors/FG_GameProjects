using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Health : MonoBehaviour, IDamagable
{

	[SerializeField] private int m_MaxHealth = 10;
	[SerializeField] private int m_Health;
	[SerializeField] private Slider m_HealthBarSlider;
	[SerializeField] private ParticleSystem m_TakeDamageParticle;
	private Animator m_Anim = null;
	private readonly int m_ShaderOpacityID = Shader.PropertyToID("_Opacity");
	private Camera m_Camera = null;

	public event Action OnHealthChanged = delegate { };

	public int GetHeatlh() { return m_Health; }
	public int GetMaxHeatlh() { return m_MaxHealth; }

	private void Awake()
	{
		if (gameObject.CompareTag("Enemy"))
		{
			StartCoroutine(FadeOpacity(0f, 1f, 0f, 1f, GetComponentInChildren<SkinnedMeshRenderer>().material));
			StartCoroutine(FadeOpacity(0f, 1f, 0f, 1f, GetComponentInChildren<MeshRenderer>().material));
		}
	}

	void Start()
	{
		m_Camera = Camera.main;
		Assert.IsNotNull(m_HealthBarSlider, "Assign healthbar in inspector!");
		m_Health = m_MaxHealth;
		m_Anim = GetComponentInChildren<Animator>();
		Assert.IsNotNull(m_Anim, "No animator found on " + gameObject.name);

	}

	private void OnEnable()
	{
		m_Health = m_MaxHealth;
		m_HealthBarSlider.maxValue = m_MaxHealth;
		m_HealthBarSlider.value = m_Health;

	}

	public void ApplyDamage(int amount)
	{
		m_Health -= amount;
		OnHealthChanged.Invoke();

		if (m_Health <= 0)
		{
			m_Health = 0;
		}
		m_HealthBarSlider.value = m_Health;

		if (m_Health == 0)
		{
			m_TakeDamageParticle.Stop(true);
			m_TakeDamageParticle.Play(true);
			Die();
		}
		else
		{
			m_TakeDamageParticle.transform.position = m_TakeDamageParticle.transform.parent.position + (m_Camera.transform.position - transform.position).normalized;
			m_TakeDamageParticle.Stop(true);
			m_TakeDamageParticle.Play(true);

			if (transform.CompareTag("Enemy"))
			{
				StartCoroutine("TakeDamageEnemyDelay");
			}
			else
				m_Anim.SetTrigger("Take Damage Player");
		}
	}

	private IEnumerator TakeDamageEnemyDelay()
	{
		yield return new WaitForSeconds(0.5f);
		m_Anim.SetTrigger("Take Damage Enemy");
		
	}

	private IEnumerator DeathDelay()
	{
		yield return new WaitForSeconds(0.5f);
		m_Anim.SetBool("Death", true);

	}

	private void Die()
	{
		TurnManager.OnKilled.Invoke(gameObject);
		if (TryGetComponent<CapsuleCollider>(out var capsule))
		{
			capsule.enabled = false;
		}

		if (gameObject.CompareTag("Enemy"))
		{
			StartCoroutine(FadeOpacity(1f, 0f, 1f, 2.5f, GetComponentInChildren<SkinnedMeshRenderer>().material));
			StartCoroutine(FadeOpacity(1f, 0f, 1f, 2.5f, GetComponentInChildren<MeshRenderer>().material));
			Destroy(gameObject, 4f);
		}
		StartCoroutine("DeathDelay");
		
	}

	private IEnumerator FadeOpacity(float start, float end, float startDelay, float duration, Material material)
	{
		if (startDelay > 0f)
		{ yield return new WaitForSeconds(startDelay); }

		float endTime = Time.time + duration;

		while (Time.time < endTime)
		{
			float t = 1 - (endTime - Time.time) / duration;
			t = Mathf.Lerp(start, end, t);
			material.SetFloat(m_ShaderOpacityID, t);

			yield return new WaitForEndOfFrame();
		}
		material.SetFloat(m_ShaderOpacityID, end);
	}
}
