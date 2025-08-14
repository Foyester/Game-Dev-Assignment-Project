
///it basically fills the data in the UI prefab that is generated while also having a call to UnitDraftMAnager when the
///button is pressed for removal.


using UnityEngine;
using UnityEngine.UI;

public class SelectedUnitDisplay : MonoBehaviour
{
    [Header("UI Elements")]
    public Image iconImage;
    public Button button;
    private UnitData unitData;
    private UnitDraftManager draftManager;
    private GameObject selfReference;

    
    public void Setup(UnitData unit, UnitDraftManager manager, GameObject self)
    {
        unitData = unit;
        draftManager = manager;
        selfReference = self;

        if (iconImage != null && unit.icon != null)
        {
            iconImage.sprite = unit.icon;
        }
        else
        {
            Debug.LogWarning("Icon image or unit icon not assigned.");
        }

        // Register button click to remove this unit
        Button button = GetComponentInChildren<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClickRemove);
        }
        else
        {
            Debug.LogError("SelectedUnitDisplay is missing a Button component.");
        }
    }

    
    private void OnClickRemove()
    {
        if (draftManager != null && unitData != null)
        {
            draftManager.RemoveUnit(unitData, selfReference);
        }
    }
}








