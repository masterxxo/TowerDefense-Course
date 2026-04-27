using System.Collections.Generic;
using System.Text;
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
    [SerializeField] private List<Transform> enemyWaypoints;
    private int _nextWaypointIndex = 0;
    private int _currentWaypointIndex = 0;

    private float _totalDistance;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;

        _agent.avoidancePriority = Mathf.RoundToInt(_agent.speed * 10);
    }

    private void Start()
    {
        CalculateTotalDistance();
    }

    public void SetupEnemy(List<Waypoint> newWaypoints)
    {
        enemyWaypoints = new List<Transform>();
        foreach (Waypoint waypoint in newWaypoints)
        {
            enemyWaypoints.Add(waypoint.transform);
        }
        
        CalculateTotalDistance();
    }

    private void Update()
    {
        FaceTarget(_agent.steeringTarget);
        if ( ShouldUpdateWaypoint())
        {
            _agent.SetDestination(GetNextWaypoint());
        }
        //_agent.SetDestination(waypoint.position);
    }

    private bool ShouldUpdateWaypoint()
    {
        if (_nextWaypointIndex >= enemyWaypoints.Count)
        {
            return false;
        }

        if (_agent.remainingDistance <= 0.5f)
            return true;
        
        Vector3 currentWaypoint = enemyWaypoints[_currentWaypointIndex].position;
        Vector3 nextWaypoint = enemyWaypoints[_nextWaypointIndex].position;
        
        float distanceToNextWaypoint = Vector3.Distance(transform.position, nextWaypoint);
        float distanceBetweenWaypoints = Vector3.Distance(currentWaypoint, nextWaypoint);

        return distanceBetweenWaypoints > distanceToNextWaypoint;
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
        if (_nextWaypointIndex >= enemyWaypoints.Count)
        {
            return transform.position;
        }
        
        Vector3 targetPoint = enemyWaypoints[_nextWaypointIndex].position;

        if (_nextWaypointIndex > 0)
        {
            float distance = Vector3.Distance(enemyWaypoints[_nextWaypointIndex].position, enemyWaypoints[_nextWaypointIndex - 1].position);
            _totalDistance -= distance;
        }
        
        _nextWaypointIndex++;
        _currentWaypointIndex = _nextWaypointIndex - 1;

        return targetPoint;
    }
    
    private void CalculateTotalDistance()
    {
        for (int i = 0; i < enemyWaypoints.Count - 1; i++)
        {
            float distance = Vector3.Distance(enemyWaypoints[i].position, enemyWaypoints[i + 1].position);
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
