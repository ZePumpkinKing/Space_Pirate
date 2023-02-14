using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    [SerializeField] Transform gunBase;
    [SerializeField] Transform cannon;
    [SerializeField] Transform sensor;

    [SerializeField] GameObject projectile;

    [SerializeField] bool laserSight;

    [SerializeField] float fireDelay;
    [SerializeField] float maxSpeed;
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

        RotateTo(gunBase, player, gunBase.up, maxSpeed);
        gunBase.localEulerAngles = new Vector3(0f, gunBase.localEulerAngles.y, 0f);

        RotateTo(cannon, player, gunBase.up, maxSpeed);


        if (laserSight) {
            Debug.DrawLine(sensor.position, player.position, Color.red);
        }
    }

    void RotateTo(Transform obj, Transform target, Vector3 upAxis, float rotationSpeed)
    {
        Vector3 dir = (target.position - obj.position).normalized;

        Quaternion newRot = Quaternion.LookRotation(dir, upAxis);

        obj.rotation = Quaternion.Slerp(obj.rotation, newRot, rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(gunBase.position, gunBase.up);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.up);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(gunBase.position, gunBase.up);
    }
}
