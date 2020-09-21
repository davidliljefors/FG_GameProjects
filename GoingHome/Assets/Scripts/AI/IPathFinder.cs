using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFinder
{
    void MoveTo(Vector3 start, Vector3 goal, int maxAllowedSteps);
    Action FinishedMoving { get; }
}
