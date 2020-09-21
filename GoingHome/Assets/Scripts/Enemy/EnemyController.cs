using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour, ITurnPawn
{
	private const int k_MaxNearbyPlayers = 4;
	[SerializeField] private float m_AttackRange = 3f;
	[SerializeField] private float m_WalkThenAttackRange = 6f;
	[SerializeField] private float maxDistance = 5f;
	[SerializeField] private float m_VisionRange = 20f;
	[SerializeField] private float m_RoamDistance = 7f;
	[SerializeField] private bool m_ShouldRoam = true;
	[SerializeField] private LayerMask m_PlayerLayer = default;
	[SerializeField] private float zoomSeconds = 2f;
	//[SerializeField] private float targetZoom = 4f;
	[SerializeField] private Sprite m_Portrait = null;
	[SerializeField] private float m_ZoomDistanceFromFight = 20f;
	[SerializeField] private float m_ZoomTime = 1.5f;
	[SerializeField] private float m_StoppingThreshold = 0.5f;

	private NavMeshAgent m_Agent;
	private NavMeshObstacle m_Obstacle;
	private float m_TurnSpeed = 150f;
	private Collider[] m_Hits;
	private Combat m_Combat;

	private bool m_Attacked = false;
	private bool m_ZoomedIn = false;
	private Vector3 cameraPos = Vector3.zero;
	private Quaternion cameraRot = Quaternion.identity;
	private Camera cam;

	private Animator m_Animator = null;


	public Sprite Portrait { get => m_Portrait; private set => m_Portrait = value; }


	private void Awake()
	{
		m_Animator = GetComponentInChildren<Animator>();
		m_Hits = new Collider[k_MaxNearbyPlayers];
		m_Agent = GetComponent<NavMeshAgent>();
		m_Obstacle = GetComponent<NavMeshObstacle>();
		m_Combat = GetComponent<Combat>();
		Assert.IsNotNull(m_Agent, "No NavAgent found on enemy");
		Assert.IsNotNull(m_Obstacle, "No NavObstacle found on enemy");
		Assert.IsNotNull(m_Combat, "No Combat found on enemy");
	}

	private void Start()
	{
		TurnManager.OnSpawned.Invoke(gameObject);
		cam = Camera.main;
	}

	#region ITurnPawn Impl

	public event Action OnFinished;

	public void BeginTurn()
	{
		m_Obstacle.enabled = false;
		// Ugly hack to prevent obstacle interfering. possibly unity bug
		Invoke("EnableEnemyController", 0.5f);
	}

	private void EnableEnemyController()
	{
		m_Agent.enabled = true;
		Vector3 camPos = cam.transform.position;
		int nearyPlayers = CheckForPlayers(m_Hits, m_WalkThenAttackRange);
		if (nearyPlayers > 0)
		{
			Collider closest = GetClosestPlayer(m_Hits, nearyPlayers);
			if (Vector3.Distance(transform.position, closest.transform.position) < m_AttackRange)
			{
				StartCoroutine(ZoomCamera(closest.gameObject));
				StartCoroutine(AttackPlayer(closest.gameObject, EndTurn));
			}
			else
			{
				WalkStraight(closest.gameObject);
				StartCoroutine(CheckIfFinishedMoving(() =>
				{
					m_Agent.isStopped = true;
					m_Agent.velocity = Vector3.zero;
					StartCoroutine(ZoomCamera(closest.gameObject));
					StartCoroutine(AttackPlayer(closest.gameObject, EndTurn));
				}));
			}
			return;
		}

		int farPlayers = CheckForPlayers(m_Hits, m_VisionRange);
		if (farPlayers > 0)
		{
			Collider closest = GetClosestPlayer(m_Hits, farPlayers);
			WalkTowardsTarget(maxDistance, m_Agent, closest.transform.position, m_Agent.transform.position);
			StartCoroutine(CheckIfFinishedMoving(EndTurn));
			return;
		}

		if(m_ShouldRoam)
		{
			if (RandomPoint(transform.position, m_RoamDistance, out Vector3 dest))
			{
				WalkTowardsTarget(maxDistance, m_Agent, dest, m_Agent.transform.position);
				StartCoroutine(CheckIfFinishedMoving(EndTurn));
				return;
			}
		}

		EndTurn();
	}

#endregion

	private void EndTurn()
	{
		m_Agent.enabled = false;
		m_Obstacle.enabled = true;
		OnFinished.Invoke();
	}

	public void ForceEndTurn()
	{
		StopAllCoroutines();
		EndTurn();
	}

	private IEnumerator AttackPlayer(GameObject target, Action nextAction)
	{
		while (!m_ZoomedIn)
		{
			yield return new WaitForEndOfFrame();
		}
		m_ZoomedIn = false;
		//Rotate to player then attack
		Quaternion targetRot = Quaternion.LookRotation(target.transform.position.Flatten() - transform.position.Flatten());
		while (Quaternion.Angle(transform.rotation, targetRot) > 0.15f)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, Time.deltaTime * m_TurnSpeed);
			yield return new WaitForEndOfFrame();
		}
		//yield return new WaitForSeconds(zoomSeconds);
		m_Combat.Attack(target.GetComponent<IDamagable>());
		AudioManager.Instance?.Play("PlayerHit"); //play SFX from AudioManager
		yield return new WaitForSeconds(zoomSeconds);

		m_Attacked = true;

		nextAction.Invoke();
	}


	IEnumerator ZoomCamera(GameObject target)
	{
		cameraPos = cam.transform.position;
		cameraRot = cam.transform.rotation;

		Vector3 pointBetweenTargetAndSelf = (target.transform.position + (transform.position - target.transform.position) * 0.5f);
		Vector3 directionToTargetCameraPos = (pointBetweenTargetAndSelf - cam.transform.position).normalized;
		Vector3 targetCameraPos = pointBetweenTargetAndSelf - directionToTargetCameraPos * m_ZoomDistanceFromFight;
		Quaternion targetCameraRot = Quaternion.LookRotation(directionToTargetCameraPos, target.transform.up);

		float startTime = Time.time;
		float step;
		while (startTime + m_ZoomTime > Time.time)
		{
			step = Mathf.Clamp01((Time.time - startTime) / m_ZoomTime);

			cam.transform.position = cameraPos + (targetCameraPos - cameraPos) * step;
			cam.transform.rotation = Quaternion.Lerp(cameraRot, targetCameraRot, step);

			yield return new WaitForEndOfFrame();
		}

		cam.transform.rotation = targetCameraRot;
		cam.transform.position = targetCameraPos;
		m_ZoomedIn = true;


		while (!m_Attacked)
		{
			yield return new WaitForEndOfFrame();
		}
		m_Attacked = false;

		cam.transform.position = cameraPos;
		cam.transform.rotation = cameraRot;
		////cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, target.transform.rotation, 0.1f);
		//cam.transform.position = Vector3.Lerp(cam.transform.position, cam.transform.position + (target.transform.position - cam.transform.position).normalized * 10f, 2f);



	}

	private void WalkStraight(GameObject target)
	{
		NavMeshPath path = new NavMeshPath();
		Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
		Vector3 positionInDirectionOfTarget = transform.position + directionToTarget * m_AttackRange;
		if (NavMesh.SamplePosition(positionInDirectionOfTarget, out NavMeshHit hit, 2f, NavMesh.AllAreas))
		{
			m_Agent.CalculatePath(hit.position, path);
			m_Agent.SetPath(path);
			m_Animator?.SetBool("Walking", true);
		}
		else
		{
			Debug.LogError("couldn't get path");
		}
	}

	private IEnumerator CheckIfFinishedMoving(Action nextAction)
	{
		bool reachedDestination = false;
		yield return new WaitForSeconds(0.5f);
		for (int i = 0; i < 200; i++)
		{
			if ((m_Agent.path.corners[m_Agent.path.corners.Length - 1] - transform.position).sqrMagnitude < m_StoppingThreshold)
			{
				reachedDestination = true;
				m_Animator?.SetBool("Walking", false);
				nextAction.Invoke();
				break;
			}
			yield return new WaitForSeconds(0.05f);
		}
		if (!reachedDestination)
		{
			nextAction.Invoke();
		}
	}


	public void WalkTowardsTarget(float maxDistance, NavMeshAgent ai, Vector3 targetPosition, Vector3 aiPosition)
	{
		m_Animator?.SetBool("Walking", true);
		NavMeshPath path = new NavMeshPath();
		//Calculate path to player
		bool hasPath = NavMesh.CalculatePath(aiPosition, targetPosition, NavMesh.AllAreas, path);
		if (!hasPath)
		{
			path = NavMeshTools.GetNearbyPathToPoint(ai, targetPosition, 3f);
			if (path.corners.Length < 2)
			{
				Debug.LogError("Enemy couldn't find path to player");
				return;
			}
		}



		path.corners[path.corners.Length - 1] = path.corners[path.corners.Length - 1] - (path.corners[path.corners.Length - 1] - path.corners[path.corners.Length - 2]).normalized * 2f;

		Vector3 pointToWalkTo = Vector3.zero;

		//How far AI will have walked so far when calculating how many more points the ai will follow
		float distance = 0f;

		//If the distance to the first corner is less than the max distance, add it as a waypoint in the list.
		if ((path.corners[1] - aiPosition).magnitude < maxDistance)
		{
			//Update point
			pointToWalkTo = path.corners[1];

			//Calculate how far the AI will have walked
			distance += (path.corners[1] - aiPosition).magnitude;
			Debug.DrawLine(path.corners[1], aiPosition, Color.magenta, 60f);

			//If the array is of length 2, the player is the next corner.
			if (path.corners.Length == 2)
			{
				ai.SetDestination(pointToWalkTo);
				return;
			}
		}
		else
		{
			//Move towards corner as far as it can
			pointToWalkTo = aiPosition + (path.corners[1] - aiPosition).normalized * maxDistance;
			Debug.DrawLine(aiPosition + (path.corners[1] - aiPosition).normalized * maxDistance, aiPosition, Color.red, 60f);
			ai.SetDestination(pointToWalkTo);
			return;
		}

		//Loop through corners to walk as far as it can
		for (int i = 1; i < path.corners.Length - 1; i++)
		{
			//If distance to the next corner is less than how far the ai can still walk, update target position and try the next corner
			if ((path.corners[i + 1] - path.corners[i]).magnitude < maxDistance - distance)
			{
				pointToWalkTo = path.corners[i + 1];
				distance += (path.corners[i + 1] - path.corners[i]).magnitude;
				Debug.DrawLine(path.corners[i + 1], path.corners[i], Color.green, 60f);
			}
			else
			{
				//If ai can't reach the next corner, walk as far as it can towards the next corner
				pointToWalkTo = path.corners[i] + (path.corners[i + 1] - path.corners[i]).normalized * (maxDistance - distance);
				Debug.DrawLine(path.corners[i + 1], path.corners[i], Color.blue, 60f);
				ai.SetDestination(pointToWalkTo);
				return;
			}
		}

		//If ai ran out of corners to loop through, it can reach the player, since the player is the last corner
		ai.SetDestination(pointToWalkTo);
		return;
	}


	private int CheckForPlayers(Collider[] colliderBuffer, float radius)
	{
		return Physics.OverlapSphereNonAlloc(transform.position, radius, colliderBuffer, m_PlayerLayer);
	}

	private Collider GetClosestPlayer(Collider[] cols, int length)
	{
		Assert.IsTrue(length > 0, "Should never check on 0 length array here!");

		Collider closest = cols[0];
		float best = (transform.position - closest.transform.position).sqrMagnitude;
		for (int i = 1; i < length; i++)
		{
			float distnace = (transform.position - cols[i].transform.position).sqrMagnitude;
			if (distnace < best)
			{
				best = distnace;
				closest = cols[i];
			}
		}
		return closest;
	}

	bool RandomPoint(Vector3 center, float range, out Vector3 result)
	{
		for (int i = 0; i < 30; i++)
		{
			Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
			NavMeshHit hit;
			if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
			{
				result = hit.position;
				return true;
			}
		}
		result = Vector3.zero;
		return false;
	}

}
