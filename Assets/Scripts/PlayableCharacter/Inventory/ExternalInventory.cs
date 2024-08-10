using UnityEngine;

public class ExternalInventory : CharacterInventory
{
    public GameObject ExternalInventoryPanel;
    private InventoryUI inventoryUI;

    public string inventoryIdentifier;

    void Start()
    {
        if (ExternalInventoryPanel != null)
        {
            inventoryUI = ExternalInventoryPanel.GetComponent<InventoryUI>();
            if (inventoryUI == null)
            {
                Debug.LogError("The ExternalInventoryPanel does not have an InventoryUI component.");
            }
        }
        else
        {
            Debug.LogError("ExternalInventoryPanel is not assigned.");
        }
        
    }

    public void OpenExternalInventory()
    {
        if (inventoryUI == null)
        {
            Debug.LogError("InventoryUI component is not found.");
            return;
        }

        inventoryUI.parentInventory = this;
        inventoryUI.inventoryIdentifier = inventoryIdentifier;
        ExternalInventoryPanel.SetActive(true);
    }

    public void CloseExternalInventory()
    {
        if (inventoryUI == null)
        {
            Debug.LogError("InventoryUI component is not found.");
            return;
        }

        inventoryUI.parentInventory = null;
        inventoryUI.inventoryIdentifier = null;
        ExternalInventoryPanel.SetActive(false);
    }
}
