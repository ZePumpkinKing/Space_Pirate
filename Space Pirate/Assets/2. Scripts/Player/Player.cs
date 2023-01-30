using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Camera cam;
    Rigidbody rb;
    [Header("Transforms")]
    public Transform orientation;
    public Transform groundCheck;
    Input input;

    [Header("Gravity Movement Vars")]
    [SerializeField] float normalSpeed;
    [SerializeField] float maxSpeed;
    public float counterMovement = 0.175f;
    [Header("0g Movement Vars")]
    [SerializeField] float zeroGspeed;
    [SerializeField] float turnSpeed;
    [SerializeField] float sensitivity;
    

    Vector3 move;
    Vector2 look;
    float rotate;

    private float threshold = 0.01f;
    
    public bool gravityEnabled;
    float buttonGrav;
    [Header("Ground Detection")]
    public LayerMask whatIsGround;
    [SerializeField] bool isGrounded;

    private void Awake()
    {
        input = new Input();

        cam = gameObject.GetComponentInChildren<Camera>();
        rb = gameObject.GetComponent<Rigidbody>();

        input.Gameplay.Debug.performed += context => EnableGravity();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        normalSpeed *= 100;
    }

    private void Update()
    {
        move = input.Gameplay.Move.ReadValue<Vector3>();
        rotate = input.Gameplay.Rotate.ReadValue<float>();
        buttonGrav = input.Gameplay.Debug.ReadValue<float>();

        Look();

        if (gravityEnabled) rb.useGravity = true;
        else rb.useGravity = false;
        if (gravityEnabled)
        {
            CheckGround();
        }
        else isGrounded = false;
        
    }

    void FixedUpdate()
    {
        if (gravityEnabled) RegularGravMovement();
        else ZeroGravMovement();
    }
    void EnableGravity()
    {
        if (gravityEnabled) // if gravity is enabled, just call disable gravity instead lol
        {
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, 0);
            DisableGravity();
        }
        else
        {
            gravityEnabled = true;
            Debug.Log("Gravity enabled");
        }
    }

    void DisableGravity()
    {
        gravityEnabled = false;
        Debug.Log("Gravity disabled");
    }

    void RegularGravMovement()
    {
        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);
        Vector2 mag = FindVelRelativeToLook();

        float xMag = mag.x, yMag = mag.y;

        CounterMovement(move.x, move.z, mag);

        //Only do movement calcs if the speed is under max speed, making sure we never go over max speed, while also letting us retain momentum from grappling
        if (Mathf.Abs(move.x) > 0 && xMag < maxSpeed) rb.AddForce(orientation.transform.right * move.x * normalSpeed * Time.deltaTime);
        if (Mathf.Abs(move.z) > 0 && yMag < maxSpeed) rb.AddForce(orientation.transform.forward * move.z * normalSpeed * Time.deltaTime); 

    }

    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitude = rb.velocity.magnitude;
        float yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        //Counter movement
        if (Mathf.Abs(mag.x) > threshold && Mathf.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(normalSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Mathf.Abs(mag.y) > threshold && Mathf.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(normalSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    void ZeroGravMovement()
    {
        rb.AddForce(cam.transform.rotation * new Vector3(move.x, 0, move.z) * zeroGspeed * Time.deltaTime, ForceMode.Impulse);
        rb.AddForce(orientation.transform.rotation * new Vector3(0, move.y, 0) * zeroGspeed * Time.deltaTime, ForceMode.Impulse);
 /*     quick explanation for the new line of code here ^. i reworked the player model so that the player model rotates with the camera
        (only when in 0g), because no rotation was causing issues with the shooting.
        I assume the reason you had the player cylinder not rotating was so that the spacebar would only ever send the player up (in world space),
        so i made the y control only go with the rotation of the new "orientation" transform
        (orientation keeps track of the player's Y rotation, and ignores X and Z.) This restores your original mechanic of spacebar sending the player up, while
        also rotating the player model
        to hunter, from keegan xoxo*/
    }

    private float desiredX;
    private float xRotation;
    void Look()
    {
        look = input.Gameplay.Look.ReadValue<Vector2>();
        float mouseX = look.x * sensitivity * Time.fixedDeltaTime;
        float mouseY = look.y * sensitivity * Time.fixedDeltaTime;

        // find current look rot
        Vector3 rot = cam.transform.rotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //rotate, & make sure we don't over or under-rotate (overrot & underrot is fine in the case of 0g, in fact it is preferred)
        xRotation -= mouseY;
        if (gravityEnabled)
        {
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        }


        //perform rotations XD
        if (gravityEnabled) // if we in normal gravity, use normal gravity look system
        {
            cam.transform.rotation = Quaternion.Euler(xRotation, desiredX, 0);
        }
        else // if we in zero gravity, use zero grav look system
        {
            cam.transform.Rotate(transform.forward, -rotate * turnSpeed * Time.deltaTime);
            cam.transform.Rotate(transform.up, look.x * sensitivity * Time.deltaTime);
            cam.transform.Rotate(transform.right, look.y * sensitivity * Time.deltaTime);
        }

        orientation.transform.rotation = Quaternion.Euler(0, desiredX, 0);
        //

    }

    void CheckGround()
    {
        isGrounded = Physics.CheckBox(groundCheck.position, new Vector3(.5f, .125f, .5f), orientation.rotation, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(.5f, .125f, .5f) * 2);
    }


    

}