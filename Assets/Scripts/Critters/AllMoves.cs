using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;


public enum MoveID
{
    None,
    HoneyDrink
}


public class HoneyDrink : Move
{
    public HoneyDrink()
    {
        Name = "Honey Drink";
        ID = MoveID.HoneyDrink;
        Affinity = CritterAffinity.None;
    }


    public override void ExecuteMove(CombatState state)
    {
        Critter user = state.GetUserFromGUID(UserGUID);
        user.IncreaseHealth(10);
    }
}