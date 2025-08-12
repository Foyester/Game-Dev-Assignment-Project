using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileHighlighter : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap highlightMap;   // Tilemap used for displaying highlights

    [Header("Highlight Tiles")]
    public Tile movementTile;      // Tile used to highlight movement range
    public Tile attackTile;        // Tile used to highlight attack range

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearHighlights();
        }
    }

    /// <summary>
    /// Highlights all tiles within a movement range.
    /// </summary>
    public void HighlightMovementRange(Vector3Int center, int range)
    {
        HighlightArea(center, range, movementTile);
    }

    /// <summary>
    /// Highlights all tiles within an attack range.
    /// </summary>
    public void HighlightAttackRange(Vector3Int center, int range)
    {
        HighlightArea(center, range, attackTile);
    }

    /// <summary>
    /// Generic highlight function that takes a tile type.
    /// </summary>
    public void HighlightArea(Vector3Int center, int range, Tile tileToUse)
    {
        Debug.Log($"Highlighting {tileToUse.name} at {center} with range {range}");
        ClearHighlights();

        List<Vector3Int> tilesInRange = GetSquareRange(center, range);
        foreach (var tilePos in tilesInRange)
        {
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
                // Use Manhattan distance
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




