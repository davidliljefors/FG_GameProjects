using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, ITurnPawn
{
	private const string k_GroundTag = "Ground";
	private const string k_InteractableTag = "Interactable";
	private const string k_EnemyTag = "Enemy";

	[SerializeField] private int m_MovesPerTurn = 2;
	[SerializeField] private Sprite m_Portrait = null;
	
	[Header("Path settings")]
	[SerializeField] Color m_GoodColour = Color.yellow;
	[SerializeField] private float m_WalkDistance = 8f;
	[SerializeField] private float m_InteractionDistance = 4f;
	[SerializeField] private float m_StartPathPreview = 2f;
	[SerializeField, Tooltip("The distance the character wants to be away from an object when the destination is set to an object")] 
	private float m_StopDistanceToObjects = 1f;


	private int m_MovesLeft = -1;
	private CharacterInteraction m_Interaction = null;
	private NavMeshObstacle m_Obstacle = null;
	private NavMeshAgent m_Agent = null;
	private LineRenderer m_PathPreview = null;
	private NavMeshPath m_Path = null;
	private PathCostText m_PathCostText = null;
	private Animator m_Animator = null;
	private bool m_Suspended = false;

	public Sprite Portrait { get => m_Portrait; private set => m_Portrait = value; }

	public event Action OnFinished;
	public int MovesLeft
	{
		get => m_MovesLeft;
		set
		{
			if(value > m_MovesLeft)
			{
				m_MovesLeft = value;
			}

			if (value == int.MinValue)
			{
				m_MovesLeft = 0;
				EndTurn();
				return;
			}

			if (TurnManager.s_FreeMoveEnabled == false)
			{
				m_MovesLeft = value;
				if (m_MovesLeft <= 0 && !m_Suspended)
				{
					EndTurn();
				}
			}
		}
	}

	private void Start()
	{
		m_Path = new NavMeshPath();
		m_PathPreview = GetComponent<LineRenderer>();
		m_Animator = GetComponentInChildren<Animator>();
		m_Obstacle = GetComponent<NavMeshObstacle>();
		m_Agent = GetComponent<NavMeshAgent>();
		m_PathCostText = GetComponentInChildren<PathCostText>();
		m_Interaction = GetComponent<CharacterInteraction>();
		m_PathPreview.material = new Material(Shader.Find("Sprites/Default"));
		Assert.IsNotNull(m_PathPreview, "No Line Renderer in PlayerController found");
		Assert.IsNotNull(m_Obstacle, "No NavObstacle in PlayerController found");
		Assert.IsNotNull(m_Agent, "No NavAgent in PlayerController found");
		Assert.IsNotNull(m_PathCostText, "No PathCostText in Player's children found");
		Assert.IsNotNull(m_Interaction, "No CharacterInteraction in Player found");
		m_PathPreview.enabled = false;

		TurnManager.OnSpawned.Invoke(gameObject);
	}

	public void BeginTurn()
	{
		m_Obstacle.enabled = false;
		// Ugly hack to prevent obstacle interfering. possibly unity bug
		Invoke("EnablePlayerController", 0.1f);
	}

	private void EnablePlayerController()
	{
		if(gameObject == null)
		{
			PlayerInput.OnBecomeCharactersTurn(null);
			OnFinished.Invoke();
		}
		m_Agent.enabled = true;
		m_PathPreview.enabled = true;
		MovesLeft = m_MovesPerTurn;
		PlayerInput.OnBecomeCharactersTurn(gameObject);
	}

	private void EndTurn()
	{
		PlayerInput.OnBecomeCharactersTurn(null);
		m_Agent.enabled = false;
		m_Obstacle.enabled = true;
		m_PathPreview.enabled = false;
		m_PathCostText.Clear();
		OnFinished.Invoke();
	}

	public void ForceEndTurn()
	{
		StopAllCoroutines();
		EndTurn();
	}

	public void Suspend(float time)
	{
		StartCoroutine(SuspendRoutine(time));
	}

	private IEnumerator SuspendRoutine(float time)
	{
		m_Suspended = true;
		PlayerInput.OnBecomeCharactersTurn(null);
		yield return new WaitForSeconds(time);
		m_Suspended = false;

		if(MovesLeft > 0)
		{
			PlayerInput.OnBecomeCharactersTurn(gameObject);
		}
		else
		{
			EndTurn();
		}
	}

	public void MouseOver(Vector3 worldpos, GameObject hit)
	{
		m_PathCostText.Clear();
		if (m_Interaction.HasFocus)
		{
			m_PathPreview.enabled = false;
			return;
		}
		m_PathPreview.enabled = true;
		m_Path = CalculatePath(worldpos, hit);
		
		if(hit.CompareTag(k_InteractableTag) || hit.CompareTag(k_EnemyTag))
		{
			if (m_Path.corners.Length == 0)
			{
				if (Vector3.Distance(transform.position, hit.transform.position) < m_InteractionDistance)
				{
					m_PathCostText.Clear();
					m_PathPreview.positionCount = 0;
					return;
				}
			}
			else if (PathLength(m_Path) < m_InteractionDistance)
			{
				m_PathCostText.Clear();
				m_PathPreview.positionCount = 0;
				return;
			}
		}

		DrawPathPreview(m_Path);

	}

	public void MouseClicked(Vector3 worldpos, GameObject clicked)
	{
		if (m_Interaction.HasFocus)
		{
			return;
		}

		m_Path = CalculatePath(worldpos, clicked);
		m_PathCostText.Clear();

		// Clicked an interactable OR enemy
		if (clicked.transform.CompareTag(k_InteractableTag) || clicked.transform.CompareTag(k_EnemyTag))
		{
			if (m_Path.corners.Length == 0)
			{
				if (Vector3.Distance(transform.position, clicked.transform.position) < m_InteractionDistance)
				{
					m_Interaction.Clicked(clicked);
				}
			}
			else if (PathLength(m_Path) < m_InteractionDistance)
			{
				m_Interaction.Clicked(clicked);
			}
			else
			{
				Move(m_Path, () => { m_Interaction.Clicked(clicked.gameObject); });
				return;
			}
		}

		// Clicked ground !
		if (clicked.transform.CompareTag(k_GroundTag))
		{
			if (m_Path.corners.Length == 0)
			{
				return;
			}
			else
			{
				Move(m_Path);
			}
		}

	}

	public void Move(NavMeshPath path)
	{
		m_Animator?.SetBool("Walking", true);
		m_PathPreview.positionCount = 0;
		PlayerInput.OnBecomeCharactersTurn(null);
		m_Agent.SetPath(path);
		StartCoroutine(CheckIfFinishedMoving(null));
	}

	public void Move(NavMeshPath path, Action nextAction)
	{
		m_Animator?.SetBool("Walking", true);
		m_PathPreview.positionCount = 0;
		PlayerInput.OnBecomeCharactersTurn(null);
		m_Agent.SetPath(path);
		StartCoroutine(CheckIfFinishedMoving(nextAction));
	}

	private void MoveFinished(Action nextAction)
	{
		MovesLeft -= GetPathCost(m_Path);
		Debug.Log("Move finish");
		Debug.Log(MovesLeft);
		if (MovesLeft > 0)
		{
			if (nextAction != null)
			{
		Debug.Log("Move finish");
				PlayerInput.OnBecomeCharactersTurn(gameObject);
				nextAction.Invoke();
			}
			else
			{
		Debug.Log("Move finish");
				PlayerInput.OnBecomeCharactersTurn(gameObject);
			}
		}
	}

	private IEnumerator CheckIfFinishedMoving(Action nextAction)
	{
		bool reachedDestination = false;
		yield return new WaitForSeconds(0.5f);
		for (int i = 0; i < 200; i++)
		{
			if ((m_Agent.path.corners[m_Agent.path.corners.Length - 1] - transform.position).sqrMagnitude < 0.5f)
			{
				reachedDestination = true;
				m_Animator?.SetBool("Walking", false);
				yield return new WaitForSeconds(0.5f);
				MoveFinished(nextAction);
				break;
			}
			yield return new WaitForSeconds(0.05f);
		}
		if (!reachedDestination)
		{
			m_Animator?.SetBool("Walking", false);
			yield return new WaitForSeconds(0.5f);
			MoveFinished(nextAction);
		}
	}

	#region Helper methods for path
	private NavMeshPath CalculatePath(Vector3 destination, GameObject destinationObj)
	{
		NavMeshPath path = new NavMeshPath();

		if (destinationObj.CompareTag(k_GroundTag))
		{
			m_Agent.CalculatePath(destination, path);
		}

		else if (destinationObj.CompareTag(k_InteractableTag) || destinationObj.CompareTag(k_EnemyTag))
		{
			float distanceToTarget = Vector3.Distance(transform.position, destinationObj.transform.position);
			if (distanceToTarget >= m_StopDistanceToObjects)
			{ distanceToTarget -= m_StopDistanceToObjects; }
			else
			{ distanceToTarget *= 0.5f; }

			Vector3 directionToTarget = (destinationObj.transform.position - transform.position).normalized;
			destination = transform.position + distanceToTarget * directionToTarget;


			if(!m_Agent.CalculatePath(destination, path))
			{
				if(NavMesh.SamplePosition(destination, out var hit, 2f, NavMesh.AllAreas))
				{
					destination = hit.position;
					m_Agent.CalculatePath(destination, path);
				}
			}
		}

		if (TurnManager.s_FreeMoveEnabled == false)
		{
			if (PathLength(path) > m_WalkDistance)
			{
				path = GetPathByStepCount(m_Agent, path, MovesLeft, m_WalkDistance);
			}
		}

		return path;
	}

	private NavMeshPath GetPathByStepCount(NavMeshAgent agent, NavMeshPath path, int steps, float walkDistance)
	{
		Vector3[] pathSteps = GetPathPointsSeparatedByDistance(path, walkDistance);

		if (steps < pathSteps.Length)
		{
			Vector3 newDestination = pathSteps[steps];
			NavMeshPath newPath = new NavMeshPath();
			agent.CalculatePath(newDestination, newPath);
			return newPath;
		}
		else
		{
			Debug.LogError("GetPathByStepCount called on too short path");
			return path;
		}
	}

	private int GetPathCost(NavMeshPath path)
	{
		float pathLength = PathLength(path);
		return Mathf.CeilToInt(pathLength / m_WalkDistance);
	}

	private void DrawPathPreview(NavMeshPath path)
	{
		Vector3[] pathPreview = new Vector3[0];
		m_PathPreview.startColor = m_GoodColour;
		m_PathPreview.endColor = m_GoodColour;

		if (path.corners.Length > 0)
		{
			int requiredSteps = GetPathCost(path);

			pathPreview = new Vector3[path.corners.Length];
			Array.Copy(path.corners, pathPreview, path.corners.Length);
			pathPreview[0] += (pathPreview[1] - pathPreview[0]).normalized * m_StartPathPreview;

			m_PathCostText.SetCost(path.corners[path.corners.Length - 1], requiredSteps, TurnManager.s_FreeMoveEnabled ? -1 : MovesLeft);
		}

		m_PathPreview.positionCount = path.corners.Length;
		m_PathPreview.SetPositions(pathPreview);
	}

	private Vector3[] GetPathPointsSeparatedByDistance(NavMeshPath path, float distance)
	{
		float pathLength = PathLength(path);
		int requiredSteps = Mathf.CeilToInt(pathLength / distance);
		Vector3[] points = new Vector3[requiredSteps + 1];

		for (int i = 0; i < requiredSteps; i++)
		{
			points[i] = PointAtPathLength(path, i * distance);
		}

		points[requiredSteps] = path.corners[path.corners.Length - 1];
		return points;
	}

	private Vector3 PointAtPathLength(NavMeshPath path, float length)
	{
		float accumulatedDistance = 0f;
		for (int i = 1; i < path.corners.Length; i++)
		{
			float dist = Vector3.Distance(path.corners[i - 1], path.corners[i]);
			if (accumulatedDistance + dist >= length)
			{
				float diff = length - accumulatedDistance;
				return path.corners[i - 1] + (path.corners[i] - path.corners[i - 1]).normalized * diff;
			}
			else
			{
				accumulatedDistance += dist;
			}
		}
		return Vector3.zero;
	}

	private float PathLength(NavMeshPath path)
	{
		float distance = 0;
		for (int i = 1; i < path.corners.Length; i++)
		{
			distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
		}
		return distance;
	}


	#endregion
}

public static class NavMeshTools
{
	public static NavMeshPath GetNearbyPathToPoint(NavMeshAgent agent, Vector3 target, float range)
	{
		NavMeshPath path = new NavMeshPath();

		if (NavMesh.SamplePosition(target, out NavMeshHit navHit, range, NavMesh.AllAreas))
		{
			agent.CalculatePath(navHit.position, path);

			Vector3 samplingDirection;
			if (path.corners.Length > 2)
			{
				samplingDirection = path.corners[path.corners.Length - 3];
			}
			else
			{
				samplingDirection = path.corners[path.corners.Length - 1];
			}

			samplingDirection = samplingDirection - target;
			samplingDirection.Normalize();

			Vector3 navigablePoint = default;
			bool found = false;
			const int iters = 20;
			const float t = 1.0f / iters;
			for (int i = 0; i < iters; i++)
			{
				Vector3 randomPoint = target + samplingDirection * i * t * range;
				NavMeshHit hit;
				if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
				{
					navigablePoint = hit.position;
					found = true;
					break;
				}
			}
			if (found)
			{
				agent.CalculatePath(navigablePoint, path);
				return path;
			}
		}
		return new NavMeshPath();
	}
	public static NavMeshPath ShortenPath(NavMeshAgent agent, NavMeshPath path, float shortenBy)
	{
		List<Vector3> corners = new List<Vector3>(path.corners);
		Vector3 newEnd = Vector3.zero;
		while (shortenBy > 0f)
		{
			float distanceBetweenLastCorners = Vector3.Distance(corners[corners.Count - 2], corners[corners.Count - 1]);
			if (shortenBy < distanceBetweenLastCorners)
			{
				newEnd = corners[corners.Count - 1] + (corners[corners.Count - 2] - corners[corners.Count - 1]).normalized * shortenBy;
				break;
			}
			else
			{
				if (corners.Count > 2)
				{
					shortenBy -= distanceBetweenLastCorners;
					corners.RemoveAt(corners.Count - 1);
				}
				else
				{
					newEnd = path.corners[0];
					break;
				}
			}
		}

		agent.CalculatePath(newEnd, path);
		return path;
	}
}
