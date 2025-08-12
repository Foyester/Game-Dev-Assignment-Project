using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections;

public class UnitMover : MonoBehaviour
{
    private Unit unit;
    private Tilemap tilemap;
    private TileHighlighter tileHighlighter;
    private MapManager mapManager;

    private bool isAwaitingMoveClick = false;

    // New flag to track movement completion
    public bool hasFinishedMoving { get; private set; } = false;

    private void Start()
    {
        unit = GetComponent<Unit>();
        tilemap = GameObject.Find("Ground Tilemap").GetComponent<Tilemap>();
        tileHighlighter = FindObjectOfType<TileHighlighter>();
        mapManager = FindObjectOfType<MapManager>();
    }

    public void PrepareForMovement()
    {
        hasFinishedMoving = false;  // Reset flag when starting new movement
        StartCoroutine(EnableMoveInputNextFrame());
    }

    private IEnumerator EnableMoveInputNextFrame()
    {
        yield return null; // Wait one frame
        isAwaitingMoveClick = true;
    }

    public void HandleMovementInput()
    {
        if (!isAwaitingMoveClick || unit.currentState != Unit.UnitState.Moving)
            return;

        Debug.Log("HandleMovementInput called");

        if (Input.GetMouseButtonDown(0)) // Second left click
        {
            Debug.Log("Mouse clicked in movement input");
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedCell = tilemap.WorldToCell(mouseWorld);

            if (tileHighlighter.IsTileHighlighted(clickedCell) && mapManager.CanMoveTo(clickedCell))
            {
                MoveTo(clickedCell);
            }
        }
    }

    private void MoveTo(Vector3Int dest)
    {
        Vector3Int current = tilemap.WorldToCell(transform.position);

        // Snap position
        transform.position = tilemap.GetCellCenterWorld(dest);

        // Update map
        mapManager.SetTileOccupied(current, false);
        mapManager.SetTileOccupied(dest, true);

        // Lock movement
        unit.hasMoved = true;
        isAwaitingMoveClick = false;

        // Mark movement finished
        hasFinishedMoving = true;

        // Transition to attack state
        tileHighlighter.ClearHighlights();
        unit.SetState(Unit.UnitState.Attacking);
    }
}






