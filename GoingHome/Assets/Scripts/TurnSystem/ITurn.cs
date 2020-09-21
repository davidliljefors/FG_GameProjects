using System;
using UnityEngine;

public interface ITurn
{
	void Enter();
	void Tick();
	void Exit();
	Action OnStateFinished { get; set; }
}
