using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class MasterCollection
{
    private static List<Type> _allCritterTypes;
    private static List<Type> _allMoveTypes;
    private static List<Type> _allAbilityTypes;


    public static List<Type> GetAllCritterTypes()
    {
        if (_allCritterTypes == null)
        {
            _allCritterTypes = new List<Type>(
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(Critter)))
            );
        }

        return _allCritterTypes;
    }

    public static List<Type> GetAllMoveTypes()
    {
        if(_allMoveTypes == null)
        {
            _allMoveTypes = new List<Type>(
                AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(Move)))
            );
        }
        return _allMoveTypes;
    }

    public static List<Move> GetAllMoves()
    {
        List<Move> allMoves = GetAllMoveTypes().Select(moveType => (Move)Activator.CreateInstance(moveType)).ToList();
        return allMoves;
    }


    public static List<Critter> GetAllCritters(List<CritterAffinity> availableAffinities)
    {
        List<Critter> allCritters = GetAllCritterTypes().Select(critterType => (Critter)Activator.CreateInstance(critterType)).ToList();
        Debug.Log("MasterCollection GetAllCritters()");
        Debug.Log(GetAllCritterTypes().Count);
        Debug.Log(allCritters.Count);

        if (availableAffinities == null || availableAffinities.Count == 0)
        {
            return allCritters;
        }

        List<Critter> availableCritters = new List<Critter>();

        foreach (Critter critter in allCritters)
        {
            Debug.Log($"Checking if {critter.Name} is available...");
            
            foreach (CritterAffinity affinity in critter.Affinities)
            {
                Debug.Log($"Checking if {critter.Name} is of affinity {affinity}...");

                if (availableAffinities.Contains(affinity))
                {
                    Debug.Log($"Critter is available!");
                    
                    availableCritters.Add(critter);

                    break;
                }
            }
        }

        Debug.Log($"returning {availableCritters.Count} critters");

        return availableCritters;
    }

    public static List<Type> GetAllAbilityTypes()
    {
        if(_allAbilityTypes == null)
        {
            _allAbilityTypes = new List<Type>(
                AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(Ability)))
            );
        }
        return _allAbilityTypes;
    }

    public static List<Ability> GetAllAbilities()
    {
        List<Ability> allAbilities = GetAllAbilityTypes().Select(abilityType => (Ability)Activator.CreateInstance(abilityType)).ToList();
        return allAbilities;
    }

    public static List<Ability> GetAllNpcUsableAbilities()
    {
        List<Ability> allAbilities = GetAllAbilities();
        List<Ability> npcUsableAbilities = allAbilities.Where(ability => ability.IsNpcUsable && ability.ID != AbilityID.None).ToList(); 
        return npcUsableAbilities;
    }
}
