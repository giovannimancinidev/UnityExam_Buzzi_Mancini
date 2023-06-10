using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float MoveSpeed = 2f;
    public float JumpForce = 10;

    // REFERENCES
    private InputActions playerAction;
    private Rigidbody rb;
    private Animator animController;

    // VARIABLES
    private Vector2 moveVector = Vector2.zero;
    private bool isJumping, isGrounded;

    // Start is called before the first frame update
    void Awake()
    {
        playerAction = new InputActions();
        rb = gameObject.GetComponent<Rigidbody>();
        animController = gameObject.GetComponent<Animator>();
        Physics.gravity = new Vector3(0, -20, 0);
    }

    private void Update()
    {
        // LOCOMOTION
        if (moveVector.x != 0 && moveVector.y == 0 && MoveSpeed < 5.0f)
        {
            MoveSpeed += Time.deltaTime * 2;
        }
        else if (moveVector.y != 0 && MoveSpeed < 2.0f)
        {
            MoveSpeed += Time.deltaTime * 2;
        }

        if (MoveSpeed > 0.5f)
        {
            animController.SetFloat("VelocityX", moveVector.y * MoveSpeed);
            animController.SetFloat("VelocityZ", moveVector.x * MoveSpeed);
        }
        else
        {
            animController.SetFloat("VelocityX", 0);
            animController.SetFloat("VelocityZ", 0);
        }

        // JUMP
    }

    private void FixedUpdate()
    {
        // LOCOMOTION
        float x = animController.deltaPosition.x;
        float z = animController.deltaPosition.z;
        Vector3 v = new Vector3(x, rb.velocity.y, z);
        rb.velocity = v.normalized * MoveSpeed;

        // JUMP
        if (isJumping && isGrounded)
        {
            isJumping = false;
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            print("EHEHEHEH");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Floor")
            isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.name == "Floor")
            isGrounded = false;
    }

    private void OnEnable()
    {
        playerAction.Enable();

        playerAction.Player.Movement.performed += OnMovementPerformed;
        playerAction.Player.Movement.canceled += OnMovementCancelled;

        playerAction.Player.Jump.performed += OnJumpPerformed;

        //playerAction.Player.EquipeWeapon.performed += OnWeaponEquipmentPerformed;
    }

    private void OnDisable()
    {
        playerAction.Disable();

        playerAction.Player.Movement.performed -= OnMovementPerformed;
        playerAction.Player.Movement.canceled -= OnMovementCancelled;

        playerAction.Player.Jump.performed -= OnJumpPerformed;

        //playerAction.Player.EquipeWeapon.performed -= OnWeaponEquipmentPerformed;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        isJumping = value.ReadValueAsButton();
    }

    //private void OnWeaponEquipmentPerformed(InputAction.CallbackContext value)
    //{
    //    bool checker = value.ReadValueAsButton();
    //    if (checker)
    //    {
    //        weaponEquiped = weaponEquiped ? false : true;
    //        animController.SetBool("Equiped", weaponEquiped);
    //    }
    //}
}
