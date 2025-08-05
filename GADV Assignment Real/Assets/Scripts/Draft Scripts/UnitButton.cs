using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    public Image icon;
    public Text costText;
    private UnitData unitData;
    private UnitDraftManager draftManager;

    public void Setup(UnitData data, UnitDraftManager manager)
    {
        unitData = data;
        draftManager = manager;

        icon.sprite = data.icon;
        costText.text = $"{data.cost} pts";
    }

    public void OnClick()
    {
        draftManager.TryAddUnit(unitData);
    }
}

