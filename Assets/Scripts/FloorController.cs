using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FloorController : MonoBehaviour
{
    [SerializeField] public EncounterController Encounters;
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

    [SerializeField] private TextMeshProUGUI[] _floorTMPs;

    [SerializeField] private Material _floorMat;
    [SerializeField] private Material _grassMat;
    [SerializeField] private Material _splodgeMat;
    [SerializeField] private Material _grassParticleMat;
    [SerializeField] private Color[] _floorColors;
    [SerializeField] private Color[] _grassColors;
    [SerializeField] private Color[] _splodgeColors;
    [SerializeField] private Material[] _skyboxMaterials;


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
        PlayerData.AddItemToInventory(new MasonJar());
        PlayerData.AddItemToInventory(new Nectar());


        //TODO: remove!----------
        // Critter starter = new BulletAnt();
        // starter.SetStartingLevel(1);
        // starter.Ability = new BugMuncher();
        // PlayerData.AddCritter(starter);
        // PlayerData.AddMoney(1000);

        // Critter boye = new Honeybee();
        // boye.SetStartingLevel(10);
        // PlayerData.AddCritter(boye);

        // Critter boi = new MonarchButterfly();
        // boi.SetStartingLevel(10);
        // PlayerData.AddCritter(boi);

        // PlayerData.AddItemToInventory(new MasonJar());
        // PlayerData.AddItemToInventory(new MasonJar());
        // PlayerData.AddItemToInventory(new MasonJar());
        // PlayerData.AddItemToInventory(new Nectar());
        // PlayerData.AddItemToInventory(new Nectar());
        // PlayerData.AddItemToInventory(new Nectar());
        // MoveManual newMoveManual = new MoveManual();
        // newMoveManual.SetRandomMove();
        // PlayerData.AddItemToInventory(newMoveManual);
        // newMoveManual = new MoveManual();
        // newMoveManual.SetRandomMove();
        // PlayerData.AddItemToInventory(newMoveManual);
        // newMoveManual = new MoveManual();
        // newMoveManual.SetRandomMove();
        // PlayerData.AddItemToInventory(newMoveManual);
        //--------------

        InitializeLevel();
    }


    public int GetCurrentLevel()
    {
        return _currentLevel;
    }


    private void InitializeLevel()
    {
        _floorMat.SetColor("_BaseColor", _floorColors[_currentLevel - 1]);
        _grassMat.SetColor("_BaseColor", _grassColors[_currentLevel - 1]);
        _grassParticleMat.SetColor("_BaseColor", _grassColors[_currentLevel - 1]);
        _splodgeMat.SetColor("_BaseColor", _splodgeColors[_currentLevel - 1]);
        //RenderSettings.skybox = _skyboxMaterials[_currentLevel - 1];

        LastMapGeneratedLevel = _currentLevel;

        int randomAffinityIndex = UnityEngine.Random.Range(1, Enum.GetNames(typeof(CritterAffinity)).Length);
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

        _floorTMPs[0].text = ("FLOOR <size=60>" + _currentLevel.ToString());
        _floorTMPs[1].text = ("FLOOR <size=60>" + _currentLevel.ToString());

        bool doubleTreasureRooms = false;
        foreach(Critter critter in PlayerData.GetCritters())
        {
            if(critter.Ability.ID == AbilityID.TreasureOptions)
            {
                doubleTreasureRooms = true;
            }
        }


        RoomGen.GenerateRooms(_collectorLevelRanges[_currentLevel], _collectorTeamSizeRanges[_currentLevel], _levelBossAffinity, Encounters, this, doubleTreasureRooms);
        //GameObject.FindObjectOfType<OverworldMenu>().GetComponentsInScene();
    }


    public void IncrementLevel()
    {
        _currentLevel++; 
        if(_currentLevel <= 5)
        {
            AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("MainGame");
        }
        else
        {
            AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("WinGame");
        }
    }
}
