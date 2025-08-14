/// A scriptable object that is only mainly used in the deployment/draft phase. but plays a role in gameplay for name, images and description in the UI. 
/// admittedly it would serve better if the stats were refered to here as well but i made the game scene first and only created this in the drafting phase, 
/// hence rather than rewrite the code, i just used it to refer to the game prefab with the gameplay scripts and stats. I only added the other stats for referencing 
/// the capabilites if units during draft but it didn't work and now does nothing

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
    public string description;
    public int maxHP = 10;
    public int attack = 4;
    public int defense = 2;
    public int movementRange = 3;
    public int attackRange = 3;

    [Header("Deployment Info (Runtime Only)")]
    [HideInInspector] public bool isDeployed = false; // True after being placed in deployment phase
    [HideInInspector] public Vector3Int deployedTile; // Grid position where the unit is deployed
}




