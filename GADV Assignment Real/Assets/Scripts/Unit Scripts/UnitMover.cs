using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Unit))]
public class UnitMover : MonoBehaviour
{
    private Tilemap tilemap;
    private TileHighlighter tileHighlighter;
    private MapManager mapManager;
    private Unit unit;

    private bool isMovementMode = false;

    private void Start()
    {
        tilemap = GameObject.Find("Ground Tilemap").GetComponent<Tilemap>();
        tileHighlighter = FindObjectOfType<TileHighlighter>();
        mapManager = FindObjectOfType<MapManager>();
        unit = GetComponent<Unit>();
    }

    void Update()
    {
        if (!isMovementMode)
        {
            // Outside movement mode, right-click could be an attack
            if (Input.GetMouseButtonDown(1))
            {
                // Raycast to see if a unit was clicked
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null)
                {
                    GameObject clickedObj = hit.collider.gameObject;

                    if (clickedObj != this.gameObject)
                    {
                        UnitCombat combat = GetComponent<UnitCombat>();
                        if (combat != null)
                        {
                            combat.TryAttack(clickedObj);
                        }
                    }
                }
            }

            return;
        }

        // Movement mode is active
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                GameObject clickedObj = hit.collider.gameObject;

                //  Strong check to prevent self-targeting
                if (clickedObj == this.gameObject)
                {
                    Debug.Log("Cannot attack yourself.");
                    return;
                }

                UnitCombat combat = GetComponent<UnitCombat>();
                if (combat != null)
                {
                    combat.TryAttack(clickedObj);
                }
            }
        }
    }

    // Called by UnitManager / UnitSelector after first click to allow movement
    public void EnableMovement()
    {
        if (unit.hasMoved)
        {
            Debug.Log("This unit has already moved this turn.");
            return;
        }

        // Activate movement mode and wait for next click to move
        isMovementMode = true;
        Debug.Log("Movement mode enabled. Waiting for tile selection.");
    }

    private void MoveTo(Vector3Int destination)
    {
        // Record the current tile so we can mark it free after move
        Vector3Int currentCell = tilemap.WorldToCell(transform.position);

        // Move the unit visually to the center of the clicked tile
        transform.position = tilemap.GetCellCenterWorld(destination);

        // Update tile occupancy in the MapManager
        mapManager.SetTileOccupied(currentCell, false);    // Free old tile
        mapManager.SetTileOccupied(destination, true);     // Occupy new tile

        // Only now mark the unit as having moved
        unit.hasMoved = true;

        // Exit movement mode
        isMovementMode = false;

        // Clear highlights now that movement is complete
        tileHighlighter.ClearHighlights();

        Debug.Log($"Unit moved to: {destination}. Movement complete.");
    }
}




