using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class MasterCollection
{
    private static List<Type> _allCritterTypes;


    public static List<Type> GetAllCritterTypes()
    {
        if (_allCritterTypes.Count == 0)
        {
            _allCritterTypes = new List<Type>(
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(Critter)))
            );
        }

        return _allCritterTypes;
    }


    public static List<Critter> GetAllCritters(List<CritterAffinity> availableAffinities)
    {
        List<Critter> allCritters = GetAllCritterTypes().Select(critterType => (Critter)Activator.CreateInstance(critterType)).ToList();

        if (availableAffinities == null || availableAffinities.Count == 0)
        {
            return allCritters;
        }

        List<Critter> availableCritters = new List<Critter>();

        foreach (Critter critter in allCritters)
        {
            foreach (CritterAffinity affinity in critter.Affinities)
            {
                if (availableAffinities.Contains(affinity))
                {
                    availableCritters.Add(critter);

                    break;
                }
            }
        }

        return availableCritters;
    }
}
