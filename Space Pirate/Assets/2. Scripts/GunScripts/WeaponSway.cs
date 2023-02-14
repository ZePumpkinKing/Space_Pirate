using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    Input input;

    [Header("Sway Values")]
    public float amount;
    public float maxAmount;
    public float smoothAmount;

    private Vector3 initPosition;

    private float movementX;
    private float movementY;
    private Vector2 movement;

    private void Awake()
    {
        input = new Input();
    }
    private void Start()
    {
        initPosition = transform.localPosition;
    }
    private void Update()
    {
        movement = input.Gameplay.Look.ReadValue<Vector2>();
        movementX = movement.x;
        movementY = movement.y;

        float moveX = Mathf.Clamp(movementX * amount, -maxAmount, maxAmount);
        float moveY = Mathf.Clamp(movementY * amount, -maxAmount, maxAmount);

        Vector3 finalPos = new Vector3(moveX, moveY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos, Time.deltaTime * smoothAmount);
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
