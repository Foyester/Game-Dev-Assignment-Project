using UnityEngine;
using UnityEngine.Tilemaps;

public class DeploySelectionInGame : MonoBehaviour
{
    public Tilemap deploymentTilemap;
    public GameObject unitPrefabFallback; // optional, if UnitData prefab is null

    private TurnManager turnManager;

    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
        if (!turnManager)
        {
            Debug.LogError("TurnManager not found in scene!");
        }

        SpawnDeployedUnits();
    }

    private void SpawnDeployedUnits()
    {
        // Spawn P1's units
        foreach (var info in DeploymentData.Instance.player1Deployed)
        {
            SpawnUnit(info, UnitManager.PlayerTeam.Player1);
        }

        // Spawn P2's units
        foreach (var info in DeploymentData.Instance.player2Deployed)
        {
            SpawnUnit(info, UnitManager.PlayerTeam.Player2);
        }
    }

    private void SpawnUnit(DeploymentInfo info, UnitManager.PlayerTeam team)
    {
        if (info == null || info.unitData == null)
        {
            Debug.LogWarning("DeploymentInfo or UnitData is null, skipping...");
            return;
        }

        GameObject prefabToSpawn = info.unitData.prefab != null
            ? info.unitData.prefab
            : unitPrefabFallback;

        if (prefabToSpawn == null)
        {
            Debug.LogError($"No prefab assigned for {info.unitData.name} and no fallback prefab set.");
            return;
        }

        Vector3 worldPos = deploymentTilemap.GetCellCenterWorld(info.gridPosition);
        GameObject spawnedUnit = Instantiate(prefabToSpawn, worldPos, Quaternion.identity);

        
        UnitManager unitManager = spawnedUnit.GetComponent<UnitManager>();
        if (unitManager)
        {
            unitManager.team = team;
        }
        else
        {
            Debug.LogWarning($"Spawned unit {spawnedUnit.name} has no UnitManager, cannot assign team.");
        }
    }
}



