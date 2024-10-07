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

    public void MoveToPlayer(Vector2Int playerCoords)
    {
        Debug.Log("Moving to player");
        StartCoroutine(SlideToPlayer(playerCoords));

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

    private IEnumerator SlideToPlayer(Vector2Int playerCoords)
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
            yield return StartCoroutine(SmoothMove(transform.position, tile.transform.position));

        }
        Debug.Log("Trainer fight starting now");
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
}
