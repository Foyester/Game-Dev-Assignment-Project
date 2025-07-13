using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private int maxHP = 10;
    private int currentHP;
    [SerializeField] private int attack = 3;
    [SerializeField] private int movementRange = 3;
    [SerializeField] private int attackRange = 1;

    private bool hasMoved = false;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }

    public void MarkAsMoved()
    {
        hasMoved = true;
    }

    public bool HasMoved()
    {
        return hasMoved;
    }

    public int GetCurrentHP() => currentHP;
    public int GetAttackPower() => attack;
    public int GetMovementRange() => movementRange;
    public int GetAttackRange() => attackRange;

    private void Die()
    {
        Destroy(gameObject);
    }
}
