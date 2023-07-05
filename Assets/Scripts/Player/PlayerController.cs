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
    public float AimDistance;
    public Transform SpawnBullet;
    public Transform PlayerPos;
    //public float AimAngleMultiplier;

    // REFERENCES
    private InputActions playerAction;
    private Rigidbody rb;
    private Collider col;
    private Animator animController;
    private RigBuilder rigRef; 

    // VARIABLES
    private Vector2 moveVector = Vector2.zero;
    private Vector2 mouseVector = Vector2.zero;
    private bool firePressed, jump, isCrouched;
    private int inverter = 1;
    private bool didOnce = false, isGravityInverted = false, isFallling = false;

    public int JumpEvent { get ; set; }

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        playerAction = new InputActions();
        rb = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<Collider>();
        animController = gameObject.GetComponent<Animator>();
        rigRef = gameObject.GetComponent<RigBuilder>();
        FindObjectOfType<GravityEventManager>().onGravityInvert.AddListener(HandleGravityInvert);
        
        energy = 100f;
    }

    private void Update()
    {
        if (!isGravityInverted)
        {
            if (MouseWorld().z < 0 && !didOnce)
            {
                didOnce = true;
                inverter = -inverter;
                transform.Rotate(0, 180, 0);
            }
            else if (MouseWorld().z > 0 && didOnce)
            {
                didOnce = false;
                inverter = -inverter;
                transform.Rotate(0, 180, 0);
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
            AimTargetPos.position = MouseWorld() * AimDistance + PlayerPos.position;
            float clampedZ = Mathf.Clamp(AimTargetPos.position.z, PlayerPos.position.z + (2f * inverter), PlayerPos.position.z + (5f * inverter));
            float clampedY = Mathf.Clamp(AimTargetPos.position.y, PlayerPos.position.y - 2.5f, PlayerPos.position.y + 1.5f);
            //AimTargetPos.position = new Vector3(0, clampedY, clampedZ);

            // SHOOTING
            if (firePressed)
            {
                firePressed = false;
                Shoot(SpawnBullet);
            }
        }
    }

    private void FixedUpdate()
    {
        // JUMP
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, 1000))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * hit.distance, Color.yellow);
            print(hit.distance);
        }


        if (hit.distance >= 2)
        {
            isFallling = true;
        }

        if (hit.distance >= 0.6 && hit.distance <= 0.9 && isFallling)
        {
            OnEnable();
            animController.SetBool("IsFalling", false);
            isFallling = false;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Surface"))
    //    {
    //        OnEnable();
    //        animController.SetBool("IsFalling", false);
    //        print("GAGA");
    //    }
    //}

    private Vector3 MouseWorld()
    {
        Vector3 screenPos = new Vector3(transform.position.x, mouseVector.y - Screen.height * 0.5f, mouseVector.x - Screen.width * 0.5f);
        return screenPos.normalized;
    }

    public void EnablingAfterJump()
    {
        rigRef.layers[0].active = true;
        col.isTrigger = false;
        rb.useGravity = true;
    }

    public void DisablingOnJump()
    {
        rigRef.layers[0].active = false;
        col.isTrigger = true;
        rb.useGravity = false;
    }

    void HandleGravityInvert(bool isInverted)
    {
        OnDisable();
        animController.SetBool("IsFalling", true);
    }

    #region INPUT SYSTEM
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
        
        isGravityInverted = false;
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

        isGravityInverted = true;
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
    #endregion
}
