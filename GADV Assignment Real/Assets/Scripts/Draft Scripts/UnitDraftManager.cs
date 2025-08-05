using UnityEngine;

public Transform availableUnitsPanel;
public GameObject unitButtonPrefab;

void PopulateAvailableUnits()
{
    foreach (UnitData data in availableUnits)
    {
        GameObject btn = Instantiate(unitButtonPrefab, availableUnitsPanel);
        btn.GetComponent<UnitButton>().Setup(data, this);
    }
}
}
