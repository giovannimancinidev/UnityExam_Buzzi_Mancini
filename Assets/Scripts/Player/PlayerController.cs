using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class PlayerController : Actor
{
    [Header("Movement Settings")]
    public float MoveSpeed = 2f;
    public float JumpForce = 2f;

    [Header("Shooting System")]
    public Transform AimTargetPos;
    public Transform SpawnBullet;
    //public float AimAngleMultiplier;

    // REFERENCES
    private InputActions playerAction;
    private Rigidbody rb;
    private Collider col;
    private Animator animController;
    private Rig rigRef; 

    // VARIABLES
    private Vector2 moveVector = Vector2.zero;
    private Vector2 mouseVector = Vector2.zero;
    private bool firePressed, jump, isCrouched;
    private int inverter = 1;

    public int JumpEvent { get ; set; }

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        playerAction = new InputActions();
        rb = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<Collider>();
        animController = gameObject.GetComponent<Animator>();
        rigRef = gameObject.GetComponentInChildren<Rig>();

        energy = 100f;
    }

    private void Update()
    {

        // MOVEMENT
        if (moveVector.x == -transform.forward.z)
        {
            transform.Rotate(0, 180, 0);
            inverter = -inverter;
        }

        float z = animController.deltaPosition.z;
        Vector3 v = new Vector3(rb.velocity.x, rb.velocity.y, z).normalized;

        rb.velocity = new Vector3(0, rb.velocity.y, v.z);

        // ANIMATOR
        animController.SetFloat("VelocityZ", inverter * moveVector.x * MoveSpeed);
        animController.SetBool("IsFiring", firePressed);
        animController.SetBool("Jump", jump);
        animController.SetBool("IsCrouched", isCrouched);

        // UPDATE AIMING
        AimTargetPos.position = new Vector3(transform.position.x, MouseWorldY(), transform.position.z + (5 * inverter));

        // SHOOTING
        if (firePressed)
        {
            firePressed = false;
            Shoot(SpawnBullet);
        }
    }

    private void FixedUpdate()
    {
        // JUMP
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, 1000))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * hit.distance, Color.yellow);

            //print(hit.distance);
        }

        if (hit.distance <= 0.5)
        {
            animController.speed = 1;
        }
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

    public void EnablingAfterJump()
    {
        rigRef.weight = 1;
        col.isTrigger = false;
        rb.useGravity = true;
    }

    public void DisablingOnJump()
    {
        rigRef.weight = 0;
        col.isTrigger = true;
        rb.useGravity = false;
    }

    private void OnEnable()
    {
        playerAction.Enable();

        playerAction.Player.Movement.performed += OnMovementPerformed;
        playerAction.Player.Movement.canceled += OnMovementCancelled;

        playerAction.Player.Fire.performed += OnFirePerformed;
        playerAction.Player.Fire.canceled += OnFireCancelled;

        playerAction.Player.Jump.performed += OnJumpPerformed;
        playerAction.Player.Jump.canceled += OnJumpCancelled;

        playerAction.Player.Crouch.performed += OnCrouchingPerformed;

        playerAction.Player.Aim.performed += OnAimingPerformed;
    }

    private void OnDisable()
    {
        playerAction.Disable();

        playerAction.Player.Movement.performed -= OnMovementPerformed;
        playerAction.Player.Movement.canceled -= OnMovementCancelled;

        playerAction.Player.Fire.performed -= OnFirePerformed;
        playerAction.Player.Fire.canceled -= OnFireCancelled;

        playerAction.Player.Jump.performed -= OnJumpPerformed;
        playerAction.Player.Jump.canceled -= OnJumpCancelled;

        playerAction.Player.Crouch.performed -= OnCrouchingPerformed;

        playerAction.Player.Aim.performed -= OnAimingPerformed;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
        isCrouched = false;
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        jump = value.ReadValue<float>() == 1;
    }

    private void OnJumpCancelled(InputAction.CallbackContext value)
    {
        jump = value.ReadValue<float>() == 1;
    }

    private void OnCrouchingPerformed(InputAction.CallbackContext value)
    {
        isCrouched = isCrouched ? false : true;
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
        firePressed = value.ReadValue<float>() == 1;
    }
}
