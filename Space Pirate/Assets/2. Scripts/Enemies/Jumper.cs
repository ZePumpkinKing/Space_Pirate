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

    Transform player;

    void Start() {
        player = GameObject.FindObjectOfType<Player>().transform;
    }

    void Update() {
        if (moving) {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            gun.rotation = new Quaternion(-0.707106829f, 0, 0, 0.707106829f);
        } else {
            gun.LookAt(player);
        }
    }

    IEnumerator Jump() {
        yield return new WaitForSeconds(jumpInterval);

        transform.parent = null;

        float xRot = Random.Range(0,180);
        float yRot = Random.Range(0,180);

        transform.Rotate(Vector3.up, xRot);
        transform.Rotate(Vector3.up, yRot);

        moving = true;
    }

    void Land() {


        moving = false;

        StartCoroutine(Jump());
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("ground")) {
            Physics.Raycast(transform.position, collision.transform.position);


            //RaycastHit hit = ;

            //Vector3 normal = ;

            Land();
        }
    }
}
