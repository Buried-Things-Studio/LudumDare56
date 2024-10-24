using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class NpcLogic
{
    private static List<Move> GetMovesWithUsesRemaining(Critter user)
    {
        return user.Moves.Where(move => move.CurrentUses > 0).ToList();
    }
    
    
    public static Move GetWildMoveChoice(CombatState state)
    {
        List<Move> npcMoves = GetMovesWithUsesRemaining(state.NpcCritter);

        if (npcMoves.Count == 0)
        {
            return new TriedItsBest();
        }

        return npcMoves[UnityEngine.Random.Range(0, npcMoves.Count)];
    }


    public static Move GetCollectorMoveChoice(CombatState state)
    {
        List<Move> npcMoves = GetMovesWithUsesRemaining(state.NpcCritter);

        if (npcMoves.Count == 0)
        {
            return new TriedItsBest();
        }
        
        List<Move> moveChoicesWeighted = new List<Move>();

        foreach (Move move in npcMoves)
        {
            if (move.BasePower <= 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    moveChoicesWeighted.Add(move);
                }
            }
            else
            {
                int effectiveness = CritterHelpers.GetDamageMultiplier(state.PlayerCritter.Affinities, move.Affinity);

                for (int i = 0; i < effectiveness; i++)
                {
                    moveChoicesWeighted.Add(move);
                }
            }
        }

        return moveChoicesWeighted[UnityEngine.Random.Range(0, moveChoicesWeighted.Count)];
    }


    public static Move GetBossMoveChoice(CombatState state)
    {
        List<Move> npcMoves = GetMovesWithUsesRemaining(state.NpcCritter);

        if (npcMoves.Count == 0)
        {
            return new TriedItsBest();
        }
        
        List<Move> superEffectiveMoves = npcMoves.Where(move => CritterHelpers.GetDamageMultiplier(state.PlayerCritter.Affinities, move.Affinity) >= 8).ToList();
        
        if (superEffectiveMoves.Count > 0)
        {
            return superEffectiveMoves[UnityEngine.Random.Range(0, superEffectiveMoves.Count)];
        }

        int mostEffectiveMove = -1;

        foreach (Move move in state.NpcCritter.Moves)
        {
            if (move.BasePower > 0)
            {
                mostEffectiveMove = Mathf.Max(mostEffectiveMove, CritterHelpers.GetDamageMultiplier(state.PlayerCritter.Affinities, move.Affinity));
            }
        }

        List<Move> moveChoicesWeighted = new List<Move>();

        foreach (Move move in state.NpcCritter.Moves)
        {
            if (move.BasePower <= 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    moveChoicesWeighted.Add(move);
                }
            }
            else
            {
                int effectiveness = CritterHelpers.GetDamageMultiplier(state.PlayerCritter.Affinities, move.Affinity);

                if (effectiveness >= mostEffectiveMove)
                {
                    for (int i = 0; i < effectiveness; i++)
                    {
                        moveChoicesWeighted.Add(move);
                    }
                }
            }
        }

        return moveChoicesWeighted[UnityEngine.Random.Range(0, moveChoicesWeighted.Count)];
    }
}
