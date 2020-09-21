using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShittyPathfinder : MonoBehaviour, IPathFinder
{
	private bool m_IsMoving;
	public Action FinishedMoving { get; set; } = delegate { PlayerTurn.CharacterFinished.Invoke(); };

	// Placeholder until we have proper movement
	public void MoveTo(Vector3 start, Vector3 goal, int maxStepsAllowed)
	{
		goal.x = Mathf.Round(goal.x);
		goal.z = Mathf.Round(goal.z);
		if(!m_IsMoving)
		{
			StartCoroutine(MoveCharacter(goal));
		}
		else
		{
			FinishedMoving.Invoke();
		}
	}

	// Placeholder until we have proper movement
	private IEnumerator MoveCharacter(Vector3 position)
	{
		float m_TurnSpeed = 200f;
		Quaternion targetRot = Quaternion.LookRotation(position.Flatten() - transform.position.Flatten());
		while (transform.rotation != targetRot)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, Time.deltaTime * m_TurnSpeed);
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(0.1f);

		float moveSpeed = 5f;
		m_IsMoving = true;
		while ((transform.position - position).sqrMagnitude > 0.01f)
		{
			transform.position = Vector3.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		m_IsMoving = false;
		FinishedMoving.Invoke();
	}
}
