using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bee : Critter
{
    public Bee()
    {
        Name = "Bee";
        MaxHealth = 50;
        CurrentHealth = 50;
        MaxSpeed = 5;
        CurrentSpeed = 5;
        Affinities.Add(CritterAffinity.Sting);
        Moves.Add(new HoneyDrink(){UserGUID = GUID});
    }
}