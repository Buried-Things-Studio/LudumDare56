using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsOrDoorsController : MonoBehaviour
{
    [SerializeField] private WallOrDoor[] _wallOrDoors;


    public void Generate()
    {
        foreach(WallOrDoor w in _wallOrDoors)
        {
            w.CheckForDoors();
        }
    }
}
