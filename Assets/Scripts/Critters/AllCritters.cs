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
        List<CritterAffinity> Affinity = new List<CritterAffinity>(){CritterAffinity.None};
    }
}