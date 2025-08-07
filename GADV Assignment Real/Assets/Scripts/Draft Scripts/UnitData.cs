using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Units/Unit Data", order = 1)]
public class UnitData : ScriptableObject
{
    public string unitName;
    public int cost;
    public Sprite icon;
    public GameObject prefab;
    public int maxCopies = 3;
}



