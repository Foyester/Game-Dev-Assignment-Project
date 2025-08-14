///simple script that waits to be called by DeployManager to spawn the prefab with UnitData

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

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

    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"UI element clicked: selecting {unitData.unitName}");
        deployManager.SelectUnitToPlace(unitData);
    }
}




