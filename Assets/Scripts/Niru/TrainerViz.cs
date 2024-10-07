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


    void Start()
    {
        GameObject nose = _noses[Random.Range(0, _noses.Length)];
        nose.SetActive(true);
        _noseRenderer = nose.GetComponent<Renderer>();

        _eyes[Random.Range(0, _eyes.Length)].SetActive(true);

        _skinRenderer.material.SetColor("_BaseColor", _skinColors[Random.Range(0, _skinColors.Length)]);
        _noseRenderer.material.SetColor("_BaseColor", _noseColors[Random.Range(0, _noseColors.Length)]);
        _teeRenderer.material.SetColor("_BaseColor", _teeColors[Random.Range(0, _teeColors.Length)]);
    }
}
