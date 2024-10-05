using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bee : Critter
{
    public Bee()
    {
        Name = "Bee";
        Affinities.Add(CritterAffinity.Bee);
        Moves.Add(new HoneyDrink(){UserGUID = GUID});

        MaxHealth = 35;
        CurrentHealth = 35;
        HealthLevelIncrease = new Vector2Int(5, 8);

        MaxSpeed = 2;
        CurrentSpeed = 2;
        SpeedLevelIncrease = new Vector2Int(1, 2);

        MaxSharpAttack = 10;
        CurrentSharpAttack = 10;
        SharpAttackLevelIncrease = new Vector2Int(3, 5);

        MaxSharpDefense = 3;
        CurrentSharpDefense = 3;
        SharpDefenseLevelIncrease = new Vector2Int(2, 3);

        MaxBluntAttack = 8;
        CurrentBluntAttack = 8;
        BluntAttackLevelIncrease = new Vector2Int(2, 3);

        MaxBluntDefense = 8;
        CurrentBluntDefense = 8;
        BluntDefenseLevelIncrease = new Vector2Int(2, 4);
    }
}