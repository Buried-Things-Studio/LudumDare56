using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ability
{
    public string Name;
    public string Description;
    public AbilityID ID;
}


public enum AbilityID
{
    None,
    BugMuncher,
    CheatDeath,
    EmergencyMedPack,
    FlirtatiousCustomer,
    Hypochondriac,
    LoyaltyCard,
    PPOrNotPP,
    PrivateMedicalInsurance,
    SkillfulSavages,
    StruggleBetter,
    TheresABugInMyJar,
    TreasureOptions, 
    Versatile,
}
