using System;
using UnityEngine;

public interface ITurnPawn
{	
	void BeginTurn();
	void ForceEndTurn();
	event Action OnFinished;
	Sprite Portrait { get; }
}
