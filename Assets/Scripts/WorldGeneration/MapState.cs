using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapState 
{
    public List<Room> Map;
    public Vector2Int PlayerRoom; 
    public Vector2Int PlayerTile; 
    public int PlayerDirection; 

    public MapState(List<Room> map, Vector2Int playerRoom, Vector2Int playerTile, int playerDirection)
    {
        Map = map; 
        PlayerRoom = playerRoom; 
        PlayerTile = playerTile;
        PlayerDirection = playerDirection;
    }
}
