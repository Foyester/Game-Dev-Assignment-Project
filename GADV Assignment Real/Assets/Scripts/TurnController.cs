using UnityEngine;

public class TurnController : MonoBehaviour
{
    private TurnManager turnManager;
    private Unit[] allUnits;

    void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
    }

    void Update()
    {
        // TEMPORARY: Press 'E' to end the turn
        if (Input.GetKeyDown(KeyCode.E))
        {
            EndTurn();
        }
    }

    public void EndTurn()
    {
        Debug.Log("Ending turn...");

        // Reset unit states for all units
        allUnits = FindObjectsOfType<Unit>();

        foreach (Unit unit in allUnits)
        {
            // Only reset for the current player's units
            UnitManager unitManager = unit.GetComponent<UnitManager>();
            if (unitManager != null)
            {
                bool isOwnedByP1 = (unitManager.team == UnitManager.PlayerTeam.Player1);
                bool currentTurnIsP1 = turnManager.IsPlayer1Turn();

                if (isOwnedByP1 == currentTurnIsP1)
                {
                    unit.hasMoved = false;
                    unit.SetState(Unit.UnitState.Idle);
                }
            }
        }

        // Swap turns
        turnManager.EndTurn();
    }
}

