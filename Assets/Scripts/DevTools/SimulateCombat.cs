using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimulateCombat : MonoBehaviour
{
    [SerializeField] private bool _isForcingCombat;
    private bool _hasActivated;


    private void Start()
    {
        if (_isForcingCombat && !_hasActivated)
        {
            _hasActivated = true;
            StartCoroutine(StartCombat());
        }
    }


    private IEnumerator StartCombat()
    {
        yield return new WaitForEndOfFrame();

        BulletAnt ant = new BulletAnt();
        ant.SetStartingLevel(5);

        StartCoroutine(GameObject.FindObjectOfType<EncounterController>().DoCombat(ant));
    }
}
