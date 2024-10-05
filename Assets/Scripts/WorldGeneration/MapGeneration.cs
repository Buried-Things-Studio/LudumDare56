using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MapGeneration
{
    private int _pathLength = 10;
    private int _branchingRooms = 10;
    [SerializeField] private GameObject _startRoomPrefab;
    [SerializeField] private GameObject _normalRoomPrefab;
    [SerializeField] private GameObject _bossRoomPrefab;
    [SerializeField] private GameObject _hospitalRoomPrefab;
    [SerializeField] private GameObject _shopRoomPrefab;
    [SerializeField] private GameObject _treasureRoomPrefab;


    public Dictionary<Vector2Int, RoomType> GenerateMainPath()
    {
        Dictionary<Vector2Int, RoomType> map = new Dictionary<Vector2Int, RoomType>();
        map.Add(new Vector2Int(0, 0), RoomType.Start);
        Vector2Int currentRoom = new Vector2Int(0, 0);
        List<Vector2Int> path = new List<Vector2Int>(){currentRoom};

        for (int i = 0; i < _pathLength - 1; i++)
        {
            Vector2Int newCoord = GetNextRoomCoordinate(path, currentRoom);
            map.Add(newCoord, RoomType.Normal);
            currentRoom = newCoord;
            path.Add(newCoord);
        }

        Vector2Int bossCoord = GetNextRoomCoordinate(path, currentRoom);
        map.Add(bossCoord, RoomType.Boss);

        for (int i = 0; i < _branchingRooms; i ++)
        {
            List<Vector2Int> possibleNextRooms = GetPossibleBranchingRooms(map);
            Vector2Int nextRoomCoords = possibleNextRooms[Random.Range(0, possibleNextRooms.Count)];
            map.Add(nextRoomCoords, RoomType.Normal);
        }

        List<Vector2Int> hospitalNextRooms = GetPossibleBranchingRooms(map);
        Vector2Int hospitalRoomCoords = hospitalNextRooms[Random.Range(0, hospitalNextRooms.Count)];
        map.Add(hospitalRoomCoords, RoomType.Hospital);

        List<Vector2Int> shopNextRooms = GetPossibleBranchingRooms(map);
        Vector2Int shopRoomCoords = shopNextRooms[Random.Range(0, shopNextRooms.Count)];
        map.Add(shopRoomCoords, RoomType.Shop);

        List<Vector2Int> treasureNextRooms = GetPossibleBranchingRooms(map);
        Vector2Int treasureRoomCoords = treasureNextRooms[Random.Range(0, treasureNextRooms.Count)];
        map.Add(treasureRoomCoords, RoomType.Treasure);

        return map;
    }

    private Vector2Int GetNextRoomCoordinate(List<Vector2Int> path, Vector2Int previousCoordinate)
    {
        List<Vector2Int> possibleCoords = GetPossibleNextRoom(previousCoordinate);
        List<Vector2Int> usableCoords = new List<Vector2Int>();
        foreach(Vector2Int coord in possibleCoords)
        {
            if(!path.Contains(coord) && CountAdjacencies(coord, path) == 1)
            {
                usableCoords.Add(coord);
            }
        }
        return usableCoords[Random.Range(0, usableCoords.Count)];
    }

    private List<Vector2Int> GetAdjacentCoordinates(Vector2Int coordinate)
    {
        List<Vector2Int> adjacentCoords = new List<Vector2Int>(){
            new Vector2Int(coordinate.x + 1, coordinate.y),
            new Vector2Int(coordinate.x - 1, coordinate.y),
            new Vector2Int(coordinate.x, coordinate.y + 1),
            new Vector2Int(coordinate.x, coordinate.y - 1)
        };
        return adjacentCoords;
    }

    private int CountAdjacencies(Vector2Int coordinate, List<Vector2Int> path)
    {
        List<Vector2Int> adjacentCoords = GetAdjacentCoordinates(coordinate);
        int count = 0;
        foreach(Vector2Int coord in adjacentCoords)
        {
            if(path.Contains(coord))
            {
                count ++;
            }
        }
        return count;
    }

    private List<Vector2Int> GetPossibleNextRoom(Vector2Int coordinate)
    {
        List<Vector2Int> adjacentCoords = new List<Vector2Int>(){
            new Vector2Int(coordinate.x + 1, coordinate.y),
            new Vector2Int(coordinate.x - 1, coordinate.y),
            new Vector2Int(coordinate.x, coordinate.y + 1)
        };
        return adjacentCoords;
        
    }

    private List<Vector2Int> GetPossibleBranchingRooms(Dictionary<Vector2Int, RoomType> map)
    {
        List<Vector2Int> allAdjacentRooms = new List<Vector2Int>();
        List<Vector2Int> path = new List<Vector2Int>();

        foreach(KeyValuePair<Vector2Int, RoomType> kvp in map)
        {
            path.Add(kvp.Key);
        }

        foreach(KeyValuePair<Vector2Int, RoomType> kvp in map)
        {
            if(kvp.Value == RoomType.Normal)
            {
                List<Vector2Int> adjRooms = GetAdjacentCoordinates(kvp.Key);
                foreach(Vector2Int room in adjRooms)
                {
                    if(!allAdjacentRooms.Contains(room) && !path.Contains(room))
                    {
                        allAdjacentRooms.Add(room);
                    }
                }
            }
        }

        List<Vector2Int> usableRooms = new List<Vector2Int>();
        foreach(Vector2Int room in allAdjacentRooms)
        {
            if(CountAdjacencies(room, path) == 1)
            {
                usableRooms.Add(room);
            }
        }
        return usableRooms;
    }

    private void DisplayMap(Dictionary<Vector2Int, RoomType> map)
    {
        foreach(KeyValuePair<Vector2Int, RoomType> kvp in map)
        {
            if(kvp.Value == RoomType.Start)
            {
                GameObject.Instantiate(_startRoomPrefab, new Vector3(kvp.Key.x, kvp.Key.y, 0f), Quaternion.identity);
            }
            if(kvp.Value == RoomType.Normal)
            {
                GameObject.Instantiate(_normalRoomPrefab, new Vector3(kvp.Key.x, kvp.Key.y, 0f), Quaternion.identity);
            }
            if(kvp.Value == RoomType.Boss)
            {
                GameObject.Instantiate(_bossRoomPrefab, new Vector3(kvp.Key.x, kvp.Key.y, 0f), Quaternion.identity);
            }
            if(kvp.Value == RoomType.Shop)
            {
                GameObject.Instantiate(_shopRoomPrefab, new Vector3(kvp.Key.x, kvp.Key.y, 0f), Quaternion.identity);
            }
            if(kvp.Value == RoomType.Treasure)
            {
                GameObject.Instantiate(_treasureRoomPrefab, new Vector3(kvp.Key.x, kvp.Key.y, 0f), Quaternion.identity);
            }
            if(kvp.Value == RoomType.Hospital)
            {
                GameObject.Instantiate(_hospitalRoomPrefab, new Vector3(kvp.Key.x, kvp.Key.y, 0f), Quaternion.identity);
            }
        }
        
    }
    
}
