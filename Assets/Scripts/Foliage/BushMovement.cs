using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushMovement : MonoBehaviour
{
    public Transform myFollowTransform;

    private void FixedUpdate()
    {
        transform.position = myFollowTransform.position;
    }
}