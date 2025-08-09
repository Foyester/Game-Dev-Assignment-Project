using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChosenUnitDisplay : MonoBehaviour
{
    public Image unitImage;
    public TextMeshProUGUI unitNameText;
    public TextMeshProUGUI countText;

    private UnitData unitData;
    private DeploymentPhaseManager deploymentManager;
    private GameObject selfObject;

    public void Setup(UnitData data, DeploymentPhaseManager manager, GameObject obj)
    {
        unitData = data;
        deploymentManager = manager;
        selfObject = obj;

        unitImage.sprite = data.icon; // Assuming UnitData has an icon Sprite
        unitNameText.text = data.unitName;
        RefreshCount(deploymentManager.unitsLeftToDeploy);
    }

    public void RefreshCount(Dictionary<UnitData, int> unitsLeft)
    {
        if (unitsLeft.ContainsKey(unitData))
            countText.text = unitsLeft[unitData].ToString();
        else
            countText.text = "0";
    }

    // Called by UI button OnClick event
    public void OnClick()
    {
        deploymentManager.OnUnitSelected(unitData);
    }
}

