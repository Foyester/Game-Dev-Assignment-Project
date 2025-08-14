///this script handles the movement with various checks of validity by seeing if the tile is highlighted,if the tile is impassible 
///and it the tile already has someone else there. also uses tilemap.WorldToCell so it aint like mildy off to the right or something
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
        hasFinishedMoving = false;  
        
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

        if (Input.GetMouseButtonDown(0)) 
        {
            Debug.Log("Mouse clicked in movement input");
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedCell = tilemap.WorldToCell(mouseWorld);

            
            if (!tileHighlighter.IsTileHighlighted(clickedCell))
            {
                Debug.Log("Clicked tile is not highlighted for movement.");
                return;
            }

            
            if (tileHighlighter.impassableTilemap != null &&
                tileHighlighter.impassableTilemap.HasTile(clickedCell))
            {
                Debug.Log("Tile is impassable — cannot move here.");
                return;
            }

            
            if (!mapManager.CanMoveTo(clickedCell))
            {
                Debug.Log("Tile is already occupied or not valid.");
                return;
            }

            MoveTo(clickedCell);
        }
    }
    private void MoveTo(Vector3Int dest)
    {
        Vector3Int current = tilemap.WorldToCell(transform.position);

        
        transform.position = tilemap.GetCellCenterWorld(dest);

        
        mapManager.SetTileOccupied(current, false);
        mapManager.SetTileOccupied(dest, true);

        
        unit.hasMoved = true;
        isAwaitingMoveClick = false;

        
        hasFinishedMoving = true;

        
        tileHighlighter.ClearHighlights();
        unit.SetState(Unit.UnitState.Attacking);
    }
}