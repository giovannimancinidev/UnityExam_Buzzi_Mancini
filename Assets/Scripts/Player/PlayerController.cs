using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float MoveSpeed = 2f;

    [Header("Shooting System")]
    public BulletsManager BulletManager;
    public GameObject BulletSpawn;

    // REFERENCES
    private InputActions playerAction;
    private Rigidbody rb;
    private Animator animController;

    // VARIABLES
    private Vector2 moveVector = Vector2.zero;
    private bool fire;

    // Start is called before the first frame update
    void Awake()
    {
        playerAction = new InputActions();
        rb = gameObject.GetComponent<Rigidbody>();
        animController = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        // ANIMATOR
        animController.SetFloat("VelocityX", -moveVector.y * MoveSpeed);
        animController.SetFloat("VelocityZ", moveVector.x * MoveSpeed);

        Shoot();
    }

    private void FixedUpdate()
    {
        float x = animController.deltaPosition.x;
        float z = animController.deltaPosition.z;
        Vector3 v = new Vector3(x, rb.velocity.y, z);
        rb.velocity = v.normalized * MoveSpeed;
    }

    private void Shoot()
    {
        if (fire)
        {
            fire = false;

        }
    }

    private void OnEnable()
    {
        playerAction.Enable();

        playerAction.Player.Movement.performed += OnMovementPerformed;
        playerAction.Player.Movement.canceled += OnMovementCancelled;

        playerAction.Player.Fire.performed += OnFirePerformed;

        //playerAction.Player.EquipeWeapon.performed += OnWeaponEquipmentPerformed;
    }

    private void OnDisable()
    {
        playerAction.Disable();

        playerAction.Player.Movement.performed -= OnMovementPerformed;
        playerAction.Player.Movement.canceled -= OnMovementCancelled;

        playerAction.Player.Fire.performed -= OnFirePerformed;

        //playerAction.Player.EquipeWeapon.performed -= OnWeaponEquipmentPerformed;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
        //MoveSpeed = 0;
    }

    private void OnFirePerformed(InputAction.CallbackContext value)
    {
        fire = value.ReadValue<bool>();
    }
}
