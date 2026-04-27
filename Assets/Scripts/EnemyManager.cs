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
    public List<EnemyPortal> enemyPortals;
    [SerializeField] private WaveDetails currentWave;
    [Space]

    [Header("Enemies Prefabs")]
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;

    private void Awake()
    {
        enemyPortals = new List<EnemyPortal>( FindObjectsByType<EnemyPortal>() );
    }

    private void Start()
    {
        SetupNextWave();
    }

    [ContextMenu("Setup new Wave")]
    private void SetupNextWave()
    {
        List<GameObject> newEnemies = NewEnemyWave();
        int portalIndex = 0;

        for (int i = 0; i < newEnemies.Count; i++)
        {
            GameObject enemyToAdd = newEnemies[i];
            EnemyPortal portalToSpawnEnemy = enemyPortals[portalIndex];
            
            portalToSpawnEnemy.AddEnemy(enemyToAdd);
            portalIndex++;

            if (portalIndex >= enemyPortals.Count)
            {
                portalIndex = 0;
            }
        }
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
