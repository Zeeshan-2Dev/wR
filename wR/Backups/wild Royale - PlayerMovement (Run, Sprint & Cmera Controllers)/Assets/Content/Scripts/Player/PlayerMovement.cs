using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Script Ref")]
    InputManager inputManager;

    [Header("Movement")]
    Vector3 moveDirection;
    public Transform camObject;
    Rigidbody playerRigidbody;
    public float movementSpeed = 2f;
    public float sprintingSpeed = 5f;
    public float rotationSpeed = 12f;

    [Header("Movement Flags")]
    public bool isMoving;
    public bool isSprinting;

    void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        moveDirection = camObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + camObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting)
        {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else
        {
            if(inputManager.moveAmount > 0.5)
            {
                moveDirection = moveDirection * movementSpeed;
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        }

        Vector3 movementVelocity = moveDirection;
        movementVelocity.y = playerRigidbody.angularVelocity.y;
        playerRigidbody.angularVelocity = movementVelocity;
    }

    void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = camObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + camObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }
}
