///this just stores the units selected in the drafting stage of the game for each player and stops it from being destoryed on scene change. 
///list was used since a unit could be selected more than once so yeah. and it wasn't storing any other data other than the choice so.

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





    
