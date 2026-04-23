using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    
    [SerializeField] private float spawnCooldown;
    private float _spawnTimer;
    
    private List<GameObject> _enemiesToCreate;

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
    }
    
    private GameObject GetRandomEnemy()
    {
        int randomIndex = Random.Range(0, _enemiesToCreate.Count);
        GameObject chosenEnemy = _enemiesToCreate[randomIndex];
        _enemiesToCreate.RemoveAt(randomIndex);
    
        return chosenEnemy;
    }

    public List<GameObject> GetEnemyList() => _enemiesToCreate;
}
