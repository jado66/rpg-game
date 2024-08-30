using UnityEngine;

public class StoreInventory : CharacterInventory
{
    public GameObject StoreInventoryGui;
    public GameObject StoreInventoryPanel;
    public InventoryUI inventoryUI;

    public void OpenStoreInventory()
    {
        Debug.Log("Tying to open store");

        StoreInventoryGui.SetActive(true);
        StoreInventoryPanel.SetActive(true);

        // Store the current inventory and set this as the new current inventory
        inventoryUI.SetInventory(this);
    }

    public void CloseStoreInventory()
    {
        Debug.Log("Tying to close store");

        StoreInventoryGui.SetActive(false);
        StoreInventoryPanel.SetActive(false);

        // Restore the previous inventory
       
        // inventoryUI.SetInventory(previousInventory);
        
    }

    public void ResetInventory()
    {
        Debug.Log("Resetting store inventory to starting items.");

        // Clear current items
        ClearItems();

        // Populate inventory with starting items
        PopulateStartingItems();

        // Sync inventory after resetting
        SyncInventory();
        
        Debug.Log("Store inventory reset complete.");
    }
}