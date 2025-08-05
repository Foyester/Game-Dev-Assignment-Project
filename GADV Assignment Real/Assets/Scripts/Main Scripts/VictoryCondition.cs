using UnityEngine;
using System.Collections;

public class VictoryCondition : MonoBehaviour
{
    public IEnumerator CheckForWinAfterDelay()
    {
        
        yield return null;
        CheckForWin();
    }

    public void CheckForWin()
    {
        Debug.Log("VictoryCondition: Checking for win condition...");

        Unit[] allUnits = FindObjectsOfType<Unit>();

        bool player1HasUnits = false;
        bool player2HasUnits = false;

        foreach (Unit unit in allUnits)
        {
            UnitManager um = unit.GetComponent<UnitManager>();
            if (um == null)
            {
                Debug.LogWarning("Unit found without UnitManager!");
                continue;
            }

            if (um.team == UnitManager.PlayerTeam.Player1)
                player1HasUnits = true;
            else if (um.team == UnitManager.PlayerTeam.Player2)
                player2HasUnits = true;
        }

        if (!player1HasUnits && player2HasUnits)
        {
            Debug.Log(" Player 2 Wins!");
        }
        else if (!player2HasUnits && player1HasUnits)
        {
            Debug.Log("Player 1 Wins!");
        }
        else if (!player1HasUnits && !player2HasUnits)
        {
            Debug.Log("It's a draw.");
        }
        else
        {
            Debug.Log("Game continues...");
        }
    }
}



