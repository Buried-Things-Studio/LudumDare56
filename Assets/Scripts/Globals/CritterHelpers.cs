using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class CritterHelpers
{
    public static Dictionary<CritterAffinity, List<CritterAffinity>> GoodDefences = new Dictionary<CritterAffinity, List<CritterAffinity>>(); //search by defending affinity in key field
    public static Dictionary<CritterAffinity, List<CritterAffinity>> BadDefences = new Dictionary<CritterAffinity, List<CritterAffinity>>(); //search by defending affinity in key field
    public static List<int> ExpToNextLevel = new List<int>(){0, 100, 200, 350, 575, 950, 1575, 2200, 2950, 4000};
    public static int MaxTeamSize = 5;

    
    public static int GetDamageMultiplier(List<CritterAffinity> defendingAffinities, CritterAffinity attackingAffinity)
    {
        if (GoodDefences.Count == 0)
        {
            PopulateAffinityTable();
        }
        
        int multiplier = 4;

        foreach (CritterAffinity defendingAffinity in defendingAffinities)
        {
            if (GoodDefences[defendingAffinity].Contains(attackingAffinity))
            {
                multiplier /= 2;
            }
            else if (BadDefences[defendingAffinity].Contains(attackingAffinity))
            {
                multiplier *= 2;
            }
        }

        return multiplier;
    }


    public static int GetDamage(CombatState state, Move move, bool isPlayerUser, out int damageMultiplier)
    {
        Critter user = isPlayerUser ? state.PlayerCritter : state.NpcCritter;
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;
        int baseDamage = move.BasePower / 5;
        int practiceBaseDamage = 0;
        if(user.Ability.ID == AbilityID.PracticeMakesPerfect)
        {
            PracticeMakesPerfect practice = (PracticeMakesPerfect)user.Ability;
            if(practice.Move == move)
            {
                practice.Count ++; 
                extraBaseDamage = 2 * practice.Count;
            }
            else 
            {
                practice.Move = move; 
                practice.Count = 0;
            }
        }
        float sameAffinityBonus = user.Affinities.Contains(move.Affinity) && opponent.Ability.ID != AbilityID.StabProofVest ? 1.5f : 1f;
        float differentAffinityBonus = user.Ability.ID == AbilityID.Versatile && !user.Affinities.Contains(move.Affinity) ? 1.2f : 1f;
        float statRatio = 0f;
        int logAttackValue = 0;
        int logDefenseValue = 0;

        if (move.IsSharp)
        {
            statRatio = (float)GetEffectiveSharpAttack(user) / (float)Mathf.Max(1, GetEffectiveSharpDefense(opponent));
            logAttackValue = GetEffectiveSharpAttack(user);
            logDefenseValue = GetEffectiveSharpDefense(opponent);
        }
        else
        {
            statRatio = (float)GetEffectiveBluntAttack(user) / (float)Mathf.Max(1, GetEffectiveBluntDefense(opponent));
            logAttackValue = GetEffectiveBluntAttack(user);
            logDefenseValue = GetEffectiveBluntDefense(opponent);
        }

        damageMultiplier = GetDamageMultiplier(opponent.Affinities, move.Affinity);
        int totalDamage = Mathf.CeilToInt((baseDamage + practiceBaseDamage) * sameAffinityBonus * differentAffinityBonus * statRatio * (damageMultiplier / 4f));
        
        Debug.Log($"DAMAGE CALCULATION FROM {user.Name} to {opponent.Name}. DAMAGE = {totalDamage} --> ( Base damage {baseDamage} + Practice Base Damage {practiceBaseDamage} ) * STAB {sameAffinityBonus} * DAB {differentAffinityBonus} * att:def {logAttackValue}:{logDefenseValue} = {statRatio} * effectiveness {damageMultiplier / 4f}");

        return totalDamage;
    }


    public static int GetConfusionDamage(Critter user)
    {
        int baseDamage = 8; //40 base power
        float statRatio = 0f;
        bool isSharp = user.MaxSharpAttack > user.MaxBluntAttack;

        if (isSharp)
        {
            statRatio = (float)GetEffectiveSharpAttack(user) / (float)Mathf.Max(1, GetEffectiveSharpDefense(user));
        }
        else
        {
            statRatio = (float)GetEffectiveBluntAttack(user) / (float)Mathf.Max(1, GetEffectiveBluntDefense(user));
        }

        return Mathf.CeilToInt(baseDamage * statRatio);
    }


    public static int GetEffectiveSpeed(Critter critter)
    {
        if (critter.SpeedStage == 0)
        {
            return critter.MaxSpeed;
        }
        else if (critter.SpeedStage > 0)
        {
            return Mathf.FloorToInt(critter.MaxSpeed + (critter.SpeedStage * 0.5f));
        }
        else
        {
            return Mathf.Max(1, Mathf.FloorToInt((critter.MaxSpeed * 2) / (float)(2 + Mathf.Abs(critter.SpeedStage))));
        }
    }


    public static int GetEffectiveBluntAttack(Critter critter)
    {
        if (critter.BluntAttackStage == 0)
        {
            return critter.MaxBluntAttack;
        }
        else if (critter.BluntAttackStage > 0)
        {
            return Mathf.FloorToInt(critter.MaxBluntAttack + (critter.BluntAttackStage * 0.5f));
        }
        else
        {
            return Mathf.Max(1, Mathf.FloorToInt((critter.MaxBluntAttack * 2) / (float)(2 + Mathf.Abs(critter.BluntAttackStage))));
        }
    }


    public static int GetEffectiveBluntDefense(Critter critter)
    {
        if (critter.BluntDefenseStage == 0)
        {
            return critter.MaxBluntDefense;
        }
        else if (critter.BluntDefenseStage > 0)
        {
            return Mathf.FloorToInt(critter.MaxBluntDefense + (critter.BluntDefenseStage * 0.5f));
        }
        else
        {
            return Mathf.Max(1, Mathf.FloorToInt((critter.MaxBluntDefense * 2) / (float)(2 + Mathf.Abs(critter.BluntDefenseStage))));
        }
    }


    public static int GetEffectiveSharpAttack(Critter critter)
    {
        if (critter.SharpAttackStage == 0)
        {
            return critter.MaxSharpAttack;
        }
        else if (critter.SharpAttackStage > 0)
        {
            return Mathf.FloorToInt(critter.MaxSharpAttack + (critter.SharpAttackStage * 0.5f));
        }
        else
        {
            return Mathf.Max(1, Mathf.FloorToInt((critter.MaxSharpAttack * 2) / (float)(2 + Mathf.Abs(critter.SharpAttackStage))));
        }
    }


    public static int GetEffectiveSharpDefense(Critter critter)
    {
        if (critter.SharpDefenseStage == 0)
        {
            return critter.MaxSharpDefense;
        }
        else if (critter.SharpDefenseStage > 0)
        {
            return Mathf.FloorToInt(critter.MaxSharpDefense + (critter.SharpDefenseStage * 0.5f));
        }
        else
        {
            return Mathf.Max(1, Mathf.FloorToInt((critter.MaxSharpDefense * 2) / (float)(2 + Mathf.Abs(critter.SharpDefenseStage))));
        }
    }


    public static int GetCatchHealthThreshold(Critter critter)
    {
        return Mathf.Max(1, Mathf.FloorToInt(critter.MaxHealth * ((11 - critter.Level) * 0.05f)));
    }


    public static int GetCatchHealthThreshold(int maxHealth, int level)
    {
        return Mathf.Max(1, Mathf.FloorToInt(maxHealth * ((11 - level) * 0.05f)));
    }


    public static float GetCatchHealthThresholdFraction(Critter critter)
    {
        return (11 - critter.Level) * 0.05f;
    }


    private static void PopulateAffinityTable()
    {
        //ANT (0)
        BadDefences[CritterAffinity.Ant] = new List<CritterAffinity>(){
            CritterAffinity.Bee,
            CritterAffinity.Spider,
        };

        GoodDefences[CritterAffinity.Ant] = new List<CritterAffinity>(){
            CritterAffinity.Butterfly,
            CritterAffinity.Mollusc,
        };
        
        //BEE (-1)
        BadDefences[CritterAffinity.Bee] = new List<CritterAffinity>(){
            CritterAffinity.Bee,
            CritterAffinity.Spider,
        };

        GoodDefences[CritterAffinity.Bee] = new List<CritterAffinity>(){
            CritterAffinity.Ant,
            CritterAffinity.Beetle,
        };

        //BEETLE (+2)
        BadDefences[CritterAffinity.Beetle] = new List<CritterAffinity>(){
            CritterAffinity.Ant,
        };
        
        GoodDefences[CritterAffinity.Beetle] = new List<CritterAffinity>(){
            CritterAffinity.Bee,
            CritterAffinity.Beetle,
            CritterAffinity.Mollusc,
        };

        //Butterfly (-1)
        BadDefences[CritterAffinity.Butterfly] = new List<CritterAffinity>(){

        };
        
        GoodDefences[CritterAffinity.Butterfly] = new List<CritterAffinity>(){

        };

        //MOLLUSC (+1)
        BadDefences[CritterAffinity.Mollusc] = new List<CritterAffinity>(){
            CritterAffinity.Beetle,
        };
        
        GoodDefences[CritterAffinity.Mollusc] = new List<CritterAffinity>(){
            CritterAffinity.Butterfly, //remove for 0 on both Butterfly and mollusc?
            CritterAffinity.Mollusc,
            CritterAffinity.Spider,
        };

        //SPIDER (0)
        BadDefences[CritterAffinity.Spider] = new List<CritterAffinity>(){
            CritterAffinity.Butterfly,
            CritterAffinity.Mollusc,
        };
        
        GoodDefences[CritterAffinity.Spider] = new List<CritterAffinity>(){
            CritterAffinity.Bee,
            CritterAffinity.Spider,
        };
    }

    public static List<string> GetAllCritterStats()
    {
        return new List<string>(){
            "Sharp Attack", 
            "Sharp Defense", 
            "Blunt Attack", 
            "Blunt Defense", 
            "Speed", 
        };
    }
}
