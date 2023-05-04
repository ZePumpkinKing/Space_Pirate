using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] float speed;
    [SerializeField] float turnSpeed;
    [SerializeField] float aliveTime;
    [SerializeField] GameObject explosion;

    public float damage;

    Transform player;
    
    bool launch;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>().transform;
        StartCoroutine(Ready());

        //Destroy(gameObject, aliveTime);
        StartCoroutine(Kill(aliveTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (launch) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), Time.deltaTime);

            transform.Translate(transform.forward * speed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameObject myLittleExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
        myLittleExplosion.transform.parent = null;
    }

    IEnumerator Ready()
    {
        launch = false;
        yield return new WaitForSeconds(delay);
        transform.LookAt(player.position);
        transform.parent = null;
        launch = true;
    }

    IEnumerator Kill(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
