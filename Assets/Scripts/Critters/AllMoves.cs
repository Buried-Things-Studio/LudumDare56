using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;


public enum MoveID
{
    None,

    SwitchActive,
    ThrowMasonJar,
    UseHealItem,
    
    Bonk,
    Dazzle,
    HoneyDrink,
    MenacingGrin,
    RollDung,
    ShellBump,
    Snip,
    Stinger, 
    WebTrap,
    WingStrike,
}


public class Bonk : Move
{
    public Bonk()
    {
        Name = "Bonk";
        Description = "A clumsy bonk.";
        ID = MoveID.Bonk;
        Affinity = CritterAffinity.Bee;
        IsSharp = false;
        IsTargeted = true;
        BasePower = 20;
        Accuracy = 95;
        MaxUses = 20;
        CurrentUses = 20;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;

        int startingHealth = opponent.CurrentHealth;
        int damageMultiplier;
        int damage = CritterHelpers.GetDamage(state, this, isPlayerUser, out damageMultiplier);
        opponent.DealDamage(damage);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.Add(new HealthChangeStep(!isPlayerUser, opponent.Level, startingHealth, opponent.CurrentHealth, opponent.MaxHealth, damageMultiplier));

        return steps;
    }
}


public class Dazzle : Move
{
    public Dazzle()
    {
        Name = "Dazzle";
        Description = "The user's beauty dazzles and confuses their opponent.";
        ID = MoveID.Dazzle;
        Affinity = CritterAffinity.Butterfly;
        IsTargeted = true;
        Accuracy = 50;
        MaxUses = 10;
        CurrentUses = 10;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;
        bool isSuccess = opponent.SetStatusEffect(StatusEffectType.Confuse);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.Add(new ApplyStatusEffectStep(opponent.Name, StatusEffectType.Confuse, isSuccess));

        return steps;
    }
}


public class HoneyDrink : Move
{
    public HoneyDrink()
    {
        Name = "Honey Drink";
        Description = "The user drinks reinvigorating honey to heal 20hp.";
        ID = MoveID.HoneyDrink;
        Affinity = CritterAffinity.Bee;
        IsTargeted = false;
        Accuracy = 100;
        MaxUses = 5;
        CurrentUses = 5;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter user = isPlayerUser ? state.PlayerCritter : state.NpcCritter;
        int startingHealth = user.CurrentHealth;
        user.IncreaseHealth(20);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.Add(new HealthChangeStep(isPlayerUser, user.Level, startingHealth, user.CurrentHealth, user.MaxHealth));

        return steps;
    }
}


public class MenacingGrin : Move
{
    public MenacingGrin()
    {
        Name = "Menacing Grin";
        Description = "The user intimidates their opponent with and lowers their attack.";
        ID = MoveID.MenacingGrin;
        Affinity = CritterAffinity.Spider;
        IsTargeted = true;
        Accuracy = 100;
        MaxUses = 15;
        CurrentUses = 15;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;
        opponent.ChangeBluntAttackStage(-1);
        opponent.ChangeSharpAttackStage(-1);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.Add(new ChangeStatStageStep(opponent.Name, "blunt att", -1));
        steps.Add(new ChangeStatStageStep(opponent.Name, "sharp att", -1));

        return steps;
    }
}


public class RollDung : Move
{
    public RollDung()
    {
        Name = "Roll Dung";
        Description = "The user builds up speed as they roll a dungball.";
        ID = MoveID.RollDung;
        Affinity = CritterAffinity.Beetle;
        IsTargeted = false;
        Accuracy = 100;
        MaxUses = 10;
        CurrentUses = 10;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter user = isPlayerUser ? state.PlayerCritter : state.NpcCritter;
        user.ChangeSpeedStage(1);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.Add(new ChangeStatStageStep(user.Name, "speed", 1));

        return steps;
    }
}


public class ShellBump : Move
{
    public ShellBump()
    {
        Name = "Shell Bump";
        Description = "The user thumps the opponent with their hard shell.";
        ID = MoveID.ShellBump;
        Affinity = CritterAffinity.Beetle;
        IsSharp = false;
        IsTargeted = true;
        BasePower = 30;
        Accuracy = 95;
        MaxUses = 20;
        CurrentUses = 20;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;

        int startingHealth = opponent.CurrentHealth;
        int damageMultiplier;
        int damage = CritterHelpers.GetDamage(state, this, isPlayerUser, out damageMultiplier);
        opponent.DealDamage(damage);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.Add(new HealthChangeStep(!isPlayerUser, opponent.Level, startingHealth, opponent.CurrentHealth, opponent.MaxHealth, damageMultiplier));

        return steps;
    }
}


public class Snip : Move
{
    public Snip()
    {
        Name = "Snip";
        Description = "The user clamps down with pincers or mandibles.";
        ID = MoveID.Snip;
        Affinity = CritterAffinity.Ant;
        IsSharp = true;
        IsTargeted = true;
        BasePower = 35;
        Accuracy = 100;
        MaxUses = 20;
        CurrentUses = 20;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;

        int startingHealth = opponent.CurrentHealth;
        int damageMultiplier;
        int damage = CritterHelpers.GetDamage(state, this, isPlayerUser, out damageMultiplier);
        opponent.DealDamage(damage);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.Add(new HealthChangeStep(!isPlayerUser, opponent.Level, startingHealth, opponent.CurrentHealth, opponent.MaxHealth, damageMultiplier));

        return steps;
    }
}


public class WebTrap : Move
{
    public WebTrap()
    {
        Name = "Web Trap";
        Description = "The user slows their opponent by trapping them in a web.";
        ID = MoveID.WebTrap;
        Affinity = CritterAffinity.Spider;
        IsTargeted = true;
        Accuracy = 100;
        MaxUses = 20;
        CurrentUses = 20;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;
        opponent.ChangeSpeedStage(-1);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.Add(new ChangeStatStageStep(opponent.Name, "speed", -1));

        return steps;
    }
}


public class WingStrike : Move
{
    public WingStrike()
    {
        Name = "Wing Strike";
        Description = "The user strikes the opponent with their wings.";
        ID = MoveID.WingStrike;
        Affinity = CritterAffinity.Butterfly;
        IsSharp = false;
        IsTargeted = true;
        BasePower = 35;
        Accuracy = 80;
        MaxUses = 10;
        CurrentUses = 10;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;

        int startingHealth = opponent.CurrentHealth;
        int damageMultiplier;
        int damage = CritterHelpers.GetDamage(state, this, isPlayerUser, out damageMultiplier);
        opponent.DealDamage(damage);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.Add(new HealthChangeStep(!isPlayerUser, opponent.Level, startingHealth, opponent.CurrentHealth, opponent.MaxHealth, damageMultiplier));

        return steps;
    }
}