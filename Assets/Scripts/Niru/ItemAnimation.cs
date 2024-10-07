using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimation : MonoBehaviour
{
    private Vector3 _startPos;
    private float _bounceAmount = 0.05f;


    private void Start()
    {
        _startPos = transform.position;
        transform.Rotate(Vector3.up, Random.Range(0, 359));
    }



    private void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 60);
        transform.position = _startPos + (Vector3.up * _bounceAmount) + (Vector3.up * Mathf.Sin(Time.time) * _bounceAmount);
    }
}
