using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControll : CharacterHealth
{
    Rigidbody rb;
    public Transform cam;
    Vector2 moveVector;
    Vector3 angles;

    public float moveSpeed;
    public float lookSpeed;

    public PlayerInventory inventory;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        moveVector = callbackContext.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext callbackContext)
    {
        Vector2 lookVector = callbackContext.ReadValue<Vector2>();
        transform.Rotate(0, lookVector.x * lookSpeed, 0);
        angles.x -= lookVector.y * lookSpeed;
        angles.x = Mathf.Clamp(angles.x, -90f, 90f);
        cam.localRotation = Quaternion.Euler(angles);
    }
    public void OnFire(InputAction.CallbackContext callbackContext)
    {

    }
    public void OnAltFire(InputAction.CallbackContext callbackContext)
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * moveVector.y * moveSpeed * Time.fixedDeltaTime + transform.right * moveVector.x * moveSpeed * Time.fixedDeltaTime);
    }
}
