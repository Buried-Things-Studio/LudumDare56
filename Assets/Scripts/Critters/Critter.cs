using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Critter
{
    public string Name;
    public Guid GUID;
    public int MaxHealth;
    public int CurrentHealth;
    public int MaxSpeed;
    public int CurrentSpeed;
    public List<CritterAffinity> Affinities;
    public List<Move> Moves;


    public int IncreaseHealth(int increaseAmount)
    {
        int healthBeforeHeal = CurrentHealth;
        CurrentHealth = Mathf.Min(CurrentHealth + increaseAmount, MaxHealth);

        return CurrentHealth - healthBeforeHeal;
    }


    public int DealDamage(int damageAmount, CritterAffinity damagingAffinity)
    {
        int healthBeforeDamage = CurrentHealth;
        float damageMultiplier = Effectiveness.GetDamageMultiplier(Affinities, damagingAffinity);
        int multipliedDamageAmount = Mathf.CeilToInt(damageAmount * damageMultiplier);

        CurrentHealth = Mathf.Max(CurrentHealth - multipliedDamageAmount, 0);

        return healthBeforeDamage - CurrentHealth;
    }
}


public enum CritterAffinity
{
    None,
}