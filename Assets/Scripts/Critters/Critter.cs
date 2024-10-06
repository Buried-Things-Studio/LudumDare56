using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Critter
{
    public string Name;
    public Guid GUID;
    public List<CritterAffinity> Affinities = new List<CritterAffinity>();
    public List<Move> Moves = new List<Move>();
    public List<Guid> Participants = new List<Guid>();
    public List<StatusEffect> StatusEffects = new List<StatusEffect>();

    public int Level;
    public int Exp;

    public int MaxHealth;
    public int CurrentHealth;
    public Vector2Int HealthLevelIncrease;

    public int MaxSpeed;
    public int SpeedStage;
    public Vector2Int SpeedLevelIncrease;

    public int MaxSharpAttack;
    public int SharpAttackStage;
    public Vector2Int SharpAttackLevelIncrease;

    public int MaxSharpDefense;
    public int SharpDefenseStage;
    public Vector2Int SharpDefenseLevelIncrease;

    public int MaxBluntAttack;
    public int BluntAttackStage;
    public Vector2Int BluntAttackLevelIncrease;

    public int MaxBluntDefense;
    public int BluntDefenseStage;
    public Vector2Int BluntDefenseLevelIncrease;


    public void SetStartingLevel(int startingLevel)
    {
        for (int i = 0; i < startingLevel; i++)
        {
            IncreaseLevel();
        }
    }

    public void SetStartingMoves(OpponentType opponentType)
    {
        int desiredMoveCount = 0;
        int moveQuality = 0 
        if(Level < 4)
        {
            if(opponentType == OpponentType.Wild)
            {
                desiredMoveCount = 2;
                moveQuality = 0;
            }
            if(opponentType == OpponentType.Trainer || opponentType == OpponentType.Boss)
            {
                desiredMoveCount = 3;
                moveQuality = 1;
            }
        }
        else
        {
            if(opponentType == OpponentType.Wild)
            {
                desiredMoveCount = 3;
                moveQuality = 1;
            }
            if(opponentType == OpponentType.Trainer || opponentType == OpponentType.Boss)
            {
                desiredMoveCount = 4;
                if(Level < 8)
                {
                    moveQuality = 2;
                }
                else 
                {
                    moveQuality = 3;
                }
            }
        }

        int remainingMovesToApply = Moves.Count - desiredMoveCount;
        List<Moves> allMoves = MasterCollection.GetAllMoveTypes();
        foreach(Move move in Moves)
        {
            allMoves.Remove(move);
        }
        List<Moves> correctTypeMoves = allMoves.Where(move => Affinities.Contains(move.Affinity)).ToList();
        List<Moves> orderedCorrectMoves = correctTypeMoves.OrderBy(move => move.MaxUses).ToList();

        if(moveQuality == 1)
        {
            if(correctTypeMoves.Count > remainingMovesToApply)
            {
                correctTypeMoves.RemoveAt(0);
            }
        }

        if(moveQuality == 2)
        {
            if(correctTypeMoves.Count > remainingMovesToApply)
            {
                correctTypeMoves.RemoveAt(correctTypeMoves.Count - 1);
            }
        }

        for(int i = 0; i < remainingMovesToApply; i ++)
        {
            if(correctTypeMoves.Count == 0)
            {
                int index = Random.Range(0, allMoves.Count);
                Moves.Add(allMoves[index]);
                allMoves.RemoveAt(index);
            }
            else if(moveQuality == 0)
            {
                Move move = correctTypeMoves.Last();
                Moves.Add(move);
                correctTypeMoves.Remove(move);
            }
            else if(moveQuality == 3)
            {
                Move move = correctTypeMoves[0];
                Moves.Add(move);
                correctTypeMoves.Remove(move);
            }
            else if(moveQuality == 1)
            {
                int index = Random.Range(0, correctTypeMoves.Count);
                Moves.Add(correctTypeMoves[index]);
                correctTypeMoves.RemoveAt(index);
            }
            else if(moveQuality == 2)
            {
                int index = Random.Range(0, correctTypeMoves.Count);
                Moves.Add(correctTypeMoves[index]);
                correctTypeMoves.RemoveAt(index);
            }
        }
    }


    public void ResetTemporaryStats()
    {
        Participants.Clear();
        StatusEffects = StatusEffects.Where(status => status.StatusType != StatusEffectType.Confuse).ToList();
        SpeedStage = 0;
        SharpAttackStage = 0;
        SharpDefenseStage = 0;
        BluntAttackStage = 0;
        BluntDefenseStage = 0;
    }


    public void IncreaseLevel()
    {
        Level++;
        Exp = 0;

        int healthIncrease = UnityEngine.Random.Range(HealthLevelIncrease.x, HealthLevelIncrease.y + 1);
        MaxHealth += healthIncrease;
        CurrentHealth += healthIncrease;

        int speedIncrease = UnityEngine.Random.Range(SpeedLevelIncrease.x, SpeedLevelIncrease.y + 1);
        MaxSpeed += speedIncrease;

        int sharpAttackIncrease = UnityEngine.Random.Range(SharpAttackLevelIncrease.x, SharpAttackLevelIncrease.y + 1);
        MaxSharpAttack += sharpAttackIncrease;

        int sharpDefenseIncrease = UnityEngine.Random.Range(SharpDefenseLevelIncrease.x, SharpDefenseLevelIncrease.y + 1);
        MaxSharpDefense += sharpDefenseIncrease;

        int bluntAttackIncrease = UnityEngine.Random.Range(BluntAttackLevelIncrease.x, BluntAttackLevelIncrease.y + 1);
        MaxBluntAttack += bluntAttackIncrease;

        int bluntDefenseIncrease = UnityEngine.Random.Range(BluntDefenseLevelIncrease.x, BluntDefenseLevelIncrease.y + 1);
        MaxBluntDefense += bluntDefenseIncrease;
    }


    public void IncreaseExp(int exp)
    {
        int remainingExpToApply = exp;

        while (remainingExpToApply > 0)
        {
            if (Level + 1 >= CritterHelpers.ExpToNextLevel.Count)
            {
                Exp = remainingExpToApply;

                break;
            }
            
            int expToNextLevel = CritterHelpers.ExpToNextLevel[Level];

            if (expToNextLevel > remainingExpToApply)
            {
                Exp = remainingExpToApply;
                remainingExpToApply = 0;
            }
            else
            {
                remainingExpToApply -= expToNextLevel;
                IncreaseLevel();
            }
        }
    }


    public int IncreaseHealth(int increaseAmount)
    {
        int healthBeforeHeal = CurrentHealth;
        CurrentHealth = Mathf.Min(CurrentHealth + increaseAmount, MaxHealth);

        return CurrentHealth - healthBeforeHeal;
    }


    public int DealDamage(int damageAmount)
    {
        int healthBeforeDamage = CurrentHealth;
        CurrentHealth = Mathf.Max(CurrentHealth - damageAmount, 0);

        return healthBeforeDamage - CurrentHealth;
    }


    public void SetStatusEffect(StatusEffectType newStatus)
    {
        if (!StatusEffects.Exists(status => status.StatusType == newStatus))
        {
            StatusEffects.Add(new StatusEffect(newStatus));
        }
    }


    public bool ReduceConfuseTurnsRemaining()
    {
        StatusEffect confuse = StatusEffects.Find(status => status.StatusType == StatusEffectType.Confuse);
        confuse.TurnsRemaining--;

        if (confuse.TurnsRemaining <= 0)
        {
            StatusEffects.Remove(confuse);

            return true;
        }

        return false;
    }


    public void ChangeSpeedStage(int change)
    {
        SpeedStage += change; //TODO: cap these?
    }


    public void ChangeBluntAttackStage(int change)
    {
        BluntAttackStage += change;
    }


    public void ChangeBluntDefenseStage(int change)
    {
        BluntDefenseStage += change;
    }


    public void ChangeSharpAttackStage(int change)
    {
        SharpAttackStage += change;
    }


    public void ChangeSharpDefenseStage(int change)
    {
        SharpDefenseStage += change;
    }
}


public class StatusEffect
{
    public StatusEffectType StatusType;
    public int TurnsRemaining;
    
    
    public StatusEffect(StatusEffectType typeOfEffect)
    {
        StatusType = typeOfEffect;
        TurnsRemaining = UnityEngine.Random.Range(2, 6);
    }
}


public enum StatusEffectType
{
    None,
    Confuse,
}


public enum CritterAffinity
{
    None,
    Ant,
    Bee,
    Beetle,
    Caterpillar,
    Mollusc,
    Spider
}