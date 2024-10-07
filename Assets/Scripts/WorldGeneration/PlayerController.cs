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
    public bool IsInvisibleToEncounters;
    private bool _isMoving; 
    private bool _newTileChecks; 
    private bool _checkContinuedMovement;
    private bool _isMovementBlockedByUI;
    private bool _isMovementBlockedByOverworldMenu;
    private bool _checkNewMovement = true;
    private List<string> _keyPressPriorityOrder = new List<string>();
    public FloorController FloorController;

    //Anim
    [SerializeField] private Transform _meshTransform;
    [SerializeField] private AnimationCurve _bounceCurve;
    [SerializeField] private ParticleSystem _grassParticleSystem;


    public void Update()
    {
        CheckPressedKeys();

        if (_isMoving || _isMovementBlockedByUI || _isMovementBlockedByOverworldMenu)
        {
            return;
        }
        else if (_newTileChecks)
        {
            CheckForEncounters();
        }

        if ((_checkContinuedMovement || _checkNewMovement) && Input.GetKeyDown(Controls.OverworldInteractKey))
        {
            CheckInteraction();
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


    public bool GetAbleToMove()
    {
        return (_checkContinuedMovement || _checkNewMovement) && !_isMovementBlockedByUI;
    }


    public void SetOverworldMenuUIBlock(bool isMenuOpen)
    {
        _isMovementBlockedByOverworldMenu = isMenuOpen;
    }


    private void CheckInteraction()
    {
        Vector2Int coordsInFront = GetTargetCoords("forward");
        GameObject tileObject = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == coordsInFront);
        Tile tile = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == coordsInFront).GetComponent<Tile>();
        TileType tileType = tile.Type;

        if (tileType == TileType.Starter && FloorController.GetCurrentLevel() == 1 && !EncounterController.IsStarterChosen)
        {
            StartCoroutine(TryChooseStarter(tile.Starter));
        }
        if(tileType == TileType.Starter && FloorController.GetCurrentLevel() > 1 )
        {
            StartCoroutine(TryChooseReward(tile.Reward));
        }
        if(tileType == TileType.Boss)
        {
            StartCoroutine(InteractWithBoss());
        }
        if(tileType == TileType.Hospital)
        {
            StartCoroutine(InteractWithHospital());
        }
        if(tileType == TileType.Shop)
        {
            StartCoroutine(InteractWithShop(tile, tileObject));
        }
        if(tileType == TileType.Treasure)
        {
            StartCoroutine(InteractWithTreasure());
        }
    }

    private IEnumerator InteractWithShop(Tile tile, GameObject tileObject)
    {
        _isMovementBlockedByUI = true;
        if(tile.ShopItem == null)
        {
            yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("Nothing more to buy here."));
            _isMovementBlockedByUI = true;
        }
        else{
            int coins = FloorController.PlayerData.GetMoney();
            Debug.Log("coins = " + coins.ToString());
            Item item = tile.ShopItem;
            Debug.Log(item.ID);
            int cost = item.Price;
            if(cost > coins)
            {
                if(item.ID == ItemType.MoveManual)
                {
                    yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMoveMessage(((MoveManual)item).TeachableMove, $"{((MoveManual)item).TeachableMove.Name} costs {cost}. Come back when you have enough coins if you would like to buy it."));
                    _isMovementBlockedByUI = false;
                }
                else{
                    yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage($"This {tile.ShopItem.Name} costs {cost}. Come back when you have enough coins if you would like to buy it."));
                    _isMovementBlockedByUI = false;
                }
            }
            else{
                if(item.ID == ItemType.MoveManual)
                {
                    yield return StartCoroutine(GlobalUI.TextBox.ShowMoveYesNoChoice(((MoveManual)item).TeachableMove, $"{((MoveManual)item).TeachableMove.Name} costs {cost}. Would you like to buy it?"));
                    if(GlobalUI.TextBox.IsSelectingYes)
                    {
                        FloorController.PlayerData.RemoveMoney(cost);
                        FloorController.PlayerData.AddItem(item);
                        tile.ShopItem = null;
                        tileObject.GetComponent<ShopTileController>().ScrollParent.SetActive(false);
                        yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("Thanks, come back again soon!"));
                        _isMovementBlockedByUI= false;
                    }
                    else 
                    {
                        yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("Have a look around, maybe there's something else that will take your fancy."));
                    }
                    _isMovementBlockedByUI= false;
                }
                else{
                    yield return StartCoroutine(GlobalUI.TextBox.ShowYesNoChoice($"This {tile.ShopItem.Name} costs {cost}. Would you like to buy it?"));
                    if(GlobalUI.TextBox.IsSelectingYes)
                    {
                        FloorController.PlayerData.RemoveMoney(cost);
                        FloorController.PlayerData.AddItem(item);
                        tile.ShopItem = null;
                        tileObject.GetComponent<ShopTileController>().NectarParent.SetActive(false);
                        tileObject.GetComponent<ShopTileController>().MasonJarParent.SetActive(false);

                        yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("Thanks, come back again soon!"));
                        _isMovementBlockedByUI= false;
                    }
                    else
                    {
                        yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("Have a look around, maybe there's something else that will take your fancy."));

                    }
                    _isMovementBlockedByUI = false;
                }
            }
        }
        yield return null;
    }

    private IEnumerator InteractWithTreasure()
    {
        _isMovementBlockedByUI = true;
        if(CurrentRoom.Treasure == null)
        {
            yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("You've already picked up the treasure for this floor. Look out for a new move on the next floor!"));
            _isMovementBlockedByUI = false;
        }
        else
        {
            yield return StartCoroutine(GlobalUI.TextBox.ShowMoveYesNoChoice(CurrentRoom.Treasure.TeachableMove, "Oooh a new bug move! Would you like to take this move?"));
            if(GlobalUI.TextBox.IsSelectingYes)
            {
                FloorController.PlayerData.AddItemToInventory(CurrentRoom.Treasure);
                CurrentRoom.Treasure = null;
                yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("Use it wisely!"));
                _isMovementBlockedByUI= false;
            }
            else
            {
                yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("Your loss!"));
                _isMovementBlockedByUI= false;
            }


        }

    }

    private IEnumerator InteractWithHospital()
    {
        _isMovementBlockedByUI = true;
        if(CurrentRoom.HospitalAlreadyUsed)
        {
            yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("Oh no! Hospitals can only be used once and you've already visited this hospital."));
            _isMovementBlockedByUI= false;
        }
        else
        {
            yield return StartCoroutine(GlobalUI.TextBox.ShowYesNoChoice("Hospitals can either heal your bugs or restore their attack uses. You can only do one. Do you want to heal your bugs?"));
            if(GlobalUI.TextBox.IsSelectingYes)
            {
                FloorController.PlayerData.HealAllCritters();
                yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("There you go, all your bugs are healed and ready to fight, good luck!"));
                _isMovementBlockedByUI= false;
            }
            else {
                yield return StartCoroutine(GlobalUI.TextBox.ShowYesNoChoice("Okay, so you want to restore your bugs attack uses?"));
                if(GlobalUI.TextBox.IsSelectingYes)
                {
                    FloorController.PlayerData.RestoreUsesToAllCritters();
                    yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("All your bugs have had their attack uses restored, use them carefully!"));
                    _isMovementBlockedByUI= false;
                }
                else{
                     yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("Then why are you wasting my time? Humph."));
                      _isMovementBlockedByUI= false;
                }
            }
        }
        yield return null;
    }

    private IEnumerator InteractWithBoss()
    {
        _isMovementBlockedByUI= true;

        if(CurrentRoom.Boss.HasBeenDefeated)
        {
            yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("Your bugs really have some bite! Good luck with the next floor."));
            _isMovementBlockedByUI= false;
        }
        else  {
            MapState mapState = new MapState(Map, CurrentRoom.Coordinates, CurrentCoords, Direction);
            FloorController.MapState = mapState;
            yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("So you think you're ready to face me?"));
            CollectorController.StartBossFight(EncounterController);

        }
    }

    private IEnumerator TryChooseReward(MoveManual reward)
    {
        _isMovementBlockedByUI = true;
        if(reward == null)
        {
            yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("You've already chosen a reward for fighting the last boss. Look out for more rewards on the next level!"));
            _isMovementBlockedByUI = false;
        }
        else{
            yield return StartCoroutine(GlobalUI.TextBox.ShowMoveYesNoChoice(reward.TeachableMove, "Would you like to choose this move?"));
            if(GlobalUI.TextBox.IsSelectingYes)
            {
                FloorController.PlayerData.AddItemToInventory(reward);
                yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("You can now teach the move to a bug of your choice."));
                List<Tile> starterTiles = RoomTiles.Where(tile => tile.GetComponent<Tile>().Type == TileType.Starter).Select(tile => tile.GetComponent<Tile>()).ToList();
                foreach(Tile tile in starterTiles)
                {
                    tile.Reward = null;
                }
                _isMovementBlockedByUI= false;
            }
            else {
                yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("Look at the other moves and pick the best one!"));
                _isMovementBlockedByUI= false;

            }
        }

    }


    private IEnumerator TryChooseStarter(Critter starter)
    {
        _isMovementBlockedByUI = true;
        
        yield return StartCoroutine(GlobalUI.TextBox.ShowStarterChoice(starter));

        if (GlobalUI.TextBox.IsSelectingYes)
        {
            EncounterController.IsStarterChosen = true;
            FloorController.PlayerData.AddCritter(starter); //hello new friend!

            List<MasonJarObject> mjos = GameObject.FindObjectsOfType<MasonJarObject>().ToList();

            // List<MasonJarObject> mjos = RoomTiles.Select(room => GetComponentInChildren<MasonJarObject>()).ToList();

            // foreach (GameObject go in RoomTiles)
            // {
            //     Debug.Log(go.GetComponent<Tile>().Type);
            // }
            
            foreach (MasonJarObject mjo in mjos)
            {
                //Debug.Log("Trying to destroy jar...");
                
                if (mjo != null)
                {
                    Debug.Log("DESTROY JAR!");
                    
                    mjo.DestroyJar();
                }
            }
        }

        _isMovementBlockedByUI = false;
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


    private bool CheckTileWalkable(Vector2Int targetCoords)
    {
        GameObject tileObject = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == targetCoords);
        
        if (tileObject == null)
        {
            return false;
        }

        Tile tile = tileObject.GetComponent<Tile>();

        if (tile.Type == TileType.Door && !EncounterController.IsStarterChosen)
        {
            return false;
        }

        return tile.IsWalkable;
    }


    private void AttemptMove(Tile currentTile, Vector2Int targetCoords, string direction)
    {
        if (CheckTileWalkable(targetCoords))
        {
            Tile targetTile = RoomTiles.Find(tile => tile.GetComponent<Tile>().Coordinates == targetCoords).GetComponent<Tile>();
            if (targetTile.Type == TileType.Grass)
            {
                _grassParticleSystem.Play();
            }
            if(targetTile.Type == TileType.Exit && CollectorController.Collector.HasBeenDefeated)
            {
                RoomGeneration.GoToNewLevel();
            }

            MoveToNewTile(targetCoords);
        }
        else if (currentTile.Type == TileType.Door)
        {
            Vector2Int newRoomCoords = CurrentCoords; 
            
            if ((Direction == 0 && direction == "forward") || (Direction == 2 && direction == "backward"))
            {
                newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x, CurrentRoom.Coordinates.y + 1);
            }
            if ((Direction == 1 && direction == "forward") || (Direction == 3 && direction == "backward"))
            {
                newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x + 1, CurrentRoom.Coordinates.y);
            }
            if ((Direction == 2 && direction == "forward") || (Direction == 0 && direction == "backward"))
            {
                newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x, CurrentRoom.Coordinates.y - 1);
            }
            if ((Direction == 3 && direction == "forward") || (Direction == 1 && direction == "backward"))
            {
                newRoomCoords = new Vector2Int(CurrentRoom.Coordinates.x - 1, CurrentRoom.Coordinates.y);
            }

            int faceIntoNewRoom = Direction;

            if (direction == "backward")
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

        MapState mapState = new MapState(Map, CurrentRoom.Coordinates, CurrentCoords, Direction);

        // trainer check
        if (CollectorController != null && !IsInvisibleToEncounters)
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
        if (currentTile.Type == TileType.Grass && !IsInvisibleToEncounters)
        {
            ////Grass particle effect
            //_grassParticleSystem.Play();

            if (EncounterController.CheckRandomEncounter(false))
            {
                FloorController.MapState = mapState;
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

        GameObject.Find("MiniMap").GetComponentInChildren<MiniMapController>().Spin(target);

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

        MapState mapState = new MapState(Map, CurrentRoom.Coordinates, CurrentCoords, Direction);
        FloorController.MapState = mapState;
        CollectorController.MoveToPlayer(CurrentCoords, EncounterController);
    }

}
