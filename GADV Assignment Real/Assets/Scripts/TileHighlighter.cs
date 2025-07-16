using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileHighlighter : MonoBehaviour
{
    public Tilemap highlightMap; // Assign your "Highlight Tilemap" in Inspector
    public Tile highlightTile;   // Assign the tile (e.g., BlueHighlightTile)

    public void HighlightArea(Vector3Int center, int range)
    {
        ClearHighlights();

        List<Vector3Int> tilesInRange = GetHexRange(center, range);
        foreach (var pos in tilesInRange)
        {
            highlightMap.SetTile(pos, highlightTile);
        }
    }

    public void ClearHighlights()
    {
        highlightMap.ClearAllTiles();
    }

    // This example assumes axial/offset hex logic — adjust if using custom layout
    public List<Vector3Int> GetHexRange(Vector3Int offsetCenter, int range)
    {
        List<Vector3Int> inRange = new List<Vector3Int>();

        Vector2Int axialCenter = OffsetToAxial(offsetCenter);

        for (int dx = -range; dx <= range; dx++)
        {
            for (int dy = Mathf.Max(-range, -dx - range); dy <= Mathf.Min(range, -dx + range); dy++)
            {
                int dz = -dx - dy; // not used but for clarity
                Vector2Int axialPos = new Vector2Int(axialCenter.x + dx, axialCenter.y + dy);
                Vector3Int offsetPos = AxialToOffset(axialPos);
                inRange.Add(offsetPos);
            }
        }

        return inRange;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearHighlights();
        }
    }
    // Convert Unity's offset (odd-q vertical) to axial coordinates
    private Vector2Int OffsetToAxial(Vector3Int offset)
    {
        int q = offset.x;
        int r = offset.y - (offset.x - (offset.x & 1)) / 2;
        return new Vector2Int(q, r);
    }

    // AXIAL to OFFSET (Odd-q, pointy-top hexes)
    // Converts from axial coordinates back to offset (Unity)
    private Vector3Int AxialToOffset(Vector2Int axial)
    {
        int col = axial.x;
        int row = axial.y + (axial.x - (axial.x & 1)) / 2;
        return new Vector3Int(col, row, 0);
    }




}
