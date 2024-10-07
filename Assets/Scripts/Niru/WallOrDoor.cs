using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOrDoor : MonoBehaviour
{
    [SerializeField] private GameObject[] _brickPiles;
    [SerializeField] private GameObject _regularDoorway;
    [SerializeField] private GameObject _shopDoorway;
    [SerializeField] private GameObject _hospitalDoorway;
    [SerializeField] private GameObject _treasureDoorway;
    [SerializeField] private GameObject _bossDoorway;


    private void Start()
    {
        CheckForDoors();
    }


    private void CheckForDoors()
    {
        bool colliders = false;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.2f);

        foreach(Collider c in hitColliders)
        {
            if (c.gameObject.tag == "Door")
            {
                colliders = true;

                ChooseDoorway(c.GetComponent<Tile>().Type);
                Debug.Log("Doorway");
            }
        }

        if (colliders == false)
        {
            ChooseBricks();
        }
    }


    private void FixedUpdate()
    {
        //CheckForDoors();
    }


    public void ChooseBricks()
    {
        _brickPiles[Random.Range(0, _brickPiles.Length)].SetActive(true);
    }


    public void ChooseDoorway(TileType type)
    {
        switch (type)
        {
            case TileType.Door:
                _regularDoorway.SetActive(true);
                break;
            case TileType.Shop:
                _regularDoorway.SetActive(true);
                break;
            case TileType.Hospital:
                _regularDoorway.SetActive(true);
                break;
            case TileType.Treasure:
                _regularDoorway.SetActive(true);
                break;
            case TileType.Boss:
                _bossDoorway.SetActive(true);
                break;
        }
    }
}
