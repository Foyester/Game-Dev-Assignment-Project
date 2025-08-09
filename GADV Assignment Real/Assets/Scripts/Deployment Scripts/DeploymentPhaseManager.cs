using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DeploymentPhaseManager : MonoBehaviour
{
    [Header("Draft Data (set this from DraftData singleton)")]
    public List<UnitData> player1Units;
    public List<UnitData> player2Units;

    [Header("UI References")]
    public Transform selectedUnitPanel;        // Panel showing units left to deploy
    public GameObject selectedUnitUIPrefab;    // UI prefab showing unit image + name + count
    public TextMeshProUGUI playerTurnText;
    public Button confirmButton;

    [Header("Tilemaps")]
    public Tilemap player1DeployTilemap;
    public Tilemap player2DeployTilemap;

    [Header("Visual Prefab")]
    public GameObject deploymentVisualPrefab; // Simple visual prefab to instantiate on map (no combat scripts)

    private int currentPlayerIndex = 1; // 1 or 2
    private List<UnitData> unitsToDeploy;
    private Dictionary<UnitData, int> unitsLeftToDeploy = new Dictionary<UnitData, int>();

    private List<GameObject> deployedVisuals = new List<GameObject>();

    private UnitData selectedUnit = null; // Currently selected unit in UI

    void Start()
    {
        SetupPlayer(1);
        confirmButton.onClick.AddListener(OnConfirmDeployment);
    }

    void SetupPlayer(int player)
    {
        currentPlayerIndex = player;
        playerTurnText.text = $"Player {player}'s Deployment";

        // Clear old UI + visuals
        foreach (Transform child in selectedUnitPanel) Destroy(child.gameObject);
        foreach (GameObject go in deployedVisuals) Destroy(go);
        deployedVisuals.Clear();
        unitsLeftToDeploy.Clear();

        // Load the player's units
        unitsToDeploy = (player == 1) ? new List<UnitData>(player1Units) : new List<UnitData>(player2Units);

        // Count units by type
        foreach (var unit in unitsToDeploy)
        {
            if (!unitsLeftToDeploy.ContainsKey(unit))
                unitsLeftToDeploy[unit] = 0;
            unitsLeftToDeploy[unit]++;
        }

        // Populate UI list
        foreach (var kvp in unitsLeftToDeploy)
        {
            GameObject uiObj = Instantiate(selectedUnitUIPrefab, selectedUnitPanel);
            SelectedUnitDisplay display = uiObj.GetComponent<SelectedUnitDisplay>();
            display.Setup(kvp.Key, this, uiObj);
        }

        selectedUnit = null; // reset selection
        confirmButton.interactable = unitsLeftToDeploy.Count == 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedUnit == null)
            {
                Debug.Log("Select a unit type from the panel before deploying.");
                return;
            }

            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedCell = player1DeployTilemap.WorldToCell(mouseWorld);

            // Check deployment zone based on current player
            bool canDeployHere = false;
            if (currentPlayerIndex == 1)
                canDeployHere = player1DeployTilemap.HasTile(clickedCell);
            else if (currentPlayerIndex == 2)
                canDeployHere = player2DeployTilemap.HasTile(clickedCell);

            if (!canDeployHere)
            {
                Debug.Log("Cannot deploy here! Not in deployment zone.");
                return;
            }

            // Check if tile already occupied by a visual
            foreach (var visual in deployedVisuals)
            {
                Vector3Int visualCell = player1DeployTilemap.WorldToCell(visual.transform.position);
                if (visualCell == clickedCell)
                {
                    Debug.Log("Tile already occupied.");
                    return;
                }
            }

            // Place visual prefab at tile center
            Vector3 spawnPos = player1DeployTilemap.GetCellCenterWorld(clickedCell);
            GameObject newVisual = Instantiate(deploymentVisualPrefab, spawnPos, Quaternion.identity);
            DeploymentVisual dv = newVisual.GetComponent<DeploymentVisual>();
            dv.unitData = selectedUnit;
            deployedVisuals.Add(newVisual);

            // Decrement count & update UI
            unitsLeftToDeploy[selectedUnit]--;
            if (unitsLeftToDeploy[selectedUnit] <= 0)
            {
                unitsLeftToDeploy.Remove(selectedUnit);
                selectedUnit = null;
            }

            RefreshUnitPanel();
            confirmButton.interactable = unitsLeftToDeploy.Count == 0;
        }
    }

    public void OnUnitSelected(UnitData unit)
    {
        if (!unitsLeftToDeploy.ContainsKey(unit))
        {
            Debug.Log("No more of this unit left to deploy.");
            return;
        }
        selectedUnit = unit;
        Debug.Log($"Selected unit to deploy: {unit.unitName}");
    }

    void RefreshUnitPanel()
    {
        // Update all UI counts in the panel
        foreach (Transform child in selectedUnitPanel)
        {
            SelectedUnitDisplay display = child.GetComponent<SelectedUnitDisplay>();
            if (display != null)
                display.RefreshCount(unitsLeftToDeploy);
        }
    }

    void OnConfirmDeployment()
    {
        if (currentPlayerIndex == 1)
        {
            // Switch to player 2
            SetupPlayer(2);

            // Optionally: Move camera to player 2 side here
            // Example: Camera.main.transform.position = new Vector3(...);
        }
        else
        {
            // Finish deployment phase

            // Prepare data to pass to gameplay scene
            BattleData.Instance.player1Deployment = GetDeploymentDataForPlayer(1);
            BattleData.Instance.player2Deployment = GetDeploymentDataForPlayer(2);

            // Load gameplay scene
            SceneManager.LoadScene("GameplayScene");
        }
    }

    List<DeploymentInfo> GetDeploymentDataForPlayer(int player)
    {
        List<DeploymentInfo> deploymentInfos = new List<DeploymentInfo>();

        foreach (var visual in deployedVisuals)
        {
            DeploymentVisual dv = visual.GetComponent<DeploymentVisual>();

            // Assuming DeploymentVisual stores unit data and position
            Vector3Int cell = player1DeployTilemap.WorldToCell(visual.transform.position);

            if (player == 1 && player1DeployTilemap.HasTile(cell))
                deploymentInfos.Add(new DeploymentInfo { unitData = dv.unitData, cellPosition = cell });
            else if (player == 2 && player2DeployTilemap.HasTile(cell))
                deploymentInfos.Add(new DeploymentInfo { unitData = dv.unitData, cellPosition = cell });
        }

        return deploymentInfos;
    }
}

[System.Serializable]
public class DeploymentInfo
{
    public UnitData unitData;
    public Vector3Int cellPosition;
}





