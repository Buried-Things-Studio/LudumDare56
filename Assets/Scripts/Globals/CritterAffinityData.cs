using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class CritterAffinityData
{
    private static List<CritterAffinityProperties> _properties = new List<CritterAffinityProperties>(){
        new CritterAffinityProperties(
            CritterAffinity.Ant,
            new Color32(0xBA, 0x4A, 0x39, 0xFF) //#BA4A39FF
        ),
        new CritterAffinityProperties(
            CritterAffinity.Bee,
            new Color32(0xEF, 0xD5, 0x30, 0xFF) //#EFD530FF
        ),
        new CritterAffinityProperties(
            CritterAffinity.Beetle,
            new Color32(0x0D, 0x0C, 0x7B, 0xFF) //#0D0C7BFF
        ),
        new CritterAffinityProperties(
            CritterAffinity.Caterpillar,
            new Color32(0x7A, 0xDE, 0x98, 0xFF) //#7ADE98FF
        ),
        new CritterAffinityProperties(
            CritterAffinity.Mollusc,
            new Color32(0x70, 0x50, 0x78, 0xFF) //#705078FF
        ),
        new CritterAffinityProperties(
            CritterAffinity.Spider,
            new Color32(0x24, 0x21, 0x1A, 0xFF) //#24211AFF
        ),
    };


    public static Color GetAffinityColor(CritterAffinity affinity)
    {
        return _properties.Find(data => data.Affinity == affinity).PrimaryColor;
    }
}


public class CritterAffinityProperties
{
    public CritterAffinity Affinity;
    public Color PrimaryColor;
    
    
    public CritterAffinityProperties(CritterAffinity affinity, Color primaryColor)
    {
        Affinity = affinity;
        PrimaryColor = primaryColor;
    }
}