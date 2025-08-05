using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(UnitManager))]
public class UnitController : MonoBehaviour
{
    private Unit unit;
    private UnitManager unitManager;
    private UnitMover mover;
    private UnitCombat combat;
    private TileHighlighter tileHighlighter;
    private Tilemap tilemap;

    private void Start()
    {
        unit = GetComponent<Unit>();
        unitManager = GetComponent<UnitManager>();
        mover = GetComponent<UnitMover>();
        combat = GetComponent<UnitCombat>();

        tileHighlighter = FindObjectOfType<TileHighlighter>();
        tilemap = GameObject.Find("Ground Tilemap").GetComponent<Tilemap>();
    }

    private void OnMouseDown()
    {
        if (!unitManager.CanActThisTurn())
        {
            Debug.Log("Not your turn.");
            return;
        }

        if (unit.currentState == Unit.UnitState.Idle)
        {
            // Highlight movement range
            Vector3Int cellPos = tilemap.WorldToCell(transform.position);
            tileHighlighter.HighlightArea(cellPos, unit.movementRange);

            // Mark unit as entering movement mode (but NOT moving yet)
            unit.SetState(Unit.UnitState.Moving);
            mover.PrepareForMovement(); // <- new method, explained below
        }
    }

    void Update()
    {
        if (unit.currentState == Unit.UnitState.Moving)
        {
            mover.HandleMovementInput();
        }
        else if (unit.currentState == Unit.UnitState.Attacking)
        {
            combat.HandleAttackInput();
        }
    }

}
