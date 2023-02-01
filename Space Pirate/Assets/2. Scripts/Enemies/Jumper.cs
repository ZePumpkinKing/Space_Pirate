using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] float jumpInterval;
    [SerializeField] float speed;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform gun;


    bool moving = true;

    Rigidbody rb;

    Transform player;

    void Start() {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindObjectOfType<Player>().transform;

        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    void Update() {
        gun.LookAt(player);

        if (moving) {
            gun.rotation = transform.rotation;
        } else {
            
        }
    }

    IEnumerator Jump() {
        yield return new WaitForSeconds(jumpInterval);

        transform.parent = null;
        transform.localScale = Vector3.one;

        float xRot = Random.Range(-45,45);
        float yRot = Random.Range(-45,45);

        transform.Rotate(Vector3.up, xRot);
        transform.Rotate(Vector3.up, yRot);

        rb.AddForce(transform.forward * speed, ForceMode.Impulse);

        moving = true;
    }

    void Land(Vector3 position, Vector3 normal) {
        rb.velocity = Vector3.zero;

        Debug.Log(position + " " + normal);
        transform.localRotation = Quaternion.LookRotation(normal);

        transform.Translate(0,0.5f,0);

        moving = false;

        StartCoroutine(Jump());
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            RaycastHit hit;

            Physics.Raycast(transform.position, collision.transform.position - transform.position, out hit);

            transform.SetParent(collision.transform, true);
            Land(hit.point, hit.normal);
        }
    }
}
