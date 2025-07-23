using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(UnitManager))]
public class UnitSelector : MonoBehaviour
{
    private TileHighlighter tileHighlighter;
    private Tilemap tilemap;

    private Unit unit;
    private UnitManager unitManager;

    private void Start()
    {
        // Get required components
        tileHighlighter = FindObjectOfType<TileHighlighter>();
        unit = GetComponent<Unit>();
        unitManager = GetComponent<UnitManager>();

        // Get the main tilemap
        GameObject tilemapGO = GameObject.Find("Ground Tilemap");
        if (tilemapGO != null)
        {
            tilemap = tilemapGO.GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogWarning("Ground Tilemap not found! Check the name of your tilemap object.");
        }
    }

    private void OnMouseDown()
    {
        // Only allow selection if it’s this unit’s turn
        if (unitManager.CanActThisTurn())
        {
            // Get the unit's current grid position
            Vector3Int cellPos = tilemap.WorldToCell(transform.position);

            // Highlight all tiles it can move to
            tileHighlighter.HighlightArea(cellPos, unit.movementRange);

            // Ask the unit manager to enable movement mode
            unitManager.TrySelect();
        }
        else
        {
            Debug.Log("This unit cannot act right now.");
        }
    }
}