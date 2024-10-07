using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimulateCombat : MonoBehaviour
{
    [SerializeField] private bool _isForcingCombat;


    private void Start()
    {
        if (_isForcingCombat)
        {
            StartCoroutine(StartCombat());
        }
    }


    private IEnumerator StartCombat()
    {
        yield return new WaitForEndOfFrame();

        BulletAnt ant = new BulletAnt();
        ant.SetStartingLevel(10);

        StartCoroutine(GameObject.FindObjectOfType<EncounterController>().DoCombat(ant));
    }
}
