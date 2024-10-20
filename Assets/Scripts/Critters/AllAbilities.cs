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
        IsNpcUsable = true;
    }
}

public class BugMuncher: Ability
{
    public BugMuncher()
    {
        Name = "Bug Muncher"; 
        Description = "If a bug with this ability is active when a new bug is caught, it will eat the bug and level up one of its stats.";
        ID = AbilityID.BugMuncher;
        IsNpcUsable = false; 
    }
}

public class CheatDeath: Ability
{
    public bool HasBeenUsed = false;
    public CheatDeath()
    {
        Name = "Cheat Death"; 
        Description = "One time only, if a party member dies, a bug with this ability will revive them with half health.";
        ID = AbilityID.CheatDeath;
        IsNpcUsable = true;
    }
}

public class EmergencyMedPack: Ability
{
    public EmergencyMedPack()
    {
        Name = "Emergency Med Pack"; 
        Description = "Bugs with this ability fully heal at the start of a boss fight.";
        ID = AbilityID.EmergencyMedPack;
        IsNpcUsable = false;
    }
}

public class FastLearner: Ability
{
    public FastLearner()
    {
        Name = "Fast Learner"; 
        Description = "Bugs with this ability get a random stat increase at the end of each turn during a fight.";
        ID = AbilityID.FastLearner;
        IsNpcUsable = true;
    }
}

public class FlirtatiousCustomer: Ability
{
    public FlirtatiousCustomer()
    {
        Name = "Flirtatious Customer"; 
        Description = "Bugs with this ability will charm the shopkeeper into charging you 20% less.";
        ID = AbilityID.FlirtatiousCustomer;
        IsNpcUsable = false;
    }
}

public class HunkerDown: Ability
{
    public int TurnsActive = 0; 
    public HunkerDown()
    {
        Name = "Hunker Down"; 
        Description = "Bugs with this ability are unable to take damage on every third turn during a fight.";
        ID = AbilityID.HunkerDown;
        IsNpcUsable = true;
    }

}

public class Hypochondriac: Ability
{
    public Hypochondriac()
    {
        Name = "Hypochondriac"; 
        Description = "When you use a hospital, bugs with this abilty will be fully healed and have all their uses restored.";
        ID = AbilityID.Hypochondriac;
        IsNpcUsable = false;
    }
}

public class LoyaltyCard: Ability
{
    public LoyaltyCard()
    {
        Name = "Loyalty Card"; 
        Description = "If a bug in your party has this ability, shops will automatically restock when you buy an item.";
        ID = AbilityID.LoyaltyCard;
        IsNpcUsable = false;
    }
}

public class Musketeer: Ability
{
    public Musketeer()
    {
        Name = "Musketeer";
        Description = "If a bug in your party has this ability, the total health of your bugs will be shared equally among them after each fight";
        ID = AbilityID.Musketeer;
        IsNpcUsable = false;
    }

}

public class PPOrNotPP: Ability
{
    public PPOrNotPP()
    {
        Name = "PP or not PP"; 
        Description = "That is the question... Each time a bug with this ability uses a move, it will have a 50% chance not to use up one of its uses.";
        ID = AbilityID.PPOrNotPP;
        IsNpcUsable = true;
    }
}

public class PracticeMakesPerfect: Ability
{
    public int Count = 0; 
    public Move Move = null;
    public PracticeMakesPerfect()
    {
        Name = "Practice Makes Perfect"; 
        Description = "That is the question... Each time a bug with this ability uses a move, it will have a 50% chance not to use up one of its uses.";
        ID = AbilityID.PracticeMakesPerfect;
        IsNpcUsable = true;
    }
}

public class PrivateMedicalInsurance: Ability
{
    public PrivateMedicalInsurance()
    {
        Name = "Private Medical Insurance"; 
        Description = "Bugs with this ability will fully heal each time you spend money.";
        ID = AbilityID.PrivateMedicalInsurance;
        IsNpcUsable = false;
    }
}

public class SkillfulSavages: Ability
{
    public SkillfulSavages()
    {
        Name = "Skillful Savages"; 
        Description = "If a bug in your party has this ability, there is a 50% chance for wild bugs that you catch to already have a random ability.";
        ID = AbilityID.SkillfulSavages;
        IsNpcUsable = false;
    }
}

public class StabProofVest: Ability
{
    public StabProofVest()
    {
        Name = "Stab Proof Vest"; 
        Description = "Moves used against bugs with this ability do not get a same affinity power bonus.";
        ID = AbilityID.StabProofVest;
        IsNpcUsable = true;
    }
}

public class StruggleBetter: Ability
{
    public StruggleBetter()
    {
        Name = "Struggle Better"; 
        Description = "When bugs with this ability are out of move uses, they will pick any move at random and perform it.";
        ID = AbilityID.StruggleBetter;
        IsNpcUsable = true;
    }
}

// public class TheresABugInMyJar: Ability
// {
//     public TheresABugInMyJar()
//     {
//         Name = "There's a Bug in my Jar"; 
//         Description = "Don't shout, or they'll all want one! Mason Jars in shops contain a bug.";
//         ID = AbilityID.TheresABugInMyJar;
//     }
// }

public class TreasureOptions: Ability
{
    public TreasureOptions()
    {
        Name = "Treasure Options"; 
        Description = "If a bug in your party has this ability, on subsequent floors the treasure room will offer a choice of 2 moves.";
        ID = AbilityID.TreasureOptions;
        IsNpcUsable = false;
    }
}

public class Versatile: Ability
{
    public Versatile()
    {
        Name = "Versatile"; 
        Description = "Bugs with this ability get a 20% increase to power on moves which don't match their Affinity.";
        ID = AbilityID.Versatile;
        IsNpcUsable = true;
    }
}
