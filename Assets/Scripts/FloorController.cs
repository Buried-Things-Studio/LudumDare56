using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FloorController : MonoBehaviour
{
    public EncounterController EncounterController;
    
    private int _currentLevel = 0;
    private List<Vector2Int> _wildEncounterLevelRanges = new List<Vector2Int>(){
        new Vector2Int(0, 0),
        new Vector2Int(1, 2),
        new Vector2Int(2, 4),
        new Vector2Int(3, 5),
        new Vector2Int(4, 7),
        new Vector2Int(5, 8),
    };
    private List<Vector2Int> _collectorLevelRanges = new List<Vector2Int>(){
        new Vector2Int(0, 0),
        new Vector2Int(1, 2),
        new Vector2Int(2, 3),
        new Vector2Int(4, 5),
        new Vector2Int(6, 7),
        new Vector2Int(8, 9),
    };
    private CritterAffinity _levelBossAffinity;


    private void Start()
    {
        EncounterController = new EncounterController();
        InitializeLevel();
    }


    private void InitializeLevel()
    {
        _currentLevel++;

        int randomAffinityIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(CritterAffinity)).Length);
        _levelBossAffinity = (CritterAffinity)randomAffinityIndex;

        EncounterController.SetAvailableCrittersOnFloor(_wildEncounterLevelRanges[_currentLevel]);
    }
}
