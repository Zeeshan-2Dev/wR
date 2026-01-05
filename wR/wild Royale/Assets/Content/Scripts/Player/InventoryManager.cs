using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public GameObject inventoryUI; // Assign in Inspector

    private bool isInventoryOpen = false;

    void Start()
    {
        
    }

    public void AddToInventory()
    {
        Debug.Log("added to inventory");
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);

        // Handle mouse cursor visibility and locking
        if (isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true;                  // Show the cursor
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center
            Cursor.visible = false;                   // Hide the cursor
        }
    }

    public bool IsInventoryOpen()
    {
        return isInventoryOpen;
    }

}
