using UnityEngine;

public class DeploymentUnit : MonoBehaviour
{
    public UnitData unitData;
    public Vector3Int gridPosition;

    public void Setup(UnitData data, Vector3Int pos)
    {
        unitData = data;
        gridPosition = pos;
        GetComponent<SpriteRenderer>().sprite = data.icon;
    }
}

