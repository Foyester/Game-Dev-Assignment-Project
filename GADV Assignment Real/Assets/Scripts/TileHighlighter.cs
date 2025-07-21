using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileHighlighter : MonoBehaviour
{
    public Tilemap highlightMap; 
    public Tile highlightTile;   

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearHighlights();
        }
    }

    
    public void HighlightArea(Vector3Int centerOffset, int range)
    {
        ClearHighlights();

        List<Vector3Int> tilesInRange = GetHexRange(centerOffset, range);
        foreach (var tilePos in tilesInRange)
        {
            highlightMap.SetTile(tilePos, highlightTile);
        }
    }

    public void ClearHighlights()
    {
        highlightMap.ClearAllTiles();
    }

    
    public List<Vector3Int> GetHexRange(Vector3Int centerOffset, int range)
    {
        List<Vector3Int> results = new List<Vector3Int>();

        Vector2Int centerAxial = OffsetToAxial(centerOffset); // Convert to axial

        for (int dx = -range; dx <= range; dx++)
        {
            for (int dy = Mathf.Max(-range, -dx - range); dy <= Mathf.Min(range, -dx + range); dy++)
            {
                int dz = -dx - dy;
                Vector2Int axial = new Vector2Int(centerAxial.x + dx, centerAxial.y + dy);
                Vector3Int offset = AxialToOffset(axial); 
                results.Add(offset);
            }
        }

        return results;
    }

   
    private Vector2Int OffsetToAxial(Vector3Int offset)
    {
        int q = offset.x;
        int r = offset.y - ((offset.x - (offset.x & 1)) / 2); 
        return new Vector2Int(q, r);
    }

    
    private Vector3Int AxialToOffset(Vector2Int axial)
    {
        int col = axial.x;
        int row = axial.y + ((axial.x - (axial.x & 1)) / 2); 
        return new Vector3Int(col, row, 0);
    }
}





