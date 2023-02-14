using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] float jumpInterval;
    [SerializeField] float speed;
    //[SerializeField] GameObject projectile;
    //[SerializeField] Transform gun;

    //Vector3[] initialScale;
    //Transform[] currentScale;

    //bool moving = true;

    Rigidbody rb;

    //Transform player;

    void Start() {
        rb = GetComponent<Rigidbody>();
        //player = GameObject.FindObjectOfType<Player>().transform;

        rb.AddForce(transform.forward * speed, ForceMode.Impulse);

        /*
        currentScale = gameObject.GetComponentsInChildren<Transform>();
        
        int i = 0;
        foreach (Transform value in currentScale) {
            initialScale[i] = value.localScale;
            i += 1;
        }
        */
    }

    void Update() {
        /*
        gun.LookAt(player);

        if (moving) {
            gun.rotation = transform.rotation;
        }
        */
    }

    IEnumerator Jump() {
        yield return new WaitForSeconds(jumpInterval);

        transform.parent = null;
        transform.localScale = Vector3.one;

        float xRot = Random.Range(-45,45);
        float yRot = Random.Range(-45,45);

        transform.Rotate(Vector3.forward, xRot);
        transform.Rotate(Vector3.forward, yRot);

        rb.AddForce(transform.forward * speed, ForceMode.Impulse);

        //moving = true;
    }

    void Land(Vector3 position, Vector3 normal) {
        rb.velocity = Vector3.zero;

        transform.position = position;
        transform.LookAt(normal);

        transform.Translate(0.5f,0,0, transform);

        //moving = false;

        StartCoroutine(Jump());
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            RaycastHit hit;

            Physics.Raycast(transform.position, collision.transform.position - transform.position, out hit);

            /*
            int i = 0;
            foreach (Transform value in currentScale) {
                value.localScale = initialScale[i];
                i += 1;
            }
            */

            //transform.SetParent(collision.transform, true);
            Land(hit.point, hit.normal);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 2);
    }
}
