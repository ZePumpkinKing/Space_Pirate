using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    [SerializeField] Transform mount;
    [SerializeField] Transform cannon;
    [SerializeField] Transform sensor;

    [SerializeField] GameObject projectile;

    [SerializeField] bool laserSight;

    [SerializeField] float fireDelay;
    [SerializeField] Vector3 offset;

    Transform player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        if (Time.frameCount % fireDelay == 0) {
            GameObject instance = Instantiate(projectile, cannon);
            instance.transform.localPosition = offset;
        }

        cannon.transform.LookAt(player, transform.up);

        mount.Rotate(transform.up, cannon.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y);

        cannon.transform.LookAt(player, transform.up);

        if (laserSight) {
            Debug.DrawLine(sensor.position, player.position, Color.red);
        }
    }
}
