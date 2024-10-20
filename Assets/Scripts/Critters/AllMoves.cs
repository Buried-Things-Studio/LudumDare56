using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MoveID
{
    None,

    SwitchActive,
    ThrowMasonJar,
    UseHealItem,
    TriedItsBest,
    
    Bonk,
    Carapace,
    Careen,
    Dazzle,
    EnvenomedBite,
    HoneyDrink,
    MenacingGrin,
    MysteriousSlime,
    RollDung,
    ShellBump,
    Smother,
    Snip,
    SpringTrap,
    Stinger,
    ToxicTouch,
    WebTrap,
    WingStrike,
}


public class TriedItsBest : Move
{
    public TriedItsBest()
    {
        Name = "tried its best";
        ID = MoveID.TriedItsBest;
        Affinity = CritterAffinity.None;
        IsSharp = false;
        IsTargeted = true;
        BasePower = 20;
        Accuracy = 100;
        MaxUses = 9999;
        CurrentUses = 9999;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter user = isPlayerUser ? state.PlayerCritter : state.NpcCritter;
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;

        List<CombatVisualStep> steps = new List<CombatVisualStep>();

        int damageMultiplier;
        int damage = CritterHelpers.GetDamage(state, this, isPlayerUser, out damageMultiplier);

        steps.AddRange(TryDealDamage(state, damage, opponent, user));

        if (opponent.CurrentHealth > 0)
        {
            steps.AddRange(TryDealDamage(state, damage, user, user));
        }

        return steps;
    }
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
        BasePower = 35;
        Accuracy = 95;
        MaxUses = 20;
        CurrentUses = 20;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter user = isPlayerUser ? state.PlayerCritter : state.NpcCritter;
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;
        List<CombatVisualStep> steps = new List<CombatVisualStep>();

        int damageMultiplier;
        int damage = CritterHelpers.GetDamage(state, this, isPlayerUser, out damageMultiplier);
        steps.AddRange(TryDealDamage(state, damage, opponent, user));

        return steps;
    }
}


public class Carapace : Move
{
    public Carapace()
    {
        Name = "Carapace";
        Description = "The user hardens their exoskeleton for a boost in sharp or blunt defense.";
        ID = MoveID.Carapace;
        Affinity = CritterAffinity.Beetle;
        IsTargeted = false;
        Accuracy = 100;
        MaxUses = 10;
        CurrentUses = 10;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter user = isPlayerUser ? state.PlayerCritter : state.NpcCritter;
        bool isSharp = UnityEngine.Random.Range(0, 2) == 0;

        if (isSharp)
        {
            user.ChangeSharpDefenseStage(1);
        }
        else
        {
            user.ChangeBluntDefenseStage(1);
        }

        List<CombatVisualStep> steps = new List<CombatVisualStep>();

        if (isSharp)
        {
            steps.Add(new ChangeStatStageStep(user.Name, "sharp def", 1));
        }
        else
        {
            steps.Add(new ChangeStatStageStep(user.Name, "blunt def", 1));
        }
        
        return steps;
    }
}


public class Careen : Move
{
    public Careen()
    {
        Name = "Careen";
        Description = "The user charges headfirst into the opponent at top speed.";
        ID = MoveID.Careen;
        Affinity = CritterAffinity.Beetle;
        IsSharp = false;
        IsTargeted = true;
        BasePower = 95;
        Accuracy = 70;
        MaxUses = 5;
        CurrentUses = 5;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;
        Critter user = isPlayerUser ? state.PlayerCritter : state.NpcCritter;

        int damageMultiplier;
        int damage = CritterHelpers.GetDamage(state, this, isPlayerUser, out damageMultiplier);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.AddRange(TryDealDamage(state, damage, opponent, user));

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


public class EnvenomedBite : Move
{
    public EnvenomedBite()
    {
        Name = "Envenomed Bite";
        Description = "A vicious bite with a venomous sting.";
        ID = MoveID.EnvenomedBite;
        Affinity = CritterAffinity.Ant;
        IsSharp = true;
        IsTargeted = true;
        BasePower = 80;
        Accuracy = 90;
        MaxUses = 5;
        CurrentUses = 5;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;
        Critter user = isPlayerUser ? state.PlayerCritter : state.NpcCritter;

        int damageMultiplier;
        int damage = CritterHelpers.GetDamage(state, this, isPlayerUser, out damageMultiplier);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.AddRange(TryDealDamage(state, damage, opponent, user));

        return steps;
    }
}


public class HoneyDrink : Move
{
    public HoneyDrink()
    {
        Name = "Honey Drink";
        Description = "The user drinks reinvigorating honey to heal 25% hp.";
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
        user.IncreaseHealth(Mathf.CeilToInt(user.MaxHealth * 0.25f));

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


public class MysteriousSlime : Move
{
    public MysteriousSlime()
    {
        Name = "Mysterious Slime";
        Description = "The user covers their opponent in an unknown compound, lowering speed and sharp attack.";
        ID = MoveID.MysteriousSlime;
        Affinity = CritterAffinity.Mollusc;
        IsSharp = false;
        IsTargeted = true;
        Accuracy = 90;
        MaxUses = 15;
        CurrentUses = 15;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;
        opponent.ChangeBluntAttackStage(-1);
        opponent.ChangeSpeedStage(-1);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.Add(new ChangeStatStageStep(opponent.Name, "blunt att", -1));
        steps.Add(new ChangeStatStageStep(opponent.Name, "speed", -1));

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
        Critter user = isPlayerUser ? state.PlayerCritter : state.NpcCritter;

        int damageMultiplier;
        int damage = CritterHelpers.GetDamage(state, this, isPlayerUser, out damageMultiplier);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.AddRange(TryDealDamage(state, damage, opponent, user));

        return steps;
    }
}


public class Smother : Move
{
    public Smother()
    {
        Name = "Smother";
        Description = "The user smothers with a full body embrace.";
        ID = MoveID.Smother;
        Affinity = CritterAffinity.Mollusc;
        IsSharp = false;
        IsTargeted = true;
        BasePower = 40;
        Accuracy = 100;
        MaxUses = 15;
        CurrentUses = 15;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;
        Critter user = isPlayerUser ? state.PlayerCritter : state.NpcCritter;

        int damageMultiplier;
        int damage = CritterHelpers.GetDamage(state, this, isPlayerUser, out damageMultiplier);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.AddRange(TryDealDamage(state, damage, opponent, user));

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
        Critter user = isPlayerUser ? state.PlayerCritter : state.NpcCritter;

        int damageMultiplier;
        int damage = CritterHelpers.GetDamage(state, this, isPlayerUser, out damageMultiplier);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.AddRange(TryDealDamage(state, damage, opponent, user));

        return steps;
    }
}


public class SpringTrap : Move
{
    public SpringTrap()
    {
        Name = "Spring Trap";
        Description = "The user pounces at its unsuspecting prey.";
        ID = MoveID.SpringTrap;
        Affinity = CritterAffinity.Spider;
        IsSharp = true;
        IsTargeted = true;
        BasePower = 85;
        Accuracy = 90;
        MaxUses = 5;
        CurrentUses = 5;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;
        Critter user = isPlayerUser ? state.PlayerCritter : state.NpcCritter;

        int damageMultiplier;
        int damage = CritterHelpers.GetDamage(state, this, isPlayerUser, out damageMultiplier);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.AddRange(TryDealDamage(state, damage, opponent, user));

        return steps;
    }
}


public class Stinger : Move
{
    public Stinger()
    {
        Name = "Stinger";
        Description = "The user stabs with a painful stinger.";
        ID = MoveID.Stinger;
        Affinity = CritterAffinity.Bee;
        IsSharp = true;
        IsTargeted = true;
        BasePower = 70;
        Accuracy = 90;
        MaxUses = 5;
        CurrentUses = 5;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;
        Critter user = isPlayerUser ? state.PlayerCritter : state.NpcCritter;

        int damageMultiplier;
        int damage = CritterHelpers.GetDamage(state, this, isPlayerUser, out damageMultiplier);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.AddRange(TryDealDamage(state, damage, opponent, user));

        return steps;
    }
}


public class ToxicTouch : Move
{
    public ToxicTouch()
    {
        Name = "Toxic Touch";
        Description = "The user secretes highly toxic compounds.";
        ID = MoveID.ToxicTouch;
        Affinity = CritterAffinity.Mollusc;
        IsSharp = false;
        IsTargeted = true;
        BasePower = 85;
        Accuracy = 95;
        MaxUses = 5;
        CurrentUses = 5;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;
        Critter user = isPlayerUser ? state.PlayerCritter : state.NpcCritter;

        int damageMultiplier;
        int damage = CritterHelpers.GetDamage(state, this, isPlayerUser, out damageMultiplier);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.AddRange(TryDealDamage(state, damage, opponent, user));

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
        MaxUses = 15;
        CurrentUses = 15;
    }


    public override List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser)
    {
        Critter opponent = isPlayerUser ? state.NpcCritter : state.PlayerCritter;
        Critter user = isPlayerUser ? state.PlayerCritter : state.NpcCritter;

        int damageMultiplier;
        int damage = CritterHelpers.GetDamage(state, this, isPlayerUser, out damageMultiplier);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();
        steps.AddRange(TryDealDamage(state, damage, opponent, user));

        return steps;
    }
}