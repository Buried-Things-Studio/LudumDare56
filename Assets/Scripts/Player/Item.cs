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


public enum ItemType
{
    None,
    MasonJar
}
