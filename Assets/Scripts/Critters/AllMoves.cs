using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;


public enum MoveID
{
    None,
    Bonk,
    HoneyDrink,
    Stinger
}


public class HoneyDrink : Move
{
    public HoneyDrink()
    {
        Name = "Honey Drink";
        Description = "The user drinks reinvigorating honey to heal 20hp.";
        ID = MoveID.HoneyDrink;
        Affinity = CritterAffinity.Bee;
        MaxUses = 5;
        CurrentUses = 5;
    }


    public override void ExecuteMove(CombatState state)
    {
        Critter user = state.GetUserFromGUID(UserGUID);
        user.IncreaseHealth(20);
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
        BasePower = 20;
        IsSharp = false;
        MaxUses = 20;
        CurrentUses = 20;
    }


    public override void ExecuteMove(CombatState state)
    {
        Critter opponent = state.GetOpponentFromGUID(UserGUID);
        opponent.DealDamage(CritterHelpers.GetDamage(state, this));
    }
}