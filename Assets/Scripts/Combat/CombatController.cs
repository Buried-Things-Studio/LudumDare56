using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatState
{
    public Critter PlayerCritter;
    public Critter NpcCritter;
    public bool IsPlayerPriority;

    public MoveID PlayerSelectedMoveID;
    public MoveID NpcSelectedMoveID;


    public Critter GetUserFromGUID(Guid GUID)
    {
        return PlayerCritter.GUID == GUID ? PlayerCritter : NpcCritter;
    }


    public Critter GetOpponentFromGUID(Guid GUID)
    {
        return PlayerCritter.GUID == GUID ? NpcCritter : PlayerCritter;
    }
}


public class CombatController : MonoBehaviour
{
    private CombatState State = new CombatState();
    
    
    public void SetupCombat(Critter playerCritter, Critter npcCritter)
    {
        State.PlayerCritter = playerCritter;
        State.NpcCritter = npcCritter;

        InitializeTurn();
    }


    private void InitializeTurn()
    {
        ClearTurnData();
        DetermineStartingCritter();
        PickNpcMove();
    }


    private void ClearTurnData()
    {
        State.PlayerSelectedMoveID = MoveID.None;
        State.NpcSelectedMoveID = MoveID.None;
    }


    private void DetermineStartingCritter()
    {
        if (State.PlayerCritter.CurrentSpeed == State.NpcCritter.CurrentSpeed)
        {
            State.IsPlayerPriority = UnityEngine.Random.Range(0, 2) == 0;

            return;
        }

        State.IsPlayerPriority = State.PlayerCritter.CurrentSpeed > State.NpcCritter.CurrentSpeed;
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

        priorityMove.ExecuteMove(State);
        priorityMove.CurrentUses--;

        if (CheckCombatFinished())
        {
            FinishCombat();
            
            return;
        }

        nonPriorityMove.ExecuteMove(State);
        nonPriorityMove.CurrentUses--;

        if (CheckCombatFinished())
        {
            FinishCombat();
        }
        else
        {
            InitializeTurn();
        }
    }


    private bool CheckCombatFinished()
    {
        return State.PlayerCritter.CurrentHealth <= 0 || State.NpcCritter.CurrentHealth <= 0;
    }


    private void FinishCombat()
    {

    }
}
