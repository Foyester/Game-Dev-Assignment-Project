using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChosenDeployUnitUI : MonoBehaviour
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
        unitName.text= data.unitName;
    }

    public void OnClick()
    {
        deployManager.SelectUnitToPlace(unitData);
    }
}


