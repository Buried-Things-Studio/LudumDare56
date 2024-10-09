using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceThePlayer : MonoBehaviour
{
    private Transform _playerTransform;
    private bool _canRotate = true;


    private void Start()
    {
        if (GetComponentInParent<CollectorController>())
        {
            transform.localRotation *= Quaternion.Euler(Vector3.up * 180);

            if (GetComponentInParent<CollectorController>().Collector.IsBoss() != true)
                _canRotate = false;
        }
    }


    void Update()
    {
        if (!_canRotate)
            return;

        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (_playerTransform != null)
            transform.LookAt(new Vector3(_playerTransform.position.x, transform.localPosition.y, _playerTransform.position.z));
    }
}
