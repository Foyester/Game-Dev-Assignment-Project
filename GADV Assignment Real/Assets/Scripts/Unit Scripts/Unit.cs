using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName = "Soldier";
    public int maxHP = 10;
    public int currentHP;
    public int attack = 3;
    public int defense = 1;
    public int movementRange = 1;
    public int attackRange = 1;

    public bool hasMoved = false; 
    public bool hasAttacked = false; 

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        Debug.Log($"{gameObject.name} took {dmg} damage. Remaining HP: {currentHP}");

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
