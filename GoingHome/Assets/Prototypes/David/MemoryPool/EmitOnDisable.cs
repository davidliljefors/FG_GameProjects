using System;
using UnityEngine;

public class EmitOnDisable : MonoBehaviour
{
	public event Action<GameObject> OnDisableGameObject;

	private void OnDisable()
	{
		OnDisableGameObject?.Invoke(gameObject);
	}
}