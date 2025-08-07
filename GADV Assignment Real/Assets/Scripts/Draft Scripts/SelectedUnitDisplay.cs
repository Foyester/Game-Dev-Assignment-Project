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

    /// <summary>
    /// Initializes the selected unit UI display.
    /// </summary>
    /// <param name="unit">The unit data to display.</param>
    /// <param name="manager">The draft manager to notify on removal.</param>
    /// <param name="self">The prefab object this script is on (for deletion).</param>
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

    /// <summary>
    /// Called when this selected unit is clicked — removes the unit from the draft.
    /// </summary>
    private void OnClickRemove()
    {
        if (draftManager != null && unitData != null)
        {
            draftManager.RemoveUnit(unitData, selfReference);
        }
    }
}


