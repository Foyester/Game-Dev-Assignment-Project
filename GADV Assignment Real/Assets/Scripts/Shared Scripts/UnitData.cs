using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Units/Unit Data", order = 1)]
public class UnitData : ScriptableObject
{
    [Header("General Info")]
    public string unitName;
    public int cost;
    public Sprite icon;
    public GameObject prefab;
    public int maxCopies = 3;

    [Header("Deployment Info (Runtime Only)")]
    [HideInInspector] public bool isDeployed = false; // True after being placed in deployment phase
    [HideInInspector] public Vector3Int deployedTile; // Grid position where the unit is deployed
}




