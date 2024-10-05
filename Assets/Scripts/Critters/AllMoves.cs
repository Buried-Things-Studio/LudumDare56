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
        ID = MoveID.HoneyDrink;
        Affinity = CritterAffinity.Bee;
        MaxUses = 5;
        CurrentUses = 5;
    }


    public override void ExecuteMove(CombatState state)
    {
        Critter user = state.GetUserFromGUID(UserGUID);
        user.IncreaseHealth(10);
    }
}


public class Bonk : Move
{
    public Bonk()
    {
        Name = "Bonk";
        ID = MoveID.Bonk;
        Affinity = CritterAffinity.Bee;
        MaxUses = 20;
        CurrentUses = 20;
    }


    public override void ExecuteMove(CombatState state)
    {
        
    }
}