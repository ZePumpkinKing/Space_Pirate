using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Camera cam;
    Rigidbody rb;

    Input input;

    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    [SerializeField] float turnSpeed;
    [SerializeField] float sensitivity;

    Vector3 move;
    Vector2 look;
    float rotate;

    private void Awake()
    {
        input = new Input();

        cam = gameObject.GetComponentInChildren<Camera>();
        rb = gameObject.GetComponent<Rigidbody>();
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

    void FixedUpdate()
    {
        move = input.Gameplay.Move.ReadValue<Vector3>();
        look = input.Gameplay.Look.ReadValue<Vector2>();
        rotate = input.Gameplay.Rotate.ReadValue<float>();

        rb.AddForce(cam.transform.rotation * move * speed, ForceMode.Impulse);
        
        cam.transform.Rotate(transform.up, look.x * sensitivity);
        cam.transform.Rotate(transform.right, -look.y * sensitivity);
        cam.transform.Rotate(transform.forward, -rotate * turnSpeed);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}