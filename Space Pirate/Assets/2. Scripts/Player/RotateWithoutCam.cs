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
            transform.rotation = orientation.rotation;
            transform.position = orientation.position;
        } 

    }
}
