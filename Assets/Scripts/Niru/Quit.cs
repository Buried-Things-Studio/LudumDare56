using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private GameObject _oneShotGO;
    [SerializeField] private AudioClip _backOutClip;

    public IEnumerator HoldMenuOpen()
    {
        while (!Input.GetKeyDown(Controls.MenuBackKey))
        {
            yield return null;
        }

        OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
        osc.MyClip = _backOutClip;
        osc.Play();

        yield return null;
    }


    void Update()
    {
        if (Input.GetKeyDown(Controls.MenuSelectKey))
        {
            Application.Quit();
        }
    }
}
