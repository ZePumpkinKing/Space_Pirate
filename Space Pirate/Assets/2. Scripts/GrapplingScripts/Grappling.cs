using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    Input input;
    private Rigidbody rb;
    private Transform orientation;
    public Vector3 grapplePoint;
    ShootHook hook;
    GameObject currentObstacle;
    public LayerMask whatIsGrappleable;
    public LayerMask whatIsObstacle;
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
    public bool isPlayerGrappling;
    public bool isObjGrappling;
    [Header("Aiming Values")]
    [SerializeField] float autoAimRadius;

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
        if (isPlayerGrappling)
        {
            CalculateGrappleMovement();
            hook.MoveHook(Vector3.zero);
        }
        if (isObjGrappling)
        {
            CalculateObjGrappleMovement(currentObstacle);
            hook.MoveHook(currentObstacle.transform.position);
        }
    }

    private void CheckGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(castPoint.position, castPoint.forward, out hit, maxGrappleDistance, whatIsObstacle)) // check if the object is an obstacle
        {
            StartObstaclePull(hit);
            hook.Hookshot(hit);
            isObjGrappling = true;
        } else if (Physics.Raycast(castPoint.position, castPoint.forward, out hit, maxGrappleDistance, whatIsGrappleable)) // check if the object is grappleable
        {
            StartGrapple(hit);
            hook.Hookshot(hit);
            isPlayerGrappling = true;
        }
        else if (Physics.SphereCast(castPoint.position, autoAimRadius, castPoint.forward, out hit, maxGrappleDistance, whatIsGrappleable)) // if we miss, try a spherecast
        {
            StartGrapple(hit);
            hook.Hookshot(hit);
            isPlayerGrappling = true;
        }
    }
    private void StartObstaclePull(RaycastHit hit)
    {
        grapplePoint = transform.position;
        currentObstacle = hit.transform.gameObject;
        joint = currentObstacle.AddComponent<SpringJoint>(); // create a spring joint which will act as the grapple
        joint.autoConfigureConnectedAnchor = false; // we don't want unity to auto configure a joint to attach to
        joint.connectedAnchor = transform.position; // we set ^ ourselves here

        float distanceFromPoint = Vector3.Distance(grapplePoint, transform.position); //gets the initial distance
        joint.maxDistance = distanceFromPoint * grappleMaxDist; // sets max dist from point
        joint.minDistance = distanceFromPoint * grappleMinDist;

        joint.spring = grappleJointStrength;
        joint.damper = grappleJointDamper;
        joint.massScale = grappleJointMassScale / 2;
    }
    private void StartGrapple(RaycastHit hit)
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
    private void EndGrapple()
    {
        isPlayerGrappling = false;
        isObjGrappling = false;
        currentObstacle = null;
        hook.ReturnHook();
        joint.connectedAnchor = Vector3.zero;
        Destroy(joint);
    }
    private void CalculateGrappleMovement()
    {
        Vector3 directionToPoint = grapplePoint - transform.position;
        rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);
 

        float distanceFromPoint = Vector3.Distance(transform.position, grapplePoint); // constantly update the distance from point to pull us further into the point
        if (joint != null)
        {
            joint.maxDistance = distanceFromPoint * grappleMaxDist;
            joint.minDistance = distanceFromPoint * grappleMinDist;
        }
    }
    private void CalculateObjGrappleMovement(GameObject obstacle)
    {
        Vector3 directionToPoint = transform.position - grapplePoint;
        obstacle.GetComponent<Rigidbody>().AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);
        joint.connectedAnchor = transform.position;
        grapplePoint = transform.position;

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
        if (isPlayerGrappling)
        {
            return grapplePoint;
        }
        if (isObjGrappling)
        {
            return currentObstacle.transform.position;
        }
        else return Vector3.zero;
        
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
