using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the turn order between the two players

public class TurnManager : MonoBehaviour
{
    // Boolean to track whose turn it is
    // true means Player 1 - false means Player 2
    public bool isPlayer1Turn = true;

    public void EndTurn()
    {
        //flips the boolean so no new code is needed to change player 2 -> player 1
        isPlayer1Turn = !isPlayer1Turn;

        // Print the current player's turn to the console for debugging.
        Debug.Log("Current Turn: " + (isPlayer1Turn ? "P1" : "P2"));
    }

    public bool IsPlayer1Turn()
    {
        return isPlayer1Turn;
    }
}
