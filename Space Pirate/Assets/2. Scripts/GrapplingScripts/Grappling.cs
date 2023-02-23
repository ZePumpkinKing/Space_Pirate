using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    Input input;
    GameObject currentEnemyGrapped;
    private Rigidbody rb;
    private Transform orientation;
    public Vector3 grapplePoint;
    ShootHook hook;
    public LayerMask whatIsGrappleable;
    public LayerMask whatIsEnemy;
    public Transform gunTip;
    public Transform castPoint;
    public float maxGrappleDistance;
    private SpringJoint joint;
    [Header("Grappling Joint Values\n")]
    public float grappleJointStrength;
    public float grappleJointDamper;
    public float grappleJointMassScale;
    private float grappleMaxDist = .8f;
    private float grappleMinDist = .25f;
    [Header("Grappling Movement Values\n")]
    public float horizontalThrustForce;
    public float forwardThrustForce;
    public bool isGrappling;
    public bool enemyGrappled;

    Vector3 move;

    private void Awake()
    {
        hook = FindObjectOfType<ShootHook>();
        rb = GetComponent<Rigidbody>();
        orientation = GetComponent<Transform>();
        input = new Input();

        input.Gameplay.Grapple.performed += context => CheckGrapple();
        input.Gameplay.Grapple.canceled += context => EndGrapple();
    }

    private void Update()
    {
        move = input.Gameplay.Move.ReadValue<Vector3>();
        if (isGrappling)
        {
            CalculateGrappleMovement();
            hook.MoveHook();
        }
    }

    private void CheckGrapple()
    {
        Debug.Log("Checkgrapple");
        RaycastHit hit;
        if (Physics.Raycast(castPoint.position, castPoint.forward, out hit, maxGrappleDistance, whatIsGrappleable)) // check if the object is grappleable
        {
            StartGrapple();
            hook.Hookshot(hit);
            isGrappling = true;
        }
        if (Physics.Raycast(castPoint.position, castPoint.forward, out hit, maxGrappleDistance, whatIsEnemy)) // check if the object is grappleable
        {
            StartGrapple();
            isGrappling = true;
        }
    }
    private void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(castPoint.position, castPoint.forward, out hit, maxGrappleDistance, whatIsGrappleable)) // check if the object is grappleable
        {
            hook.Hookshot(hit);
            grapplePoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>(); // create a spring joint which will act as the grapple
            joint.autoConfigureConnectedAnchor = false; // we don't want unity to auto configure a joint to attach to
            joint.connectedAnchor = grapplePoint; // we set ^ ourselves here

            float distanceFromPoint = Vector3.Distance(transform.position, grapplePoint); //gets the initial distance
            joint.maxDistance = distanceFromPoint * grappleMaxDist; // sets max dist from point
            joint.minDistance = distanceFromPoint * grappleMinDist;

            joint.spring = grappleJointStrength;
            joint.damper = grappleJointDamper;
            joint.massScale = grappleJointMassScale;

        }
        else if (Physics.Raycast(castPoint.position, castPoint.forward, out hit, maxGrappleDistance, whatIsEnemy)) // check if the object is an enemy
        {
            hook.Hookshot(hit);
            enemyGrappled = true;
            currentEnemyGrapped = hit.transform.gameObject;
            grapplePoint = currentEnemyGrapped.transform.position;
            joint = gameObject.AddComponent<SpringJoint>(); // create a spring joint which will act as the grapple
            joint.autoConfigureConnectedAnchor = false; // we don't want unity to auto configure a joint to attach to
            joint.connectedAnchor = grapplePoint; // we set ^ ourselves here

            float distanceFromPoint = Vector3.Distance(transform.position, grapplePoint); //gets the initial distance
            joint.maxDistance = distanceFromPoint * grappleMaxDist; // sets max dist from point
            joint.minDistance = distanceFromPoint * grappleMinDist;

            joint.spring = grappleJointStrength;
            joint.damper = grappleJointDamper;
            joint.massScale = grappleJointMassScale;

        }
    }
    private void EndGrapple()
    {
        enemyGrappled = false;
        isGrappling = false;
        hook.ReturnHook();
        Destroy(joint);
    }
    private void CalculateGrappleMovement()
    {

        if (move.x > 0) // right
        {
            rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);
        }
        if (move.x < 0) // left
        {
            rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);
        }
        if (move.z > 0) // forward
        {
            rb.AddForce(orientation.forward * forwardThrustForce * Time.deltaTime);
        }
        if (move.z < 0) // backward
        {
            rb.AddForce(-orientation.forward * forwardThrustForce * Time.deltaTime);
        } // air movement

        Vector3 directionToPoint = grapplePoint - transform.position;
        rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);
        if (enemyGrappled && currentEnemyGrapped != null)
        {
            grapplePoint = currentEnemyGrapped.transform.position;
        }
        if (currentEnemyGrapped == null && enemyGrappled)
        {
            EndGrapple();
            isGrappling = false;
            enemyGrappled = false;
        }

        float distanceFromPoint = Vector3.Distance(transform.position, grapplePoint); // constantly update the distance from point to pull us further into the point
        if (joint != null)
        {
            joint.maxDistance = distanceFromPoint * grappleMaxDist;
            joint.minDistance = distanceFromPoint * grappleMinDist;
        }
    }
    public bool IsGrappling()
    {
        return joint != null;
    }
    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
