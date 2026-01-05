// InventoryInput.cs
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryInput : MonoBehaviour
{
    public GameObject inventoryPanel;

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

}
