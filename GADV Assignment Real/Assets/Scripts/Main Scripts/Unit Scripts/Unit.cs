///This sceipt basically provides the values for the unit prefabs such as it's state and stats. It also takes charge of combat math and killing itself in destory 
///gameobject. I used states in enum because i was having a stroke trying to do the clicking logic code and this idea was recommended by both rediit and chatgpt
///i also made it so the minimum dmg done has to be 3 because of my flat formula and the very real possibility of someone doing 0 damage. last is the vic condition
///which it needs cuz it calls it after a unit dies since the victory conditions is total annahiliation
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData unitData; 
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
    public int attackRange = 3;
    public bool hasMoved = false;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        
        int actualDamage = Mathf.Max(dmg, 3);

        currentHP -= actualDamage;
        Debug.Log($"{gameObject.name} takes {actualDamage} damage! HP: {currentHP}");

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

