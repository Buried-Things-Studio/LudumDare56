using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Critter
{
    public string Name;
    public Guid GUID;
    public List<CritterAffinity> Affinities;
    public List<Move> Moves;

    public int Level;
    public int Exp;

    public int MaxHealth;
    public int CurrentHealth;
    public Vector2Int HealthLevelIncrease;

    public int MaxSpeed;
    public int CurrentSpeed;
    public Vector2Int SpeedLevelIncrease;

    public int MaxSharpAttack;
    public int CurrentSharpAttack;
    public Vector2Int SharpAttackLevelIncrease;

    public int MaxSharpDefense;
    public int CurrentSharpDefense;
    public Vector2Int SharpDefenseLevelIncrease;

    public int MaxBluntAttack;
    public int CurrentBluntAttack;
    public Vector2Int BluntAttackLevelIncrease;

    public int MaxBluntDefense;
    public int CurrentBluntDefense;
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
        CurrentSpeed += speedIncrease;

        int sharpAttackIncrease = UnityEngine.Random.Range(SharpAttackLevelIncrease.x, SharpAttackLevelIncrease.y + 1);
        MaxSharpAttack += sharpAttackIncrease;
        CurrentSharpAttack += sharpAttackIncrease;

        int sharpDefenseIncrease = UnityEngine.Random.Range(SharpDefenseLevelIncrease.x, SharpDefenseLevelIncrease.y + 1);
        MaxSharpDefense += sharpDefenseIncrease;
        CurrentSharpDefense += sharpDefenseIncrease;

        int bluntAttackIncrease = UnityEngine.Random.Range(BluntAttackLevelIncrease.x, BluntAttackLevelIncrease.y + 1);
        MaxBluntAttack += bluntAttackIncrease;
        CurrentBluntAttack += bluntAttackIncrease;

        int bluntDefenseIncrease = UnityEngine.Random.Range(BluntDefenseLevelIncrease.x, BluntDefenseLevelIncrease.y + 1);
        MaxBluntDefense += bluntDefenseIncrease;
        CurrentBluntDefense += bluntDefenseIncrease;
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


    public int DealDamage(int damageAmount, CritterAffinity damagingAffinity)
    {
        int healthBeforeDamage = CurrentHealth;
        float damageMultiplier = CritterHelpers.GetDamageMultiplier(Affinities, damagingAffinity);
        int multipliedDamageAmount = Mathf.CeilToInt(damageAmount * damageMultiplier);

        CurrentHealth = Mathf.Max(CurrentHealth - multipliedDamageAmount, 0);

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