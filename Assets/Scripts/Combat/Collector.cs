using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Collector
{
    private List<Critter> _critters;
    private bool _isBoss;


    public Collector(bool isBoss, int teamSize, List<CritterAffinity> availableAffinities)
    {
        _isBoss = isBoss;

        List<Critter> availableCritters = MasterCollection.GetAllCritters(availableAffinities);

        for (int i = 0; i < teamSize; i++)
        {
            Critter randomCritterToClone = availableCritters[UnityEngine.Random.Range(0, availableCritters.Count)];
            Critter newCritter = Activator.CreateInstance(randomCritterToClone.GetType()) as Critter;
            _critters.Add(newCritter);
        }
    }
}
