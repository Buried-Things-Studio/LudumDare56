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
}
