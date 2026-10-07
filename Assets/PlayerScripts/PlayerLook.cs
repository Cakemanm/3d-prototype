using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public Transform cameraTransform;
    public float sensitivity = 0.1f;

    private Vector2 lookInput;
    private float xRotation = 0f;

    void Start()
    {
        //hide and lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnLook(InputValue value) {
        lookInput = value.Get<Vector2>();
    }

    private bool isThirdPerson = false;

    void Update()
    {
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        //tilt camera up/dpwn, clamped so it cant flip
        xRotation -= mouseY;
        if (isThirdPerson) {
            xRotation = Mathf.Clamp(xRotation, -10f, 65f);
        } else {
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        }
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //turn body left/right
        transform.Rotate(Vector3.up * mouseX);
    }

    public Vector3 firstPersonPosition = new Vector3(0f, 0.72f, 0f);
    public Vector3 thirdPersonPosition = new Vector3(1f, 1.5f, -2f);
    
    public void OnSwitchCamera(InputValue value) {
        if (!value.isPressed) return;
        isThirdPerson = !isThirdPerson;
        cameraTransform.localPosition = isThirdPerson ? thirdPersonPosition : firstPersonPosition;
    }
}
