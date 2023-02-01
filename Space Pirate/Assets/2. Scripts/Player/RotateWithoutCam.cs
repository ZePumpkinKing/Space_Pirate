using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithoutCam : MonoBehaviour
{
    Transform cam;
    Player player;
    Transform orientation;

    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        orientation = GameObject.FindGameObjectWithTag("Orientation").GetComponent<Transform>();
    }
    private void LateUpdate()
    {
        if (player.gravityEnabled)
        {
            transform.rotation = orientation.rotation;  // player model is now a child of camera to rotate with it, so this changes the rotation and orientation to only
            transform.position = orientation.position; //  follow the parent's transform instructions when zero gravity is enabled
        } 

    }
}
