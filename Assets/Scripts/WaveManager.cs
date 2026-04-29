using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveDetails
{
    public GridBuilder nextGrid;
    public EnemyPortal[] newPortals;
    public int basicEnemy;
    public int fastEnemy;
}
public class WaveManager : MonoBehaviour
{
    [SerializeField] private GridBuilder currentGrid;
    
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
            CheckForNewLevelLayout();
            
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

    private void CheckForNewLevelLayout()
    {
        if (_waveIndex >= levelWaves.Length)
            return;
        WaveDetails nextWave = levelWaves[_waveIndex];

        if (nextWave.nextGrid != null)
        {
            UpdateLevelTile(nextWave.nextGrid);
            EnableNewPortals(nextWave.newPortals);
        }
        
        currentGrid.UpdateNavMesh();
    }

    private void UpdateLevelTile(GridBuilder nextGrid)
    {
        List<GameObject> grid = currentGrid.GetTileSetup();
        List<GameObject> newGrid = nextGrid.GetTileSetup();
        for (int i = 0; i < grid.Count; i++)
        {
            TileSlot currentTile = grid[i].GetComponent<TileSlot>();
            TileSlot newTile = newGrid[i].GetComponent<TileSlot>();
            
            bool shouldBeUpdated = currentTile.GetMesh() != newTile.GetMesh() || 
                                   currentTile.GetMaterial() != newTile.GetMaterial() || 
                                   currentTile.GetAllChildren().Count != newTile.GetAllChildren().Count ||
                                   currentTile.transform.rotation != newTile.transform.rotation;

            if (shouldBeUpdated)
            {
                currentTile.gameObject.SetActive(false);
                
                newTile.gameObject.SetActive(true);
                newTile.transform.parent = currentGrid.transform;

                grid[i] = newTile.gameObject;
                Destroy(currentTile.gameObject);
            }
        }
    }
    
    private void EnableNewPortals(EnemyPortal[] newPortals)
    {
        foreach (EnemyPortal portal in newPortals)
        {
            portal.gameObject.SetActive(true);
            _enemyPortals.Add(portal);
        }
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
