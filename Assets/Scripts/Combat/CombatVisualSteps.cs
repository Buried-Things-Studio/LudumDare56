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


public class CritterSquishedStep : CombatVisualStep
{
    public string Name;


    public CritterSquishedStep(string name)
    {
        Name = name;
    }


    public override string GetPopulatedMessage()
    {
        List<string> messages = new List<string>(){
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was squished",
            "was beaten to a pulp",
            "is now just a splat on the ground",
            "won't be coming back! ever",
            "is unequivalocally dead",
        };
        
        return $"{Name} {messages[UnityEngine.Random.Range(0, messages.Count)]}!";
    }
}


public class ExpGainStep : CombatVisualStep
{
    public string Name;
    public int ExpGained;


    public ExpGainStep(string name, int expGained)
    {
        Name = name;
        ExpGained = expGained;
    }


    public override string GetPopulatedMessage()
    {
        return $"{Name} gained {ExpGained} EXP!";
    }
}


public class LevelGainStep : CombatVisualStep
{
    public string Name;
    public int LevelReached;


    public LevelGainStep(string name, int levelReached)
    {
        Name = name;
        LevelReached = levelReached;
    }


    public override string GetPopulatedMessage()
    {
        return $"{Name} reached level {LevelReached}!";
    }
}


public class ChangeActiveStep : CombatVisualStep
{
    public string Name;
    
    
    public ChangeActiveStep(string name)
    {
        Name = name;
    }


    public override string GetPopulatedMessage()
    {
        return $"Swapping to {Name}!";
    }
}


public class HealMessageStep : CombatVisualStep
{
    public string Name;
    public int HealAmount;
    
    
    public HealMessageStep(string name, int healAmount)
    {
        Name = name;
        HealAmount = healAmount;
    }


    public override string GetPopulatedMessage()
    {
        return $"{Name} healed for {HealAmount}!";
    }
}


public class HealthChangeStep : CombatVisualStep
{
    public bool IsPlayerCritter;
    public int StartingHealth;
    public int TargetHealth;
    public int MaxHealth;
    public int DamageMultiplier;


    public HealthChangeStep(bool isPlayerCritter, int startingHealth, int targetHealth, int maxHealth, int damageMultiplier = 4)
    {
        IsPlayerCritter = isPlayerCritter;
        StartingHealth = startingHealth;
        TargetHealth = targetHealth;
        MaxHealth = maxHealth;
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


public class TryCatchStep : CombatVisualStep
{
    public string Name;
    public bool IsSuccess;


    public TryCatchStep(string name, bool isSuccess)
    {
        Name = name;
        IsSuccess = isSuccess;
    }


    public override string GetPopulatedMessage()
    {
        return IsSuccess ? $"You caught the {Name}!" : $"You didn't catch the {Name}... It fled in panic!";
    }
}


public class TryCatchCollectorCritterStep : CombatVisualStep
{
    public override string GetPopulatedMessage()
    {
        List<string> messages = new List<string>(){
            "The opponent is angry you tried. That was uncool!",
            "The opponent swipes the jar out of the air!",
            "The opponent smashes your jar!",
            "You successfully catch the bug... But the opponent just opens the jar!",
            "That's not yours!",
        };
        
        return $"{messages[UnityEngine.Random.Range(0, messages.Count)]}";
    }
}


public class TryCatchTooFullCritterStep : CombatVisualStep
{
    public override string GetPopulatedMessage()
    {
        List<string> messages = new List<string>(){
            "You try to catch it... But your team is full!",
            "You throw a jar... But you have no space to bring this with you!",
            "Don't you think you have enough bugs?"
        };
        
        return $"{messages[UnityEngine.Random.Range(0, messages.Count)]}";
    }
}