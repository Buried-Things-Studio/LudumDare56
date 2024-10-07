using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FloorController : MonoBehaviour
{
    [SerializeField] private EncounterController Encounters;
    public RoomGeneration RoomGen;
    public Player PlayerData;
    
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
    private List<Vector2Int> _collectorTeamSizeRanges = new List<Vector2Int>(){
        new Vector2Int(0, 0),
        new Vector2Int(1, 2),
        new Vector2Int(2, 3),
        new Vector2Int(2, 4),
        new Vector2Int(3, 4),
        new Vector2Int(3, 5),
    };
    private CritterAffinity _levelBossAffinity;


    private void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
        
        PlayerData = new Player();
        Encounters.PlayerData = PlayerData;

        //TODO: remove!----------
        Critter starter = new BulletAnt();
        starter.SetStartingLevel(10);
        PlayerData.AddCritter(starter);

        Critter boye = new Bumblebee();
        boye.SetStartingLevel(2);
        PlayerData.AddCritter(boye);

        Critter boi = new MonarchButterfly();
        boi.SetStartingLevel(3);
        PlayerData.AddCritter(boi);

        PlayerData.AddItemToInventory(new MasonJar());
        PlayerData.AddItemToInventory(new MasonJar());
        PlayerData.AddItemToInventory(new MasonJar());
        PlayerData.AddItemToInventory(new Nectar());
        PlayerData.AddItemToInventory(new Nectar());
        PlayerData.AddItemToInventory(new Nectar());
        //--------------

        InitializeLevel();
    }


    private void InitializeLevel()
    {
        _currentLevel++;

        //int randomAffinityIndex = UnityEngine.Random.Range(1, Enum.GetNames(typeof(CritterAffinity)).Length);
        int randomAffinityIndex = 2; //TODO: make random again
        _levelBossAffinity = (CritterAffinity)randomAffinityIndex;

        Encounters.SetAvailableCrittersOnFloor(_wildEncounterLevelRanges[_currentLevel]);
        RoomGen.GenerateRooms(_collectorLevelRanges[_currentLevel], _collectorTeamSizeRanges[_currentLevel], _levelBossAffinity, Encounters);

        if (_currentLevel == 1)
        {
            RoomGen.GenerateStarterCritters();
        }
    }
}
