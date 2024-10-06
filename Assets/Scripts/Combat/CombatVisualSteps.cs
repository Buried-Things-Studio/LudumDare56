using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatVisualSteps
{
    public List<CombatVisualStep> CurrentSteps = new List<CombatVisualStep>();
}


public abstract class CombatVisualStep
{
    public abstract string GetPopulatedMessage();
}

// STEPS --v

public class DoMoveStep : CombatVisualStep
{
    public string UserName;
    public string MoveName;


    public DoMoveStep(string userName, string moveName)
    {
        UserName = userName;
        MoveName = moveName;
    }


    public override string GetPopulatedMessage()
    {
        return $"{UserName} used {MoveName}!";
    }
}


public class HealthChangeStep : CombatVisualStep
{
    public bool IsPlayerCritter;
    public int StartingHealth;
    public int TargetHealth;
    public int DamageMultiplier;


    public HealthChangeStep(bool isPlayerCritter, int startingHealth, int targetHealth, int damageMultiplier = 4)
    {
        IsPlayerCritter = isPlayerCritter;
        StartingHealth = startingHealth;
        TargetHealth = targetHealth;
        DamageMultiplier = damageMultiplier;
    }


    public override string GetPopulatedMessage()
    {
        if (DamageMultiplier == 1)
        {
            return "It did almost nothing...";
        }
        else if (DamageMultiplier == 2)
        {
            return "It didn't seem to do much...";
        }
        else if (DamageMultiplier == 8)
        {
            return "It was really effective!";
        }
        else if (DamageMultiplier == 16)
        {
            return "It was extremely effective!";
        }

        return "";
    }
}


public class ChangeStatStageStep : CombatVisualStep
{
    public string CritterName;
    public string StatName;
    public string IsIncreaseString;


    public ChangeStatStageStep(string critterName, string statName, int change)
    {
        CritterName = critterName;
        StatName = statName;
        IsIncreaseString = change > 0 ? "increased" : "decreased";
    }


    public override string GetPopulatedMessage()
    {
        return $"{CritterName}'s {StatName} {IsIncreaseString}!";
    }
}


public class ApplyStatusEffectStep : CombatVisualStep
{
    public string Name;
    public StatusEffectType StatusType;
    public string IsSuccessString;


    public ApplyStatusEffectStep(string name, StatusEffectType statusType, bool isSuccess)
    {
        Name = name;
        StatusType = statusType;
        IsSuccessString = isSuccess ? "is now" : "is already";
    }


    public override string GetPopulatedMessage()
    {
        string statusName = "";

        if (StatusType == StatusEffectType.Confuse) //TODO: add future statuses
        {

        }
        else
        {
            throw new System.Exception("name this status effect!");
        }
        
        return $"{Name} {IsSuccessString} {statusName}!";
    }
}


public class ConfuseStatusUpdateStep : CombatVisualStep
{
    public string Name;
    public string IsStillConfusedString;


    public ConfuseStatusUpdateStep(string name, bool isStillConfused)
    {
        Name = name;
        IsStillConfusedString = isStillConfused ? "is still" : "is no longer";
    }


    public override string GetPopulatedMessage()
    {
        return $"{Name} {IsStillConfusedString} confused!";
    }
}


public class ConfuseCheckFailureStep : CombatVisualStep
{
    public string Name;


    public ConfuseCheckFailureStep(string name)
    {
        Name = name;
    }


    public override string GetPopulatedMessage()
    {
        return $"{Name} stumbled and fell!";
    }
}


public class MoveAccuracyCheckFailureStep : CombatVisualStep
{
    public string Name;


    public MoveAccuracyCheckFailureStep(string name)
    {
        Name = name;
    }


    public override string GetPopulatedMessage()
    {
        return $"{Name} missed!";
    }
}