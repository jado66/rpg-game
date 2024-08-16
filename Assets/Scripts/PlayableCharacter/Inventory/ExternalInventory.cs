using UnityEngine;

public class ExternalInventory : CharacterInventory
{
    public GameObject ExternalInventoryGui;
    public GameObject ExternalInventoryPanel;
    public InventoryUI inventoryUI;

    public void OpenExternalInventory()
    {
        Debug.Log("external inv = open inv");
        ExternalInventoryGui.SetActive(true);
        ExternalInventoryPanel.SetActive(true);

        // Store the current inventory and set this as the new current inventory
        inventoryUI.SetInventory(this);
    }

    public void CloseExternalInventory()
    {
        ExternalInventoryGui.SetActive(false);
        ExternalInventoryPanel.SetActive(false);

        // Restore the previous inventory
       
        // inventoryUI.SetInventory(previousInventory);
        
    }
}