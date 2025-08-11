using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeploymentManager : MonoBehaviour
{
    public Tilemap player1DeployTM;
    public Tilemap player2DeployTM;
    public Camera mainCamera;
    public Transform placeholderParent;
    public GameObject placeholderPrefab;

    public Transform player1CameraPos;
    public Transform player2CameraPos;

    public Transform unitUIPanel;
    public GameObject unitUIElementPrefab;

    private int currentPlayer = 1;
    private List<UnitData> player1Units;
    private List<UnitData> player2Units;

    private UnitData selectedUnit;
    private Tilemap activeDeployTM;

    void Start()
    {
        Debug.Log("DeploymentManager Start: Loading drafted units from DraftData.");
        player1Units = new List<UnitData>(DraftData.Instance.player1Units);
        player2Units = new List<UnitData>(DraftData.Instance.player2Units);

        Debug.Log("Clearing previous deployments in DeploymentData.");
        DeploymentData.Instance.ClearDeployments();

        SetupDeploymentUI();
        SwitchToPlayer(1);
    }

    void SetupDeploymentUI()
    {
        Debug.Log($"Setting up deployment UI for Player {currentPlayer} with {((currentPlayer == 1) ? player1Units.Count : player2Units.Count)} units.");

        foreach (Transform child in unitUIPanel)
        {
            Destroy(child.gameObject);
        }

        List<UnitData> unitsToShow = (currentPlayer == 1) ? player1Units : player2Units;

        foreach (UnitData unit in unitsToShow)
        {
            GameObject uiObj = Instantiate(unitUIElementPrefab, unitUIPanel);
            uiObj.GetComponent<ChosenDeployUnitUI>().Setup(unit, this);
            Debug.Log($"Added UI element for unit: {unit.unitName}");
        }
    }

    public void SelectUnitToPlace(UnitData unit)
    {
        selectedUnit = unit;
        Debug.Log($"Selected unit to place: {unit.unitName}");
    }

    void Update()
    {
        if (selectedUnit != null && Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = activeDeployTM.WorldToCell(worldPos);

            Debug.Log($"Clicked cell position: {cellPos}");

            if (activeDeployTM.HasTile(cellPos))
            {
                Debug.Log("Valid deployment tile found.");

                GameObject placedObj = Instantiate(placeholderPrefab, placeholderParent);
                placedObj.transform.position = activeDeployTM.GetCellCenterWorld(cellPos);

                DeploymentUnit du = placedObj.GetComponent<DeploymentUnit>();
                du.Setup(selectedUnit, cellPos);

                Debug.Log($"Placing visual placeholder for unit {selectedUnit.unitName} at {cellPos}.");

                DeploymentData.Instance.AddDeployment(
                    currentPlayer == 1,
                    selectedUnit,
                    cellPos
                );

                Debug.Log($"Added deployment data for unit {selectedUnit.unitName} at {cellPos}.");

                RemoveUnitFromUI(selectedUnit);

                selectedUnit = null;
            }
            else
            {
                Debug.LogWarning("Invalid deployment tile! Cannot place unit here.");
            }
        }
    }

    void RemoveUnitFromUI(UnitData unit)
    {
        if (currentPlayer == 1)
        {
            player1Units.Remove(unit);
            Debug.Log($"Removed unit {unit.unitName} from Player 1's deployment UI list.");
        }
        else
        {
            player2Units.Remove(unit);
            Debug.Log($"Removed unit {unit.unitName} from Player 2's deployment UI list.");
        }

        SetupDeploymentUI();
    }

    public void ConfirmDeployment()
    {
        Debug.Log($"Player {currentPlayer} confirmed deployment.");

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

        Debug.Log($"Switched to Player {player}'s deployment phase. Camera moved and deployment tiles set.");

        SetupDeploymentUI();
    }

    void EndDeploymentPhase()
    {
        Debug.Log("Deployment phase ended. Loading GameScene.");

        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}









