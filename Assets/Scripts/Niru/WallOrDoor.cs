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


    public void CheckForDoors()
    {
        foreach(GameObject g in _brickPiles)
        {
            g.SetActive(false);
        }

        _regularDoorway.SetActive(false);
        _shopDoorway.SetActive(false);
        _hospitalDoorway.SetActive(false);
        _treasureDoorway.SetActive(false);
        _bossDoorway.SetActive(false);

        bool colliders = false;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.2f);


        foreach(Collider c in hitColliders)
        {
            //Debug.Log("Collision with " + c.gameObject.name);

            if (c.gameObject.tag == "Door")
            {
                colliders = true;

                ChooseDoorway(c.GetComponent<Tile>().ConnectingRoom);
                Debug.Log("Doorway");

                return;
            }
        }

        ChooseBricks();
    }


    public void ChooseBricks()
    {
        _brickPiles[Random.Range(0, _brickPiles.Length)].SetActive(true);
    }


    public void ChooseDoorway(RoomType type)
    {
        switch (type)
        {
            case RoomType.Normal:
                _regularDoorway.SetActive(true);
                break;
            case RoomType.Start:
                _regularDoorway.SetActive(true);
                break;
            case RoomType.Shop:
                _shopDoorway.SetActive(true);
                break;
            case RoomType.Hospital:
                _hospitalDoorway.SetActive(true);
                break;
            case RoomType.Treasure:
                _treasureDoorway.SetActive(true);
                break;
            case RoomType.Boss:
                _bossDoorway.SetActive(true);
                break;
        }
    }
}
