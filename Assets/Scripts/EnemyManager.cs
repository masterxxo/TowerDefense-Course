using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveDetails
{
    public int basicEnemy;
    public int fastEnemy;
}
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private WaveDetails currentWave;
    [Space]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float spawnCooldown;
    private float _spawnTimer;
    
    private List<GameObject> _enemiesToCreate;
    [Header("Enemies Prefabs")]
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;

    private void Start()
    {
        _enemiesToCreate = NewEnemyWave();
    }
    
    private void Update()
    {
        _spawnTimer -= Time.deltaTime;

        if (_spawnTimer <= 0 && _enemiesToCreate.Count > 0)
        {
            CreateEnemy();
            _spawnTimer = spawnCooldown;
        }
    }

    private void CreateEnemy()
    {
        GameObject randomEnemy = GetRandomEnemy();
        GameObject newEnemy = Instantiate(randomEnemy, respawnPoint.position, Quaternion.identity);
    }

    private GameObject GetRandomEnemy()
    {
        int randomIndex = Random.Range(0, _enemiesToCreate.Count);
        GameObject chosenEnemy = _enemiesToCreate[randomIndex];
        _enemiesToCreate.RemoveAt(randomIndex);

        return chosenEnemy;
    }

    private List<GameObject> NewEnemyWave()
    {
        List<GameObject> enemies = new List<GameObject>();
        
        for (int i = 0; i < currentWave.basicEnemy; i++)
        {
            enemies.Add(basicEnemy);
        }

        for (int i = 0; i < currentWave.fastEnemy; i++)
        {
            enemies.Add(fastEnemy);
        }
        
        return enemies;
    }
}
