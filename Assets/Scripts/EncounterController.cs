using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EncounterController
{
    private float _encounterChance = 0.125f;
    private int _critterTypesAvailablePerFloor = 5;
    private List<Type> _critterTypesAvailableOnFloor = new List<Type>();
    private Vector2Int _wildEncounterLevelRange;


    public void SetAvailableCrittersOnFloor(Vector2Int levelRange)
    {
        _wildEncounterLevelRange = levelRange;
        
        _critterTypesAvailableOnFloor.Clear();
        List<Type> availableCritterTypes = new List<Type>(MasterCollection.GetAllCritterTypes());

        Debug.Log("CRITTERS AVAILABLE ON FLOOR:");

        for (int i = 0; i < _critterTypesAvailablePerFloor; i++)
        {
            if (availableCritterTypes.Count == 0)
            {
                break;
            }
            
            int randomIndex = UnityEngine.Random.Range(0, availableCritterTypes.Count);
            _critterTypesAvailableOnFloor.Add(availableCritterTypes[i]);

            Debug.Log($"-- {availableCritterTypes[i].GetType()}");

            availableCritterTypes.RemoveAt(randomIndex);
        }
    }
    
    
    public bool CheckRandomEncounter()
    {
        bool isEnteringEncounter = UnityEngine.Random.Range(0f, 1f) < _encounterChance;

        if (isEnteringEncounter)
        {
            Type randomCritterType = _critterTypesAvailableOnFloor[UnityEngine.Random.Range(0, _critterTypesAvailableOnFloor.Count)];
            Critter randomCritter = Activator.CreateInstance(randomCritterType) as Critter;
            randomCritter.SetStartingLevel(UnityEngine.Random.Range(_wildEncounterLevelRange.x, _wildEncounterLevelRange.y + 1));

            //TODO: GIVE RANDOM CRITTER TO COMBAT CONTROLLER
        }

        return isEnteringEncounter;
    }
}
