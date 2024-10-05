using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Collector
{
    private List<Critter> _critters;
    private bool _isBoss;
    public bool HasBeenDefeated;


    public Collector(bool isBoss, int teamSize, int floorNumber, List<CritterAffinity> availableAffinities)
    {
        _isBoss = isBoss;
        List<Critter> availableCritters = MasterCollection.GetAllCritters(availableAffinities);

        for (int i = 0; i < teamSize; i++)
        {
            int level = floorNumber + UnityEngine.Random.Range(0, 2);
            level += isBoss ? 1 : 0;
            
            Critter randomCritterToClone = availableCritters[UnityEngine.Random.Range(0, availableCritters.Count)];
            Critter newCritter = Activator.CreateInstance(randomCritterToClone.GetType()) as Critter;
            newCritter.SetStartingLevel(level);
            _critters.Add(newCritter);
        }
    }


    public List<Critter> GetCritters()
    {
        return _critters;
    }


    public Critter GetActiveCritter()
    {
        foreach (Critter critter in _critters)
        {
            if (critter.CurrentHealth > 0)
            {
                return critter;
            }
        }

        return null; //should never be hit
    }
}
