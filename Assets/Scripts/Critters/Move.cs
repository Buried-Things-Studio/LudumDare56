using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Move
{
    public string Name;
    public MoveID ID;
    public Guid UserGUID;
    public CritterAffinity Affinity;


    public virtual void ExecuteMove(CombatState state){}
}
