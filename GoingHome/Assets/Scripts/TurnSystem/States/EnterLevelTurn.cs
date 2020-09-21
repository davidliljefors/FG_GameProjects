using System;
using UnityEngine;

public class EnterLevelTurn : ITurn
{
	public Action OnStateFinished { get; set; }

	public void Enter()
	{
		Debug.Log("Turn system started");
	}

	public void Tick()
	{
		OnStateFinished.Invoke();
	}

	public void Exit()
	{
		
	}
}