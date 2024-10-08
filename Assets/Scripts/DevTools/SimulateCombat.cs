using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimulateCombat : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(StartCombat());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject.FindObjectOfType<EncounterController>().IsStarterChosen = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerController pc = GameObject.FindObjectOfType<PlayerController>();
            pc.IsInvisibleToEncounters = !pc.IsInvisibleToEncounters;
        }
    }


    private IEnumerator StartCombat()
    {
        yield return null;

        BulletAnt ant = new BulletAnt();
        ant.SetStartingLevel(5);

        StartCoroutine(GameObject.FindObjectOfType<EncounterController>().DoCombat(ant));
    }
}
