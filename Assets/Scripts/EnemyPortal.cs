using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    [SerializeField] private float spawnCooldown;
    private float _spawnTimer;
    
    [Space]
    
    [SerializeField] private List<Waypoint> waypoints;
    
    private List<GameObject> _enemiesToCreate = new List<GameObject>();

    private void Awake()
    {
        CollectWaypoints();
    }

    private void Update()
    {
        if (CanMakeNewEnemy())
        {
            CreateEnemy();
        }
    }

    private bool CanMakeNewEnemy()
    {
        _spawnTimer -= Time.deltaTime;

        if (_spawnTimer <= 0 && _enemiesToCreate.Count > 0)
        {
            _spawnTimer = spawnCooldown;
            return true;
        }
        return false;
    }
    
    private void CreateEnemy()
    {
        GameObject randomEnemy = GetRandomEnemy();
        GameObject newEnemy = Instantiate(randomEnemy, transform.position, Quaternion.identity);

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.SetupEnemy(waypoints);
    }
    
    private GameObject GetRandomEnemy()
    {
        int randomIndex = Random.Range(0, _enemiesToCreate.Count);
        GameObject chosenEnemy = _enemiesToCreate[randomIndex];
        _enemiesToCreate.RemoveAt(randomIndex);
    
        return chosenEnemy;
    }

    public void AddEnemy(GameObject enemyToAdd) => _enemiesToCreate.Add(enemyToAdd);

    [ContextMenu("Collect Waypoints")]
    private void CollectWaypoints()
    {
        waypoints = new List<Waypoint>();
        foreach (Transform child in transform)
        {
            Waypoint waypoint = child.GetComponent<Waypoint>();
            if (waypoint != null)
            {
                waypoints.Add(waypoint);
            }
        }
    }
}
