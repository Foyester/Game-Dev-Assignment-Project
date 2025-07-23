using UnityEngine;

[RequireComponent(typeof(UnitMover))]
public class UnitManager : MonoBehaviour
{
    public enum PlayerTeam { Player1, Player2 }
    public PlayerTeam team;

    private TurnManager turnManager;
    private UnitMover mover;

    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
        mover = GetComponent<UnitMover>();
    }

    // Can this unit act this turn?
    public bool CanActThisTurn()
    {
        // Compare this unit’s team to the active team from TurnManager
        if (team == PlayerTeam.Player1 && turnManager.IsPlayer1Turn())
            return true;
        if (team == PlayerTeam.Player2 && !turnManager.IsPlayer1Turn())
            return true;

        return false;
    }

    // Called when unit is clicked and deemed eligible
    public void TrySelect()
    {
        if (CanActThisTurn())
        {
            mover.EnableMovement();
        }
        else
        {
            Debug.Log("Not your turn!");
        }
    }
}

