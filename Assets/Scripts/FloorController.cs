using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FloorController : MonoBehaviour
{
    [SerializeField] private EncounterController Encounters;
    public RoomGeneration RoomGen;
    public Player PlayerData;
    public static FloorController SingleFloorController;
    public bool IsActivated = false;
    public MapState MapState;
    public int LastMapGeneratedLevel = 1;
    
    private int _currentLevel = 1;
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
        Debug.Log("Floor controller AWAKE function");

        if (SingleFloorController == null)
        {
            SingleFloorController = this;
        }
        else if (SingleFloorController != this)
        {
            bool doesMapExist = SingleFloorController.LastMapGeneratedLevel == SingleFloorController._currentLevel;

            Debug.Log("Does map exist  = " +  doesMapExist.ToString());

            if (doesMapExist)
            {
                SingleFloorController.RoomGen.GenerateMapFromMapState(SingleFloorController.MapState);
                //GameObject.FindObjectOfType<OverworldMenu>().GetComponentsInScene();
            }
            else
            {
                SingleFloorController.InitializeLevel();
            }


            Debug.Log("Not the original, destroying ... ");

            GameObject.Destroy(this.gameObject);

            return;
        }
        
        if (IsActivated)
        {
            return;
        }

        IsActivated = true;
        Debug.Log("FloorController AWAKE running");
        GameObject.DontDestroyOnLoad(this.gameObject);
        
        PlayerData = new Player();
        Encounters.PlayerData = PlayerData;

        //TODO: remove!----------
        Critter starter = new BulletAnt();
        starter.SetStartingLevel(10);
        PlayerData.AddCritter(starter);

        foreach (Move move in starter.Moves)
        {
            move.CurrentUses = 0;
        }

        Critter boye = new Honeybee();
        boye.SetStartingLevel(10);
        PlayerData.AddCritter(boye);

        Critter boi = new MonarchButterfly();
        boi.SetStartingLevel(10);
        PlayerData.AddCritter(boi);

        PlayerData.AddItemToInventory(new MasonJar());
        PlayerData.AddItemToInventory(new MasonJar());
        PlayerData.AddItemToInventory(new MasonJar());
        PlayerData.AddItemToInventory(new Nectar());
        PlayerData.AddItemToInventory(new Nectar());
        PlayerData.AddItemToInventory(new Nectar());
        MoveManual newMoveManual = new MoveManual();
        newMoveManual.SetRandomMove();
        PlayerData.AddItemToInventory(newMoveManual);
        newMoveManual = new MoveManual();
        newMoveManual.SetRandomMove();
        PlayerData.AddItemToInventory(newMoveManual);
        newMoveManual = new MoveManual();
        newMoveManual.SetRandomMove();
        PlayerData.AddItemToInventory(newMoveManual);
        //--------------

        InitializeLevel();
    }


    public int GetCurrentLevel()
    {
        return _currentLevel;
    }


    private void InitializeLevel()
    {
        LastMapGeneratedLevel = _currentLevel;

        //int randomAffinityIndex = UnityEngine.Random.Range(1, Enum.GetNames(typeof(CritterAffinity)).Length);
        int randomAffinityIndex = 2; //TODO: make random again
        _levelBossAffinity = (CritterAffinity)randomAffinityIndex;

        Encounters.SetAvailableCrittersOnFloor(_wildEncounterLevelRanges[_currentLevel]);

        if (_currentLevel == 1)
        {
            RoomGen.GenerateStarterCritters();
        }
        else
        {
            RoomGen.GenerateRewardMoves();
        }

        RoomGen.GenerateRooms(_collectorLevelRanges[_currentLevel], _collectorTeamSizeRanges[_currentLevel], _levelBossAffinity, Encounters, this);
        //GameObject.FindObjectOfType<OverworldMenu>().GetComponentsInScene();
    }


    public void IncrementLevel()
    {
        _currentLevel++; 
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("MainGame");
    }
}
