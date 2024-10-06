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
    public CollectorController CollectorController;
    public EncounterController EncounterController;


    public void Update()
    {
        Tile currentTile = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == CurrentCoords).GetComponent<Tile>();
        if(Input.GetKeyDown("up"))
        {
            Vector2Int targetCoords = new Vector2Int(CurrentCoords.x, CurrentCoords.y +1);
            if(RoomTiles.Exists(tile => tile.GetComponent<Tile>().Coordinates == targetCoords))
            {
                MoveToNewTile(targetCoords);
            }
            else if(currentTile.Type == TileType.Door)
            {
                Vector2Int newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x, CurrentRoom.Coordinates.y + 1);
                RoomGeneration.MoveRooms(newRoomCoords, CurrentCoords);
            }
        }
        if(Input.GetKeyDown("down"))
        {
            Vector2Int targetCoords = new Vector2Int(CurrentCoords.x, CurrentCoords.y -1);
            if(RoomTiles.Exists(tile => tile.GetComponent<Tile>().Coordinates == targetCoords))
            {
                MoveToNewTile(targetCoords);
            }
            else if(currentTile.Type == TileType.Door)
            {
                Vector2Int newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x, CurrentRoom.Coordinates.y - 1);
                RoomGeneration.MoveRooms(newRoomCoords, CurrentCoords);
            }
        }
        if(Input.GetKeyDown("right"))
        {
            Vector2Int targetCoords = new Vector2Int(CurrentCoords.x + 1, CurrentCoords.y);
            if(RoomTiles.Exists(tile => tile.GetComponent<Tile>().Coordinates == targetCoords))
            {
                MoveToNewTile(targetCoords);
            }
            else if(currentTile.Type == TileType.Door)
            {
                Vector2Int newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x + 1, CurrentRoom.Coordinates.y);
                RoomGeneration.MoveRooms(newRoomCoords, CurrentCoords);
            }
        }
        if(Input.GetKeyDown("left"))
        {
            Vector2Int targetCoords = new Vector2Int(CurrentCoords.x - 1, CurrentCoords.y);
            if(RoomTiles.Exists(tile => tile.GetComponent<Tile>().Coordinates == targetCoords))
            {
                MoveToNewTile(targetCoords);
            }
            else if(currentTile.Type == TileType.Door)
            {
                Vector2Int newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x - 1, CurrentRoom.Coordinates.y);
                RoomGeneration.MoveRooms(newRoomCoords, CurrentCoords);
            }
        }
    }

    private void MoveToNewTile(Vector2Int targetCoords)
    {
        GameObject tileObject = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == targetCoords);
        Tile tile = tileObject.GetComponent<Tile>();
        PlayerObject.transform.parent = tileObject.transform;
        PlayerObject.transform.localPosition = Vector3.zero;
        CurrentCoords = targetCoords;

        // trainer check
        if(CollectorController != null)
        {
            if(CollectorController.VisibleCoords.Contains(targetCoords))
            {
                CollectorController.MoveToPlayer();
            }
        }

        // grass check 
        if(tile.Type == TileType.Grass)
        {
            EncounterController.CheckRandomEncounter();
        }
    }
}
