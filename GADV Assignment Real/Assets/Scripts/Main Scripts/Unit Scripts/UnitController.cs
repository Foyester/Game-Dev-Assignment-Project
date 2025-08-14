///agony manifest cuz the mouse logic is unpleasant. I used a 'state' system to manages a unit behavior during its turn so clicks don't cause the different 
///calls to override each other. And in the end, i just forced it so movement has to be done before the attacking logic can be started. In hindsight, i would've 
///prolly added a button system to select what state you want to be in but time is lacking so no. It also forcefully ends the unit state if there isnt any enemies 
///in attack range since without it, i couldn't end the turn if i misclicked a unit with no targets in range.

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

    private Unit.UnitState lastState; 

    private void Start()
    {
        unit = GetComponent<Unit>();
        unitManager = GetComponent<UnitManager>();
        mover = GetComponent<UnitMover>();
        combat = GetComponent<UnitCombat>();
        tileHighlighter = FindObjectOfType<TileHighlighter>();
        tilemap = GameObject.Find("Ground Tilemap").GetComponent<Tilemap>();

        if (!tileHighlighter) Debug.LogWarning("[UnitController] TileHighlighter not found!");
        if (!tilemap) Debug.LogWarning("[UnitController] Ground Tilemap not found!");

        lastState = unit.currentState;
        Debug.Log($"[UnitController] {name} initialized. Starting state = {lastState}");
    }

    private void Update()
    {
        
        if (unit.currentState != lastState)
        {
            OnStateChanged(lastState, unit.currentState);
            lastState = unit.currentState;
        }

        
        switch (unit.currentState)
        {
            case Unit.UnitState.Moving:
                mover.HandleMovementInput();
                if (mover.hasFinishedMoving)
                {
                    Debug.Log($"[UnitController] {name} finished moving, checking attack state...");
                    TryEnterAttackState();
                }
                break;

            case Unit.UnitState.Attacking:
                combat.HandleAttackInput();
                break;

                
        }
    }

    private void OnMouseDown()
    {
        if (!unitManager.CanActThisTurn())
        {
            Debug.Log($"[UnitController] {name}: Not your turn.");
            return;
        }

        if (unit.currentState == Unit.UnitState.Idle)
        {
            
            Debug.Log($"[UnitController] {name}: Selected. Entering Moving state.");
            unit.SetState(Unit.UnitState.Moving);
        }
    }

   
    private void OnStateChanged(Unit.UnitState oldState, Unit.UnitState newState)
    {
        Debug.Log($"[UnitController] {name}: State change {oldState} -> {newState}");

        
        if (tileHighlighter) tileHighlighter.ClearHighlights();

        switch (newState)
        {
            case Unit.UnitState.Moving:
                {
                    
                    mover.PrepareForMovement(); 
                    Vector3Int cellPos = tilemap.WorldToCell(transform.position);
                    Debug.Log($"[UnitController] {name}: Drawing movement range from {cellPos} (range {unit.movementRange})");
                    tileHighlighter.HighlightArea(cellPos, unit.movementRange, tileHighlighter.movementTile, true); 
                    break;
                }

            case Unit.UnitState.Attacking:
                {
                    
                    Vector3Int cellPos = tilemap.WorldToCell(transform.position);
                    Debug.Log($"[UnitController] {name}: Drawing attack range from {cellPos} (range {unit.attackRange})");
                    tileHighlighter.HighlightArea(cellPos, unit.attackRange, tileHighlighter.attackTile, false); 
                    break;
                }

            case Unit.UnitState.Done:
            case Unit.UnitState.Idle:
            default:
                
                break;
        }
    }

   
    private void TryEnterAttackState()
    {
        if (combat.HasValidTargets())
        {
            Debug.Log($"[UnitController] {name}: Valid targets found. Entering Attacking.");
            unit.SetState(Unit.UnitState.Attacking);
        }
        else
        {
            Debug.Log($"[UnitController] {name}: No targets. Ending turn (Done).");
            unit.SetState(Unit.UnitState.Done);
        }
    }
}






