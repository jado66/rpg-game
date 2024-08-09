using UnityEngine;
using System.Collections.Generic;

public class CharacterHotbar : CharacterInventory 
{
    public InventoryItem GetHotbarItem(int slot)
    {
        if (Items.ContainsKey(slot))
        {
            return Items[slot];
        }
        else
        {
            Debug.LogWarning($"Slot {slot} is empty or does not exist.");
            return null;
        }
    }

    public void SetHotbarItem(int slot, InventoryItem item)
    {
        AddOrSwapItem(slot, item);
        
    }
}
