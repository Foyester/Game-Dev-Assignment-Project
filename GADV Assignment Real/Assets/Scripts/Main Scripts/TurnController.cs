///basically connects the end button to the scripts and resets the states of the units for the next round
using UnityEngine;
using UnityEngine.UI; 

public class TurnController : MonoBehaviour
{
    [SerializeField] private Button endTurnButton; 

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

        
        allUnits = FindObjectsOfType<Unit>();

        foreach (Unit unit in allUnits)
        {
            
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

        
        turnManager.EndTurn();
    }
}


