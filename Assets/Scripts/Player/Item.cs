using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item
{
    public string Name;
    public ItemType ID;
    public int OwnedCount;
    public int Price;
}


public enum ItemType
{
    None,
}
