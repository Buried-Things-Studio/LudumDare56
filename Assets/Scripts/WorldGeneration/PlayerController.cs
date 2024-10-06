using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public Vector2Int CurrentCoords;
    public Room CurrentRoom; 
    public List<GameObject> RoomTiles;
    public List<Room> Map;
    public GameObject PlayerObject;
    public RoomGeneration RoomGeneration;
    public CollectorController CollectorController;
    public EncounterController EncounterController;

    private bool _isMoving; 
    private bool _newTileChecks; 
    private bool _checkContinuedMovement;
    private bool _checkNewMovement = true;
    private List<string> _keyPressPriorityOrder = new List<string>();


    public void Update()
    {
        CheckPressedKeys();

        if(_isMoving)
        {
            return;
        }
        else if (_newTileChecks)
        {
            CheckForEncounters();
        }

        if (_checkContinuedMovement)
        {
            CheckForContinuedMovement();
        }
        if (_checkNewMovement)
        {
            CheckForNewMovement();
        }
    }

    private void MoveToNewTile(Vector2Int targetCoords)
    {
        _checkContinuedMovement = false;
        _checkNewMovement = false;
        GameObject tileObject = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == targetCoords);
        Tile tile = tileObject.GetComponent<Tile>();
        CurrentCoords = targetCoords;
        StartCoroutine(SmoothMove(transform.position, tile.transform.position));
    }


    private void AttemptMoveUp(Tile currentTile)
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


    private void AttemptMoveDown(Tile currentTile)
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


    private void AttemptMoveRight(Tile currentTile)
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


    private void AttemptMoveLeft(Tile currentTile)
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


    private void CheckForNewMovement()
    {
        Tile currentTile = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == CurrentCoords).GetComponent<Tile>();
        if(Input.GetKeyDown("up"))
        {
            AttemptMoveUp(currentTile);
        }
        else if(Input.GetKeyDown("down"))
        {
            AttemptMoveDown(currentTile);
        }
        else if(Input.GetKeyDown("right"))
        {
            AttemptMoveRight(currentTile);
        }
        else if(Input.GetKeyDown("left"))
        {
            AttemptMoveLeft(currentTile);
        }
    }
    

    private void CheckForContinuedMovement()
    {
        if(_keyPressPriorityOrder.Count == 0)
        {
            _checkContinuedMovement = false;
            _checkNewMovement = true;
            return;
        }
        string _topPriorityKey = _keyPressPriorityOrder.Last();
        if(Input.GetKey(_topPriorityKey))
        {
            Tile currentTile = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == CurrentCoords).GetComponent<Tile>();
            if(_topPriorityKey == "up")
            {
                AttemptMoveUp(currentTile);
            }
            if(_topPriorityKey == "down")
            {
                AttemptMoveDown(currentTile);
            }
            if(_topPriorityKey == "right")
            {
                AttemptMoveRight(currentTile);
            }
            if(_topPriorityKey == "left")
            {
                AttemptMoveLeft(currentTile);
            }
        }
        else
        {
            _checkContinuedMovement = false;
            _checkNewMovement = true;
            
        }
    }


    private void CheckForEncounters()
    {
        _checkContinuedMovement = false;
        _checkNewMovement = false;

        // trainer check
        if (CollectorController != null)
        {
            if (CollectorController.VisibleCoords.Contains(CurrentCoords))
            {
                _newTileChecks = false;
                CollectorController.MoveToPlayer();

                return;
            }
        }

        Tile currentTile = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == CurrentCoords).GetComponent<Tile>();

        // grass check 
        if (currentTile.Type == TileType.Grass)
        {
            if (EncounterController.CheckRandomEncounter())
            {
                _newTileChecks = false;

                return;
            }
        }

        _newTileChecks = false;
        _checkContinuedMovement = true;
    }


    private void CheckPressedKeys()
    {
        List<string> directionKeys = new List<string>(){
            "up", 
            "down", 
            "left", 
            "right"
        };

        foreach(string direction in directionKeys)
        {
            if(Input.GetKeyDown(direction))
            {
                _keyPressPriorityOrder.Add(direction);
            }
            if(Input.GetKeyUp(direction))
            {
                _keyPressPriorityOrder.Remove(direction);
            }
        }
    }

    private IEnumerator SmoothMove(Vector3 startPosition, Vector3 endPosition)
    {
        _isMoving = true;
        float elapsedTime = 0f;
        float timeToMove = 0.25f;

        while(elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPosition;
        _isMoving = false;
        _newTileChecks = true;
    }
}
