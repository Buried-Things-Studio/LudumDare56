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

        GameObject.FindObjectOfType<EncounterController>().CheckRandomEncounter(true);
    }
}
