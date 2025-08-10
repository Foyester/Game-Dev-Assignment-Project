using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeploymentInfo
{
    public UnitData unitData;      // Which unit this is (reference from DraftData)
    public Vector3Int gridPosition; // Where it’s placed on the map (tilemap coords)

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

    public void ClearDeployments()
    {
        player1Deployed.Clear();
        player2Deployed.Clear();
    }
}

