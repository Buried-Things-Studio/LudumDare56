using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class CritterHelpers
{
    public static Dictionary<CritterAffinity, List<CritterAffinity>> GoodDefences = new Dictionary<CritterAffinity, List<CritterAffinity>>(); //search by defending affinity in key field
    public static Dictionary<CritterAffinity, List<CritterAffinity>> BadDefences = new Dictionary<CritterAffinity, List<CritterAffinity>>(); //search by defending affinity in key field
    public static List<int> ExpToNextLevel = new List<int>(){0, 100, 200, 350, 575, 950, 1575, 2200, 2950, 4000, 5200};


    public static float GetDamageMultiplier(List<CritterAffinity> defendingAffinities, CritterAffinity attackingAffinity)
    {
        if (GoodDefences.Count == 0)
        {
            InitializeAffinityTable();
        }
        
        float multiplier = 1f;

        foreach (CritterAffinity defendingAffinity in defendingAffinities)
        {
            if (GoodDefences[defendingAffinity].Contains(attackingAffinity))
            {
                multiplier *= 0.5f;
            }
            else if (BadDefences[defendingAffinity].Contains(attackingAffinity))
            {
                multiplier *= 2f;
            }
        }

        return multiplier;
    }


    private static void InitializeAffinityTable()
    {
        BadDefences[CritterAffinity.Bee] = new List<CritterAffinity>(){
            CritterAffinity.Bee,
        };
    }
}
