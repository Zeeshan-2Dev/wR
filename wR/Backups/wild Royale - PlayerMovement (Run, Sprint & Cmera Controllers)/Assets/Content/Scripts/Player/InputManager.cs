using UnityEngine;

public class InputManager : MonoBehaviour
{
    //Ref Scripts
    PlayerControls playerControls;
    PlayerMovement playerMovement;
    AnimatorManager animatorManager;

    //Player Movements
    public Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    //Camera Controls
    private Vector2 cameraInput;
    public float cameraInputX;
    public float cameraInputY;

    [Header("Input Button Flags")]
    public bool sprintInput;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animatorManager = GetComponent<AnimatorManager>();
    }

    void OnEnable()
    {
        if (playerControls == null)
        { 
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.CameraMovement.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
        }
        playerControls.Enable();
    }
    void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
    }

    public void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimValues(0, moveAmount, playerMovement.isSprinting);
    }

    void HandleSprintingInput()
    {
        if(sprintInput && moveAmount > 0.5f)
        {
            playerMovement.isSprinting = true;
        }
        else
        {
            playerMovement.isSprinting = false;
        }
    }

}
