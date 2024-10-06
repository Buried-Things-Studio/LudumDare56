using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;


public enum MoveID
{
    None,
    SwitchActive,
    UseItem,
    
    Astonish,
    Bonk,
    HoneyDrink,
    MenacingGrin,
    RollDung,
    ShellBump,
    Snip,
    Stinger, 
    WebTrap,
    WingStrike,
}


public class Astonish : Move
{
    public Astonish()
    {
        Name = "Astonish";
        Description = "The user's beauty astonishes and confuses their opponent.";
        ID = MoveID.Astonish;
        Affinity = CritterAffinity.Caterpillar;
        Accuracy = 50;
        MaxUses = 10;
        CurrentUses = 10;
    }


    public override void ExecuteMove(CombatState state)
    {
        Critter opponent = state.GetOpponentFromGUID(UserGUID);
        // Confuse opponent
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
        BasePower = 20;
        Accuracy = 95;
        MaxUses = 20;
        CurrentUses = 20;
    }


    public override void ExecuteMove(CombatState state)
    {
        Critter opponent = state.GetOpponentFromGUID(UserGUID);
        opponent.DealDamage(CritterHelpers.GetDamage(state, this));
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
        Accuracy = 100;
        MaxUses = 5;
        CurrentUses = 5;
    }


    public override void ExecuteMove(CombatState state)
    {
        Critter user = state.GetUserFromGUID(UserGUID);
        user.IncreaseHealth(20);
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
        Accuracy = 100;
        MaxUses = 15;
        CurrentUses = 15;
    }


    public override void ExecuteMove(CombatState state)
    {
        Critter opponent = state.GetOpponentFromGUID(UserGUID);
        //TODO: Lower opponent attack here 
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
        Accuracy = 100;
        MaxUses = 10;
        CurrentUses = 10;
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
        BasePower = 30;
        Accuracy = 95;
        MaxUses = 20;
        CurrentUses = 20;
    }


    public override void ExecuteMove(CombatState state)
    {
        Critter opponent = state.GetOpponentFromGUID(UserGUID);
        opponent.DealDamage(CritterHelpers.GetDamage(state, this));
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
        BasePower = 35;
        Accuracy = 100;
        MaxUses = 20;
        CurrentUses = 20;
    }


    public override void ExecuteMove(CombatState state)
    {
        Critter opponent = state.GetOpponentFromGUID(UserGUID);
        opponent.DealDamage(CritterHelpers.GetDamage(state, this));
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
        Accuracy = 100;
        MaxUses = 20;
        CurrentUses = 20;
    }


    public override void ExecuteMove(CombatState state)
    {
        Critter opponent = state.GetOpponentFromGUID(UserGUID);
        //TODO: reduce speed of opponent here 
    }
}


public class WingStrike : Move
{
    public WingStrike()
    {
        Name = "Wing Strike";
        Description = "The user strikes the opponent with their wings.";
        ID = MoveID.WingStrike;
        Affinity = CritterAffinity.Caterpillar;
        IsSharp = false;
        BasePower = 35;
        Accuracy = 80;
        MaxUses = 10;
        CurrentUses = 10;
    }


    public override void ExecuteMove(CombatState state)
    {
        Critter opponent = state.GetOpponentFromGUID(UserGUID);
        opponent.DealDamage(CritterHelpers.GetDamage(state, this));
    }
}