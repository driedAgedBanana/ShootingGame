using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float sprintSpeed;

    bool isGrounded = false; // Check if the player is grounded

    public Rigidbody rb;

    public float rotationSpeed = 2;
    public Camera cam; // Player's camera
    private float camX; // Camera's vertical angle

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam = Camera.main;
    }

    void Update()
    {
        camMovement();
        HandleMovement();        
    }

    private void camMovement()
    {
        // Rotate player based on mouse input
        transform.Rotate(transform.up * Input.GetAxis("Mouse X") * rotationSpeed);

        // Adjust camera's vertical angle based on mouse input
        camX -= Input.GetAxis("Mouse Y") * rotationSpeed;
        camX = Mathf.Clamp(camX, -75, 75); // Clamp camera angle to prevent excessive tilting

        cam.transform.localEulerAngles = new Vector3(camX, 0, 0); // Apply vertical angle to the camera
    }

    private void HandleMovement()
    {
        // Get input and determine movement direction in local space (relative to camera)
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        // Use sprint speed if Left Shift is held down, otherwise use move speed
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        // Convert moveDirection to align with camera's forward direction
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        // Keep movement on the horizontal plane
        forward.y = 0f;
        right.y = 0f;

        // Calculate the final movement direction relative to the camera
        Vector3 moveInLookDirection = (forward * moveDirection.z + right * moveDirection.x).normalized;

        // Apply movement
        rb.velocity = new Vector3(moveInLookDirection.x * speed, rb.velocity.y, moveInLookDirection.z * speed);

        // Check if grounded
        isGrounded = Physics.Raycast(transform.position, -transform.up, 1.3f);
    }
}
