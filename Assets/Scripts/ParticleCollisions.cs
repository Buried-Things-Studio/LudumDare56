using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisions : MonoBehaviour
{
    private LayerMask foliageMask;
    public Rigidbody myParentRigidbody;
    [SerializeField] private ParticleSystem leafBurstParticleSystem;
    [SerializeField] private ParticleSystem grassBurstParticleSystem;

    void Start()
    {
        foliageMask = 1 << LayerMask.NameToLayer("Foliage");
        //myParentRigidbody = transform.parent.gameObject.GetComponent<Rigidbody>();
    }
    
    void OnCollisionEnter(Collision col)
    {
        //if 
        if (myParentRigidbody.velocity.magnitude >= 30)
        {
            if(col.gameObject.tag == "Bush")
            {
                leafBurstParticleSystem.Play();
            }

            if(col.gameObject.tag == "Grass")
            {
                grassBurstParticleSystem.Play();
            }
        }
    }
}