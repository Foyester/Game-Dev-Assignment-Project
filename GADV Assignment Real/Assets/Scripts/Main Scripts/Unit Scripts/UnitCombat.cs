using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(UnitManager))]
[RequireComponent(typeof(MapManager))]
public class UnitCombat : MonoBehaviour
{
    private Unit unit;
    private UnitManager manager;
    private Tilemap terrainTilemap;
    private TileHighlighter tileHighlighter;  // Added field

    private void Start()
    {
        unit = GetComponent<Unit>();
        manager = GetComponent<UnitManager>();

        // Get terrain tilemap from MapManager
        MapManager mapManager = FindObjectOfType<MapManager>();
        if (mapManager != null)
        {
            terrainTilemap = mapManager.terrainTilemap;
        }
        else
        {
            Debug.LogWarning("MapManager not found in scene!");
        }

        // Find TileHighlighter in the scene
        tileHighlighter = FindObjectOfType<TileHighlighter>();
        if (tileHighlighter == null)
        {
            Debug.LogWarning("TileHighlighter not found in scene!");
        }
    }

    public void HandleAttackInput()
    {
        if (unit.currentState != Unit.UnitState.Attacking) return;

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                GameObject target = hit.collider.gameObject;
                if (target == this.gameObject) return;

                Unit targetUnit = target.GetComponent<Unit>();
                UnitManager targetManager = target.GetComponent<UnitManager>();

                if (targetUnit != null && targetManager != null &&
                    targetManager.GetTeam() != manager.GetTeam())
                {
                    Vector3Int attackerCell = terrainTilemap.WorldToCell(transform.position);
                    Vector3Int targetCell = terrainTilemap.WorldToCell(target.transform.position);
                    int distance = Mathf.Abs(attackerCell.x - targetCell.x) + Mathf.Abs(attackerCell.y - targetCell.y);

                    if (distance <= unit.attackRange)
                    {
                        int damage = Mathf.Max(0, unit.attack - targetUnit.defense);
                        targetUnit.TakeDamage(damage);
                        Debug.Log($"{gameObject.name} attacked {target.name} for {damage}! Range: {distance}");
                        unit.SetState(Unit.UnitState.Done);
                    }
                    else
                    {
                        Debug.Log($"{target.name} is out of range! Needed ≤ {unit.attackRange}, was {distance}");
                    }
                }
            }
        }
    }

    public bool HasValidTargets()
    {
        if (tileHighlighter == null || terrainTilemap == null)
        {
            Debug.LogWarning("Cannot check targets: tileHighlighter or terrainTilemap is null.");
            return false;
        }

        Vector3Int attackerCell = terrainTilemap.WorldToCell(transform.position);
        var tilesInRange = tileHighlighter.GetSquareRange(attackerCell, unit.attackRange);

        foreach (var tilePos in tilesInRange)
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(terrainTilemap.GetCellCenterWorld(tilePos));
            foreach (var col in colliders)
            {
                Unit targetUnit = col.GetComponent<Unit>();
                UnitManager targetManager = col.GetComponent<UnitManager>();
                if (targetUnit != null && targetManager != null && targetManager.GetTeam() != manager.GetTeam())
                {
                    return true; // Found valid enemy target
                }
            }
        }

        return false; // No targets found
    }
}
