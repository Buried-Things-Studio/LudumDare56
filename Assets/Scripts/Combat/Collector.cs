using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Collector
{
    private List<Critter> _critters = new List<Critter>();
    private bool _isBoss;
    public bool HasBeenDefeated;
    public string position = "-1";


    public Collector(bool isBoss, int teamSize, Vector2Int levelRange, List<CritterAffinity> availableAffinities)
    {
        _isBoss = isBoss;
        List<Critter> availableCritters = MasterCollection.GetAllCritters(availableAffinities);
        Debug.Log($"available critters count = {availableCritters.Count.ToString()}");
        Debug.Log($"teamSize = " + teamSize.ToString());

        for (int i = 0; i < teamSize; i++)
        {
            int level = UnityEngine.Random.Range(levelRange.x, levelRange.y + 1);
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
