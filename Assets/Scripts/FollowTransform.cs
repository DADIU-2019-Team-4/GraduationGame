using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform targetToFollow;
    public bool usePhysics;
    Rigidbody rb;

    private void Start()
    {
        if (usePhysics)
            rb = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (!usePhysics)
            transform.position = targetToFollow.position;
        else
            rb.MovePosition(targetToFollow.position);
    }
}
