using UnityEngine;

public class ExternalInventory : CharacterInventory
{
    public GameObject ExternalInventoryGui;
    public GameObject ExternalInventoryPanel;
    public InventoryUI inventoryUI;

    public void Start(){

        CharacterUI playerUI = GameObject.Find("PlayerUI").GetComponent<CharacterUI>();

        ExternalInventoryGui = playerUI.externalInventoryGui;
        ExternalInventoryPanel = playerUI.externalInventoryPanels;
        inventoryUI = playerUI.externalInventoryPanels.GetComponent<InventoryUI>();
    }

    public  void OpenInventory()
    {
        Debug.Log("external inv = open inv");
        ExternalInventoryGui.SetActive(true);
        ExternalInventoryPanel.SetActive(true);

        // Store the current inventory and set this as the new current inventory
        inventoryUI.SetInventory(this);

    }

    public  void CloseInventory()
    {
        ExternalInventoryGui.SetActive(false);
        ExternalInventoryPanel.SetActive(false);

        // Restore the previous inventory
       
        // inventoryUI.SetInventory(previousInventory);
        
    }
}