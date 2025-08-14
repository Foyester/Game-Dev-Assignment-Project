/// This is stroke inducer number 3 because the main camera was being annoying and it constantly refuse to let me assign stuff in the 
/// inspector, therefore i just amde it so it automaticaly finds it's required compenents upon awake. anyways purposes wise
/// it is to fill a panel with the apporpriate data of the unit the mouse is hovering over so players can check their stats. I 
/// used the ability to hide the thing rather than spawn a prefab so it saves me the trouble of creating a container for it

using UnityEngine;

public class UnitHover : MonoBehaviour
{
    [Header("Settings")]
    public float hoverDelay = 2f;

    private Camera mainCamera;
    private Unit hoveredUnit;
    private float hoverTimer;

    private UnitStatsPopupUI popupUI;

    private void Awake()
    {
        
        if (Camera.main != null)
        {
            mainCamera = Camera.main;
        }
        else
        {
            Debug.LogError("No Main Camera found in scene!");
        }

        
        GameObject statsUIPanel = GameObject.Find("StatsUI");
        if (statsUIPanel != null)
        {
            popupUI = statsUIPanel.GetComponent<UnitStatsPopupUI>();
            if (popupUI == null)
                Debug.LogError("StatsUI GameObject does not have a UnitStatsPopupUI component!");
        }
        else
        {
            Debug.LogError("No GameObject named 'StatsUI' found in the scene!");
        }
    }

    private void Update()
    {
        if (mainCamera == null || popupUI == null) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null)
        {
            Unit unit = hit.collider.GetComponent<Unit>();
            if (unit != null)
            {
                if (unit != hoveredUnit)
                {
                    hoveredUnit = unit;
                    hoverTimer = 0f;
                    popupUI.Hide();
                }

                hoverTimer += Time.deltaTime;
                if (hoverTimer >= hoverDelay)
                {
                    popupUI.Show(unit);
                }
            }
            else
            {
                ResetHover();
            }
        }
        else
        {
            ResetHover();
        }
    }

    private void ResetHover()
    {
        hoveredUnit = null;
        hoverTimer = 0f;
        popupUI?.Hide();
    }
}




