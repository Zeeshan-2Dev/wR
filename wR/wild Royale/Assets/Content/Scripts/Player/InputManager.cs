using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //Ref Scripts
    PlayerControls playerControls;
    PlayerMovement playerMovement;
    AnimatorManager animatorManager;
    InventoryManager inventoryManager;

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
    public bool jumpInput;
    public bool couchInput;
    public bool inventoryInput;
    public bool actionInput;


    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animatorManager = GetComponent<AnimatorManager>();
        inventoryManager = GetComponent<InventoryManager>();
    }

    void Update()
    {
        HandleAllInputs();
        if (inventoryInput)
        {
            inventoryManager.ToggleInventory();
            inventoryInput = false; // Reset input flag to avoid repeated toggle
        }

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
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.InventoryBag.performed += i => inventoryInput = true;
            playerControls.PlayerActions.Action.performed += i => actionInput = true;

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
        //StartCoroutine(HandleJumpInput());

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



    //IEnumerator HandleJumpInput()
    //{
    //    yield return new WaitForSeconds(.2f);
    //    if(jumpInput)
    //    {
    //        jumpInput = false;
    //    }
    //}

}
