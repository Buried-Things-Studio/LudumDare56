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
    private int _direction = 0;
    private bool _isMoving; 
    private bool _newTileChecks; 
    private bool _checkContinuedMovement;
    private bool _checkNewMovement = true;
    private List<string> _keyPressPriorityOrder = new List<string>();

    //Anim
    [SerializeField] private Transform _meshTransform;
    [SerializeField] private AnimationCurve _bounceCurve;


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

    private Vector2Int GetTargetCoords(string direction)
    {
        Vector2Int targetCoords = new Vector2Int();
        if((direction == "forward" && _direction == 0) || (direction == "backward" && _direction == 2))
        {
            targetCoords = new Vector2Int(CurrentCoords.x, CurrentCoords.y + 1);
        }
        if((direction == "forward" && _direction == 2) || (direction == "backward" && _direction == 0))
        {
            targetCoords = new Vector2Int(CurrentCoords.x, CurrentCoords.y - 1);
        }
        if((direction == "forward" && _direction == 1) || (direction == "backward" && _direction == 3))
        {
            targetCoords = new Vector2Int(CurrentCoords.x + 1, CurrentCoords.y);
        }
        if((direction == "forward" && _direction == 3) || (direction == "backward" && _direction == 1))
        {
            targetCoords = new Vector2Int(CurrentCoords.x - 1, CurrentCoords.y);
        }
        return targetCoords;
    }

    private void AttemptMove(Tile currentTile, Vector2Int targetCoords)
    {
        if(RoomTiles.Exists(tile => tile.GetComponent<Tile>().Coordinates == targetCoords && tile.GetComponent<Tile>().IsWalkable))
        {
            MoveToNewTile(targetCoords);
        }
        else if(currentTile.Type == TileType.Door)
        {
            Vector2Int newRoomCoords = CurrentCoords; 
            if(_direction == 0)
            {
                newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x, CurrentRoom.Coordinates.y + 1);
            }
            if(_direction == 1)
            {
                newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x + 1, CurrentRoom.Coordinates.y);
            }
            if(_direction == 2)
            {
                newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x, CurrentRoom.Coordinates.y - 1);
            }
            if(_direction == 3)
            {
                newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x - 1, CurrentRoom.Coordinates.y);
            }
            RoomGeneration.MoveRooms(newRoomCoords, CurrentCoords);
        }
    }


    private void CheckForNewMovement()
    {
        Tile currentTile = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == CurrentCoords).GetComponent<Tile>();
        if(Input.GetKeyDown("up"))
        {
            AttemptMove(currentTile, GetTargetCoords("forward"));
        }
        else if(Input.GetKeyDown("down"))
        {
            AttemptMove(currentTile, GetTargetCoords("backward"));
        }
        else if(Input.GetKeyDown("right"))
        {
            StartCoroutine(SmoothRotate(1));
        }
        else if(Input.GetKeyDown("left"))
        {
            StartCoroutine(SmoothRotate(3));
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
                AttemptMove(currentTile, GetTargetCoords("forward"));
            }
            if(_topPriorityKey == "down")
            {
                AttemptMove(currentTile, GetTargetCoords("backward"));
            }
            if(_topPriorityKey == "left")
            {
                StartCoroutine(SmoothRotate(3));
            }
            if(_topPriorityKey == "right")
            {
                StartCoroutine(SmoothRotate(1));
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
                CollectorController.MoveToPlayer(CurrentCoords);

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
        float timeToMove = 0.15f;

        Vector3 meshStartPos = _meshTransform.localPosition;

        while (elapsedTime < timeToMove)
        {
            elapsedTime += Time.deltaTime;

            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / timeToMove));

            _meshTransform.localPosition = meshStartPos + Vector3.up * _bounceCurve.Evaluate((elapsedTime / 15) * 100);

            yield return null;
        }

        _meshTransform.localPosition = meshStartPos;

        transform.position = endPosition;
        _isMoving = false;
        _newTileChecks = true;
    }

    IEnumerator SmoothRotate(int direction) 
	{	
        _checkNewMovement = false;
        _checkContinuedMovement = false;
        _isMoving = true;
        float angle = direction == 1 ? 90f : -90;
        float currentDirection = 0f; 
        if(_direction == 0)
        {
            currentDirection = 0f;
        }
        else if(_direction == 1)
        {
            currentDirection = 90f; 
        }
        else if(_direction == 2)
        {
            currentDirection = 180f; 
        }
        else if(_direction == 3)
        {
            currentDirection = 270f;
        }
        float target = currentDirection + angle;
        float elapsedTime = 0f;
        float timeToMove = 0.3f;

        while (elapsedTime < timeToMove)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Euler(new Vector3(0f, Mathf.Lerp(currentDirection, target, (elapsedTime / timeToMove)), 0f));
            yield return null;
        }
        transform.rotation = Quaternion.Euler(new Vector3(0f, target, 0f));
        int targetDegrees = (int)target;
        int newDirection = targetDegrees/90; 
        _direction = (newDirection + 4) % 4;
        Debug.Log("Direction = " + _direction.ToString());
        _isMoving = false;
        _checkContinuedMovement = true;
	}

}
