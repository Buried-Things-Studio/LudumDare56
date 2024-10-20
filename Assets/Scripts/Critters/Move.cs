using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Move
{
    public string Name;
    public string Description;
    public MoveID ID;
    public Guid UserGUID;
    public CritterAffinity Affinity;
    public int BasePower;
    public bool IsTargeted;
    public bool IsSharp;
    public int Accuracy = 100;
    public int MaxUses;
    public int CurrentUses;


    public abstract List<CombatVisualStep> ExecuteMove(CombatState state, bool isPlayerUser);

    public List<CombatVisualStep> TryDealDamage(CombatState state, int damage, Critter opponent, Critter user)
    {
        bool opponentIsPlayer = opponent == state.PlayerCritter ? true : false;
        int startingHealth = opponent.CurrentHealth;
        bool canDealDamage = opponent.DealDamage(damage);

        List<CombatVisualStep> steps = new List<CombatVisualStep>();

        if(canDealDamage)
        {
            steps.Add(new HealthChangeStep(opponentIsPlayer, opponent.Level, startingHealth, opponent.CurrentHealth, opponent.MaxHealth));
        }
        else 
        {
            steps.Add(new HunkerDownStep(opponent.Name));
        }
        return steps;

    }
}
