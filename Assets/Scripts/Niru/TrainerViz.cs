using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerViz : MonoBehaviour
{
    [SerializeField] private GameObject[] _noses;
    [SerializeField] private GameObject[] _eyes;

    [SerializeField] private Renderer _skinRenderer;
    private Renderer _noseRenderer;
    [SerializeField] private Renderer _teeRenderer;

    [SerializeField] private Color[] _skinColors;
    [SerializeField] private Color[] _noseColors;
    [SerializeField] private Color[] _teeColors;


    public void DisplayCorrectAttributes(int noseValue, int eye, int skinColour, int noseColour, int teeColour)
    {
        GameObject nose = _noses[noseValue];
        nose.SetActive(true);
        _noseRenderer = nose.GetComponent<Renderer>();

        _eyes[eye].SetActive(true);

        _skinRenderer.material.SetColor("_BaseColor", _skinColors[skinColour]);
        _noseRenderer.material.SetColor("_BaseColor", _noseColors[noseColour]);
        _teeRenderer.material.SetColor("_BaseColor", _teeColors[teeColour]);

    }
}
