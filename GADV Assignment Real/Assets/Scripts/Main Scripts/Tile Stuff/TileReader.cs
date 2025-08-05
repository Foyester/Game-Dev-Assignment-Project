using UnityEngine;
using UnityEngine.Tilemaps;

//this script reads and fills the data of the tiles in the dictionary within MapManager
public class TileReader : MonoBehaviour
{
    public MapManager mapManager;

    void Start()
    {
        InitializeTileData();
    }

    void InitializeTileData()
    {
        BoundsInt bounds = mapManager.terrainTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (!mapManager.terrainTilemap.HasTile(pos)) continue;

            TileBase tile = mapManager.terrainTilemap.GetTile(pos);
            TileData data = new TileData();

            if (tile.name.ToLower().Contains("mountain"))
            {
                data.isWalkable = false;
                data.tileType = "Mountain";
            }
            else if (tile.name.ToLower().Contains("water"))
            {
                data.isWalkable = false;
                data.tileType = "Water";
            }
            else
            {
                data.tileType = tile.name;
            }

            mapManager.tileData[pos] = data;
        }
    }
}
