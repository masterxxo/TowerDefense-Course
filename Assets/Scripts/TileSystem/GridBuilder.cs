using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Mathematics;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] private GameObject mainPrefab;
    [SerializeField] private int gridLength = 10;
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private List<GameObject> createdTiles;
    
    public List<GameObject> GetTileSetup() => createdTiles;
    private NavMeshSurface _gridNavmeshSurface => GetComponent<NavMeshSurface>();
    
    

    [ContextMenu("Build grid")]
    private void BuildGrid()
    {
        ClearGrid();
        createdTiles = new List<GameObject>();
        
        for (int x = 0; x < gridLength; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                CreateTile(x, z);
            }
            
        }
    }

    [ContextMenu("Clear grid")]
    private void ClearGrid()
    {
        foreach (var tile in createdTiles)
        {
            DestroyImmediate(tile);
        }
        
        createdTiles.Clear();
    }

    private void CreateTile(float xPosition, float zPosition)
    {
        Vector3 newPosition = new Vector3(xPosition, 0, zPosition);
        GameObject newTile = Instantiate(mainPrefab, newPosition, quaternion.identity, transform);
        
        createdTiles.Add(newTile);
    }
    
    public void UpdateNavMesh() => _gridNavmeshSurface.BuildNavMesh();
}
