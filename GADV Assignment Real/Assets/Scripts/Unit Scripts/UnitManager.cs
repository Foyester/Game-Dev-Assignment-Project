using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public enum PlayerTeam { Player1, Player2 }
    public PlayerTeam team;

    private TurnManager turnManager;

    void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
    }

    public bool CanActThisTurn()
    {
        return (team == PlayerTeam.Player1 && turnManager.IsPlayer1Turn()) ||
               (team == PlayerTeam.Player2 && !turnManager.IsPlayer1Turn());
    }

    public PlayerTeam GetTeam()
    {
        return team;
    }
}



