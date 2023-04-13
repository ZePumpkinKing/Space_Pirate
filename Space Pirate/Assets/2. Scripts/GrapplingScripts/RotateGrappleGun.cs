using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGrappleGun : MonoBehaviour
{
    Grappling grappling;

    private Quaternion desiredRot;
    private float rotSpeed = 10f;
    private void Awake()
    {
        grappling = FindObjectOfType<Grappling>();
    }
    private void Update()
    {
        if (!grappling.IsGrappling())
        {
            desiredRot = transform.parent.rotation;
        } else desiredRot = Quaternion.LookRotation(grappling.GetGrapplePoint() - transform.position);


        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, Time.deltaTime * rotSpeed);
    }
}
