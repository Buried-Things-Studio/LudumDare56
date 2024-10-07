using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MasonJarObject : MonoBehaviour
{
    [SerializeField] private GameObject _bugInJarGO;


    public void Glow(Critter critter)
    {
        //Testing
        /*
        int critter = Random.Range(0, 3);

        if (critter == 0)
            MyCritter = new BulletAnt();

        if (critter == 1)
            MyCritter = new MonarchButterfly();

        if (critter == 2)
            MyCritter = new BlackWidowSpider();
            */

        float multiplier = Mathf.Pow(2, 3);
        Color HDRColor = CritterAffinityData.GetAffinityColor(critter.Affinities[0]);
        HDRColor = new Color(HDRColor.r * multiplier, HDRColor.g * multiplier, HDRColor.b * multiplier, 1);

        _bugInJarGO.GetComponent<Renderer>().material.SetColor("_EmissionColor", HDRColor);
    }
}
