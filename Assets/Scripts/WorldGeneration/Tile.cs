using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    public Vector2Int Coordinates;
    public TileType Type;
    public bool IsWalkable;
    public Critter Starter;
    public MoveManual Reward;
    public Item ShopItem;
    public MoveManual Treasure;
    public RoomType ConnectingRoom = RoomType.None; 
}
