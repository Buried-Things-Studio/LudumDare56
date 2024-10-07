using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CombatState
{
    public Critter PlayerCritter;
    public Critter NpcCritter;
    public bool IsPlayerPriority;

    public MoveID PlayerSelectedMoveID;
    public MoveID NpcSelectedMoveID;

    public Guid PlayerSelectedHealItemTarget;
    public Guid PlayerSelectedSwitchActiveGUID;
}


public class CombatController : MonoBehaviour
{
    [SerializeField] private CombatUIController _viz;
    
    public Player PlayerData;
    public Collector OpponentData;
    public CombatState State = new CombatState();
    public MapState MapState; 
    
    
    public void SetupCombat(Player playerData, Collector opponentData, Critter npcCritter, MapState mapState)
    {
        PlayerData = playerData;
        OpponentData = opponentData;
        MapState = mapState;
        State.PlayerCritter = PlayerData.GetActiveCritter();
        State.NpcCritter = OpponentData == null ? npcCritter : OpponentData.GetActiveCritter();
        State.PlayerCritter.ResetTemporaryStats();
        State.NpcCritter.ResetTemporaryStats();

        _viz.InitializeCombatUI(this, playerData, State.PlayerCritter, State.NpcCritter);
        StartCoroutine(InitializeTurn());
    }


    private IEnumerator InitializeTurn()
    {
        ClearTurnData();
        yield return StartCoroutine(GetNewActiveCritters());
        DetermineStartingCritter();
        PopulateParticipant();
        PickNpcMove();
        _viz.StartPlayerBattleActionChoice();

        yield return null;
    }


    private IEnumerator GetNewActiveCritters()
    {
        if (State.PlayerCritter.CurrentHealth <= 0)
        {
            yield return StartCoroutine(_viz.ChooseNewCritter());

            State.PlayerCritter = PlayerData.GetActiveCritter();
            string critterName = State.PlayerCritter.Name;
            string article =
                State.PlayerCritter.Name.StartsWith("A")
                || State.PlayerCritter.Name.StartsWith("E")
                || State.PlayerCritter.Name.StartsWith("I")
                || State.PlayerCritter.Name.StartsWith("O")
                || State.PlayerCritter.Name.StartsWith("U")
                ? "an"
                : "a";

            yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage($"Let's try {article} {critterName}!"));

            _viz.UpdatePlayerBugData(State.PlayerCritter);
        }

        if (OpponentData != null && State.NpcCritter.CurrentHealth <= 0)
        {
            State.NpcCritter = OpponentData.GetActiveCritter();
            string critterName = State.NpcCritter.Name;
            string article =
                State.PlayerCritter.Name.StartsWith("A")
                || State.PlayerCritter.Name.StartsWith("E")
                || State.PlayerCritter.Name.StartsWith("I")
                || State.PlayerCritter.Name.StartsWith("O")
                || State.PlayerCritter.Name.StartsWith("U")
                ? "an"
                : "a";

            yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage($"The opponent releases {article} {critterName}!"));

            _viz.UpdateNpcBugData(State.NpcCritter);
        }

        yield return null;
    }


    private void ClearTurnData()
    {
        State.PlayerSelectedMoveID = MoveID.None;
        State.NpcSelectedMoveID = MoveID.None;
        State.PlayerSelectedHealItemTarget = Guid.Empty;
        State.PlayerSelectedSwitchActiveGUID = Guid.Empty;
    }


    private void DetermineStartingCritter()
    {
        int playerSpeed = CritterHelpers.GetEffectiveSpeed(State.PlayerCritter);
        int npcSpeed = CritterHelpers.GetEffectiveSpeed(State.NpcCritter);
        
        if (playerSpeed == npcSpeed)
        {
            State.IsPlayerPriority = UnityEngine.Random.Range(0, 2) == 0;

            return;
        }

        State.IsPlayerPriority = playerSpeed > npcSpeed;
    }


    private void PopulateParticipant()
    {
        if (!State.NpcCritter.Participants.Contains(State.PlayerCritter.GUID))
        {
            State.NpcCritter.Participants.Add(State.PlayerCritter.GUID);
        }
    }


    private void PickNpcMove()
    {
        List<Move> npcMoves = State.NpcCritter.Moves;
        State.NpcSelectedMoveID = npcMoves[UnityEngine.Random.Range(0, npcMoves.Count)].ID;
    }


    public void SetPlayerMove(MoveID moveID)
    {
        State.PlayerSelectedMoveID = moveID;
        ExecuteMoves();
    }


    private void ExecuteMoves()
    {
        Critter priorityCritter = State.IsPlayerPriority ? State.PlayerCritter : State.NpcCritter;
        Critter nonPriorityCritter = State.IsPlayerPriority ? State.NpcCritter : State.PlayerCritter;
        MoveID priorityMoveID = State.IsPlayerPriority ? State.PlayerSelectedMoveID : State.NpcSelectedMoveID;
        MoveID nonPriorityMoveID = State.IsPlayerPriority ? State.NpcSelectedMoveID : State.PlayerSelectedMoveID;
        Move priorityMove = priorityCritter.Moves.Find(move => move.ID == priorityMoveID);
        Move nonPriorityMove = nonPriorityCritter.Moves.Find(move => move.ID == nonPriorityMoveID);

        if (State.PlayerSelectedMoveID == MoveID.SwitchActive || State.PlayerSelectedMoveID == MoveID.ThrowMasonJar || State.PlayerSelectedMoveID == MoveID.UseHealItem)
        {
            State.IsPlayerPriority = true;
            priorityCritter = State.PlayerCritter;
            nonPriorityCritter = State.NpcCritter;
            priorityMoveID = State.PlayerSelectedMoveID;
            priorityMove = null;
            nonPriorityMoveID = State.NpcSelectedMoveID;
            nonPriorityMove = State.NpcCritter.Moves.Find(move => move.ID == nonPriorityMoveID);
        }
        else if (State.NpcSelectedMoveID == MoveID.SwitchActive || State.NpcSelectedMoveID == MoveID.UseHealItem)
        {
            State.IsPlayerPriority = false;
            priorityCritter = State.NpcCritter;
            nonPriorityCritter = State.PlayerCritter;
            priorityMoveID = State.NpcSelectedMoveID;
            priorityMove = null;
            nonPriorityMoveID = State.PlayerSelectedMoveID;
            nonPriorityMove = State.PlayerCritter.Moves.Find(move => move.ID == nonPriorityMoveID);
        }

        bool isNonPriorityDead = false;
        bool isWildCatchAttempt = false;
        bool isEndingCombat = false;

        if (priorityMove == null)
        {
            isWildCatchAttempt = ExecuteBattleAction(priorityMoveID);
        }
        else
        {
            TryExecuteMove(priorityCritter, priorityMove, State.IsPlayerPriority);

            if (CheckDeath())
            {
                isEndingCombat = ExecuteDeath();
                
                isNonPriorityDead = true;
            }
        }

        if (!isNonPriorityDead && !isWildCatchAttempt)
        {
            if (nonPriorityMove == null)
            {
                ExecuteBattleAction(nonPriorityMoveID);
            }
            else
            {
                TryExecuteMove(nonPriorityCritter, nonPriorityMove, !State.IsPlayerPriority);

                if (CheckDeath())
                {
                    isEndingCombat = isEndingCombat || ExecuteDeath();
                }
            }
        }

        if (isWildCatchAttempt || isEndingCombat)
        {
            StartCoroutine(GoToMainGame());
        }
        else
        {
            StartCoroutine(ShowTurn());
        }
    }


    private IEnumerator ShowTurn()
    {
        yield return StartCoroutine(_viz.ExecuteVisualSteps());
        
        StartCoroutine(InitializeTurn());
    }


    private bool ExecuteBattleAction(MoveID ID)
    {
        if (ID == MoveID.SwitchActive)
        {
            PlayerSwitchActive();
        }
        else if (ID == MoveID.ThrowMasonJar)
        {
            return PlayerThrowMasonJar();
        }
        else if (ID == MoveID.UseHealItem)
        {
            PlayerUseHealItem();
        }

        return false;
    }


    private bool PlayerThrowMasonJar()
    {
        PlayerData.RemoveItemFromInventory(ItemType.MasonJar);
        
        bool isCatchSuccessful =
            OpponentData == null
            && PlayerData.GetCritters().Count < CritterHelpers.MaxTeamSize
            && State.NpcCritter.CurrentHealth <= CritterHelpers.GetCatchHealthThreshold(State.NpcCritter);
        
        if (OpponentData != null)
        {
            _viz.AddVisualStep(new TryCatchCollectorCritterStep());
        }
        else if (PlayerData.GetCritters().Count >= CritterHelpers.MaxTeamSize)
        {
            _viz.AddVisualStep(new TryCatchTooFullCritterStep());
        }
        {
            _viz.AddVisualStep(new TryCatchStep(State.NpcCritter.Name, isCatchSuccessful));
        }

        if (isCatchSuccessful)
        {
            PlayerData.AddCritter(State.NpcCritter);
            State.NpcCritter.RestoreAllHealth();
            State.NpcCritter.RestoreAllMoveUses();
        }

        return OpponentData == null;
    }


    private void PlayerUseHealItem()
    {
        PlayerData.RemoveItemFromInventory(ItemType.Nectar);
        Critter healTarget = PlayerData.GetCritters().Find(critter => critter.GUID == State.PlayerSelectedHealItemTarget);

        int startingHealth = healTarget.CurrentHealth;
        healTarget.IncreaseHealth(30);

        _viz.AddVisualStep(new HealMessageStep(healTarget.Name, healTarget.CurrentHealth - startingHealth));
        _viz.AddVisualStep(new HealthChangeStep(true, startingHealth, healTarget.CurrentHealth, healTarget.MaxHealth));
    }


    private void PlayerSwitchActive()
    {
        State.PlayerCritter.ResetTemporaryStats();
        PlayerData.SetActiveCritter(State.PlayerSelectedSwitchActiveGUID);
        State.PlayerCritter = PlayerData.GetActiveCritter();

        PopulateParticipant();

        _viz.AddVisualStep(new ChangeActiveStep(State.PlayerCritter.Name));
    }


    private void TryExecuteMove(Critter user, Move move, bool isPlayerUser)
    {
        if (user.StatusEffects.Exists(status => status.StatusType == StatusEffectType.Confuse))
        {
            bool isNoLongerConfused = user.ReduceConfuseTurnsRemaining();
            _viz.AddVisualStep(new ConfuseStatusUpdateStep(user.Name, !isNoLongerConfused));

            if (!isNoLongerConfused && UnityEngine.Random.Range(0, 2) == 0)
            {
                _viz.AddVisualStep(new ConfuseCheckFailureStep(user.Name));
                FailConfusionCheck(user);

                return;
            }
        }

        _viz.AddVisualStep(new DoMoveStep(user.Name, move.Name));
        
        if (!move.IsTargeted || UnityEngine.Random.Range(0, 100) < move.Accuracy)
        {
            List<CombatVisualStep> steps = move.ExecuteMove(State, isPlayerUser);
            _viz.AddVisualSteps(steps);
        }
        else
        {
            _viz.AddVisualStep(new MoveAccuracyCheckFailureStep(user.Name));
        }

        move.CurrentUses--;
    }


    private void FailConfusionCheck(Critter critter)
    {
        critter.DealDamage(CritterHelpers.GetConfusionDamage(critter));
    }


    private bool CheckDeath()
    {
        return State.PlayerCritter.CurrentHealth <= 0 || State.NpcCritter.CurrentHealth <= 0;
    }


    private bool ExecuteDeath()
    {
        if (State.NpcCritter.CurrentHealth <= 0)
        {
            _viz.AddVisualStep(new CritterSquishedStep(State.NpcCritter.Name));
            
            List<Critter> crittersReceivingExp = State.NpcCritter.Participants
                .Select(participantGuid => PlayerData.GetCritters().Find(teamCritter => teamCritter.GUID == participantGuid))
                .Where(critter => critter.CurrentHealth > 0)
                .ToList();
            
            foreach (Critter critter in crittersReceivingExp)
            {
                List<CombatVisualStep> expSteps = critter.IncreaseExp(State.NpcCritter.Level * 100 / crittersReceivingExp.Count);
                _viz.AddVisualSteps(expSteps);
            }
        }

        if (!PlayerData.GetCritters().Exists(critter => critter.CurrentHealth > 0))
        {
            //TODO: go to game over
            //StartCoroutine(GoToMainGame());
            return true;
        }
        else if (OpponentData != null && !OpponentData.GetCritters().Exists(critter => critter.CurrentHealth > 0))
        {
            //TODO: go to win
            //StartCoroutine(GoToMainGame());

            int winnings = 100 * OpponentData.GetCritters().Count;
            PlayerData.AddMoney(winnings);
            _viz.AddVisualStep(new WinningsStep(winnings));

            return true;
        }
        else if (OpponentData == null && State.NpcCritter.CurrentHealth <= 0)
        {
            //StartCoroutine(GoToMainGame());
            return true;
        }

        return false;
    }


    private IEnumerator GoToMainGame()
    {
        yield return StartCoroutine(_viz.ExecuteVisualSteps());

        PlayerData.ClearDeadCritters();
        
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("MainGame");

        RoomGeneration roomGeneration = GameObject.FindObjectOfType<RoomGeneration>();

        while (!sceneLoading.isDone)
        {
            yield return null;
        }

        roomGeneration.GenerateMapFromMapState(MapState);
    }
}
