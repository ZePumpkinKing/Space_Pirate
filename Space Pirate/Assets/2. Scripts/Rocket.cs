using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] float speed;
    [SerializeField] float turnSpeed;
    [SerializeField] float aliveTime;

    Transform player;
    Rigidbody rb;
    
    bool launch;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindObjectOfType<Player>().transform;
        StartCoroutine(Ready());

        Destroy(gameObject, aliveTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);

        if (launch) {
            rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        }
    }

    IEnumerator Ready()
    {
        launch = false;
        yield return new WaitForSeconds(delay);
        launch = true;
    }
}
