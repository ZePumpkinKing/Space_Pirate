using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Grappling grappleScript;
    Recoil recoilScript;
    Camera cam;
    Rigidbody rb;
    [Header("Transforms")]
    public Transform orientation;
    public Transform groundCheck;
    Input input;

    [Header("Gravity Movement Vars")]
    [SerializeField] float normalSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float maxMomentum = 20;
    [SerializeField] float playerJumpForce;
    public float counterMovement = 0.175f;
    [Header("0g Movement Vars")]
    [SerializeField] float zeroGspeed;
    [SerializeField] float turnSpeed;
    [SerializeField] float sensitivity;
    
    //internals
    Vector3 move;
    Vector2 look;
    float rotate;
    bool switchingGravity;

    private float threshold = 0.01f;
    
    public bool gravityEnabled;
    float buttonGrav;

    [Header("Detection")]
    public LayerMask whatIsGround;
    [SerializeField] bool isGrounded;
    [SerializeField] float interactDistance;
    [SerializeField] LayerMask interactables;
    Transform target;

    float gravAdjusting;

    private void Awake()
    {
        input = new Input();
        recoilScript = FindObjectOfType<Recoil>();
        grappleScript = FindObjectOfType<Grappling>();

        cam = gameObject.GetComponentInChildren<Camera>();
        rb = gameObject.GetComponent<Rigidbody>();

        input.Gameplay.Debug.performed += context => SwitchGravity();
        input.Gameplay.Jump.performed += context => Jump();
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

    bool EnableGravRot()
    {
        if (0.02 >= cam.transform.rotation.eulerAngles.x && cam.transform.rotation.eulerAngles.x >= -0.02 && 0.02 >= cam.transform.rotation.eulerAngles.z 
            && cam.transform.rotation.eulerAngles.z >= -0.02)
        {
            return false;
        }
        else
        {
            return true;
        }
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

        // Button Checking
        RaycastHit hit;
        if (Physics.Raycast(cam.gameObject.transform.position, cam.gameObject.transform.forward, out hit, interactDistance, interactables)) {
            target = hit.transform.parent;
            target.GetComponent<Button>().selected = true;
            if (input.Gameplay.Interact.WasPressedThisFrame()) {
                target.GetComponent<Button>().held = true;
                target.GetComponent<Button>().continuing = true;
            } else {
                target.GetComponent<Button>().held = false;
            }
        } else if (target != null) {
            target = null;
        }
    }

    void FixedUpdate()
    {
        if (gravityEnabled && !grappleScript.isGrappling && isGrounded) RegularGravMovement();

        if (!isGrounded) CalculateAirMovement();

        if (!gravityEnabled) ZeroGravMovement();
        HardMomentumStop();

    }

    private void HardMomentumStop()
    {
        if (Mathf.Abs(rb.velocity.x) > maxMomentum)
        {
            rb.velocity = new Vector3(maxMomentum * Mathf.Sign(rb.velocity.x), rb.velocity.y, rb.velocity.z);
        }
        if (Mathf.Abs(rb.velocity.z) > maxMomentum)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxMomentum * Mathf.Sign(rb.velocity.z));
        }
        if (rb.velocity.y > maxMomentum * 2)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxMomentum * 2, rb.velocity.z);
        }
    }

    private bool GoingOverDiagonalSpeed()
    {
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            return true;
        }
        else return false;
    }
    private void CalculateAirMovement()
    {
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        if (Mathf.Abs(move.x) > 0 && Mathf.Abs(xMag) < maxSpeed) // right
        {
            rb.AddForce((orientation.right * Mathf.Sign(move.x)) * (grappleScript.horizontalThrustForce * 10) * Time.deltaTime);
        }
        if (Mathf.Abs(move.z) > 0 && Mathf.Abs(yMag) < maxSpeed && !GoingOverDiagonalSpeed()) // forward
        {
            rb.AddForce(orientation.forward * Mathf.Sign(move.z) * (grappleScript.forwardThrustForce * 10) * Time.deltaTime);
        }

    }
    void SwitchGravity()
    {
        if (gravityEnabled) // if gravity is enabled, just call disable gravity instead
        {
            gravityEnabled = false;
            Debug.Log("Gravity disabled");
        }
        else
        {
            StartCoroutine(EnableGravity());
            
        }
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

        //Limit diagonal running
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    void ZeroGravMovement()
    {
        rb.AddForce(cam.transform.rotation * move * zeroGspeed * Time.deltaTime, ForceMode.Impulse);
    }

    private float desiredX, desiredY, desiredZ;
    public float xRotation;
    void Look()
    {
        look = input.Gameplay.Look.ReadValue<Vector2>();
        float mouseX = look.x * sensitivity * Time.fixedDeltaTime;
        float mouseY = look.y * sensitivity * Time.fixedDeltaTime;

        // find current look rot
        Vector3 rot = cam.transform.rotation.eulerAngles;
        desiredY = rot.y + mouseX;

        //rotate, & make sure we don't over or under-rotate (overrot & underrot is fine in the case of 0g, in fact it is preferred)
        xRotation -= mouseY;
        desiredX = xRotation + recoilScript.currentRotation.x;
        desiredZ = 0 + recoilScript.currentRotation.z;
        if (gravityEnabled)
        {
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            desiredX = Mathf.Clamp(desiredX, -90f, 90f);
        }

        //perform rotations XD
        if (gravityEnabled) // if we in normal gravity, use normal gravity look system
        {
            if (switchingGravity)
            {
                cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(desiredX, desiredY, desiredZ), Time.deltaTime * 10);
            } else cam.transform.rotation = Quaternion.Euler(desiredX, desiredY, desiredZ);
        }
        else // if we in zero gravity, use zero grav look system
        {
            cam.transform.Rotate(transform.forward, rotate * -turnSpeed * Time.deltaTime);
            cam.transform.Rotate(transform.up, look.x * sensitivity * Time.deltaTime);
            cam.transform.Rotate(transform.right, look.y * -sensitivity * Time.deltaTime);

            //cam.transform.Rotate(cam.transform.right, recoilScript.gunScript.currentGun.snappiness);
        }

        orientation.transform.rotation = Quaternion.Euler(desiredX, desiredY, desiredZ);
    }

    IEnumerator EnableGravity()
    {
        gravityEnabled = true;
        Debug.Log("Gravity enabled");
        
        switchingGravity = true;
        yield return new WaitForSeconds(.3f);
        switchingGravity = false;
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckBox(groundCheck.position, new Vector3(transform.localScale.x / 2, transform.localScale.y / 8f, transform.localScale.z / 2),
            orientation.rotation, whatIsGround); //we divide scale x and z by 2 because physics.checkbox wants half of a cube, and then upscales it by 2
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * playerJumpForce, ForceMode.Impulse); // jump bitch
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(transform.localScale.x / 2, .125f, transform.localScale.z / 2) * 2); //
    }




}