using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class None: Ability
{
    public None()
    {
        Name = "None"; 
        Description = "No ability to see here";
        ID = AbilityID.None;
    }
}

public class BugMuncher: Ability
{
    public BugMuncher()
    {
        Name = "Bug Muncher"; 
        Description = "If this bug is active when a new bug is caught, it will eat the bug and level up one of its stats.";
        ID = AbilityID.BugMuncher;
    }
}

public class CheatDeath: Ability
{
    public bool HasBeenUsed = false;
    public CheatDeath()
    {
        Name = "Cheat Death"; 
        Description = "One time only, if a party member (including this bug) dies, revive them with half health.";
        ID = AbilityID.CheatDeath;
    }
}

public class EmergencyMedPack: Ability
{
    public EmergencyMedPack()
    {
        Name = "Emergency Med Pack"; 
        Description = "Bugs with this ability fully heal at the start of a boss fight.";
        ID = AbilityID.EmergencyMedPack;
    }
}

public class FlirtatiousCustomer: Ability
{
    public FlirtatiousCustomer()
    {
        Name = "FlirtatiousCustomer"; 
        Description = "Your bug charms the shopkeeper into charging 20% less.";
        ID = AbilityID.FlirtatiousCustomer;
    }
}

public class Hypochondriac: Ability
{
    public Hypochondriac()
    {
        Name = "Hypochondriac"; 
        Description = "When you use a hospital, this bug is fully healed and has its uses restored.";
        ID = AbilityID.Hypochondriac;
    }
}

public class LoyaltyCard: Ability
{
    public LoyaltyCard()
    {
        Name = "Loyalty Card"; 
        Description = "Shops automatically restock when you buy an item.";
        ID = AbilityID.LoyaltyCard;
    }
}

public class PPOrNotPP: Ability
{
    public PPOrNotPP()
    {
        Name = "PP or not PP"; 
        Description = "That is the question... Each time you use a move you will have a 50% chance not to use up one of its uses.";
        ID = AbilityID.PPOrNotPP;
    }
}

public class PrivateMedicalInsurance: Ability
{
    public PrivateMedicalInsurance()
    {
        Name = "Private Medical Insurance"; 
        Description = "Each time you spend money, this bug will fully heal.";
        ID = AbilityID.PrivateMedicalInsurance;
    }
}

public class SkillfulSavages: Ability
{
    public SkillfulSavages()
    {
        Name = "Skillful Savages"; 
        Description = "50% chance for wild bugs that you catch to already have a random ability.";
        ID = AbilityID.SkillfulSavages;
    }
}

public class StruggleBetter: Ability
{
    public StruggleBetter()
    {
        Name = "Struggle Better"; 
        Description = "If all moves are out of uses, pick a random move from the game and perform it.";
        ID = AbilityID.StruggleBetter;
    }
}

public class TheresABugInMyJar: Ability
{
    public TheresABugInMyJar()
    {
        Name = "There's a Bug in my Jar"; 
        Description = "Don't shout, or they'll all want one! Mason Jars in shops contain a bug.";
        ID = AbilityID.TheresABugInMyJar;
    }
}

public class TreasureOptions: Ability
{
    public TreasureOptions()
    {
        Name = "Treasure Options"; 
        Description = "On subsequent floors, the treasure room will offer a choice of 2 moves.";
        ID = AbilityID.TreasureOptions;
    }
}
