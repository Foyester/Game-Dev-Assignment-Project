using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public enum PlayerTeam { Player1, Player2 }
    public PlayerTeam team;

    private TurnManager turnManager;
    private UnitMover mover;

    void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
        mover = GetComponent<UnitMover>();
    }

    // Call this when trying to interact with the unit (e.g., when clicked)
    public bool CanActThisTurn()
    {
        // Compare unit team with current turn
        if (team == PlayerTeam.Player1 && turnManager.IsPlayer1Turn())
            return true;
        if (team == PlayerTeam.Player2 && !turnManager.IsPlayer1Turn())
            return true;

        return false;
    }

    public void TrySelect()
    {
        if (CanActThisTurn())
        {
            mover.EnableMovement(); // tells UnitMover it's ok to move
        }
        else
        {
            Debug.Log("Not your turn!");
        }
    }
}
