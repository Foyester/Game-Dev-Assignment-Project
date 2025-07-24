using UnityEngine;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(UnitManager))]
public class UnitCombat : MonoBehaviour
{
    private Unit unit;
    private UnitManager unitManager;

    private void Start()
    {
        unit = GetComponent<Unit>();
        unitManager = GetComponent<UnitManager>();
    }

    // Called when player right-clicks on a unit to attack it
    public void TryAttack(GameObject targetGO)
    {
        if (targetGO == null || targetGO == this.gameObject)
        {
            Debug.Log("Invalid target: can't attack self or null.");
            return;
        }

        Unit targetUnit = targetGO.GetComponent<Unit>();
        UnitManager targetManager = targetGO.GetComponent<UnitManager>();

        if (targetUnit == null || targetManager == null)
        {
            Debug.Log("Target is not a valid unit.");
            return;
        }

        if (targetManager.GetTeam() == unitManager.GetTeam())
        {
            Debug.Log("Can't attack friendly units.");
            return;
        }

        // Basic deterministic damage
        int rawDamage = Mathf.Max(0, unit.attack - targetUnit.defense);
        Debug.Log($"{gameObject.name} attacks {targetGO.name} for {rawDamage} damage!");

        targetUnit.TakeDamage(rawDamage);
    }
}

