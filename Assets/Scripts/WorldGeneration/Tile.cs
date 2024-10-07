using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    public Vector2Int Coordinates;
    public TileType Type;
    public bool IsWalkable;

    //specific tile type data
    public Critter Starter;
}
