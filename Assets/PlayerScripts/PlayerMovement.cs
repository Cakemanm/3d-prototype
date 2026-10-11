using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    [Header("Movement Settings")]
    public float walkSpeed = 4.3f;
    public float sprintSpeed = 5.6f;
    public float airControl = 2f; //steering in air

    [Header("Jump Settings")]
    public float jumpHeight = 1.25f;
    public float gravity = -32f;
    public float fallMultiplier = 1f; // fall faster than jump

    [Header("Dash Settings")]
    public float dashSpeed = 18f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;

    [Header("Sprint FOV")]
    public Camera playerCamera;
    public float normalFOV = 60f;
    public float sprintFOV = 70f;
    public float fovSpeed = 8f;

    private PlayerStats stats;

    private Vector2 movementInput;
    private Vector3 horizontalVelocity;
    private Vector3 velocity;

    // private bool isRunning = false;
    private bool jumpHeld = false;
    private bool jumpPressed = false;
    private bool sprintHeld = false;
    private bool dashPressed = false;

    private int airJumpsLeft;
    private float dashTimeLeft = 0f;
    private float nextDashTime = 0f;
    private Vector3 dashDirection;
    private bool dashStartedInAir;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    public void OnMove(InputValue value) {
        movementInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value) {
        jumpHeld = value.isPressed;
        if (value.isPressed) {
            jumpPressed = true;
        }
    }

    public void OnSprint(InputValue value) {
        sprintHeld = value.isPressed;
    }
    
    public void OnDash(InputValue value) {
        if (value.isPressed) {
            dashPressed = true;
        }
    }

    float jumpVelocity() {
        return Mathf.Sqrt(jumpHeight * 2f * -gravity);
    }

    void Update()
    {
        bool isSprinting = sprintHeld && movementInput.y > 0;
        float speed = isSprinting ? sprintSpeed : walkSpeed;
        Vector3 inputDirection = (transform.right * movementInput.x + transform.forward * movementInput.y).normalized;
        Vector3 targetVelocity =  inputDirection * speed;

        // control on ground, limited steering in air
        if (controller.isGrounded) {
            horizontalVelocity = targetVelocity;
            airJumpsLeft = stats.extraJumps; // Reset air jumps when grounded
            if (velocity.y < 0) {
                velocity.y = -2f; // Small downward force to keep grounded
            }
        } else {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, targetVelocity, airControl * Time.deltaTime);
        }

        // jump
        if (jumpHeld && controller.isGrounded) {
            velocity.y = jumpVelocity();
        } else if (jumpPressed && !controller.isGrounded && airJumpsLeft > 0) {
            velocity.y = jumpVelocity();
            horizontalVelocity = targetVelocity; // Apply horizontal velocity during air jump, you can change direction
            airJumpsLeft--;
            dashTimeLeft = 0f; // Reset dash time when air jumping, jumping cancels air dash
        }
        jumpPressed = false; // Reset jumpPressed after processing

        //dash
        if (dashPressed && stats.canDash && Time.time >= nextDashTime) {
            //dash in steering direction if moving, otherwise dash forward
            dashDirection = inputDirection.sqrMagnitude > 0.01 ? inputDirection.normalized : transform.forward; // Dash in the forward direction
            dashTimeLeft = dashDuration;
            nextDashTime = Time.time + dashCooldown;
            dashStartedInAir = !controller.isGrounded; // Check if dash started in air
        }
        dashPressed = false;

        bool dashing = dashTimeLeft > 0f;
        if (dashing) {
            dashTimeLeft -= Time.deltaTime;
            horizontalVelocity = dashDirection * dashSpeed;
            if (dashTimeLeft <= 0f) {
                horizontalVelocity = dashDirection * speed; // Resume normal movement after dash
            }
        }

        if (dashing && dashStartedInAir)
        {
            velocity.y = 0f; // Cancel vertical velocity during air dash
        } else
        {
            // stronger gravity on way down
            float currentGravity = velocity.y < 0 ? gravity * fallMultiplier : gravity;
            velocity.y += currentGravity * Time.deltaTime;
        }

        controller.Move((horizontalVelocity + Vector3.up * velocity.y) * Time.deltaTime);

        float targetFOV = isSprinting ? sprintFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fovSpeed * Time.deltaTime);
    }
}
