using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    [SerializeField] Transform mount;
    [SerializeField] Transform cannon;
    [SerializeField] Transform sensor;

    [SerializeField] GameObject projectile;

    [SerializeField] bool laserSight;

    Transform player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        cannon.transform.LookAt(player, transform.up);

        mount.Rotate(transform.up, cannon.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y);

        cannon.transform.LookAt(player, transform.up);

        if (laserSight) {
            Debug.DrawLine(sensor.position, player.position, Color.red);
        }
    }
}
