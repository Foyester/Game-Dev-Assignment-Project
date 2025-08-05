using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitState
    {
        Idle,
        Moving,
        Attacking,
        Done
    }

    public UnitState currentState = UnitState.Idle;


    public int maxHP = 10;
    public int currentHP = 10;
    public int attack = 4;
    public int defense = 2;
    public int movementRange = 3;
    public bool hasMoved = false;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        Debug.Log($"{gameObject.name} takes {dmg} damage! HP: {currentHP}");

        if (currentHP <= 0)
        {
            Debug.Log($"{gameObject.name} is dead!");
            Die();
        }
    }

    public void SetState(UnitState newState)
    {
        Debug.Log($"{gameObject.name} changes state to {newState}");
        currentState = newState;
    }

    public bool CanAct()
    {
        return currentState == UnitState.Idle || currentState == UnitState.Moving || currentState == UnitState.Attacking;
    }

    private void Die()
    {
        VictoryCondition vm = FindObjectOfType<VictoryCondition>();
        if (vm != null)
        {
            Debug.Log("VictoryCondition found. Starting coroutine...");
            vm.StartCoroutine(vm.CheckForWinAfterDelay());
        }
        else
        {
            Debug.LogWarning("VictoryCondition not found!");
        }

        Destroy(gameObject);
    }


}

