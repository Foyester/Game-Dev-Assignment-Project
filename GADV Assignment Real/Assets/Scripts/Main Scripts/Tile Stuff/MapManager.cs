using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [Header("Tilemap References")]
    public Tilemap terrainTilemap;

    [Header("Runtime Tile Data")]
    public Dictionary<Vector3Int, TileData> tileData = new Dictionary<Vector3Int, TileData>();

    private void Awake()
    {
        if (terrainTilemap == null)
        {
            
            terrainTilemap = GameObject.Find("Ground Tilemap").GetComponent<Tilemap>();

            if (terrainTilemap == null)
            {
                Debug.LogError("Terrain Tilemap not assigned and could not be found!");
                return;
            }
        }

        InitializeTileData();
    }


    private void InitializeTileData()
    {
        BoundsInt bounds = terrainTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = terrainTilemap.GetTile(pos);
            if (tile != null && !tileData.ContainsKey(pos))
            {
                // You can customize this by tile name/type
                tileData[pos] = new TileData
                {
                    isWalkable = true,
                    movementCost = 1,
                    tileType = tile.name,
                    isOccupied = false
                };
            }
        }
    }

    // Full walkability check — tile exists, is walkable, and not occupied
    public bool CanMoveTo(Vector3Int cellPos)
    {
        return tileData.ContainsKey(cellPos)
            && tileData[cellPos].isWalkable
            && !tileData[cellPos].isOccupied;
    }

    public bool IsTileWalkable(Vector3Int cellPos)
    {
        return tileData.ContainsKey(cellPos) && tileData[cellPos].isWalkable;
    }

    public bool IsTileOccupied(Vector3Int cellPos)
    {
        return tileData.ContainsKey(cellPos) && tileData[cellPos].isOccupied;
    }

    public void SetTileOccupied(Vector3Int cellPos, bool occupied)
    {
        if (tileData.ContainsKey(cellPos))
        {
            tileData[cellPos].isOccupied = occupied;
        }
    }

    public int GetTileMovementCost(Vector3Int cellPos)
    {
        if (tileData.ContainsKey(cellPos))
        {
            return tileData[cellPos].movementCost;
        }

        return 99; // Default very high cost if not known
    }

    public string GetTileType(Vector3Int cellPos)
    {
        if (tileData.ContainsKey(cellPos))
        {
            return tileData[cellPos].tileType;
        }

        return "Unknown";
    }
}
