using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitSelector : MonoBehaviour
{
    private TileHighlighter tileHighlighter;
    private Unit unit;
    private Tilemap tilemap; // The tilemap used to get tile positions (e.g., terrain)

    private void Start()
    {
        // Find the TileHighlighter in the scene
        tileHighlighter = FindObjectOfType<TileHighlighter>();
        unit = GetComponent<Unit>();

        // Optionally get the tilemap for position conversion (if needed)
        tilemap = GameObject.Find("Ground Tilemap").GetComponent<Tilemap>();
    }


    private void OnMouseDown()
    {
        if (tileHighlighter != null && unit != null)
        {
            Vector3 worldPos = transform.position;
            Vector3Int cellPos = tilemap.WorldToCell(worldPos);
            tileHighlighter.HighlightArea(cellPos, unit.movementRange);
            Debug.Log($"Highlighting for {unit.unitName} at {cellPos}");
        }
    }
    
    
    
    ///private void OnMouseDown()
    //{
        //Debug.Log("Unit clicked: " + gameObject.name);

        // Access the Unit component
        //Unit unit = GetComponent<Unit>();
        //if (unit != null)
        //{
            //Debug.Log("Selected Unit: " + unit.unitName + " | HP: " + unit.currentHP);
        //}

        // TODO: Highlight movement range, show UI, etc.
    //}
}

