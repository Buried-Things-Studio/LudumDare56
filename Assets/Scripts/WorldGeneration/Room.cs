using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room
{
    public RoomType Type;
    public Vector2Int Coordinates;
    public List<List<string>> Layout;
    public bool Explored;
    public bool AdjacentToExplored;
    public bool ContainsPlayer;

    public List<Item> ShopItems = new List<Item>();
    public List<Collector> Collectors = new List<Collector>();
    public List<Item> RewardItems = new List<Item>();
    public List<MoveManual> Treasure = new List<MoveManual>();
    public List<Critter> StarterPicks = new List<Critter>();
    public Collector Boss; 
    public bool HospitalAlreadyUsed = false;
    public List<Type> CritterTypesAvailableInRoom = new List<Type>();

    public Room(RoomType type, Vector2Int coords, List<List<string>> layout)
    {
        Type = type;
        Coordinates = coords;
        Layout = layout;
    }
}
