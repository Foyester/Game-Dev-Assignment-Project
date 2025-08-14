/// A script that keeps track of the coordinates of the units location in the deployment phase and keeps itself from evaporting on scene change. List once again 
/// unit can be repeated.

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeploymentInfo
{
    public UnitData unitData;
    public Vector3Int gridPosition; 

    public DeploymentInfo(UnitData data, Vector3Int pos)
    {
        unitData = data;
        gridPosition = pos;
    }
}

public class DeploymentData : MonoBehaviour
{
    public static DeploymentData Instance;

    public List<DeploymentInfo> player1Deployed = new List<DeploymentInfo>();
    public List<DeploymentInfo> player2Deployed = new List<DeploymentInfo>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddDeployment(bool isPlayer1, UnitData unitData, Vector3Int position)
    {
        if (isPlayer1)
        {
            player1Deployed.Add(new DeploymentInfo(unitData, position));
        }
        else
        {
            player2Deployed.Add(new DeploymentInfo(unitData, position));
        }
    }
    public void RemoveDeployment(bool isPlayer1, UnitData unitData, Vector3Int position)
    {
        List<DeploymentInfo> deployments = isPlayer1 ? player1Deployed : player2Deployed;

        for (int i = deployments.Count - 1; i >= 0; i--)
        {
            DeploymentInfo info = deployments[i];
            if (info.unitData == unitData && info.gridPosition == position)
            {
                deployments.RemoveAt(i);
                Debug.Log($"Removed deployment of {unitData.unitName} at {position} for player {(isPlayer1 ? "1" : "2")}");
                return;
            }
        }
        Debug.LogWarning($"No deployment found for {unitData.unitName} at {position} to remove for player {(isPlayer1 ? "1" : "2")}");
    }


    public void ClearDeployments()
    {
        player1Deployed.Clear();
        player2Deployed.Clear();
    }
}

