using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;



public class DraggableUnit : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UnitData unitData;
    private DraggableUnitUI uiDisplay;
    private DeploymentManager deployManager;
    private Tilemap map;
    private Tilemap deployZone;


    private Vector3 offset;
    private bool placed = false;
    private GameObject placedUnitObj;

  

    public void Setup(UnitData data, DeploymentManager manager, Tilemap map, Tilemap deployZone)
    {
        unitData = data;
        deployManager = manager;
        this.map = map;
        this.deployZone = deployZone;

        uiDisplay = GetComponent<DraggableUnitUI>();
        if (uiDisplay != null)
            uiDisplay.Setup(data);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;
        Vector3Int cellPos = map.WorldToCell(worldPos);

        if (deployZone.HasTile(cellPos))
        {
            // Valid drop — place the actual unit
            if (!placed)
            {
                Vector3 spawnPos = map.GetCellCenterWorld(cellPos);
                placedUnitObj = Instantiate(unitData.prefab, spawnPos, Quaternion.identity);
                placed = true;
                deployManager.OnUnitPlaced(gameObject);
                gameObject.SetActive(false); // hide the draggable icon
            }
        }
        else
        {
            Debug.Log("Invalid drop location.");
        }
    }
}

