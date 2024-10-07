using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VolFx;

public class Pixelator : MonoBehaviour
{
    [SerializeField] private PixelationVol _pixelVol;


    // Start is called before the first frame update
    void Start()
    {
        VolumeProfile volumeProfile = GetComponent<Volume>()?.profile;
        PixelationVol vignette;
        if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
        _pixelVol = vignette;
    }

    // Update is called once per frame
    void Update()
    {
        ClampedFloatParameter intensity = _pixelVol.m_Scale;
        intensity.value = Mathf.Clamp(Mathf.Sin(Time.time), 0, 1);

        //_pixelVol.m_Scale = new ClampedFloatParameter(1, 0, Mathf.Clamp(0,0,Mathf.Sin(Time.time)));
    }
}
