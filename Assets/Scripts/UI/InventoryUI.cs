using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public List<GameItemUI> uiItems = new List<GameItemUI>();
    public GameObject slotPrefab;
    public Transform slotPanel;
    
    public CharacterInventory parentInventory;
    public int inventorySize;
    
    private float[,] inventoryGuiTransformPoints = new float[4, 4] {
        {134.44f, -87.1f, 38.36f, 186.75f}, // Stores 5
        {148.34f, -87.1f, 73.12f, 186.75f}, // Stores 10
        {0.1f, 0.1f, 0.1f, 0.1f},
        {0.1f, 0.1f, 0.1f, 0.1f}
    };


    public void UpdateItemAmounts()
    {
        foreach (var item in uiItems)
        {
            item.UpdateItemAmount();
        }
    }

    public void UpdateItemAmount(GameItem item)
    {
        int slot = uiItems.FindIndex(i => i.gameItem == item);
        if (slot >= 0)
        {
            uiItems[slot].UpdateItemAmount();
        }
    }

    public void UpdateSlot(int slot, GameItem item)
    {
        if (slot >= 0 && slot < uiItems.Count)
        {
            uiItems[slot].UpdateGameItem(item);
        }
    }

    public void AddNewItem(GameItem item)
    {
        UpdateSlot(uiItems.FindIndex(i => i.gameItem == null), item);
    }

    public void RemoveItem(GameItem item)
    {
        Debug.Log("Updating slots");
        UpdateSlot(uiItems.FindIndex(i => i.gameItem == item), null);
    }

    
}

// Definitions for UIItem, IItem, and GameItem are required for this script to compile and function.
