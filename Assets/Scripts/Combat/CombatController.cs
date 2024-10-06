using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CombatState
{
    public Critter PlayerCritter;
    public Critter NpcCritter;
    public bool IsPlayerPriority;

    public MoveID PlayerSelectedMoveID;
    public MoveID NpcSelectedMoveID;

    public Guid PlayerSelectedHealItemTarget;
    public Guid PlayerSelectedSwitchActiveGUID;


    public Critter GetUserFromGUID(Guid GUID)
    {
        return PlayerCritter.GUID == GUID ? PlayerCritter : NpcCritter;
    }


    public Critter GetOpponentFromGUID(Guid GUID)
    {
        return PlayerCritter.GUID == GUID ? NpcCritter : PlayerCritter;
    }


    public bool IsUserPlayer(Guid GUID)
    {
        return PlayerCritter.GUID == GUID;
    }
}


public class CombatController : MonoBehaviour
{
    [SerializeField] private CombatUIController _viz;
    
    public Player PlayerData;
    public Collector OpponentData;
    public CombatState State = new CombatState();
    
    
    public void SetupCombat(Player playerData, Collector opponentData, Critter npcCritter)
    {
        PlayerData = playerData;
        OpponentData = opponentData;
        State.PlayerCritter = PlayerData.GetActiveCritter();
        State.NpcCritter = OpponentData == null ? npcCritter : OpponentData.GetActiveCritter();
        State.PlayerCritter.ResetTemporaryStats();
        State.NpcCritter.ResetTemporaryStats();

        InitializeTurn();

        _viz.InitializeCombatUI(this, playerData, State.PlayerCritter, State.NpcCritter);
        _viz.StartPlayerBattleActionChoice();
    }


    private void InitializeTurn()
    {
        ClearTurnData();
        DetermineStartingCritter();
        PopulateParticipant();
        PickNpcMove();
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

        bool isNonPriorityDead = false;

        if (priorityMove == null)
        {
            ExecuteBattleAction(priorityMoveID);
        }
        else
        {
            TryExecuteMove(priorityCritter, priorityMove);

            if (CheckDeath())
            {
                ExecuteDeath();
                
                isNonPriorityDead = true;
            }
        }

        if (!isNonPriorityDead)
        {
            if (nonPriorityMove == null)
            {
                ExecuteBattleAction(nonPriorityMoveID);
            }
            else
            {
                TryExecuteMove(nonPriorityCritter, nonPriorityMove);

                if (CheckDeath())
                {
                    ExecuteDeath();
                }
            }
        }

        InitializeTurn();
    }


    private void ExecuteBattleAction(MoveID ID)
    {
        if (ID == MoveID.SwitchActive)
        {
            PlayerSwitchActive();
        }
        else if (ID == MoveID.ThrowMasonJar)
        {
            PlayerThrowMasonJar();
        }
        else if (ID == MoveID.UseHealItem)
        {
            PlayerUseHealItem();
        }
    }


    private void PlayerThrowMasonJar()
    {
        bool isCatchSuccessful =
            OpponentData == null
            && PlayerData.GetCritters().Count < CritterHelpers.MaxTeamSize
            && State.NpcCritter.CurrentHealth <= CritterHelpers.GetCatchHealthThreshold(State.NpcCritter);
    }


    private void PlayerUseHealItem()
    {
        //TODO: HEAL
    }


    private void PlayerSwitchActive()
    {
        State.PlayerCritter.ResetTemporaryStats();
        State.PlayerCritter = PlayerData.GetCritters().Find(critter => critter.GUID == State.PlayerSelectedSwitchActiveGUID);

        PopulateParticipant();
    }


    private void TryExecuteMove(Critter user, Move move)
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

        
        if (!move.IsTargeted || UnityEngine.Random.Range(0, 100) < move.Accuracy)
        {
            _viz.AddVisualStep(new DoMoveStep(user.Name, move.Name));
            List<CombatVisualStep> steps = move.ExecuteMove(State);
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


    private void ExecuteDeath()
    {
        if (State.NpcCritter.CurrentHealth <= 0)
        {
            List<Critter> crittersReceivingExp = State.NpcCritter.Participants
                .Select(participantGuid => PlayerData.GetCritters().Find(teamCritter => teamCritter.GUID == participantGuid))
                .Where(critter => critter.CurrentHealth > 0)
                .ToList();
            
            foreach (Critter critter in crittersReceivingExp)
            {
                critter.IncreaseExp(State.NpcCritter.Level * 100 / crittersReceivingExp.Count);
            }
        }

        if (!PlayerData.GetCritters().Exists(critter => critter.CurrentHealth > 0))
        {
            //TODO: go to game over
        }
        else if (OpponentData != null && !OpponentData.GetCritters().Exists(critter => critter.CurrentHealth > 0))
        {
            //TODO: go to win
        }
        else if (State.NpcCritter.CurrentHealth <= 0)
        {
            //TODO: go to win
        }
    }
}
