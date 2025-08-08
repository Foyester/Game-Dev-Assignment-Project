using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeploymentManager : MonoBehaviour
{
    [Header("References")]
    public Tilemap gridMap;
    public Tilemap deploymentZoneMap; // only contains deployable tiles
    public Transform unitUIPanel;
    public GameObject draggableUnitPrefab;
    public Button confirmButton;

    private List<UnitData> unitsToDeploy;
    private List<GameObject> placedUnits = new List<GameObject>();

    void Start()
    {
        unitsToDeploy = DraftData.Instance.player1Units; // Assuming Player 1
        confirmButton.interactable = false;
        GenerateDraggableUnits();
    }

    void GenerateDraggableUnits()
    {
        foreach (var unit in unitsToDeploy)
        {
            GameObject draggable = Instantiate(draggableUnitPrefab, unitUIPanel);
            DraggableUnit dragScript = draggable.GetComponent<DraggableUnit>();
            dragScript.Setup(unit, this, gridMap, deploymentZoneMap);
        }
    }

    public void OnUnitPlaced(GameObject unitGO)
    {
        if (!placedUnits.Contains(unitGO))
            placedUnits.Add(unitGO);

        confirmButton.interactable = (placedUnits.Count == unitsToDeploy.Count);
    }

    public void OnUnitRemoved(GameObject unitGO)
    {
        placedUnits.Remove(unitGO);
        confirmButton.interactable = false;
    }

    public void ConfirmDeployment()
    {
        // You could store positions here in BattleData or GameStateManager
    }
}

