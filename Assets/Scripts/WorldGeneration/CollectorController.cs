using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorController : MonoBehaviour
{
    public string Direction;
    public Vector2Int Coordinates = new Vector2Int(); 
    public Collector Collector; 
    public List<Vector2Int> VisibleCoords = new List<Vector2Int>();

    public void CalculateVisibleCoords()
    {
        if(Direction == "0")
        {
            for(int i = Coordinates.y + 1; i < 9; i++)
            {
                VisibleCoords.Add(new Vector2Int(Coordinates.x, i));
            }
        }
        else if(Direction == "1")
        {
            for(int i = Coordinates.x + 1; i < 16; i++)
            {
                VisibleCoords.Add(new Vector2Int(i, Coordinates.y));
            }

        }
        else if(Direction == "2")
        {
            for(int i = Coordinates.y - 1; i >= 0; i --)
            {
                VisibleCoords.Add(new Vector2Int(Coordinates.x, i));
            }
        }
        else if(Direction == "3")
        {
            for(int i = Coordinates.x - 1; i >= 0; i --)
            {
                VisibleCoords.Add(new Vector2Int(i, Coordinates.y));
            }

        }
    }

    public void MoveToPlayer()
    {
        Debug.Log("Entering trainer fight");

    }
}
