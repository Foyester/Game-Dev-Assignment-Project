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
    private bool movementFinishedHandled = false;

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
            Vector3Int cellPos = tilemap.WorldToCell(transform.position);
            Debug.Log($"Highlighting movement range for {unit.name} at {cellPos} with range {unit.movementRange}");
            tileHighlighter.HighlightArea(cellPos, unit.movementRange, tileHighlighter.movementTile);

            unit.SetState(Unit.UnitState.Moving);
            mover.PrepareForMovement();
        }
    }

    private void Update()
    {
        if (unit.currentState == Unit.UnitState.Moving)
        {
            mover.HandleMovementInput();

            if (mover.hasFinishedMoving && !movementFinishedHandled)
            {
                movementFinishedHandled = true;
                TryEnterAttackState();
            }
        }
        else if (unit.currentState == Unit.UnitState.Attacking)
        {
            combat.HandleAttackInput();

            // Highlight attack range while attacking
            Vector3Int cellPos = tilemap.WorldToCell(transform.position);
            tileHighlighter.HighlightArea(cellPos, unit.attackRange, tileHighlighter.attackTile);
        }
        else
        {
            movementFinishedHandled = false; // reset when not moving
        }
    }

    private void TryEnterAttackState()
    {
        if (combat.HasValidTargets())  // Make sure this returns true if enemies are in range
        {
            unit.SetState(Unit.UnitState.Attacking);
            Debug.Log($"{unit.name} entered attack state.");
        }
        else
        {
            unit.SetState(Unit.UnitState.Done);
            Debug.Log($"{unit.name} has no targets and ends its turn.");
        }
    }
}

