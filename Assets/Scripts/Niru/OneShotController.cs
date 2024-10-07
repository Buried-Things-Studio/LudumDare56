using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotController : MonoBehaviour
{
    public AudioClip MyClip;
    [SerializeField] private AudioSource _mySource;

    public void PlayWithVariance()
    {
        float i = Random.Range(0.92f, 1.08f);
        _mySource.clip = MyClip;
        _mySource.pitch = i;
        _mySource.Play();
    }
}
