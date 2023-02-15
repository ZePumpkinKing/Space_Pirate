using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    Input input;

    [Header("Position")]
    public float amount;
    public float maxAmount;
    public float smoothAmount;
    [Header("Rotation")]
    public float rotationAmount = 4f;
    public float maxRotationAmount = 5f;
    public float smoothRotation = 12f;

    [Space]
    public bool rotationX = true;
    public bool rotationY = true;
    public bool rotationZ = true;


    private Vector3 initPosition;
    private Quaternion initRotation;

    private float inputX;
    private float inputY;
    private Vector2 movement;

    private void Awake()
    {
        input = new Input();
    }
    private void Start()
    {
        initPosition = transform.localPosition;
        initRotation = transform.localRotation;
    }
    private void Update()
    {
        movement = input.Gameplay.Look.ReadValue<Vector2>();
        CalculateSway();
        MoveSway();
        TiltSway();

    }

    private void CalculateSway()
    {
        inputX = movement.x;
        inputY = movement.y;
    }
    private void MoveSway()
    {
        float moveX = Mathf.Clamp(inputX * amount, -maxAmount, maxAmount);
        float moveY = Mathf.Clamp(inputY * amount, -maxAmount, maxAmount);

        Vector3 finalPos = new Vector3(moveX, moveY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos, Time.deltaTime * smoothAmount);
    }
    private void TiltSway()
    {
        float tiltY = Mathf.Clamp(inputX * rotationAmount, -maxRotationAmount, maxRotationAmount);
        float tiltX = Mathf.Clamp(inputY * rotationAmount, -maxRotationAmount, maxRotationAmount);

        Quaternion finalRot = Quaternion.Euler(rotationX ? -tiltX : 0f, rotationY ? tiltY : 0f, rotationZ ? tiltY : 0f);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRot * initRotation, Time.deltaTime * smoothRotation);
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
