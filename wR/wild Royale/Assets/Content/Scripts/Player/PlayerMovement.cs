using System;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Script Ref")]
    InputManager inputManager;
    PlayerManager playerManager;
    AnimatorManager animatorManager;

    [Header("Movement")]
    Vector3 moveDirection;
    public Transform camObject;
    Rigidbody playerRigidbody;

    [Header("Movement Values")]
    [Range(0f, 10f)] public float movementSpeed = 5f;
    [Range(0f, 20f)] public float sprintingSpeed = 10f;
    [Range(0f, 15f)] public float rotationSpeed = 12f;
    [Range(0f, 100f)] public float jumpForce = 10f;

    [Header("Falling & Landing")]
    public float inAirTimer;
    public float rayCastHeightOffset = 0.5f;
    public LayerMask groundLayer;

    [Header("Gravity")]
    public float gravity = -9.81f;
    public float gravityMultiplier = 2f;

    [Header("Movement Flags")]
    public bool isMoving;
    public bool isSprinting;
    public bool isGrounded;


    public float fallSpeed = 5f;

    void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
    }

    void FixedUpdate()
    {
        HandleAllMovement();
        HandleFallingAndLanding();
    }

    void Update()
    {
        //HandleJump();
    }

    public void HandleAllMovement()
    {

        if (playerManager.isInteracting)
            return;

        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        moveDirection = camObject.forward * inputManager.verticalInput;
        moveDirection += camObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float currentSpeed = 0f;

        if (isSprinting)
        {
            currentSpeed = sprintingSpeed;
        }
        else if (inputManager.moveAmount > 0.5f)
        {
            currentSpeed = movementSpeed;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        Vector3 movementVelocity = moveDirection * currentSpeed;
        movementVelocity.y = playerRigidbody.linearVelocity.y;

        movementVelocity.y = playerRigidbody.linearVelocity.y; // Preserve vertical velocity (gravity)

        playerRigidbody.linearVelocity = movementVelocity;
    }

    void HandleRotation()
    {
        Vector3 targetDirection = camObject.forward * inputManager.verticalInput + camObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = playerRotation;
    }

    void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * rayCastHeightOffset;

        // Ground Check
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 1.2f, groundLayer))
        {
            // Landing animation (not added yet, so skip)
            // if (!isGrounded && !playerManager.isInteracting)
            // {
            //     animatorManager.PlayTargetAnim("Landing", true);
            // }

            isGrounded = true;
            inAirTimer = 0;
        }
        else
        {
            // Falling animation (not added yet, so skip)
            // if (isGrounded && !playerManager.isInteracting)
            // {
            //     animatorManager.PlayTargetAnim("Falling", true);
            // }

            isGrounded = false;
        }

        // Apply gravity when in air
        if (!isGrounded)
        {
            inAirTimer += Time.fixedDeltaTime;
            Vector3 gravityForce = Vector3.up * gravity * gravityMultiplier;
            playerRigidbody.AddForce(gravityForce, ForceMode.Acceleration);
        }
    }

    //void ApplyGravity()
    //{
    //    if (!isGrounded)
    //    {
    //        Vector3 velocity = playerRigidbody.linearVelocity;
    //        velocity.y += gravity * Time.deltaTime;
    //        playerRigidbody.linearVelocity = velocity;
    //    }
    //}

    //void HandleJump()
    //{
    //    if (inputManager.jumpInput && isGrounded)
    //    {
    //        Vector3 jumpVelocity = playerRigidbody.linearVelocity;
    //        jumpVelocity.y = 0f;
    //        playerRigidbody.linearVelocity = jumpVelocity;
    //        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    //        animatorManager.PlayTargetAnim("Jump", true);
    //        isGrounded = false;
    //    }
    //}

    //void HandleFallingAndLanding()
    //{
    //    RaycastHit hit;
    //    Vector3 rayCastOrigin = transform.position + Vector3.up * rayCastHeightOffset;

    //    if (!isGrounded)
    //    {
    //        if (!playerManager.isInteracting)
    //        {
    //            animatorManager.PlayTargetAnim("Falling", true);
    //        }

    //        inAirTimer += Time.deltaTime;
    //        playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer); // Apply downward force
    //    }

    //    if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, 1.2f, groundLayer))
    //    {
    //        if (!isGrounded)
    //        {
    //            animatorManager.PlayTargetAnim("Landing", true);
    //        }

    //        inAirTimer = 0;
    //        isGrounded = true;
    //    }
    //    else
    //    {
    //        isGrounded = false;
    //    }
    //}
}
