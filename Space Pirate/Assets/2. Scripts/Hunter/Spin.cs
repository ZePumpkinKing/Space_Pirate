using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform target;

    [SerializeField] bool solo;

    // Update is called once per frame
    void Update()
    {
        if (solo) {
            transform.Rotate(Vector3.right, speed * Time.deltaTime);
        } else {
            transform.LookAt(target);
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
}
