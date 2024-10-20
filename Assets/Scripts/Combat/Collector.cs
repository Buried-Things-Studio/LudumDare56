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
    public Vector2Int Coords = new Vector2Int(-100, -100); 

    // Viz values
    public int NoseValue; 
    public int EyeValue;
    public int SkinColour; 
    public int NoseColour; 
    public int TeeColour;



    public Collector(bool isBoss, int teamSize, Vector2Int levelRange, List<CritterAffinity> availableAffinities, int abilityChance)
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
            int randomAbilityChance = UnityEngine.Random.Range(0, 10);
            if(randomAbilityChance < abilityChance)
            {
                List<Ability> availableAbilities = MasterCollection.GetAllNpcUsableAbilities();
                newCritter.Ability = availableAbilities[UnityEngine.Random.Range(0, availableAbilities.Count)];
            }
            _critters.Add(newCritter);
        }
        NoseValue = UnityEngine.Random.Range(0, 4);
        EyeValue = UnityEngine.Random.Range(0, 5);
        SkinColour = UnityEngine.Random.Range(0,4);
        NoseColour = UnityEngine.Random.Range(0, 3);
        TeeColour = UnityEngine.Random.Range(0, 5);
    }


    public bool IsBoss()
    {
        return _isBoss;
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
