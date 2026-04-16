using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Basic,
    Fast,
    None
}

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private Transform centerPoint;
    public int healthPoints = 4;
    private NavMeshAgent _agent;

    [Header("Movement")] [SerializeField] private float turnSpeed = 10f;
    private Transform[] _waypoints;
    private int _waypointIndex = 0;

    private float _totalDistance;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;

        _agent.avoidancePriority = Mathf.RoundToInt(_agent.speed * 10);
    }

    private void Start()
    {
        _waypoints = FindAnyObjectByType<WaypointManager>().GetWaypoints();
        CalculateTotalDistance();
    }

    private void Update()
    {
        FaceTarget(_agent.steeringTarget);
        if (_agent.remainingDistance <= 0.25f)
        {
            _agent.SetDestination(GetNextWaypoint());
        }
        //_agent.SetDestination(waypoint.position);
    }

    public float DistanceToFinishLine()
    {
        return _totalDistance + _agent.remainingDistance;
    }

    private void FaceTarget(Vector3 newTarget)
    {
        Vector3 directionToTarget = newTarget - transform.position; // Calculate direction from position to newTarget
        directionToTarget.y = 0; // Ignore vertical position differences

        Quaternion newRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
    }

    private Vector3 GetNextWaypoint()
    {
        if (_waypointIndex >= _waypoints.Length)
        {
            return transform.position;
        }
        
        Vector3 targetPoint = _waypoints[_waypointIndex].position;

        if (_waypointIndex > 0)
        {
            float distance = Vector3.Distance(_waypoints[_waypointIndex].position, _waypoints[_waypointIndex - 1].position);
            _totalDistance -= distance;
        }
        
        _waypointIndex++;

        return targetPoint;
    }
    
    private void CalculateTotalDistance()
    {
        for (int i = 0; i < _waypoints.Length - 1; i++)
        {
            float distance = Vector3.Distance(_waypoints[i].position, _waypoints[i + 1].position);
            _totalDistance += distance;
        }
    }

    public Vector3 GetCenterPoint()
    {
        return centerPoint.position;
    }

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;

        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
