using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _agent;
    [SerializeField] private float turnSpeed = 10f;
    private Transform[] _waypoints;

    private int _waypointIndex = 0;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;

        _agent.avoidancePriority = Mathf.RoundToInt(_agent.speed * 10);
    }

    private void Start()
    {
        _waypoints = FindAnyObjectByType<WaypointManager>().GetWaypoints();
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
        _waypointIndex++;

        return targetPoint;
    }
}
