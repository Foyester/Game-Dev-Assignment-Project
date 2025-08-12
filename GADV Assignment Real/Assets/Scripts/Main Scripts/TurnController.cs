using UnityEngine;
using UnityEngine.UI; // Needed for UI Button

public class TurnController : MonoBehaviour
{
    [SerializeField] private Button endTurnButton; // Assign in Inspector

    private TurnManager turnManager;
    private Unit[] allUnits;

    void Start()
    {
        Debug.Log("TurnController Start called.");
        turnManager = FindObjectOfType<TurnManager>();

        
        if (endTurnButton != null)
        {
            endTurnButton.onClick.AddListener(EndTurn);
        }
        else
        {
            Debug.LogWarning("End Turn Button is not assigned in the inspector!");
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


