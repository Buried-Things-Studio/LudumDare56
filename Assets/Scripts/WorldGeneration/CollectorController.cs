using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorController : MonoBehaviour
{
    public string Direction;
    public Vector2Int Coordinates = new Vector2Int(); 
    public Collector Collector; 
    public List<Vector2Int> VisibleCoords = new List<Vector2Int>();
    public List<GameObject> RoomTiles = new List<GameObject>();

    //Anim
    [SerializeField] private Transform _meshTransform;
    [SerializeField] private AnimationCurve _bounceCurve;

    public void CalculateVisibleCoords()
    {
        SnapToDirection();
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

    public void MoveToPlayer(Vector2Int playerCoords, EncounterController encounterController)
    {
        Debug.Log("Moving to player");
        StartCoroutine(SlideToPlayer(playerCoords, encounterController));
    }


    private int CalculateManhattanDistance(Vector2Int currentCoord,  Vector2Int otherCoord)
    {
        int xDistance = Mathf.Abs(currentCoord.x - otherCoord.x);
        int yDistance = Mathf.Abs(currentCoord.y - otherCoord.y);
        return xDistance + yDistance;
    }

    private List<Vector2Int> GetAdjacentCoords()
    {
        List<Vector2Int> adjCoords = new List<Vector2Int>()
        {
            new Vector2Int(Coordinates.x + 1, Coordinates.y),
            new Vector2Int(Coordinates.x - 1, Coordinates.y),
            new Vector2Int(Coordinates.x, Coordinates.y + 1),
            new Vector2Int(Coordinates.x, Coordinates.y - 1),
        };
        return adjCoords; 
    }

    private List<Vector2Int> GetCloserAdjacentCoords(Vector2Int playerCoords)
    {
        List<Vector2Int> adjCoords = GetAdjacentCoords();
        List<Vector2Int> closerAdjCoords = new List<Vector2Int>();
        int currentDistance = CalculateManhattanDistance(Coordinates, playerCoords);
        foreach(Vector2Int coord in adjCoords)
        {
            if(CalculateManhattanDistance(coord, playerCoords) < currentDistance)
            {
                closerAdjCoords.Add(coord);
            }
        }
        return closerAdjCoords;
    }

    private IEnumerator SlideToPlayer(Vector2Int playerCoords, EncounterController encounterController)
    {
        while(CalculateManhattanDistance(Coordinates, playerCoords) > 1)
        {
            List<Vector2Int> closerCoords = GetCloserAdjacentCoords(playerCoords);
            Vector2Int chosenCoord = closerCoords[Random.Range(0, closerCoords.Count)];
            GameObject oldTileObject = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == Coordinates);
            Tile oldTile = oldTileObject.GetComponent<Tile>();
            oldTile.IsWalkable = true;

            GameObject tileObject = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == chosenCoord);
            Tile tile = tileObject.GetComponent<Tile>();
            tile.IsWalkable = false;
            Coordinates = chosenCoord;
            Collector.Coords = chosenCoord;
            yield return StartCoroutine(SmoothMove(transform.position, tile.transform.position));

        }

        Collector.HasBeenDefeated = true;

        yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("You there, battle my bugs!"));

        encounterController.StartCollectorCombat(Collector);
        
        yield return null;
    }


    private IEnumerator SmoothMove(Vector3 startPosition, Vector3 endPosition)
    {
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
    }

    public void SnapToDirection()
    {
        float targetDirection = 0f; 
        if(Direction == "0")
        {
            targetDirection = 0f;
        }
        else if(Direction == "1")
        {
            targetDirection = 90f; 
        }
        else if(Direction == "2")
        {
            targetDirection = 180f; 
        }
        else if(Direction == "3")
        {
            targetDirection = 270f;
        }

        transform.rotation = Quaternion.Euler(new Vector3(0f, targetDirection, 0f));
    }

    public void MoveToTile()
    {
        if(Collector.Coords != new Vector2Int(-100, -100))
        {
            Vector3 meshStartPos = _meshTransform.localPosition;
            GameObject newTileObject = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == Collector.Coords);
            GameObject currentTileObject = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == Collector.Coords);
            newTileObject.GetComponent<Tile>().IsWalkable = false;
            currentTileObject.GetComponent<Tile>().IsWalkable = true;
            transform.position = newTileObject.transform.position;
            _meshTransform.localPosition = meshStartPos;
        }
    }
}
