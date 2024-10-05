using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EncounterController : MonoBehaviour
{
    private float _encounterChance = 0.125f;
    private int _critterTypesAvailablePerFloor = 5;
    private List<Type> _allCritterTypes;
    private List<Type> _critterTypesAvailableOnFloor = new List<Type>();


    private void Start()
    {
        _allCritterTypes = new List<Type>(
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(Critter)))
        );
    }


    public void SetAvailableCrittersOnFloor()
    {
        _critterTypesAvailableOnFloor.Clear();
        List<Type> availableCritterTypes = new List<Type>(_allCritterTypes);

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

            //TODO: GIVE RANDOM CRITTER TO COMBAT CONTROLLER
        }

        return isEnteringEncounter;
    }
}
