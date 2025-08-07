using System.Collections.Generic;
using UnityEngine;

public class DraftData : MonoBehaviour
{
    public static DraftData Instance;

    public List<UnitData> player1Units = new List<UnitData>();
    public List<UnitData> player2Units = new List<UnitData>();
    public bool isPlayer1Turn = true;
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
    public void ClearAllData()
    {
        player1Units.Clear();
        player2Units.Clear();
    }

}





    
