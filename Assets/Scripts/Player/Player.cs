using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Player
{
    private List<Critter> _critters = new List<Critter>();
    private List<Item> _items = new List<Item>();
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


    public List<Item> GetBattleItems()
    {
        return _items.Where(item => item.ID == ItemType.MasonJar || item.ID == ItemType.Nectar).ToList();
    }


    public void SetActiveCritter(Guid guid)
    {
        Critter newActiveCritter = _critters.Find(critter => critter.GUID == guid);
        _critters.Remove(newActiveCritter);
        _critters.Insert(0, newActiveCritter);
    }


    public void AddItemToInventory(Item item)
    {
        Item existingItem = _items.Find(ownedItem => ownedItem.ID == item.ID);

        if (existingItem == null || item.ID == ItemType.MoveManual)
        {
            _items.Add(item);
            item.OwnedCount = 1;
        }
        else
        {
            existingItem.OwnedCount++;
        }
    }


    public void RemoveItemFromInventory(ItemType itemType)
    {
        Item existingItem = _items.Find(ownedItem => ownedItem.ID == itemType);
        existingItem.OwnedCount--;

        if (existingItem.OwnedCount <= 0)
        {
            _items.Remove(existingItem);
        }
    }


    public List<Critter> GetCritters()
    {
        return _critters;
    }


    public void ClearDeadCritters()
    {
        _critters = _critters.Where(critter => critter.CurrentHealth > 0).ToList();
    }


    public List<Item> GetItems()
    {
        return _items;
    }

    public void AddItem(Item itemToAdd)
    {
        _items.Add(itemToAdd);
    }


    public void AddMoney(int moneyToAdd)
    {
        _money += moneyToAdd;
    }

    public void RemoveMoney(int moneyToRemove)
    {
        _money -= moneyToRemove;
    }

    public int GetMoney()
    {
        return _money;
    }


    public void AddCritter(Critter critter)
    {
        _critters.Add(critter);
    }


    public void TeachMoveToCritter(MoveManual moveManual, Guid critterGuid)
    {
        _critters.Find(critter => critter.GUID == critterGuid).Moves.Add(moveManual.TeachableMove);
        _items.Remove(moveManual);
    }

    public void TeachAbilityToCritter(AbilityManual abilityManual, Guid critterGuid)
    {
        _critters.Find(critter => critter.GUID == critterGuid).Ability = abilityManual.TeachableAbility;
        _items.Remove(abilityManual);
    }

    public void HealAllCritters()
    {
        foreach (Critter critter in _critters)
        {
            critter.RestoreAllHealth();
            if(critter.Ability.ID == AbilityID.Hypochondriac)
            {
                foreach (Move move in critter.Moves)
                {
                    move.CurrentUses = move.MaxUses;
                }
            }
        }
    }


    public void RestoreUsesToAllCritters()
    {
        foreach (Critter critter in _critters)
        {
            foreach (Move move in critter.Moves)
            {
                move.CurrentUses = move.MaxUses;
            }

            if(critter.Ability.ID == AbilityID.Hypochondriac)
            {
                critter.RestoreAllHealth();
            }
        }
    }
}
