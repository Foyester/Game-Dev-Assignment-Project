/// TileHighlighter is a script that spawns the hihglight tiles that show a unit's range and attack based off it's data from Unit.cs. A good chunk of the code
/// is cross-referencing with an impassible terrain tilemap and not generating tiles if it overlaps. It's doesn't actually clear the tiles itself after generation
/// rather calling a method from highlightmap to delete. Also manhattan distance is a life saver compared to the math i tried with hexagons like omg it so much easier



using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileHighlighter : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap highlightMap;   
    public Tilemap impassableTilemap; 

    [Header("Highlight Tiles")]
    public Tile movementTile;      
    public Tile attackTile;        

    
    

    /// <summary>
    /// Highlights all tiles within a movement range.
    /// </summary>
    public void HighlightMovementRange(Vector3Int center, int range)
    {
        HighlightArea(center, range, movementTile, true); // true = check impassable
    }

    /// <summary>
    /// Highlights all tiles within an attack range.
    /// </summary>
    public void HighlightAttackRange(Vector3Int center, int range)
    {
        HighlightArea(center, range, attackTile, false); // false = ignore impassable
    }

    /// <summary>
    /// Generic highlight function that takes a tile type and an impassable check flag.
    /// </summary>
    public void HighlightArea(Vector3Int center, int range, Tile tileToUse, bool blockOnImpassable)
    {
        Debug.Log($"Highlighting {tileToUse.name} at {center} with range {range}");
        ClearHighlights();

        List<Vector3Int> tilesInRange = GetSquareRange(center, range);
        foreach (var tilePos in tilesInRange)
        {
            // Skip impassable tiles if required
            if (blockOnImpassable && impassableTilemap != null && impassableTilemap.HasTile(tilePos))
                continue;

            highlightMap.SetTile(tilePos, tileToUse);
        }
    }

    public void ClearHighlights()
    {
        highlightMap.ClearAllTiles();
    }

    public List<Vector3Int> GetSquareRange(Vector3Int center, int range)
    {
        List<Vector3Int> results = new List<Vector3Int>();

        for (int dx = -range; dx <= range; dx++)
        {
            for (int dy = -range; dy <= range; dy++)
            {
                
                if (Mathf.Abs(dx) + Mathf.Abs(dy) <= range)
                {
                    Vector3Int tilePos = new Vector3Int(center.x + dx, center.y + dy, center.z);
                    results.Add(tilePos);
                }
            }
        }

        return results;
    }

    public bool IsTileHighlighted(Vector3Int pos)
    {
        TileBase tileAtPos = highlightMap.GetTile(pos);
        return tileAtPos == movementTile || tileAtPos == attackTile;
    }
}








