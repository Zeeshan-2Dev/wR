using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public InputManager inputManager;
    public Transform playerTransform;
    public Transform cameraPivot;

    private Vector3 camFollowVelocity = Vector3.zero;

    [Header("Camera Movement and Rotation")]
    public float camFollowSpeed = 0f;
    public float camLookSpeed = 0.1f;
    public float camPivotSpeed = 0.1f;
    public float lookAngle;
    public float pivotAngle;
    public float minimumPivotAngle = -30f;
    public float maximumPivotAngle = 30f;

    private InventoryManager inventoryManager;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerTransform = FindAnyObjectByType<PlayerManager>().transform;
        
        // 👇 Automatically find the InventoryManager
        inventoryManager = FindAnyObjectByType<InventoryManager>();

    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
    }

    void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, playerTransform.position, ref camFollowVelocity, camFollowSpeed);
        transform.position = targetPosition;
    }

    void RotateCamera()
    {
        // ❌ Skip rotation if inventory is open
        if (inventoryManager != null && inventoryManager.IsInventoryOpen())
            return;

        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle = lookAngle + (inputManager.cameraInputX * camLookSpeed);

        pivotAngle = pivotAngle - (inputManager.cameraInputY * camLookSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }
}
