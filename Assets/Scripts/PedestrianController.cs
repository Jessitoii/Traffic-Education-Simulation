using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PedestrianController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3.0f;
    public float rotationSpeed = 10.0f;
    public float gravity = 9.81f;

    [Header("References")]
    public Animator animator;
    public Transform cameraTransform; // To move relative to camera view

    [Header("Input")]
    public InputActionProperty moveInputSource;

    private CharacterController characterController;
    private Vector3 velocity;

    void OnEnable()
    {
        if (moveInputSource.action != null)
            moveInputSource.action.Enable();
    }

    void OnDisable()
    {
        if (moveInputSource.action != null)
            moveInputSource.action.Disable();
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
        
        // If camera transform is not assigned, try to find the Main Camera
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector2 input = GetInput();
        Vector3 moveDirection = Vector3.zero;

        // Calculate movement direction relative to camera or world
        // For third person, usually relative to camera forward (projected on ground)
        if (cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            moveDirection = (camForward * input.y + camRight * input.x).normalized;
        }
        else
        {
            moveDirection = new Vector3(input.x, 0, input.y).normalized;
        }

        // Apply movement
        if (moveDirection.magnitude >= 0.1f)
        {
            // Rotate towards move direction
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        // Apply Gravity
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y -= gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        // Update Animator
        if (animator != null)
        {
            // Assuming the animator has a "Speed" or "Move" parameter
            // Checking standard naming conventions or just setting magnitude
            // For now, let's assume a simple "Speed" float parameter
            animator.SetFloat("Speed", input.magnitude);
            
            // Also try "IsWalking" bool if that's what it uses
            animator.SetBool("IsWalking", input.magnitude > 0.1f);
        }
    }

    Vector2 GetInput()
    {
        Vector2 input = Vector2.zero;

        // 1. XR Input
        if (moveInputSource.action != null && moveInputSource.action.enabled)
        {
            input = moveInputSource.action.ReadValue<Vector2>();
        }

        // 2. Keyboard Input (Fallback)
        if (input == Vector2.zero)
        {
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");
        }

        return input;
    }
}
