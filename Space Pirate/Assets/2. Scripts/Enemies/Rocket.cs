using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] float speed;
    [SerializeField] float turnSpeed;
    [SerializeField] float aliveTime;

    public float damage;

    Transform player;
    Rigidbody rb;
    
    bool launch;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindObjectOfType<Player>().transform;
        StartCoroutine(Ready());

        //Destroy(gameObject, aliveTime);
        StartCoroutine(Kill(aliveTime));
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);

        if (launch) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((transform.position - player.position).normalized, transform.up), 1);

            transform.Translate(transform.forward * speed);
        }
    }

    IEnumerator Ready()
    {
        launch = false;
        yield return new WaitForSeconds(delay);
        transform.parent = null;
        launch = true;
    }

    IEnumerator Kill(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
