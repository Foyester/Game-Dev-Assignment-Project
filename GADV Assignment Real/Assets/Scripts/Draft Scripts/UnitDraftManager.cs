using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UnitDraftManager : MonoBehaviour
{
    [Header("Data")]
    public List<UnitData> availableUnits;

    [Header("UI References")]
    public Transform unitListContainer;
    public GameObject unitButtonPrefab;
    public Transform selectedUnitPanel;
    public GameObject selectedUnitPrefab;
    public TextMeshProUGUI pointsText;
    public Button confirmButton;

    [Header("Draft Settings")]
    public int pointLimit = 20;

    private int currentPoints = 0;
    private List<UnitData> selectedUnits = new List<UnitData>();
    private Dictionary<UnitData, int> unitCounts = new Dictionary<UnitData, int>();
    private List<UnitButton> unitButtons = new List<UnitButton>();
    [SerializeField] private int maxUnitCount = 10;


    void Start()
    {
        PopulateUnitList();
        UpdateUI();
        confirmButton.onClick.AddListener(ConfirmSelection);
    }
    


    void PopulateUnitList()
    {
        foreach (UnitData unit in availableUnits)
        {
            GameObject buttonObj = Instantiate(unitButtonPrefab, unitListContainer);
            UnitButton btn = buttonObj.GetComponent<UnitButton>();
            btn.Setup(unit, this);
            unitButtons.Add(btn); 
        }
    }

    public void TryAddUnit(UnitData unit)
    {
        if (selectedUnits.Count >= maxUnitCount)
        {
            Debug.Log("Cannot add more units. Max unit count reached.");
            return;
        }

        if (currentPoints + unit.cost > pointLimit)
        {
            Debug.Log("Too expensive!");
            return;
        }

        if (!unitCounts.ContainsKey(unit))
            unitCounts[unit] = 0;

        if (unitCounts[unit] >= unit.maxCopies)
        {
            Debug.Log($"Cannot add more than {unit.maxCopies} copies of {unit.unitName}");
            return;
        }

        selectedUnits.Add(unit);
        unitCounts[unit]++;
        currentPoints += unit.cost;

       

        GameObject selectedObj = Instantiate(selectedUnitPrefab, selectedUnitPanel);
        SelectedUnitDisplay display = selectedObj.GetComponent<SelectedUnitDisplay>();
        display.Setup(unit, this, selectedObj);

        UpdateUI();
        RefreshAllButtons();

    }

    public void RemoveUnit(UnitData unit, GameObject displayObject)
    {
        if (selectedUnits.Contains(unit))
        {
            selectedUnits.Remove(unit);
            unitCounts[unit]--;
            currentPoints -= unit.cost;
            Destroy(displayObject);
            UpdateUI();
            RefreshAllButtons(); 
        }
    }

    void UpdateUI()
    {
        pointsText.text = $"Points: {currentPoints}/{pointLimit}";
        confirmButton.interactable = selectedUnits.Count > 0;
    }

    private void RefreshAllButtons()
    {
        foreach (UnitButton button in unitButtons)
        {
            button.RefreshCount(); 
        }
    }

    public void ConfirmSelection()
    {
        if (DraftData.Instance.isPlayer1Turn)
        {
            DraftData.Instance.player1Units = new List<UnitData>(selectedUnits);
            DraftData.Instance.isPlayer1Turn = false;
            SceneManager.LoadScene("Player2DraftScene"); 
        }
        else
        {
            DraftData.Instance.player2Units = new List<UnitData>(selectedUnits);
            SceneManager.LoadScene("DeploymentScene"); 
        }
    }



    public int GetUnitCount(UnitData unit)
    {
        return unitCounts.ContainsKey(unit) ? unitCounts[unit] : 0;
    }
}



