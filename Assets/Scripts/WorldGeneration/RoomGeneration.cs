using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class RoomGeneration: MonoBehaviour
{
    private Room _currentRoom; 
    private Dictionary<Vector2Int, RoomType> _map;
    private List<Room> _allRooms;
    private List<GameObject> _floorTiles = new List<GameObject>();
    private GameObject _player;
    private CollectorController _collectorController;
    private EncounterController _encounterController;
    [SerializeField] private List<GameObject> _normalTilePrefabs;
    [SerializeField] private List<GameObject> _grassTilePrefabs;
    [SerializeField] private List<GameObject> _doorTilePrefabs;
    [SerializeField] private List<GameObject> _exitTilePrefabs;
    [SerializeField] private List<GameObject> _shopTilePrefabs;
    [SerializeField] private List<GameObject> _treasureTilePrefabs;
    [SerializeField] private List<GameObject> _hospitalTilePrefabs;
    [SerializeField] private List<GameObject> _bossTilePrefabs;
    [SerializeField] private List<GameObject> _trainerTilePrefabs;
    [SerializeField] private List<GameObject> _starterTilePrefabs;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _collectorPrefab;
    [SerializeField] private MapGeneration _mapGeneration;
    [SerializeField] private MiniMapController _miniMapController;
    private FloorController _floorController; 
    private List<Critter> _starters = new List<Critter>();
    private List<AbilityManual> _rewards = new List<AbilityManual>();

    [SerializeField] private WallsOrDoorsController _wallsOrDoorsController;


    public void GenerateRooms(Vector2Int collectorLevelRange, Vector2Int teamSizeRange, CritterAffinity bossAffinity, EncounterController encounterController, FloorController floorController, bool doubleTreasureRoom = false)
    {
        _encounterController = encounterController;
        _map = _mapGeneration.SafeGenerateMainPath();
        _floorController = floorController;
        if(_map == null)
        {
            Debug.Log("Map generation failed too many times. No map generated");
        }

        _allRooms = GenerateRoomData(_map, collectorLevelRange, teamSizeRange, bossAffinity, doubleTreasureRoom);
        GenerateCollectors(collectorLevelRange, teamSizeRange, bossAffinity);

        _currentRoom = _allRooms.Find(room => room.Type == RoomType.Start);
        DisplayCurrentRoom();
        GeneratePlayer();
        _miniMapController = GameObject.FindObjectOfType<MiniMapController>();
        _miniMapController.Map = _allRooms;
        _miniMapController.UpdateMap();
    }


    public void GenerateMapFromMapState(MapState mapState)
    {
        Debug.Log("Generating Map from MapState");
        _allRooms = mapState.Map;
        _currentRoom = _allRooms.Find(room => room.Coordinates == mapState.PlayerRoom);
        _miniMapController = GameObject.FindObjectOfType<MiniMapController>();
        _miniMapController.Map = _allRooms;
        DisplayCurrentRoom();
        PlacePlayerAtCoords(mapState.PlayerTile, mapState.PlayerDirection);
        _miniMapController.UpdateMap();
    }


    private List<Room> GetAdjacentRooms(Room room)
    {
        Vector2Int coord = room.Coordinates;
        List<Vector2Int> adjCoords = new List<Vector2Int>(){
            new Vector2Int(coord.x, coord.y + 1),
            new Vector2Int(coord.x + 1, coord.y),
            new Vector2Int(coord.x, coord.y - 1),
            new Vector2Int(coord.x - 1, coord.y),
        };

        List<Room> adjRooms = new List<Room>();

        foreach(Vector2Int adjCoord in adjCoords)
        {
            if(_allRooms.Exists(room => room.Coordinates == adjCoord))
            {
                adjRooms.Add(_allRooms.Find(room => room.Coordinates == adjCoord));
            }
        }
        return adjRooms;
    }


    private void MarkAdjacentRooms(Room currentRoom)
    {
        List<Room> adjRooms = GetAdjacentRooms(currentRoom);
        foreach(Room room in adjRooms)
        {
            room.AdjacentToExplored = true;
        }
    }


    private List<Vector2Int> GetAllCoordsInMap(Dictionary<Vector2Int, RoomType> map)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        foreach (KeyValuePair<Vector2Int, RoomType> kvp in map)
        {
            path.Add(kvp.Key);
        }

        return path;
    }


    private List<bool> GetAdjacencyArray(Vector2Int coord, List<Vector2Int> path)
    {
        Vector2Int northCoord = new Vector2Int(coord.x, coord.y + 1);
        Vector2Int eastCoord = new Vector2Int(coord.x + 1, coord.y);
        Vector2Int southCoord = new Vector2Int(coord.x, coord.y - 1);
        Vector2Int westCoord = new Vector2Int(coord.x - 1, coord.y);

        List<bool> adjacencies = new List<bool>()
        {
            path.Contains(northCoord),
            path.Contains(eastCoord),
            path.Contains(southCoord),
            path.Contains(westCoord)
        };

        return adjacencies;
    }


    private List<List<string>> GetRoomLayoutBasedOnAdjacencies(List<bool> adjacencies, RoomType roomType, bool doubleTreasureRoom = false)
    {

        List<List<List<string>>> usableRoomLayouts = new List<List<List<string>>>();
        if(roomType == RoomType.Normal)
        {
            usableRoomLayouts = RoomLayouts.NormalRoomLayouts;
        }
        else if(roomType == RoomType.Start)
        {
            usableRoomLayouts = RoomLayouts.StartRoomLayouts;
        }
        else if(roomType == RoomType.Shop)
        {
            usableRoomLayouts = RoomLayouts.ShopRoomLayouts;
        }
        else if(roomType == RoomType.Boss)
        {
            usableRoomLayouts = RoomLayouts.BossRoomLayouts;
        }
        else if(roomType == RoomType.Hospital)
        {
            usableRoomLayouts = RoomLayouts.HospitalRoomLayouts;
        }
        else if(roomType == RoomType.Treasure)
        {
            if(doubleTreasureRoom)
            {
                usableRoomLayouts = RoomLayouts.DoubleTreasureRoomLayouts;
            }
            else
            {
                usableRoomLayouts = RoomLayouts.TreasureRoomLayouts;
            }
        }

        if(adjacencies[0])
        {
            usableRoomLayouts = usableRoomLayouts.Where(room => room[0][4] == "D").ToList();
        }
        else
        {
            usableRoomLayouts = usableRoomLayouts.Where(room => room[0][4] != "D").ToList();
        }

        if(adjacencies[1])
        {
            usableRoomLayouts = usableRoomLayouts.Where(room => room[4][8] == "D").ToList();
        }
        else
        {
            usableRoomLayouts = usableRoomLayouts.Where(room => room[4][8] != "D").ToList();
        }

        if(adjacencies[2])
        {
            usableRoomLayouts = usableRoomLayouts.Where(room => room[8][4] == "D").ToList();
        }
        else
        {
            usableRoomLayouts = usableRoomLayouts.Where(room => room[8][4] != "D").ToList();
        }

        if(adjacencies[3])
        {
            usableRoomLayouts = usableRoomLayouts.Where(room => room[4][0] == "D").ToList();
        }
        else
        {
            usableRoomLayouts = usableRoomLayouts.Where(room => room[4][0] != "D").ToList();
        }

        return usableRoomLayouts[UnityEngine.Random.Range(0, usableRoomLayouts.Count)];
    }


    public List<Room> GenerateRoomData(Dictionary<Vector2Int, RoomType> map, Vector2Int collectorLevelRange, Vector2Int teamSizeRange, CritterAffinity bossAffinity, bool doubleTreasureRoom = false)
    {
        List<Room> allRooms = new List<Room>();
        List<Vector2Int> path = GetAllCoordsInMap(map);

        foreach (KeyValuePair<Vector2Int, RoomType> kvp in map)
        {
            List<bool> adjacencyArray = GetAdjacencyArray(kvp.Key, path);
            List<List<string>> layout = GetRoomLayoutBasedOnAdjacencies(adjacencyArray, kvp.Value, doubleTreasureRoom);
            Room newRoom = new Room(kvp.Value, kvp.Key, layout);
            allRooms.Add(newRoom);

            PopulateUniqueRoomData(newRoom, teamSizeRange, collectorLevelRange, bossAffinity, doubleTreasureRoom);
        }

        return allRooms;
    }


    private void PopulateUniqueRoomData(Room room, Vector2Int teamSizeRange, Vector2Int collectorLevelRange, CritterAffinity bossAffinity, bool doubleTreasureRoom = false)
    {
        if (room.Type == RoomType.Boss)
        {
            Collector boss = new Collector(true, teamSizeRange.y, collectorLevelRange, new List<CritterAffinity>(){bossAffinity});
            room.Collectors.Add(boss);
        }
        else if (room.Type == RoomType.Shop)
        {
            room.ShopItems.Add(new MasonJar());
            room.ShopItems.Add(new Nectar());
            room.ShopItems.Add(new Nectar());
            MoveManual moveManual= new MoveManual();
            moveManual.SetRandomMove();
            room.ShopItems.Add(moveManual);
        }
        else if(room.Type == RoomType.Treasure)
        {
            if(doubleTreasureRoom)
            {
                MoveManual moveManual1 = new MoveManual();
                MoveManual moveManual2 = new MoveManual();
                moveManual1.SetRandomMove();
                moveManual2.SetRandomMove();
                room.Treasure.Add(moveManual1);
                room.Treasure.Add(moveManual2);
            }
            else 
            {
                MoveManual moveManual = new MoveManual();
                moveManual.SetRandomMove();
                room.Treasure.Add(moveManual);
            }
        }
    }


    private void GenerateCollectors(Vector2Int teamSizeRange, Vector2Int collectorLevelRange, CritterAffinity bossAffinity)
    {
        List<Room> availableRooms = _allRooms.Where(room => room.Type == RoomType.Normal).ToList();

        for (int i = 0; i < availableRooms.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(1, availableRooms.Count);

            if (availableRooms[randomIndex].Collectors.Count == 0)
            {
                availableRooms[randomIndex].Collectors.Add(new Collector(false, Mathf.Min(UnityEngine.Random.Range(teamSizeRange.x, teamSizeRange.y + 1), 5), collectorLevelRange, null));
            }
            
            //Debug.Log("Collector at " + availableRooms[i].Coordinates.ToString());
            //availableRooms.RemoveAt(randomIndex);
        }

        Room bossRoom = _allRooms.Find(room => room.Type == RoomType.Boss);
        bossRoom.Boss = new Collector(true, UnityEngine.Random.Range(teamSizeRange.x, teamSizeRange.y + 1), collectorLevelRange, new List<CritterAffinity>(){bossAffinity});
    }


    public void DisplayCurrentRoom()
    {
        MoneyCanvasController moneyCanvasController = GameObject.FindObjectOfType<MoneyCanvasController>();
        moneyCanvasController.SetMoney(_floorController.PlayerData.GetMoney());
        _floorTiles.Clear();
        string collectorPosition = "-1";

        if (_currentRoom.Collectors.Count > 0)
        {
            if(_currentRoom.Collectors[0].position == "-1")
            {
                collectorPosition = UnityEngine.Random.Range(0,3).ToString();
                _currentRoom.Collectors[0].position = collectorPosition;
            }
            else{
                collectorPosition = _currentRoom.Collectors[0].position;
            }
        }
        else 
        {
            _collectorController = null;
        }

        Debug.Log("collectorPosition = " + collectorPosition.ToString());

        int starterTileIndex = 0;

        for(int i = 0; i < 9; i ++)
        {
            for(int j = 0; j < 9; j ++)
            {
                if(_currentRoom.Layout[i][j] == "N")
                {
                    GameObject randomNormalTile = _normalTilePrefabs[UnityEngine.Random.Range(0, _normalTilePrefabs.Count)];
                    GameObject tileObject = GameObject.Instantiate(randomNormalTile, new Vector3(j, 0f, 8-i), Quaternion.identity);
                    tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                    tileObject.GetComponent<Tile>().Type = TileType.Normal;
                    tileObject.GetComponent<Tile>().IsWalkable = true;
                    _floorTiles.Add(tileObject);
                }
                if(_currentRoom.Layout[i][j] == "G")
                {
                    GameObject randomGrassTile = _grassTilePrefabs[UnityEngine.Random.Range(0, _grassTilePrefabs.Count)];
                    GameObject tileObject = GameObject.Instantiate(randomGrassTile, new Vector3(j, 0f, 8-i), Quaternion.identity);
                    tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                    tileObject.GetComponent<Tile>().Type = TileType.Grass;
                    tileObject.GetComponent<Tile>().IsWalkable = true;
                    _floorTiles.Add(tileObject);                
                }
                if(_currentRoom.Layout[i][j] == "D")
                {
                    GameObject randomDoorTile = _doorTilePrefabs[UnityEngine.Random.Range(0, _doorTilePrefabs.Count)];
                    GameObject tileObject = GameObject.Instantiate(randomDoorTile, new Vector3(j, 0f, 8-i), Quaternion.identity);
                    tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                    tileObject.GetComponent<Tile>().Type = TileType.Door;
                    tileObject.GetComponent<Tile>().IsWalkable = true;
                    tileObject.GetComponent<Tile>().ConnectingRoom = GetConnectingRoom(new Vector2Int(j, 8-i), tileObject);
                    _floorTiles.Add(tileObject);
                }
                if(_currentRoom.Layout[i][j] == "B0"
                    || _currentRoom.Layout[i][j] == "B1"
                    || _currentRoom.Layout[i][j] == "B2"
                    || _currentRoom.Layout[i][j] == "B3")
                {
                    string tileCode = _currentRoom.Layout[i][j];
                    GameObject randomBossTile = _bossTilePrefabs[UnityEngine.Random.Range(0, _bossTilePrefabs.Count)];
                    GameObject tileObject = GameObject.Instantiate(randomBossTile, new Vector3(j, 0f, 8-i), Quaternion.identity);
                    tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                    tileObject.GetComponent<Tile>().Type = TileType.Boss;
                    tileObject.GetComponent<Tile>().IsWalkable = false;
                    _floorTiles.Add(tileObject);
                    string direction = tileCode == "B0" ? "0" : tileCode == "B1" ? "1" : tileCode == "B2" ? "2" : "3";
                    GenerateTrainer(tileObject, direction, _currentRoom.Boss, true);
                }
                if(_currentRoom.Layout[i][j] == "E")
                {
                    GameObject randomExitTile = _exitTilePrefabs[UnityEngine.Random.Range(0, _exitTilePrefabs.Count)];
                    GameObject tileObject = GameObject.Instantiate(randomExitTile, new Vector3(j, 0f, 8-i), Quaternion.identity);
                    tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                    tileObject.GetComponent<Tile>().Type = TileType.Exit;
                    tileObject.GetComponent<Tile>().IsWalkable = true;
                    _floorTiles.Add(tileObject);
                    if(_currentRoom.Boss.HasBeenDefeated)
                    {
                        tileObject.GetComponent<ExitTileController>().OpenTrapDoor.SetActive(true);
                        tileObject.GetComponent<ExitTileController>().ClosedTrapDoor.SetActive(false);
                    }
                    else
                    {
                        tileObject.GetComponent<ExitTileController>().OpenTrapDoor.SetActive(false);
                        tileObject.GetComponent<ExitTileController>().ClosedTrapDoor.SetActive(true);
                    }
                }
                if(_currentRoom.Layout[i][j] == "M0" || _currentRoom.Layout[i][j] == "M1")
                {
                    string tileCode = _currentRoom.Layout[i][j];
                    string number = tileCode == "M0" ? "0" : "1";
                    int numb  = int.Parse(number);
                    GameObject randomTreasureTile = _treasureTilePrefabs[UnityEngine.Random.Range(0, _treasureTilePrefabs.Count)];
                    GameObject tileObject = GameObject.Instantiate(randomTreasureTile, new Vector3(j, 0f, 8-i), Quaternion.identity);
                    tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                    tileObject.GetComponent<Tile>().Type = TileType.Treasure;
                    tileObject.GetComponent<Tile>().IsWalkable = false;
                    tileObject.GetComponent<Tile>().Treasure = _currentRoom.Treasure[numb];
                    if(_currentRoom.Treasure[numb] == null)
                    {
                        tileObject.GetComponent<TreasureTileController>().ScrollParent.SetActive(false);
                    }
                    else
                    {
                        tileObject.GetComponent<TreasureTileController>().ScrollParent.SetActive(true);
                    }
                    _floorTiles.Add(tileObject);
                }
                if(_currentRoom.Layout[i][j] == "S0"
                    || _currentRoom.Layout[i][j] == "S1"
                    || _currentRoom.Layout[i][j] == "S2"
                    || _currentRoom.Layout[i][j] == "S3")
                {
                    string tileCode = _currentRoom.Layout[i][j];
                    GameObject randomShopTile = _shopTilePrefabs[UnityEngine.Random.Range(0, _shopTilePrefabs.Count)];
                    GameObject tileObject = GameObject.Instantiate(randomShopTile, new Vector3(j, 0f, 8-i), Quaternion.identity);
                    tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                    tileObject.GetComponent<Tile>().Type = TileType.Shop;
                    tileObject.GetComponent<Tile>().IsWalkable = false;
                    _floorTiles.Add(tileObject);
                    string number = tileCode == "S0" ? "0" : tileCode == "S1" ? "1" : tileCode == "S2" ? "2" : "3";
                    int numb  = int.Parse(number);
                    Debug.Log("Setting shop item" + _currentRoom.ShopItems[numb].Name);
                    Item item = _currentRoom.ShopItems[numb];
                    ShopTileController shopTileController = tileObject.GetComponent<ShopTileController>();
                    if(item == null)
                    {
                        shopTileController.NectarParent.SetActive(false);
                        shopTileController.ScrollParent.SetActive(false);
                        shopTileController.MasonJarParent.SetActive(false);
                    }
                    else if(item.ID == ItemType.MoveManual)
                    {
                        shopTileController.NectarParent.SetActive(false);
                        shopTileController.ScrollParent.SetActive(true);
                        shopTileController.MasonJarParent.SetActive(false);
                    }
                    else if(item.ID == ItemType.Nectar)
                    {
                        shopTileController.NectarParent.SetActive(true);
                        shopTileController.ScrollParent.SetActive(false);
                        shopTileController.MasonJarParent.SetActive(false);
                    }
                    else if(item.ID == ItemType.MasonJar)
                    {
                        shopTileController.NectarParent.SetActive(false);
                        shopTileController.ScrollParent.SetActive(false);
                        shopTileController.MasonJarParent.SetActive(true);
                    }
                    tileObject.GetComponent<Tile>().ShopItem = _currentRoom.ShopItems[numb];
                }
                if(_currentRoom.Layout[i][j] == "H")
                {
                    GameObject randomHospitalTile = _hospitalTilePrefabs[UnityEngine.Random.Range(0, _hospitalTilePrefabs.Count)];
                    GameObject tileObject = GameObject.Instantiate(randomHospitalTile, new Vector3(j, 0f, 8-i), Quaternion.identity);
                    tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                    tileObject.GetComponent<Tile>().Type = TileType.Hospital;
                    tileObject.GetComponent<Tile>().IsWalkable = false;
                    _floorTiles.Add(tileObject);
                }
                if(_currentRoom.Layout[i][j] == "R")
                {
                    GameObject randomStarterTile = _starterTilePrefabs[UnityEngine.Random.Range(0, _starterTilePrefabs.Count)];
                    GameObject tileObject = GameObject.Instantiate(randomStarterTile, new Vector3(j, 0f, 8-i), Quaternion.identity);
                    tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                    tileObject.GetComponent<Tile>().Type = TileType.Starter;
                    tileObject.GetComponent<Tile>().IsWalkable = false;
                    StarterTileController starterTileController = tileObject.GetComponent<StarterTileController>();
                    if(_floorController.GetCurrentLevel() == 1)
                    {
                        tileObject.GetComponent<Tile>().Starter = _starters[starterTileIndex];
                        if (_encounterController.IsStarterChosen)
                        {
                            starterTileController.MasonJarParent.SetActive(false);
                            starterTileController.BookParent.SetActive(false);
                        }
                        else
                        {
                            starterTileController.MasonJarParent.SetActive(true);
                            starterTileController.BookParent.SetActive(false);
                            tileObject.GetComponentInChildren<MasonJarObject>().Glow(_starters[starterTileIndex]);
                        }
                    }
                    else
                    {
                        tileObject.GetComponent<Tile>().Reward = _rewards[starterTileIndex];
                        if(_rewards[starterTileIndex] == null)
                        {
                            starterTileController.MasonJarParent.SetActive(false);
                            starterTileController.BookParent.SetActive(false);
                        }
                        else{
                            starterTileController.MasonJarParent.SetActive(false);
                            starterTileController.BookParent.SetActive(true);
                        }
                    }


                    _floorTiles.Add(tileObject);
                    starterTileIndex++;
                }
                if(_currentRoom.Layout[i][j] == "0" 
                || _currentRoom.Layout[i][j] == "1" 
                || _currentRoom.Layout[i][j] == "2" 
                || _currentRoom.Layout[i][j] == "3" )
                {
                    if(_currentRoom.Layout[i][j] == collectorPosition)
                    {
                        GameObject randomTrainerTile = _grassTilePrefabs[UnityEngine.Random.Range(0, _grassTilePrefabs.Count)];
                        GameObject tileObject = GameObject.Instantiate(randomTrainerTile, new Vector3(j, 0f, 8-i), Quaternion.identity);
                        tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                        tileObject.GetComponent<Tile>().Type = TileType.Trainer;
                        tileObject.GetComponent<Tile>().IsWalkable = false;
                        _floorTiles.Add(tileObject);
                        GenerateTrainer(tileObject, _currentRoom.Layout[i][j], _currentRoom.Collectors[0], false);
                        if(_collectorController.Collector.Coords != new Vector2Int(-100, -100))
                        {
                            tileObject.GetComponent<Tile>().IsWalkable = true;
                        }
                    }
                    else 
                    {
                        GameObject randomNormalTile = _normalTilePrefabs[UnityEngine.Random.Range(0, _normalTilePrefabs.Count)];
                        GameObject tileObject = GameObject.Instantiate(randomNormalTile, new Vector3(j, 0f, 8-i), Quaternion.identity);
                        tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                        tileObject.GetComponent<Tile>().Type = TileType.Normal;
                        tileObject.GetComponent<Tile>().IsWalkable = true;
                        _floorTiles.Add(tileObject);
                    }
                }
            }
        }
        if(_collectorController != null)
        {
            _collectorController.RoomTiles = _floorTiles;
            _collectorController.MoveToTile();
            if(_collectorController.Collector.Coords != new Vector2Int(-100, -100))
            {
                _floorTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == _collectorController.Collector.Coords).GetComponent<Tile>().IsWalkable = false;
            }
        
        }

        //Run my walls and doors code
        _wallsOrDoorsController.gameObject.SetActive(true);
        _wallsOrDoorsController.Generate();
    }


    public void GeneratePlayer()
    {
        _player = GameObject.Instantiate(_playerPrefab, _floorTiles[40].transform);
        _player.transform.localPosition = Vector3.zero;
        PlayerController playerController = _player.GetComponent<PlayerController>();
        playerController.CurrentCoords = _floorTiles[40].GetComponent<Tile>().Coordinates;
        Debug.Log("Setting current coords to " + _floorTiles[40].GetComponent<Tile>().Coordinates.ToString());
        playerController.CurrentRoom = _currentRoom;
        _currentRoom.Explored = true;
        MarkAdjacentRooms(_currentRoom);
        SetContainsPlayer(_currentRoom);
        playerController.RoomTiles = _floorTiles;
        playerController.Map = _allRooms;
        playerController.RoomGeneration = this;
        playerController.FloorController = _floorController;
        playerController.EncounterController = _encounterController;
        playerController.CollectorController = _collectorController;

        OverworldMenu menu = GameObject.FindObjectOfType<OverworldMenu>();
        menu.GetComponentsInScene(_floorController.PlayerData, playerController);
    }

    public void GenerateTrainer(GameObject parentTile, string direction, Collector collector, bool isBoss)
    {
        GameObject trainer = GameObject.Instantiate(_collectorPrefab, parentTile.transform);
        trainer.transform.localPosition = Vector3.zero;
        CollectorController collectorController = trainer.GetComponent<CollectorController>();
        TrainerViz trainerViz = trainer.GetComponentInChildren<TrainerViz>();
        collectorController.Coordinates = parentTile.GetComponent<Tile>().Coordinates;
        collectorController.Direction = direction;
        collectorController.Collector = collector;
        if(!isBoss)
        {
            collectorController.CalculateVisibleCoords();
        }
        collectorController.SnapToDirection();
        _collectorController = collectorController;
        trainerViz.DisplayCorrectAttributes(collector.NoseValue, collector.EyeValue, collector.SkinColour, collector.NoseColour, collector.TeeColour);
    }

    public void GenerateBoss(GameObject parentTile, string direction, Collector collector)
    {

    }

    public void PlacePlayerAtCoords(Vector2Int coords, int direction)
    {
        _player = GameObject.Instantiate(_playerPrefab, _floorTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == coords).transform);
        _player.transform.localPosition = Vector3.zero;
        PlayerController playerController = _player.GetComponent<PlayerController>();
        playerController.CurrentCoords = coords;
        playerController.CurrentRoom = _currentRoom;
        _currentRoom.Explored = true;
        MarkAdjacentRooms(_currentRoom);
        SetContainsPlayer(_currentRoom);
        playerController.RoomTiles = _floorTiles;
        playerController.Map = _allRooms;
        playerController.RoomGeneration = this;
        playerController.EncounterController = _encounterController;
        playerController.FloorController = _floorController;
        playerController.CollectorController = _collectorController;
        playerController.Direction = direction;
        playerController.SnapToDirection();
        _miniMapController.UpdateMap();

        OverworldMenu menu = GameObject.FindObjectOfType<OverworldMenu>();
        menu.GetComponentsInScene(_floorController.PlayerData, playerController);
    }

    public void PlacePlayerInNewRoom(Vector2Int coords, int direction)
    {
        Vector2Int newCoords = new Vector2Int(0, 0);
        if(coords == new Vector2Int(4, 8))
        {
            newCoords = new Vector2Int(4, 0);
        }
        if(coords == new Vector2Int(4, 0))
        {
            newCoords = new Vector2Int(4, 8);
        }
        if(coords == new Vector2Int(0, 4))
        {
            newCoords = new Vector2Int(8, 4);
        }
        if(coords == new Vector2Int(8, 4))
        {
            newCoords = new Vector2Int(0, 4);
        }
        Debug.Log("Instantiating now");
        _player = GameObject.Instantiate(_playerPrefab, _floorTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == newCoords).transform);
        _player.transform.localPosition = Vector3.zero;
        PlayerController playerController = _player.GetComponent<PlayerController>();
        playerController.CurrentCoords = newCoords;
        playerController.CurrentRoom = _currentRoom;
        _currentRoom.Explored = true;
        MarkAdjacentRooms(_currentRoom);
        SetContainsPlayer(_currentRoom);
        playerController.RoomTiles = _floorTiles;
        playerController.Map = _allRooms;
        playerController.RoomGeneration = this;
        playerController.EncounterController = _encounterController;
        playerController.FloorController = _floorController;
        playerController.CollectorController = _collectorController;
        playerController.Direction = direction;
        playerController.SnapToDirection();
        _miniMapController.UpdateMap();

        OverworldMenu menu = GameObject.FindObjectOfType<OverworldMenu>();
        menu.GetComponentsInScene(_floorController.PlayerData, playerController);
    }


    public void MoveRooms(Vector2Int newRoomCoords, Vector2Int currentTileCoords, int direction)
    {
        foreach(GameObject tile in _floorTiles)
        {
            GameObject.Destroy(tile);
        }
        _floorTiles.Clear();
        _currentRoom = _allRooms.Find(room => room.Coordinates == newRoomCoords);
        Debug.Log(_currentRoom == null);
        _player.GetComponent<PlayerController>().CurrentRoom = _currentRoom;
        DisplayCurrentRoom();
        PlacePlayerInNewRoom(currentTileCoords, direction);
    }


    public void GenerateStarterCritters()
    {
        List<Type> availableCritterTypes = new List<Type>(){
            typeof(GhostAnt),
            typeof(Ladybird),
            typeof(LimeButterfly),
            typeof(HawaiianSmilingSpider),
            typeof(Honeybee),
            typeof(GreyFieldSlug),
        };

        for (int i = 0; i < 3; i++)
        {
            Type randomCritterType = availableCritterTypes[UnityEngine.Random.Range(0, availableCritterTypes.Count)];
            availableCritterTypes.Remove(randomCritterType);

            Critter randomCritter = Activator.CreateInstance(randomCritterType) as Critter;
            randomCritter.SetStartingLevel(3);
            _starters.Add(randomCritter);
        }
    }

    public void GenerateRewardAbilities()
    {
        List<AbilityManual> abilities = new List<AbilityManual>();
        for(int i = 0; i < 3; i ++)
        {
            AbilityManual abilityManual= new AbilityManual();
            abilityManual.SetRandomAbility();
            abilities.Add(abilityManual);
        }
        _rewards = abilities;
    }


    public void SetContainsPlayer(Room roomContainingPlayer)
    {
        foreach(Room room in _allRooms)
        {
            room.ContainsPlayer = false;
        }
        roomContainingPlayer.ContainsPlayer = true;
    }

    private RoomType GetConnectingRoom(Vector2Int tileCoords, GameObject tileObject)
    {
        Debug.Log(tileCoords.ToString());
        Room adjRoom = null;
        if(tileCoords == new Vector2Int(0,4))
        {
            adjRoom = _allRooms.Find(room => room.Coordinates == new Vector2Int(_currentRoom.Coordinates.x - 1, _currentRoom.Coordinates.y));
            if(_floorController.GetCurrentLevel() == 1 && !_floorController.Encounters.IsStarterChosen)
            {
                tileObject.GetComponent<DoorTileController>().NorthDoorBlock.SetActive(false);
                tileObject.GetComponent<DoorTileController>().EastDoorBlock.SetActive(false);
                tileObject.GetComponent<DoorTileController>().SouthDoorBlock.SetActive(true);
                tileObject.GetComponent<DoorTileController>().WestDoorBlock.SetActive(false);
            }

        }
        if(tileCoords == new Vector2Int(8,4))
        {
            adjRoom = _allRooms.Find(room => room.Coordinates == new Vector2Int(_currentRoom.Coordinates.x + 1, _currentRoom.Coordinates.y));
            if(_floorController.GetCurrentLevel() == 1 && !_floorController.Encounters.IsStarterChosen)
            {
                tileObject.GetComponent<DoorTileController>().NorthDoorBlock.SetActive(false);
                tileObject.GetComponent<DoorTileController>().EastDoorBlock.SetActive(false);
                tileObject.GetComponent<DoorTileController>().SouthDoorBlock.SetActive(false);
                tileObject.GetComponent<DoorTileController>().WestDoorBlock.SetActive(true);
            }
        }
        if(tileCoords == new Vector2Int(4,0))
        {
            adjRoom = _allRooms.Find(room => room.Coordinates == new Vector2Int(_currentRoom.Coordinates.x, _currentRoom.Coordinates.y - 1));
            if(_floorController.GetCurrentLevel() == 1 && !_floorController.Encounters.IsStarterChosen)
            {
                tileObject.GetComponent<DoorTileController>().NorthDoorBlock.SetActive(true);
                tileObject.GetComponent<DoorTileController>().EastDoorBlock.SetActive(false);
                tileObject.GetComponent<DoorTileController>().SouthDoorBlock.SetActive(false);
                tileObject.GetComponent<DoorTileController>().WestDoorBlock.SetActive(false);
            }
        }
        if(tileCoords == new Vector2Int(4,8))
        {
            adjRoom = _allRooms.Find(room => room.Coordinates == new Vector2Int(_currentRoom.Coordinates.x, _currentRoom.Coordinates.y + 1));
            if(_floorController.GetCurrentLevel() == 1 && !_floorController.Encounters.IsStarterChosen)
            {
                tileObject.GetComponent<DoorTileController>().NorthDoorBlock.SetActive(false);
                tileObject.GetComponent<DoorTileController>().EastDoorBlock.SetActive(true);
                tileObject.GetComponent<DoorTileController>().SouthDoorBlock.SetActive(false);
                tileObject.GetComponent<DoorTileController>().WestDoorBlock.SetActive(false);
            }
        }
        return adjRoom.Type;
    }

    public void GoToNewLevel()
    {
        StartCoroutine(WaitToDescend());
    }

    
    private IEnumerator WaitToDescend()
    {
        yield return new WaitForSeconds(0.4f);

        _floorController.IncrementLevel();
    }
}
