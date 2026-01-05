using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerMovement playerMovement;
    Animator animator;
    public CameraManager cameraManager;

    public bool isInteracting;

    void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;  // Disable root motion
    }

    void Update()
    {
        inputManager.HandleAllInputs();
        cameraManager.HandleAllCameraMovement();
        
    }

    void FixedUpdate()
    {
        playerMovement.HandleAllMovement();
    }

    void LateUpdate()
    {

        isInteracting = animator.GetBool("isInteracting");
    }

}
