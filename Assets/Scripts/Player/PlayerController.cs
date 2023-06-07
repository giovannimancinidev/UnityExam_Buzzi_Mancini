using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 2f;

    private InputActions moveAction;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        moveAction = new InputActions();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(-moveVector.y, rb.velocity.y, moveVector.x) * MoveSpeed;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        moveAction.Player.Movement.performed += OnMovementPerformed;
        moveAction.Player.Movement.canceled += OnMovementCancelled;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        moveAction.Player.Movement.performed -= OnMovementPerformed;
        moveAction.Player.Movement.canceled -= OnMovementCancelled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }
}
