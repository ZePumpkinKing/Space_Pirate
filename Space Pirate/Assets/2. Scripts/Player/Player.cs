using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Camera cam;
    Rigidbody rb;
    public Transform orientation;

    Input input;

    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    [SerializeField] float turnSpeed;
    [SerializeField] float sensitivity;

    Vector3 move;
    Vector2 look;
    float rotate;

    public bool gravityEnabled;
    bool temp;
    float buttonGrav;

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
    }

    private void Update()
    {
        //Debug.Log(buttonGrav);
        move = input.Gameplay.Move.ReadValue<Vector3>();
        rotate = input.Gameplay.Rotate.ReadValue<float>();
        buttonGrav = input.Gameplay.Debug.ReadValue<float>();
        rb.AddForce(cam.transform.rotation * move * speed * Time.deltaTime, ForceMode.Impulse);

        Look();
        
        if (gravityEnabled)
        {
            rb.useGravity = true;
        }
        else
        {
            rb.useGravity = false;
        }

    }

    void FixedUpdate()
    {
        
    }
    void EnableGravity()
    {

        if (gravityEnabled)
        {
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
            cam.transform.Rotate(-transform.right, look.y * sensitivity * Time.deltaTime);
        }

        orientation.transform.rotation = Quaternion.Euler(0, desiredX, 0);
        //

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}