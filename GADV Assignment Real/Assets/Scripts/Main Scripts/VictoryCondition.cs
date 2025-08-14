///THis script checks for wins by seeing if any player has 0 units on the field left. If so, it just sends them to the end scene. There is also a delay cuz it checking
///too early, meaning the unit hit 0 hp, was checked by this and saw it was alive, then afterwards it would get destroyed and therefore breaking the flow

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene("End");
        }
        else if (!player2HasUnits && player1HasUnits)
        {
            Debug.Log("Player 1 Wins!");
            SceneManager.LoadScene("End");
        }
        else if (!player1HasUnits && !player2HasUnits)
        {
            Debug.Log("It's a draw.");
            SceneManager.LoadScene("End");
        }
        else
        {
            Debug.Log("Game continues...");
        }
    }
}



