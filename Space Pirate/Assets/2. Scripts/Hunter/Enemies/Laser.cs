using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] bool distanceBased;
    [SerializeField] float length;

    LineRenderer laser;

    // Start is called before the first frame update
    void Awake() {
        laser = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (distanceBased)
        {
            Vector3[] positions = new Vector3[2];

            RaycastHit target;

            Physics.Raycast(new Ray(transform.position, transform.forward), out target, length, 1 << 6);

            positions[0] = transform.position;

            if (target.transform != null)
            {
                positions[1] = target.point;
            }
            else
            {
                positions[1] = transform.position + transform.forward * length;
            }

            laser.SetPositions(positions);

            //Debug.Log(positions[1]);
        }
        else
        {
            Vector3[] positions = new Vector3[2];

            RaycastHit target;

            Physics.Raycast(new Ray(transform.position, transform.forward), out target, 9999f);

            positions[0] = transform.position;
            positions[1] = target.point;

            laser.SetPositions(positions);
        }
    }

    private void OnDrawGizmos()
    {
        laser = GetComponent<LineRenderer>();

        if (distanceBased)
        {
            Vector3[] positions = new Vector3[2];

            RaycastHit target;

            Physics.Raycast(new Ray(transform.position, transform.forward), out target, length, 1 << 6);

            positions[0] = transform.position;

            if (target.transform != null)
            {
                positions[1] = target.point;
            }
            else
            {
                positions[1] = transform.position + transform.forward * length;
            }

            laser.SetPositions(positions);
        }
        else
        {
            Vector3[] positions = new Vector3[2];

            RaycastHit target;

            Physics.Raycast(new Ray(transform.position, transform.forward), out target, 9999f);

            positions[0] = transform.position;
            positions[1] = target.point;

            laser.SetPositions(positions);
        }
    }
}
