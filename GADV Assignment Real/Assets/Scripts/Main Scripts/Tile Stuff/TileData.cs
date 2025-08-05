using UnityEngine;

[System.Serializable]
public class TileData
{
    public bool isWalkable = true;
    public int movementCost = 1;
    public string tileType = "Grass";
    public bool isOccupied = false;
}


