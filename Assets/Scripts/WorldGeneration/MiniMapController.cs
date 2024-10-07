using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MiniMapController : MonoBehaviour
{
    [SerializeField] private RectTransform _myRT;
    public List<Room> Map; 
    public float PixelWidth = 300f;
    private int _lowestX; 
    private int _lowestZ;
    [SerializeField] private GridLayoutGroup _mapLayoutGroup;
    [SerializeField] private GameObject _normalRoomIcon; 
    [SerializeField] private GameObject _shopRoomIcon; 
    [SerializeField] private GameObject _treasureRoomIcon;
    [SerializeField] private GameObject _hospitalRoomIcon; 
    [SerializeField] private GameObject _bossRoomIcon; 
    [SerializeField] private GameObject _unknownRoomIcon; 
    [SerializeField] private GameObject _noRoomIcon; 
    private List<GameObject> icons = new List<GameObject>();
    private Color transparentColor = new Color(1f, 1f, 1f, 0.25f);


    public void Spin(float targetRotation)
    {
        StopAllCoroutines();
        StartCoroutine(SpinToRotation(targetRotation));
    }


    private IEnumerator SpinToRotation(float targetRotation)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / 0.3f;

            _myRT.localRotation = Quaternion.Lerp(_myRT.localRotation, Quaternion.Euler(Vector3.forward * targetRotation), t);

            yield return null;
        }
    }


    public void UpdateMap()
    {
        Vector2Int mapDimensions = CalculateDimensions();
        int maxDimension = Mathf.Max(mapDimensions.x, mapDimensions.y);    
        float squareDimension = PixelWidth/(maxDimension+1);  
        _mapLayoutGroup.cellSize = new Vector2(squareDimension, squareDimension);
        _mapLayoutGroup.constraintCount = mapDimensions.x + 1;

        foreach(GameObject icon in icons)
        {
            Destroy(icon);
        }
        icons.Clear();

        for(int x = _lowestX; x < _lowestX + mapDimensions.x +1; x ++)
        {
            for(int z = _lowestZ; z < _lowestZ + mapDimensions.y + 1; z++)
            {
                if(Map.Exists(room => room.Coordinates == new Vector2Int(x, z)))
                {
                    Room room = Map.Find(room => room.Coordinates == new Vector2Int(x, z));
                    if(room.AdjacentToExplored)
                    {
                        if(room.Type == RoomType.Normal)
                        {
                            GameObject icon = Instantiate(_normalRoomIcon, _mapLayoutGroup.transform);
                            icons.Add(icon);
                            if(!room.ContainsPlayer)
                            {
                                icon.GetComponent<Image>().color = transparentColor;
                            }
                        }
                        if(room.Type == RoomType.Shop)
                        {
                            GameObject icon = Instantiate(_shopRoomIcon, _mapLayoutGroup.transform);
                            icons.Add(icon);
                            if(!room.ContainsPlayer)
                            {
                                icon.GetComponent<Image>().color = transparentColor;
                            }
                        }
                        if(room.Type == RoomType.Treasure)
                        {
                            GameObject icon = Instantiate(_treasureRoomIcon, _mapLayoutGroup.transform);
                            icons.Add(icon);
                            if(!room.ContainsPlayer)
                            {
                                icon.GetComponent<Image>().color = transparentColor;
                            }                        
                        }
                        if(room.Type == RoomType.Hospital)
                        {
                            GameObject icon = Instantiate(_hospitalRoomIcon, _mapLayoutGroup.transform);
                            icons.Add(icon);
                            if(!room.ContainsPlayer)
                            {
                                icon.GetComponent<Image>().color = transparentColor;
                            }                        
                        }
                        if(room.Type == RoomType.Boss)
                        {
                            GameObject icon = Instantiate(_bossRoomIcon, _mapLayoutGroup.transform);
                            icons.Add(icon);
                            if(!room.ContainsPlayer)
                            {
                                icon.GetComponent<Image>().color = transparentColor;
                            }                        
                        }
                        if(room.Type == RoomType.Start)
                        {
                            GameObject icon = Instantiate(_normalRoomIcon, _mapLayoutGroup.transform);
                            icons.Add(icon);
                            if(!room.ContainsPlayer)
                            {
                                icon.GetComponent<Image>().color = transparentColor;
                            }                        
                        }
                    }
                    else
                    {
                        GameObject icon = Instantiate(_noRoomIcon, _mapLayoutGroup.transform);
                        icons.Add(icon);
                        icon.GetComponent<Image>().color = transparentColor;
                    }
                }
                else
                {
                    GameObject icon = Instantiate(_noRoomIcon, _mapLayoutGroup.transform);
                    icons.Add(icon);
                    icon.GetComponent<Image>().color = transparentColor;
                }
            }
        }
    }


    private Vector2Int CalculateDimensions()
    {
        int highestX = 0; 
        int lowestX = 0;
        int highestZ = 0; 
        int lowestZ = 0;

        foreach(Room room in Map)
        {
            if(room.Coordinates.x > highestX)
            {
                highestX = room.Coordinates.x;
            }
            if(room.Coordinates.x < lowestX)
            {
                lowestX = room.Coordinates.x;
            }
            if(room.Coordinates.y > highestZ)
            {
                highestZ = room.Coordinates.y;
            }
            if(room.Coordinates.y < lowestZ)
            {
                lowestZ = room.Coordinates.y;
            }
        }
        _lowestX = lowestX;
        _lowestZ = lowestZ;
        int xDist = Mathf.Abs(highestX - lowestX);
        int zDist = Mathf.Abs(highestZ - lowestZ);
        return new Vector2Int(xDist, zDist);
    }
}
