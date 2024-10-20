using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ability
{
    public string Name;
    public string Description;
    public AbilityID ID;
    public bool IsNpcUsable; 
}


public enum AbilityID
{
    None,
    BugMuncher,
    CheatDeath,
    EmergencyMedPack,
    FastLearner, 
    FlirtatiousCustomer,
    HunkerDown,
    Hypochondriac,
    LoyaltyCard,
    Musketeer,
    PPOrNotPP,
    PracticeMakesPerfect,
    PrivateMedicalInsurance,
    SkillfulSavages,
    StabProofVest,
    StruggleBetter,
    TheresABugInMyJar,
    TreasureOptions, 
    Versatile,
}
