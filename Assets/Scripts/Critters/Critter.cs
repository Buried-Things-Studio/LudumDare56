using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Critter
{
    public string Name;
    public int MaxHealth;
    public int CurrentHealth;
    public List<CritterAffinity> Affinity;
}


public enum CritterAffinity
{
    None,
}