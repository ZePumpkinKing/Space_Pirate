using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] float jumpInterval;
    [SerializeField] float speed;
    [SerializeField] float turnSpeed;
    //[SerializeField] GameObject projectile;
    //[SerializeField] Transform gun;

    [SerializeField] Transform jumpChecker;

    //Vector3[] initialScale;
    //Transform[] currentScale;

    //bool moving = true;

    Vector3 destination;
    Vector3 destinationNormal;
    bool turning;
    Rigidbody rb;
    SphereCollider hitbox;

    //Transform player;

    void Start() {
        rb = GetComponent<Rigidbody>();
        //player = GameObject.FindObjectOfType<Player>().transform;

        hitbox = GetComponent<SphereCollider>();

        StartCoroutine(Jump());

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
        if ((destination - transform.position).magnitude <= 5 && !turning)
        {
            VelocityTurn(true);
        }
    }

    IEnumerator Jump() {
        yield return new WaitForSeconds(jumpInterval);

        //transform.parent = null;
        //transform.localScale = Vector3.one;

        float xRot = Random.Range(-45,45);
        float yRot = Random.Range(-45,45);

        jumpChecker.transform.Rotate(Vector3.up, xRot);
        jumpChecker.transform.Rotate(Vector3.right, yRot);

        FindDestination();

        Debug.Log((transform.localPosition - destination).magnitude);
        Debug.DrawLine(transform.localPosition, destination, Color.yellow, 10);

        rb.AddForce(jumpChecker.forward * speed, ForceMode.VelocityChange);

        jumpChecker.localRotation = Quaternion.identity;

        StartCoroutine(TurnWait());

        //hitbox.enabled = true;

        //moving = true;
    }

    void FindDestination()
    {
        RaycastHit hit;
        Physics.SphereCast(new Ray(jumpChecker.position, jumpChecker.forward), 0.5f, out hit, LayerMask.GetMask("3"), 1);
        destination = hit.point;
        destinationNormal = hit.normal;

        if ((destination - transform.position).magnitude <= 5) {
            FindDestination();
        }
    }

    void VelocityTurn(bool reverse)
    {
        if (reverse)
        {
            turning = true;
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(destinationNormal, transform.up), turnSpeed);
        } else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rb.velocity.normalized, transform.up), turnSpeed);
        }
    }

    IEnumerator TurnWait()
    {
        yield return new WaitForSeconds(0.1f);
        VelocityTurn(false);
    }

    void Land(Vector3 position, Vector3 normal) {

        rb.velocity = Vector3.zero;

        //transform.LookAt(transform.position + normal);
        transform.rotation = Quaternion.LookRotation(normal);

        //hitbox.enabled = false;

        //transform.position = position;


        //moving = false;

        turning = false;

        StartCoroutine(Jump());
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            RaycastHit hit;

            Physics.Raycast(transform.position, collision.transform.position - transform.position, out hit);

            Vector3 lookPoint = hit.normal * 10;
            /*
            int i = 0;
            foreach (Transform value in currentScale) {
                value.localScale = initialScale[i];
                i += 1;
            }
            */

            //transform.SetParent(collision.transform, true);
            Land(hit.point, lookPoint);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 2);
    }
}
