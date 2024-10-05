using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public Vector2Int CurrentCoords;
    public bool CanMove;
    public Room CurrentRoom; 
    public List<GameObject> RoomTiles;
    public List<Room> Map;
    public GameObject PlayerObject;
    public RoomGeneration RoomGeneration;

    public void Update()
    {
        Tile currentTile = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == CurrentCoords).GetComponent<Tile>();
        if(Input.GetKeyDown("up"))
        {
            Vector2Int targetCoords = new Vector2Int(CurrentCoords.x, CurrentCoords.y +1);
            if(RoomTiles.Exists(tile => tile.GetComponent<Tile>().Coordinates == targetCoords))
            {
                PlayerObject.transform.parent = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == targetCoords).transform;
                PlayerObject.transform.localPosition = Vector3.zero;
                CurrentCoords = targetCoords;
            }
            else if(currentTile.Type == TileType.Door)
            {
                Vector2Int newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x, CurrentRoom.Coordinates.y + 1);
                RoomGeneration.MoveRooms(newRoomCoords, CurrentCoords);
            }
        }
        if(Input.GetKeyDown("down"))
        {
            if(RoomTiles.Exists(tile => tile.GetComponent<Tile>().Coordinates == new Vector2Int(CurrentCoords.x, CurrentCoords.y -1)))
            {
                PlayerObject.transform.parent = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == new Vector2Int(CurrentCoords.x, CurrentCoords.y -1)).transform;
                PlayerObject.transform.localPosition = Vector3.zero;
                CurrentCoords = new Vector2Int(CurrentCoords.x, CurrentCoords.y -1);
            }
            else if(currentTile.Type == TileType.Door)
            {
                Vector2Int newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x, CurrentRoom.Coordinates.y - 1);
                RoomGeneration.MoveRooms(newRoomCoords, CurrentCoords);
            }
        }
        if(Input.GetKeyDown("right"))
        {
            if(RoomTiles.Exists(tile => tile.GetComponent<Tile>().Coordinates == new Vector2Int(CurrentCoords.x + 1, CurrentCoords.y)))
            {
                PlayerObject.transform.parent = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == new Vector2Int(CurrentCoords.x + 1, CurrentCoords.y)).transform;
                PlayerObject.transform.localPosition = Vector3.zero;
                CurrentCoords = new Vector2Int(CurrentCoords.x + 1, CurrentCoords.y);
            }
            else if(currentTile.Type == TileType.Door)
            {
                Vector2Int newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x + 1, CurrentRoom.Coordinates.y);
                RoomGeneration.MoveRooms(newRoomCoords, CurrentCoords);
            }
        }
        if(Input.GetKeyDown("left"))
        {
            if(RoomTiles.Exists(tile => tile.GetComponent<Tile>().Coordinates == new Vector2Int(CurrentCoords.x - 1, CurrentCoords.y)))
            {
                PlayerObject.transform.parent = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == new Vector2Int(CurrentCoords.x - 1, CurrentCoords.y)).transform;
                PlayerObject.transform.localPosition = Vector3.zero;
                CurrentCoords = new Vector2Int(CurrentCoords.x - 1, CurrentCoords.y);
            }
            else if(currentTile.Type == TileType.Door)
            {
                Vector2Int newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x - 1, CurrentRoom.Coordinates.y);
                RoomGeneration.MoveRooms(newRoomCoords, CurrentCoords);
            }
        }
    }
}
