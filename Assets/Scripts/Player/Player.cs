using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player
{
    private List<Critter> _critters;
    private List<Item> _items;
    private int _money;


    public Critter GetActiveCritter()
    {
        foreach (Critter critter in _critters)
        {
            if (critter.CurrentHealth > 0)
            {
                return critter;
            }
        }

        return null; //should never be hit
    }


    public void AddItemToInventory(Item item)
    {
        _money = Mathf.Min(_money - item.Price, 0); //should never need to clamp the value
        
        Item existingItem = _items.Find(ownedItem => ownedItem.ID == item.ID);

        if (existingItem == null)
        {
            _items.Add(item);
            item.OwnedCount = 1;
        }
        else
        {
            existingItem.OwnedCount++;
        }
    }


    public void AddMoney(int moneyToAdd)
    {
        _money += moneyToAdd;
    }


    public void AddCritter(Critter critter)
    {
        _critters.Add(critter);
    }
}
