using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeploymentManager : MonoBehaviour
{
    public Tilemap player1DeployTM;
    public Tilemap player2DeployTM;
    public Camera mainCamera;
    public Transform placeholderParent; // Parent object for visual units
    public GameObject placeholderPrefab; // Visual-only unit prefab

    public Transform player1CameraPos;
    public Transform player2CameraPos;

    public Transform unitUIPanel; // Where UI unit icons are listed
    public GameObject unitUIElementPrefab;

    private int currentPlayer = 1;
    private List<UnitData> player1Units;
    private List<UnitData> player2Units;

    private UnitData selectedUnit;
    private Tilemap activeDeployTM;

    void Start()
    {
        // Load chosen units from Drafting phase
        player1Units = DraftData.Instance.player1Units;
        player2Units = DraftData.Instance.player2Units;

        SetupDeploymentUI();
        SwitchToPlayer(1);
    }

    void SetupDeploymentUI()
    {
        foreach (Transform child in unitUIPanel)
            Destroy(child.gameObject);

        List<UnitData> unitsToShow = (currentPlayer == 1) ? player1Units : player2Units;

        foreach (UnitData unit in unitsToShow)
        {
            GameObject uiObj = Instantiate(unitUIElementPrefab, unitUIPanel);
            uiObj.GetComponent<ChosenDeployUnitUI>().Setup(unit, this);
        }
    }

    public void SelectUnitToPlace(UnitData unit)
    {
        selectedUnit = unit;
    }

    void Update()
    {
        if (selectedUnit != null && Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = activeDeployTM.WorldToCell(worldPos);

            if (activeDeployTM.HasTile(cellPos))
            {
                // Place visual placeholder
                GameObject placedObj = Instantiate(placeholderPrefab, placeholderParent);
                placedObj.transform.position = activeDeployTM.GetCellCenterWorld(cellPos);

                DeploymentUnit du = placedObj.GetComponent<DeploymentUnit>();
                du.Setup(selectedUnit, cellPos);

                // Save to the correct player's deployment list
                DeploymentData.Instance.AddDeployment(
                    currentPlayer == 1,
                    selectedUnit,
                    cellPos
                );

                // Remove from UI list
                RemoveUnitFromUI(selectedUnit);

                selectedUnit = null;
            }
            else
            {
                Debug.Log("Invalid deployment tile!");
            }
        }
    }

    void RemoveUnitFromUI(UnitData unit)
    {
        if (currentPlayer == 1)
            player1Units.Remove(unit);
        else
            player2Units.Remove(unit);

        SetupDeploymentUI();
    }

    public void ConfirmDeployment()
    {
        if (currentPlayer == 1)
        {
            SwitchToPlayer(2);
        }
        else
        {
            EndDeploymentPhase();
        }
    }

    void SwitchToPlayer(int player)
    {
        currentPlayer = player;
        activeDeployTM = (player == 1) ? player1DeployTM : player2DeployTM;
        mainCamera.transform.position = (player == 1) ? player1CameraPos.position : player2CameraPos.position;
        SetupDeploymentUI();
    }

    void EndDeploymentPhase()
    {
        // Go to gameplay scene with stored deployment data
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameplayScene");
    }
}







