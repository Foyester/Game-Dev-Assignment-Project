using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour
{
    public static BattleData Instance;

    // Store deployment info for both players
    public List<DeploymentInfo> player1Deployment = new List<DeploymentInfo>();
    public List<DeploymentInfo> player2Deployment = new List<DeploymentInfo>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject); // Enforce singleton uniqueness
        }
    }
}

