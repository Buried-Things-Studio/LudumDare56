using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotController : MonoBehaviour
{
    public AudioClip MyClip;
    [SerializeField] private AudioSource _mySource;

    public void PlayWithVariance()
    {
        float i = Random.Range(0.9f, 1.1f);
        _mySource.clip = MyClip;
        _mySource.pitch = i;
        _mySource.Play();

        Destroy(this.gameObject, MyClip.length);
    }


    public void Play()
    {
        _mySource.clip = MyClip;
        _mySource.Play();

        Destroy(this.gameObject, MyClip.length);
    }
}
