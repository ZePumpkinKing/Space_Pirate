using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float maxSize;
    [SerializeField] float scaleSpeed;
    [SerializeField] float knockback;
    [SerializeField] float damage;

    Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.magnitude < maxSize)
        {
            transform.localScale += new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime;
        } else
        {
            Destroy(gameObject);
        }

        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(knockback, transform.position, maxSize);
        }
        else
        {
            try
            {
                other.gameObject.GetComponent<Target>().TakeDamage(damage);
            }
            catch
            {

            }
        }
    }
}
