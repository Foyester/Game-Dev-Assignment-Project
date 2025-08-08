using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DraggableUnitUI : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;
    public TMP_Text nameText;

    [HideInInspector] public UnitData unitData;

    public void Setup(UnitData data)
    {
        unitData = data;

        if (iconImage != null)
            iconImage.sprite = unitData.icon;

        if (nameText != null)
            nameText.text = unitData.unitName;
    }
}

