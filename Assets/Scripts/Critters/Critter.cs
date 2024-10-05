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