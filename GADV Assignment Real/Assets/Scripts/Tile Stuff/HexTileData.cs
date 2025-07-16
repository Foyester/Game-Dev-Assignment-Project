using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

[System.Serializable]
public class HexTileData
{
    public bool isWalkable = true;
    public int movementCost = 1;
    public string tileType = "Grass";
}

