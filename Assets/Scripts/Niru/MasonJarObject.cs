using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MasonJarObject : MonoBehaviour
{
    public Critter MyCritter;
    [SerializeField] private GameObject _bugInJarGO;


    private void Start()
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

        if (MyCritter != null)
        {
            _bugInJarGO.SetActive(true);

            float multiplier = Mathf.Pow(2, 5);
            Color HDRColor = CritterAffinityData.GetAffinityColor(MyCritter.Affinities[0]);
            HDRColor = new Color(HDRColor.r * multiplier, HDRColor.g * multiplier, HDRColor.b * multiplier, 1);

            _bugInJarGO.GetComponent<Renderer>().material.SetColor("_EmissionColor", HDRColor);
        }
    }
}
