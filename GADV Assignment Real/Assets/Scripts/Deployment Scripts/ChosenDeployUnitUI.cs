using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Needed for click detection

public class ChosenDeployUnitUI : MonoBehaviour, IPointerClickHandler
{
    public Image unitImage;
    public TMP_Text unitName;
    private UnitData unitData;
    private DeploymentManager deployManager;

    public void Setup(UnitData data, DeploymentManager manager)
    {
        unitData = data;
        deployManager = manager;
        unitImage.sprite = data.icon;
        unitName.text = data.unitName;

        Debug.Log($"Setup complete for {data.unitName}");
    }

    // This will fire when the UI element is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"UI element clicked: selecting {unitData.unitName}");
        deployManager.SelectUnitToPlace(unitData);
    }
}



