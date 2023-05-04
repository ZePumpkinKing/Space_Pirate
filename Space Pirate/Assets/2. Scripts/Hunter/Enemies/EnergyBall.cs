using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour {
    [SerializeField] float chargeSize;
    [SerializeField] float launchSize;
    [SerializeField] float speed;
    [SerializeField] float chargeSpeed;
    [SerializeField] float spinSpeed;
    [SerializeField] float fireDelay;

    public float damage;

    bool launch;
    Transform player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        launch = false;
    }

    void Update() {
        //transform.Rotate(transform.forward, spinSpeed * Time.deltaTime);

        if (launch) {
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        } else {
            if (transform.localScale.magnitude < chargeSize && fireDelay > 0)
            {
                transform.localScale += new Vector3(chargeSpeed, chargeSpeed, chargeSpeed) * Time.deltaTime;
            }
            else
            {
                if (fireDelay > 0)
                {
                    fireDelay -= Time.deltaTime;
                }
                else
                {
                    transform.parent = null;
                    transform.LookAt(player);
                    transform.localScale = new Vector3(launchSize, launchSize, launchSize);
                    launch = true;
                }
            }
        }
    }

    IEnumerator KillMe() {
        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, transform.forward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground") || collision.transform.CompareTag("Player"))
        {
            if (collision.transform.CompareTag("Player"))
            {
                collision.transform.GetComponent<Rigidbody>().AddForce(collision.contacts[0].normal * -collision.transform.GetComponent<Rigidbody>().velocity.magnitude * 2, ForceMode.Impulse);
            }

            Destroy(gameObject);
        }
    }
}
