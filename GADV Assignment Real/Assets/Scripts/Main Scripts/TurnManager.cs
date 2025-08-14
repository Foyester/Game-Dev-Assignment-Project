///I could've used a list or something but since i was determined to avoid work and my specifications were two players, i just used a boolean to track turns.
///this is a very early code so i sucked and hence yeahhhhh, boolean simple.  Not expadable for a third party.
using UnityEngine;



public class TurnManager : MonoBehaviour
{
    
    public bool isPlayer1Turn = true;

    public void EndTurn()
    {
        
        isPlayer1Turn = !isPlayer1Turn;

        
        Debug.Log("Current Turn: " + (isPlayer1Turn ? "P1" : "P2"));
    }

    public bool IsPlayer1Turn()
    {
        return isPlayer1Turn;
    }
}
