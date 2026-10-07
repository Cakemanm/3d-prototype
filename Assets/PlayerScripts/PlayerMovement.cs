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

    private Vector2 movementInput;
    private Vector3 horizontalVelocity;
    private Vector3 velocity;
    // private bool isRunning = false;
    private bool jumpHeld = false;

    [Header("Sprint FOV")]
    public Camera playerCamera;
    public float normalFOV = 60f;
    public float sprintFOV = 70f;
    public float fovSpeed = 8f;
    private bool sprintHeld = false;

    public void OnMove(InputValue value) {
        movementInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value) {
        jumpHeld = value.isPressed;
    }

    public void OnSprint(InputValue value) {
        sprintHeld = value.isPressed;
    }

    void Update()
    {
        bool isSprinting = sprintHeld && movementInput.y > 0;
        float speed = isSprinting ? sprintSpeed : walkSpeed;
        // float speed = isRunning ? runSpeed : walkSpeed;
        Vector3 targetVelocity = (transform.right * movementInput.x + transform.forward * movementInput.y) * speed;

        // control on ground, limited steering in air
        if (controller.isGrounded) {
            horizontalVelocity = targetVelocity;
        } else {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, targetVelocity, airControl * Time.deltaTime);
        }

        // gravity
        if (controller.isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        // jump
        if (jumpHeld && controller.isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * -gravity);
        }

        // stronger gravity on way down
        float currentGravity = velocity.y < 0 ? gravity * fallMultiplier : gravity;
        velocity.y += currentGravity * Time.deltaTime;

        controller.Move((horizontalVelocity + Vector3.up * velocity.y) * Time.deltaTime);
        float targetFOV = isSprinting ? sprintFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fovSpeed * Time.deltaTime);
    }
}
