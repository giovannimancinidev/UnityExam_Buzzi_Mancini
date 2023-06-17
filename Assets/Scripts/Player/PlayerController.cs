using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Actor
{
    [Header("Movement Settings")]
    public float MoveSpeed = 2f;

    [Header("Shooting System")]
    public Transform AimTargetPos;
    public Transform SpawnBullet;
    //public float AimAngleMultiplier;

    // REFERENCES
    private InputActions playerAction;
    private Rigidbody rb;
    private Animator animController;

    // VARIABLES
    private Vector2 moveVector = Vector2.zero;
    private Vector2 mouseVector = Vector2.zero;
    private bool firePressed;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        playerAction = new InputActions();
        rb = gameObject.GetComponent<Rigidbody>();
        animController = gameObject.GetComponent<Animator>();

        energy = 100f;
    }

    private void Update()
    {
        // ANIMATOR
        if (!GravityInverter.isGravityInverted)
        {
            animController.SetFloat("VelocityX", -moveVector.y * MoveSpeed);
        }
        else
        {
            animController.SetFloat("VelocityX", moveVector.y * MoveSpeed);
        }
        
        animController.SetFloat("VelocityZ", moveVector.x * MoveSpeed);

        // UPDATE AIMING
        AimTargetPos.position = new Vector3(transform.position.x, MouseWorldY(), transform.position.z + 5);

        if (firePressed)
        {
            firePressed = false;
            Shoot(SpawnBullet);
        }
    }

    private void FixedUpdate()
    {
        // MOVEMENT
        float x = animController.deltaPosition.x;
        float z = animController.deltaPosition.z;
        Vector3 v = new Vector3(x, rb.velocity.y, z).normalized;

        rb.velocity = new Vector3(v.x * MoveSpeed, rb.velocity.y, v.z * MoveSpeed);
    }

    private float MouseWorldY()
    {
        Vector3 screenPos = new Vector3(mouseVector.x, mouseVector.y, Camera.main.nearClipPlane + 1);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        if (!GravityInverter.isGravityInverted)
        {
            return Unity.Mathematics.math.remap(-0.5f, 1f, -10f, 10f, worldPos.y);
        }
        else
        {
            return Unity.Mathematics.math.remap(1.5f, 3f, -10f, 10f, worldPos.y);
        }
    }

    private void OnEnable()
    {
        playerAction.Enable();

        playerAction.Player.Movement.performed += OnMovementPerformed;
        playerAction.Player.Movement.canceled += OnMovementCancelled;

        playerAction.Player.Fire.performed += OnFirePerformed;
        playerAction.Player.Fire.canceled += OnFireCancelled;

        playerAction.Player.Aim.performed += OnAimingPerformed;

        //playerAction.Player.EquipeWeapon.performed += OnWeaponEquipmentPerformed;
    }

    private void OnDisable()
    {
        playerAction.Disable();

        playerAction.Player.Movement.performed -= OnMovementPerformed;
        playerAction.Player.Movement.canceled -= OnMovementCancelled;

        playerAction.Player.Fire.performed -= OnFirePerformed;
        playerAction.Player.Fire.canceled -= OnFireCancelled;

        playerAction.Player.Aim.performed -= OnAimingPerformed;

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

    private void OnAimingPerformed(InputAction.CallbackContext value)
    {
        mouseVector = value.ReadValue<Vector2>();
    }

    private void OnFirePerformed(InputAction.CallbackContext value)
    {
        firePressed = value.ReadValue<float>() == 1;
    }

    private void OnFireCancelled(InputAction.CallbackContext value)
    {

    }
}
