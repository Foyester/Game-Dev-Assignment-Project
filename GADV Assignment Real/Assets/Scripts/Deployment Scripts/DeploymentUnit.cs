using UnityEngine;

public class DeploymentUnit : MonoBehaviour
{
    public UnitData unitData;
    public Vector3Int gridPosition;

    public void Setup(UnitData data, Vector3Int pos)
    {
        unitData = data;
        gridPosition = pos;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogWarning("No SpriteRenderer found, searching children...");
            sr = GetComponentInChildren<SpriteRenderer>();
        }

        if (sr != null)
        {
            sr.sprite = data.icon;
            Debug.Log("Sprite assigned: " + data.icon.name);
        }
        else
        {
            Debug.LogError("No SpriteRenderer found at all!");
        }
        sr.color = new Color(22f, 22f, 22f, 22f);
        transform.localScale = Vector3.one * 22f;
    }
}
