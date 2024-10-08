using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item
{
    public string Name;
    public string Description;
    public ItemType ID;
    public int OwnedCount;
    public int Price;
}


public class MasonJar : Item
{
    public MasonJar()
    {
        Name = "Mason Jar";
        Description = "Throw it at bugs to catch them!";
        ID = ItemType.MasonJar;
        Price = 500;
    }
}


public class Nectar : Item
{
    public Nectar()
    {
        Name = "Healing Nectar";
        Description = "Restore your bugs' vitality with this sweet nectar. heals 30 hp.";
        ID = ItemType.Nectar;
        Price = 200;
    }
}


public class MoveManual : Item
{
    public Move TeachableMove;
    
    
    public MoveManual()
    {
        Name = "Move Scroll";
        ID = ItemType.MoveManual;
        Price = 2000;
    }


    public void SetRandomMove()
    {
        
        List<Type> moveTypes = MasterCollection.GetAllMoveTypes();
        Debug.Log($"moves before taking out Tried Its Best: {moveTypes.Count}");
        moveTypes.Remove(typeof(TriedItsBest));
        Debug.Log($"moves after taking out Tried Its Best: {moveTypes.Count}");

        Move move = Activator.CreateInstance(moveTypes[UnityEngine.Random.Range(0, moveTypes.Count)]) as Move;

        SetTeachableMove(move);
    }


    public void SetTeachableMove(Move move)
    {
        TeachableMove = move;
        Description = $"Teach one bug the move {TeachableMove.Name}.";
    }
}


public enum ItemType
{
    None,
    MasonJar,
    MoveManual,
    Nectar,
}
