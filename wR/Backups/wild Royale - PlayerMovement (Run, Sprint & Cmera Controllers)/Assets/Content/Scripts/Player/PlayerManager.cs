using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerMovement playerMovement;
    public CameraManager cameraManager;

    void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        inputManager.HandleAllInputs();
        cameraManager.HandleAllCameraMovement();
        
    }

    void LateUpdate()
    {
        playerMovement.HandleAllMovement();
    }
}
