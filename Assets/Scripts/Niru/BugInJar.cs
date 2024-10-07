using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugInJar : MonoBehaviour
{
    private Vector3 _startPos;
    private float _movementAmount = 0.04f;

    private void Start()
    {
        _startPos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = _startPos + new Vector3(Mathf.Sin(Time.time * 4) * (_movementAmount / 2), Mathf.Sin(Time.time * 2) * _movementAmount, 0);
    }
}
