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
    public int Direction = 0;
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
        if((direction == "forward" && Direction == 0) || (direction == "backward" && Direction == 2))
        {
            targetCoords = new Vector2Int(CurrentCoords.x, CurrentCoords.y + 1);
        }
        if((direction == "forward" && Direction == 2) || (direction == "backward" && Direction == 0))
        {
            targetCoords = new Vector2Int(CurrentCoords.x, CurrentCoords.y - 1);
        }
        if((direction == "forward" && Direction == 1) || (direction == "backward" && Direction == 3))
        {
            targetCoords = new Vector2Int(CurrentCoords.x + 1, CurrentCoords.y);
        }
        if((direction == "forward" && Direction == 3) || (direction == "backward" && Direction == 1))
        {
            targetCoords = new Vector2Int(CurrentCoords.x - 1, CurrentCoords.y);
        }
        return targetCoords;
    }

    private void AttemptMove(Tile currentTile, Vector2Int targetCoords, string direction)
    {
        if(RoomTiles.Exists(tile => tile.GetComponent<Tile>().Coordinates == targetCoords && tile.GetComponent<Tile>().IsWalkable))
        {
            MoveToNewTile(targetCoords);
        }
        else if(currentTile.Type == TileType.Door)
        {
            Vector2Int newRoomCoords = CurrentCoords; 
            if((Direction == 0 && direction == "forward") || (Direction == 2 && direction == "backward"))
            {
                newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x, CurrentRoom.Coordinates.y + 1);
            }
            if((Direction == 1 && direction == "forward") || (Direction == 3 && direction == "backward"))
            {
                newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x + 1, CurrentRoom.Coordinates.y);
            }
            if((Direction == 2 && direction == "forward") || (Direction == 0 && direction == "backward"))
            {
                newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x, CurrentRoom.Coordinates.y - 1);
            }
            if((Direction == 3 && direction == "forward") || (Direction == 1 && direction == "backward"))
            {
                newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x - 1, CurrentRoom.Coordinates.y);
            }
            int faceIntoNewRoom = Direction;
            if(direction == "backward")
            {
                faceIntoNewRoom = (Direction + 2) % 4;
            }
            RoomGeneration.MoveRooms(newRoomCoords, CurrentCoords, faceIntoNewRoom);
        }
    }


    private void CheckForNewMovement()
    {
        Tile currentTile = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == CurrentCoords).GetComponent<Tile>();
        if(Input.GetKeyDown("up"))
        {
            AttemptMove(currentTile, GetTargetCoords("forward"), "forward");
        }
        else if(Input.GetKeyDown("down"))
        {
            AttemptMove(currentTile, GetTargetCoords("backward"), "backward");
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
                AttemptMove(currentTile, GetTargetCoords("forward"), "forward");
            }
            if(_topPriorityKey == "down")
            {
                AttemptMove(currentTile, GetTargetCoords("backward"), "backward");
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
            if (!CollectorController.Collector.HasBeenDefeated && CollectorController.VisibleCoords.Contains(CurrentCoords))
            {
                _newTileChecks = false;
                StartCoroutine(TurnTowardsTrainer());
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
        if(Direction == 0)
        {
            currentDirection = 0f;
        }
        else if(Direction == 1)
        {
            currentDirection = 90f; 
        }
        else if(Direction == 2)
        {
            currentDirection = 180f; 
        }
        else if(Direction == 3)
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
        Direction = (newDirection + 4) % 4;
        Debug.Log("Direction = " + Direction.ToString());
        _isMoving = false;
        _checkContinuedMovement = true;
	}

    public void SnapToDirection()
    {
        float targetDirection = 0f; 
        if(Direction == 0)
        {
            targetDirection = 0f;
        }
        else if(Direction == 1)
        {
            targetDirection = 90f; 
        }
        else if(Direction == 2)
        {
            targetDirection = 180f; 
        }
        else if(Direction == 3)
        {
            targetDirection = 270f;
        }

        transform.rotation = Quaternion.Euler(new Vector3(0f, targetDirection, 0f));
    }

    private IEnumerator TurnTowardsTrainer()
    {
        int desiredDirection = 0;
        Vector2Int trainerCoords = CollectorController.Coordinates;
        if(CurrentCoords.x == trainerCoords.x && CurrentCoords.y < trainerCoords.y)
        {
            desiredDirection = 0;
        }
        if(CurrentCoords.x == trainerCoords.x && CurrentCoords.y > trainerCoords.y)
        {
            desiredDirection = 2;
        }
        if(CurrentCoords.x < trainerCoords.x && CurrentCoords.y == trainerCoords.y)
        {
            desiredDirection = 1;
        }
        if(CurrentCoords.x > trainerCoords.x && CurrentCoords.y == trainerCoords.y)
        {
            desiredDirection = 3;
        }

        int offset = desiredDirection - Direction; 
        offset = (offset + 4) % 4;

        if(offset == 3)
        {
            yield return StartCoroutine(SmoothRotate(3));
        }
        if(offset == 1)
        {
            yield return StartCoroutine(SmoothRotate(1));
        }
        if(offset == 2)
        {
            yield return StartCoroutine(SmoothRotate(3));
            yield return StartCoroutine(SmoothRotate(3));
        }
        
        _checkContinuedMovement = false;
        _checkNewMovement = false;
        
        CollectorController.MoveToPlayer(CurrentCoords, EncounterController);
    }

}
