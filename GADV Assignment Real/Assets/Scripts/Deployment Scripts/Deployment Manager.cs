///This is a fairly meaty script because i kinda stopped following ORP laws after the first scene. This scene manages a lot of things tied to the deployment 
///logic during the deployment scene. It has two tilemaps to refer to for where the player can deploy since not only is it less tedious to code manually but also
///allows me to change the zones if needed by simply editing the map. This idea of adjustable locations also applies to the camera and it's empty game object positioners
///taken using inspiration from the dropper game. I've used a dictonary to keep track of the unit locations to prevent double stacks and allow for recall
///unlike an array that  would prolly combust with the idea of removal. I've also created UI prefabs rather than spawing the actual prefabs with logic in them out of fear 
///that their logic would break the scene and class with existing code. Alot of debug logs for checking what broke as well.


using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeploymentManager : MonoBehaviour
{
    [Header("Tilemaps & Camera")]
    public Tilemap player1DeployTM;
    public Tilemap player2DeployTM;
    public Camera mainCamera;

    [Header("Placement Objects")]
    public Transform placeholderParent;
    public GameObject placeholderPrefab;

    [Header("Camera Positions")]
    public Transform player1CameraPos;
    public Transform player2CameraPos;

    [Header("UI")]
    public Transform unitUIPanel;
    public GameObject unitUIElementPrefab;
    public Button confirmButton; 

    private int currentPlayer = 1;
    private List<UnitData> player1Units;
    private List<UnitData> player2Units;

    private UnitData selectedUnit;
    private Tilemap activeDeployTM;

    // Track deployed units for double stacking prevention and recall
    private Dictionary<Vector3Int, GameObject> deployedUnits = new Dictionary<Vector3Int, GameObject>();

    void Start()
    {
        Debug.Log("DeploymentManager Start: Loading drafted units from DraftData.");
        player1Units = new List<UnitData>(DraftData.Instance.player1Units);
        player2Units = new List<UnitData>(DraftData.Instance.player2Units);

        Debug.Log("Clearing previous deployments in DeploymentData.");
        DeploymentData.Instance.ClearDeployments();

        // Hook up confirm button if assigned
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(ConfirmDeployment);
            Debug.Log("Confirm button hooked up in Start().");
        }
        else
        {
            Debug.LogWarning("Confirm button not assigned in the Inspector!");
        }

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
        Debug.Log("Update called");

        // Safety checks and logs first
        if (DeploymentData.Instance == null)
        {
            Debug.LogError("DeploymentData.Instance is NULL! Did you forget to put DeploymentData in the scene?");
            return;
        }
        else
        {
            Debug.Log("DeploymentData.Instance found");
        }

        if (player1DeployTM == null)
        {
            Debug.LogError("Player1DeployTM is NULL! Did you assign the Tilemap in the Inspector?");
            return;
        }
        else
        {
            Debug.Log("Player1DeployTM reference found");
        }
        if (player2DeployTM == null)
        {
            Debug.LogError("Player2DeployTM is NULL! Did you assign the Tilemap in the Inspector?");
            return;
        }
        else
        {
            Debug.Log("Player2DeployTM reference found");
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = activeDeployTM.WorldToCell(worldPos);

            // 1) Check if clicking on existing deployed unit to recall (undeploy) it
            if (deployedUnits.ContainsKey(cellPos))
            {
                Debug.Log($"Clicked deployed unit at {cellPos}, recalling...");
                RecallUnit(cellPos);
                return;  // Early exit: don't place another unit this click
            }

            // 2) If no deployed unit clicked, attempt to place selected unit (if any)
            if (selectedUnit != null)
            {
                if (activeDeployTM.HasTile(cellPos))
                {
                    Debug.Log("Valid deployment tile found.");

                    // Prevent double-stacking
                    if (deployedUnits.ContainsKey(cellPos))
                    {
                        Debug.LogWarning($"Tile {cellPos} already occupied! Cannot deploy unit here.");
                        return;
                    }

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

                    deployedUnits[cellPos] = placedObj; // Track deployed unit

                    Debug.Log($"Added deployment data for unit {selectedUnit.unitName} at {cellPos}.");

                    RemoveUnitFromUI(selectedUnit);

                    selectedUnit = null;
                }
                else
                {
                    Debug.LogWarning("Invalid deployment tile! Cannot place unit here.");
                }
            }
            else
            {
                Debug.Log("No unit selected to place.");
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

    // Recall deployed unit and return it to selection list
    private void RecallUnit(Vector3Int cellPos)
    {
        if (deployedUnits.TryGetValue(cellPos, out GameObject unitObj))
        {
            DeploymentUnit du = unitObj.GetComponent<DeploymentUnit>();

            // Remove deployment data record
            DeploymentData.Instance.RemoveDeployment(currentPlayer == 1, du.unitData, cellPos);

            Debug.Log($"Recalling unit {du.unitData.unitName} from {cellPos} back to selection.");

            // Return unit to player's deployment list
            if (currentPlayer == 1)
            {
                player1Units.Add(du.unitData);
            }
            else
            {
                player2Units.Add(du.unitData);
            }

            Destroy(unitObj);
            deployedUnits.Remove(cellPos);

            SetupDeploymentUI();
        }
        else
        {
            Debug.LogWarning($"No deployed unit found at {cellPos} to recall.");
        }
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










