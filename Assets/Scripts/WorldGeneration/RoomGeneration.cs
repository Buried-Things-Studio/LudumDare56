using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomGeneration : MonoBehaviour
{
    private Room _currentRoom; 
    private Dictionary<Vector2Int, RoomType> _map;
    private List<Room> _allRooms;
    private List<GameObject> _floorTiles = new List<GameObject>();
    private GameObject _player;
    [SerializeField] private List<GameObject> _normalTilePrefabs;
    [SerializeField] private List<GameObject> _grassTilePrefabs;
    [SerializeField] private List<GameObject> _doorTilePrefabs;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private MapGeneration _mapGeneration;

    public void Start()
    {
        _map = _mapGeneration.GenerateMainPath();
        _allRooms = GenerateRoomData(_map);
        _currentRoom = _allRooms.Find(room => room.Type == RoomType.Start);
        DisplayCurrentRoom();
        GeneratePlayer();
        
    }


    private List<Vector2Int> GetAllCoordsInMap (Dictionary<Vector2Int, RoomType> map)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        foreach(KeyValuePair<Vector2Int, RoomType> kvp in map)
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

        List<bool> adjacencies = new List<bool>();

        adjacencies.Add(path.Contains(northCoord));
        adjacencies.Add(path.Contains(eastCoord));
        adjacencies.Add(path.Contains(southCoord));
        adjacencies.Add(path.Contains(westCoord));

        return adjacencies;
    }

    private List<List<string>> GetNormalRoomLayoutBasedOnAdjacencies(List<bool> adjacencies)
    {
        List<List<List<string>>> usableRoomLayouts = RoomLayouts.NormalRoomLayouts;
        if(adjacencies[0])
        {
            usableRoomLayouts = usableRoomLayouts.Where(room => room[0][7] == "D" && room[0][8] == "D").ToList();
        }
        else
        {
            usableRoomLayouts = usableRoomLayouts.Where(room => room[0][7] != "D" && room[0][8] != "D").ToList();
        }

        if(adjacencies[1])
        {
            usableRoomLayouts = usableRoomLayouts.Where(room => room[4][15] == "D").ToList();
        }
        else
        {
            usableRoomLayouts = usableRoomLayouts.Where(room => room[4][15] != "D").ToList();
        }

        if(adjacencies[2])
        {
            usableRoomLayouts = usableRoomLayouts.Where(room => room[8][7] == "D" && room[8][8] == "D").ToList();
        }
        else
        {
            usableRoomLayouts = usableRoomLayouts.Where(room => room[8][7] != "D" && room[8][8] != "D").ToList();
        }

        if(adjacencies[3])
        {
            usableRoomLayouts = usableRoomLayouts.Where(room => room[4][0] == "D").ToList();
        }
        else
        {
            usableRoomLayouts = usableRoomLayouts.Where(room => room[4][0] != "D").ToList();
        }

        return usableRoomLayouts[Random.Range(0, usableRoomLayouts.Count)];
    }

    public List<Room> GenerateRoomData(Dictionary<Vector2Int, RoomType> map)
    {
        List<Room> allRooms = new List<Room>();
        List<Vector2Int> path = GetAllCoordsInMap(map);

        foreach(KeyValuePair<Vector2Int, RoomType> kvp in map)
        { /// TODO: different layouts for different room types
            List<bool> adjacencyArray = GetAdjacencyArray(kvp.Key, path);
            List<List<string>> layout = GetNormalRoomLayoutBasedOnAdjacencies(adjacencyArray);
            Room newRoom = new Room(kvp.Value, kvp.Key, layout);
            allRooms.Add(newRoom);
        }

        return allRooms;
    }

    public void DisplayCurrentRoom()
    {
        for(int i = 0; i < 9; i ++)
        {
            for(int j = 0; j < 16; j ++)
            {
                if(_currentRoom.Layout[i][j] == "N")
                {
                    GameObject randomNormalTile = _normalTilePrefabs[Random.Range(0, _normalTilePrefabs.Count)];
                    GameObject tileObject = Instantiate(randomNormalTile, new Vector3(j, 8-i, 0f), Quaternion.identity);
                    tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                    tileObject.GetComponent<Tile>().Type = TileType.Normal;
                    _floorTiles.Add(tileObject);
                }
                if(_currentRoom.Layout[i][j] == "G")
                {
                    GameObject randomGrassTile = _grassTilePrefabs[Random.Range(0, _grassTilePrefabs.Count)];
                    GameObject tileObject = Instantiate(randomGrassTile, new Vector3(j, 8-i, 0f), Quaternion.identity);
                    tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                    tileObject.GetComponent<Tile>().Type = TileType.Grass;
                    _floorTiles.Add(tileObject);                
                }
                if(_currentRoom.Layout[i][j] == "D")
                {
                    GameObject randomDoorTile = _doorTilePrefabs[Random.Range(0, _doorTilePrefabs.Count)];
                    GameObject tileObject = Instantiate(randomDoorTile, new Vector3(j, 8-i, 0f), Quaternion.identity);
                    tileObject.GetComponent<Tile>().Coordinates = new Vector2Int(j, 8-i);
                    tileObject.GetComponent<Tile>().Type = TileType.Door;
                    _floorTiles.Add(tileObject);
                }
            }
        }
    }

    public void GeneratePlayer()
    {
        _player = Instantiate(_playerPrefab, _floorTiles[72].transform);
        _player.transform.localPosition = Vector3.zero;
        PlayerController playerController = _player.GetComponent<PlayerController>();
        playerController.CurrentCoords = _floorTiles[72].GetComponent<Tile>().Coordinates;
        Debug.Log("Setting current coords to " + _floorTiles[72].GetComponent<Tile>().Coordinates.ToString());
        playerController.CanMove = true;
        playerController.CurrentRoom = _currentRoom;
        playerController.RoomTiles = _floorTiles;
        playerController.Map = _allRooms;
        playerController.RoomGeneration = this;
    }

    public void PlacePlayerInNewRoom(Vector2Int coords)
    {
        Vector2Int newCoords = new Vector2Int(0, 0);
        if(coords == new Vector2Int(8, 8))
        {
            newCoords = new Vector2Int(8, 0);
        }
        if(coords == new Vector2Int(7, 8))
        {
            newCoords = new Vector2Int(7, 0);
        }
        if(coords == new Vector2Int(8, 0))
        {
            newCoords = new Vector2Int(8, 8);
        }
        if(coords == new Vector2Int(7, 0))
        {
            newCoords = new Vector2Int(7, 8);
        }
        if(coords == new Vector2Int(0, 4))
        {
            newCoords = new Vector2Int(15, 4);
        }
        if(coords == new Vector2Int(15, 4))
        {
            newCoords = new Vector2Int(0, 4);
        }
        Debug.Log("Instantiating now");
        _player = Instantiate(_playerPrefab, _floorTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == newCoords).transform);
        _player.transform.localPosition = Vector3.zero;
        PlayerController playerController = _player.GetComponent<PlayerController>();
        playerController.CurrentCoords = newCoords;
        playerController.CanMove = true;
        playerController.CurrentRoom = _currentRoom;
        playerController.RoomTiles = _floorTiles;
        playerController.Map = _allRooms;
        playerController.RoomGeneration = this;
    }

    public void MoveRooms(Vector2Int newRoomCoords, Vector2Int currentTileCoords)
    {
        foreach(GameObject tile in _floorTiles)
        {
            Destroy(tile);
        }
        _floorTiles.Clear();
        _currentRoom = _allRooms.Find(room => room.Coordinates == newRoomCoords);
        _player.GetComponent<PlayerController>().CurrentRoom = _currentRoom;
        DisplayCurrentRoom();
        PlacePlayerInNewRoom(currentTileCoords);
    }
}
