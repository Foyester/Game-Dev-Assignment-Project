///this just checks what player a unit belongs to and prevents units from acting on an enemy turn.

using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitManager : MonoBehaviour
{
    public enum PlayerTeam { Player1, Player2 }
    public PlayerTeam team;  

    private TurnManager turnManager;

    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();

        
        if (!turnManager)
            Debug.LogWarning("TurnManager not found!");

        
        Debug.Log($"{gameObject.name} assigned to team: {team}");
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

    private void OnDestroy()
    {
        Debug.Log($"{gameObject.name} was destroyed. Team: {team}");
    }
}




