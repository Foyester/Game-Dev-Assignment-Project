using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public Tilemap terrainTilemap;

    public Dictionary<Vector3Int, HexTileData> tileData = new Dictionary<Vector3Int, HexTileData>();

    public bool IsTileWalkable(Vector3Int tilePos)
    {
        return tileData.ContainsKey(tilePos) && tileData[tilePos].isWalkable;
    }
}