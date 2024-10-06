using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSamplerA : MonoBehaviour
{
    [SerializeField] private float Strength;
    [SerializeField] private Vector3 WindDirection;
    private Transform myParticleTransform;

    void OnEnable()
    {
        myParticleTransform = transform.GetChild(0);
        myParticleTransform.LookAt(transform.position + WindDirection.normalized + myParticleTransform.position);
        myParticleTransform.Rotate(Vector3.right * 90);
        myParticleTransform.gameObject.GetComponent<ParticleSystem>().startSpeed = Strength/100 * 3;
    }
    
    void OnTriggerStay(Collider col)
    {
        Rigidbody colRigidbody = col.GetComponent<Rigidbody>();
        if (colRigidbody != null)
        {
            colRigidbody.AddForce(WindDirection.normalized * Random.Range(0, Strength) * Mathf.Sin(Time.time* Random.Range(1.5f, 9.0f)));
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, WindDirection.normalized * 5);
        Gizmos.DrawWireSphere(WindDirection.normalized * 5 + transform.position, 0.2f);
    }
}