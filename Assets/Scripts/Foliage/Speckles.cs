using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speckles : MonoBehaviour
{
    void Start()
    {
        transform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0, 359));
    }
}
