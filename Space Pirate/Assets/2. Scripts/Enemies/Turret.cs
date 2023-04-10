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

    bool firing;
    bool keepFiring;

    void Start() {
        keepFiring = false;
        firing = false;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        RaycastHit hit;
        Physics.Raycast(transform.position, player.position - transform.position, out hit);

        //Debug.Log(hit.transform.tag);

        if (hit.transform.CompareTag("Player") && !firing) {
            //Debug.Log("Ready Weapon!");
            StartCoroutine(Fire());
        }

        RotateTo(gunBase, player, gunBase.up, maxSpeed);
        gunBase.localEulerAngles = new Vector3(0f, gunBase.localEulerAngles.y, 0f);

        RotateTo(cannon, player, gunBase.up, maxSpeed);


        if (laserSight) {
            Debug.DrawLine(sensor.position, player.position, Color.red);
        }
    }

    IEnumerator Fire() {
        firing = true;
        //Debug.Log("charging");
        yield return new WaitForSeconds(fireDelay);
        //Debug.Log("fire!");
        GameObject instance = Instantiate(projectile, cannon);
        instance.transform.localPosition = offset;
        firing = false;
        if (keepFiring) {
            StartCoroutine(Fire());
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
