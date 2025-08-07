using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitButton : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI copiesLeftText;
    public Button selectButton;

    private UnitData data;
    private UnitDraftManager manager;

    public void Setup(UnitData unit, UnitDraftManager draftManager)
    {
        data = unit;
        manager = draftManager;

        iconImage.sprite = unit.icon;
        nameText.text = unit.unitName;
        costText.text = $"{unit.cost}";

        selectButton.onClick.AddListener(() => manager.TryAddUnit(data));
        RefreshCount();
    }
    public void RefreshCount()
    {
        int used = manager.GetUnitCount(data);
        int max = data.maxCopies;
        copiesLeftText.text = $"{max - used}/{max}";
    }

    public UnitData GetUnitData()
    {
        return data;
    }
}


