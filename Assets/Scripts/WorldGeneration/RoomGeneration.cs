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
    private int _collectorsPerFloor = 3;


    public void GenerateRooms(Vector2Int collectorLevelRange, Vector2Int teamSizeRange, CritterAffinity bossAffinity, EncounterController encounterController)
    {
        _encounterController = encounterController;
        _map = _mapGeneration.SafeGenerateMainPath();
        if(_map == null)
        {
            Debug.Log("Map generation failed too many times. No map generated");
        }

        _allRooms = GenerateRoomData(_map, collectorLevelRange, teamSizeRange, bossAffinity);
        GenerateCollectors(collectorLevelRange, teamSizeRange);

        _currentRoom = _allRooms.Find(room => room.Type == RoomType.Start);
        DisplayCurrentRoom();
        GeneratePlayer();
        _miniMapController.Map = _allRooms;
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


    private List<List<string>> GetRoomLayoutBasedOnAdjacencies(List<bool> adjacencies, RoomType roomType)
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
            usableRoomLayouts = RoomLayouts.TreasureRoomLayouts;
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


    public List<Room> GenerateRoomData(Dictionary<Vector2Int, RoomType> map, Vector2Int collectorLevelRange, Vector2Int teamSizeRange, CritterAffinity bossAffinity)
    {
        List<Room> allRooms = new List<Room>();
        List<Vector2Int> path = GetAllCoordsInMap(map);

        foreach (KeyValuePair<Vector2Int, RoomType> kvp in map)
        {
            List<bool> adjacencyArray = GetAdjacencyArray(kvp.Key, path);
            List<List<string>> layout = GetRoomLayoutBasedOnAdjacencies(adjacencyArray, kvp.Value);
            Room newRoom = new Room(kvp.Value, kvp.Key, layout);
            allRooms.Add(newRoom);

            PopulateUniqueRoomData(newRoom, teamSizeRange, collectorLevelRange, bossAffinity);
        }

        return allRooms;
    }


    private void PopulateUniqueRoomData(Room room, Vector2Int teamSizeRange, Vector2Int collectorLevelRange, CritterAffinity bossAffinity)
    {
        if (room.Type == RoomType.Boss)
        {
            Collector boss = new Collector(true, teamSizeRange.y, collectorLevelRange, new List<CritterAffinity>(){bossAffinity});
            room.Collectors.Add(boss);
        }
        else if (room.Type == RoomType.Shop)
        {
            room.ShopItems.Add(new MasonJar());
            room.ShopItems.Add(new MasonJar());
            room.ShopItems.Add(new MasonJar());
        }
    }


    private void GenerateCollectors(Vector2Int teamSizeRange, Vector2Int collectorLevelRange)
    {
        List<Room> availableRooms = _allRooms.Where(room => room.Type == RoomType.Normal).ToList();

        for (int i = 0; i < _collectorsPerFloor; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableRooms.Count);
            availableRooms[i].Collectors.Add(new Collector(false, UnityEngine.Random.Range(teamSizeRange.x, teamSizeRange.y+1), collectorLevelRange, null));
            Debug.Log("Collector at " + availableRooms[i].Coordinates.ToString());
            availableRooms.RemoveAt(i);
        }
    }


    public void DisplayCurrentRoom()
    {
        string collectorPosition = "-1";
        if(_currentRoom.Collectors.Count > 0)
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
                    _floorTiles.Add(tileObject);
                }
                if(_currentRoom.Layout[i][j] == "B")
                {
                    GameObject randomBossTile = _bossTilePrefabs[UnityEngine.Random.Range(0, _bossTilePrefabs.Count)];
                    GameObject tileObject = GameObject.Instantiate(randomBossTile, new Vector3(j, 0f, 8-i), Quaternion.identity);
                    tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                    tileObject.GetComponent<Tile>().Type = TileType.Boss;
                    tileObject.GetComponent<Tile>().IsWalkable = false;
                    _floorTiles.Add(tileObject);
                }
                if(_currentRoom.Layout[i][j] == "E")
                {
                    GameObject randomExitTile = _exitTilePrefabs[UnityEngine.Random.Range(0, _exitTilePrefabs.Count)];
                    GameObject tileObject = GameObject.Instantiate(randomExitTile, new Vector3(j, 0f, 8-i), Quaternion.identity);
                    tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                    tileObject.GetComponent<Tile>().Type = TileType.Exit;
                    tileObject.GetComponent<Tile>().IsWalkable = true;
                    _floorTiles.Add(tileObject);
                }
                if(_currentRoom.Layout[i][j] == "M")
                {
                    GameObject randomTreasureTile = _treasureTilePrefabs[UnityEngine.Random.Range(0, _treasureTilePrefabs.Count)];
                    GameObject tileObject = GameObject.Instantiate(randomTreasureTile, new Vector3(j, 0f, 8-i), Quaternion.identity);
                    tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                    tileObject.GetComponent<Tile>().Type = TileType.Treasure;
                    tileObject.GetComponent<Tile>().IsWalkable = false;
                    _floorTiles.Add(tileObject);
                }
                if(_currentRoom.Layout[i][j] == "S")
                {
                    GameObject randomShopTile = _shopTilePrefabs[UnityEngine.Random.Range(0, _shopTilePrefabs.Count)];
                    GameObject tileObject = GameObject.Instantiate(randomShopTile, new Vector3(j, 0f, 8-i), Quaternion.identity);
                    tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                    tileObject.GetComponent<Tile>().Type = TileType.Shop;
                    tileObject.GetComponent<Tile>().IsWalkable = false;
                    _floorTiles.Add(tileObject);
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
                    _floorTiles.Add(tileObject);
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
                        GenerateTrainer(tileObject, _currentRoom.Layout[i][j], _currentRoom.Collectors[0]);
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
        }
    }


    public void GeneratePlayer()
    {
        _player = GameObject.Instantiate(_playerPrefab, _floorTiles[72].transform);
        _player.transform.localPosition = Vector3.zero;
        PlayerController playerController = _player.GetComponent<PlayerController>();
        playerController.CurrentCoords = _floorTiles[72].GetComponent<Tile>().Coordinates;
        Debug.Log("Setting current coords to " + _floorTiles[72].GetComponent<Tile>().Coordinates.ToString());
        playerController.CurrentRoom = _currentRoom;
        _currentRoom.Explored = true;
        MarkAdjacentRooms(_currentRoom);
        SetContainsPlayer(_currentRoom);
        playerController.RoomTiles = _floorTiles;
        playerController.Map = _allRooms;
        playerController.RoomGeneration = this;
        playerController.EncounterController = _encounterController;
        playerController.CollectorController = _collectorController;
    }

    public void GenerateTrainer(GameObject parentTile, string direction, Collector collector)
    {
        GameObject trainer = GameObject.Instantiate(_collectorPrefab, parentTile.transform);
        trainer.transform.localPosition = Vector3.zero;
        CollectorController collectorController = trainer.GetComponent<CollectorController>();
        collectorController.Coordinates = parentTile.GetComponent<Tile>().Coordinates;
        collectorController.Direction = direction;
        collectorController.Collector = collector;
        collectorController.CalculateVisibleCoords();
        _collectorController = collectorController;
    }

    public void PlacePlayerInNewRoom(Vector2Int coords)
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
        playerController.CollectorController = _collectorController;
        _miniMapController.UpdateMap();
    }


    public void MoveRooms(Vector2Int newRoomCoords, Vector2Int currentTileCoords)
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
        PlacePlayerInNewRoom(currentTileCoords);
    }


    public void GenerateStarterCritters()
    {
        Room startRoom = _allRooms.Find(room => room.Type == RoomType.Start);
        List<Type> availableCritterTypes = MasterCollection.GetAllCritterTypes();

        for (int i = 0; i < 3; i++)
        {
            Type randomCritterType = availableCritterTypes[UnityEngine.Random.Range(0, availableCritterTypes.Count)];
            availableCritterTypes.Remove(randomCritterType);

            Critter randomCritter = Activator.CreateInstance(randomCritterType) as Critter;
            randomCritter.SetStartingLevel(1);
            startRoom.StarterPicks.Add(randomCritter);
        }
    }

    public void SetContainsPlayer(Room roomContainingPlayer)
    {
        foreach(Room room in _allRooms)
        {
            room.ContainsPlayer = false;
        }
        roomContainingPlayer.ContainsPlayer = true;
    }
}
