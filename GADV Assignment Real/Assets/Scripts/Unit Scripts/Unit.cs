using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName = "Soldier";
    public int maxHP = 10;
    public int currentHP;
    public int attack = 3;
    public int movementRange = 3;

    public bool hasMoved = false;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(unitName + " has been defeated.");
        Destroy(gameObject);
    }
}
