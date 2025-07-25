using UnityEngine;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(UnitManager))]
public class UnitCombat : MonoBehaviour
{
    private Unit unit;
    private UnitManager manager;

    private void Start()
    {
        unit = GetComponent<Unit>();
        manager = GetComponent<UnitManager>();
    }

    public void HandleAttackInput()
    {
        if (unit.currentState != Unit.UnitState.Attacking) return;

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                GameObject target = hit.collider.gameObject;

                if (target == this.gameObject) return;

                Unit targetUnit = target.GetComponent<Unit>();
                UnitManager targetManager = target.GetComponent<UnitManager>();

                if (targetUnit != null && targetManager != null &&
                    targetManager.GetTeam() != manager.GetTeam())
                {
                    int damage = Mathf.Max(0, unit.attack - targetUnit.defense);
                    targetUnit.TakeDamage(damage);

                    Debug.Log($"{gameObject.name} attacked {target.name} for {damage}!");

                    // End the unit's turn
                    unit.SetState(Unit.UnitState.Done);
                }
            }
        }
    }

}


