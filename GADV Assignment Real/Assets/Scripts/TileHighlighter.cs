using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileHighlighter : MonoBehaviour
{
    public Tilemap highlightMap;   // Tilemap used for displaying highlights
    public Tile highlightTile;     // Tile used to highlight valid tiles

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearHighlights();
        }
    }

    public void HighlightArea(Vector3Int center, int range)
    {
        ClearHighlights();

        List<Vector3Int> tilesInRange = GetSquareRange(center, range);
        foreach (var tilePos in tilesInRange)
        {
            highlightMap.SetTile(tilePos, highlightTile);
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
        return highlightMap.GetTile(pos) == highlightTile;
    }
}





