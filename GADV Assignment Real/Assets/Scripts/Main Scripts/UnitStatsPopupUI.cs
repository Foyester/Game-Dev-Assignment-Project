///This is the code that actually fills the data by calling from both unit.cs and unitdata.cs since my dumbass didn't plan ahead and has 
///two sets to get the full spectrum of info

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitStatsPopupUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image portraitImage;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI descriptionText;

    public void Show(Unit unit)
    {
        // Pull from UnitData (meta info)
        if (unit.unitData != null)
        {
            nameText.text = unit.unitData.unitName;
            portraitImage.sprite = unit.unitData.icon;
            descriptionText.text = unit.unitData.description;
        }
        else
        {
            nameText.text = "Unknown Unit";
            portraitImage.sprite = null;
            descriptionText.text = "";
        }

        
        statsText.text =
            $"HP: {unit.currentHP}\n" +
            $"ATK: {unit.attack}\n" +
            $"DEF: {unit.defense}\n" +
            $"Move: {unit.movementRange}\n" +
            $"Range: {unit.attackRange}";

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}


