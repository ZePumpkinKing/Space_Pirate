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
    [SerializeField] float knockback;

    public float damage;

    bool launch;
    Transform player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        launch = false;
        Destroy(gameObject, 10);
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
                collision.transform.GetComponent<PlayerHealth>().TakeDamage(damage);
                collision.transform.GetComponent<Rigidbody>().AddForce(transform.forward * knockback, ForceMode.Impulse);
            }

            Destroy(gameObject);
        }

        Destroy(gameObject);
    }
}
