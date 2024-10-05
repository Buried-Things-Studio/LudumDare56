using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item
{
    public string Name;
    public ItemType ID;
    public int OwnedCount;
    public int Price;


    public Item(ItemType type, int price)
    {
        ID = type;
        Price = price;
    }
}


public enum ItemType
{
    None,
    MasonJar
}
