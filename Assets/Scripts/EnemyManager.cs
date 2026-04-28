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
    public bool waveCompleted;
    public float waveCooldown = 10;
    public float waveTimer;

    private float _checkInterval = .5f;
    private float _nextCheckTime;
    
    [SerializeField] private WaveDetails[] levelWaves;
    private int _waveIndex = 0;
    [Space]

    [Header("Enemies Prefabs")]
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;
    
    [Header("Enemy Portals")]
    private List<EnemyPortal> _enemyPortals;

    private void Awake()
    {
        _enemyPortals = new List<EnemyPortal>( FindObjectsByType<EnemyPortal>() );
    }

    private void Update()
    {
        HandleWaveCompletion();
        HandleWaveTiming();
    }

    private void HandleWaveCompletion()
    {
        if(!ReadyToCheck())
            return;
        
        if (!waveCompleted && AllEnemiesDefeated())
        {
            waveCompleted = true;
            waveTimer = waveCooldown;
        }
    }

    private void HandleWaveTiming()
    {
        if (waveCompleted)
        {
            waveTimer -= Time.deltaTime;
            if (waveTimer <= 0)
            {
                SetupNextWave();
            }
        }
    }

    private void Start()
    {
        SetupNextWave();
    }

    public void ForceNextWave()
    {
        if (!AllEnemiesDefeated())
        {
            return;
        }
        SetupNextWave();
    }

    [ContextMenu("Setup new Wave")]
    private void SetupNextWave()
    {
        List<GameObject> newEnemies = NewEnemyWave();
        int portalIndex = 0;

        if (newEnemies == null)
        {
            Debug.LogWarning("No enemies to spawn");
            return;
        }
            

        for (int i = 0; i < newEnemies.Count; i++)
        {
            GameObject enemyToAdd = newEnemies[i];
            EnemyPortal portalToSpawnEnemy = _enemyPortals[portalIndex];
            
            portalToSpawnEnemy.AddEnemy(enemyToAdd);
            portalIndex++;

            if (portalIndex >= _enemyPortals.Count)
            {
                portalIndex = 0;
            }
        }
        
        waveCompleted = false;
    }

    private List<GameObject> NewEnemyWave()
    {
        if (_waveIndex >= levelWaves.Length)
        {
            Debug.LogWarning("Wave " + _waveIndex + " does not exist");
            return null;
        }

        
        List<GameObject> enemies = new List<GameObject>();
        
        for (int i = 0; i < levelWaves[_waveIndex].basicEnemy; i++)
        {
            enemies.Add(basicEnemy);
        }

        for (int i = 0; i < levelWaves[_waveIndex].fastEnemy; i++)
        {
            enemies.Add(fastEnemy);
        }

        _waveIndex++;
        
        return enemies;
    }

    private bool AllEnemiesDefeated()
    {
        foreach (EnemyPortal portal in _enemyPortals)
        {
            if (portal.GetActiveEnemies().Count > 0)
            {
                return false;
            }
        }
        return true;
    }

    private bool ReadyToCheck()
    {
        if (Time.time >= _nextCheckTime)
        {
            _nextCheckTime = Time.time + _checkInterval;
            return true;
        }

        return false;
    }
}
