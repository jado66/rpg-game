using UnityEngine;
using System.Collections.Generic;

public class CharacterInventory : MonoBehaviour
{
    [SerializeField] 
    private Dictionary<int, InventoryItem> items = new Dictionary<int, InventoryItem>();

    // Public getter for Items dictionary
    public Dictionary<int, InventoryItem> Items => items;

    public int inventorySize;

    public string inventoryIdentifier;

    public event System.Action<string> OnInventoryChanged; // Changed to include identifier

    [SerializeField]
    private List<string> debugStartingItems; // List to hold starting item names


    public void InitializeComponents(Character character)
    {
        // Initialization logic here
    }

    void Start()
    {
        PopulateStartingItems();
    }

    private void PopulateStartingItems()
    {
        foreach (var itemName in debugStartingItems)
        {
            TryAddItem(itemName);
        }
    }

    public InventoryItem AddOrSwapItem(int slot, InventoryItem item)
    {
        // Cloning the old item before replacement
        InventoryItem oldItem = null;

        if (items.ContainsKey(slot))
        {
            oldItem = items[slot].Clone();
            Debug.Log($"Replacing item in slot {slot}. Old Item: {oldItem}, New Item: {item}");
        }
        else
        {
            Debug.Log($"Adding new item in slot {slot}. New Item: {item}");
        }

        // Adding or swapping the item
        items[slot] = item;

        // Triggering any relevant events
        Debug.Log("Triggering OnInventoryChanged event.");
        OnInventoryChanged?.Invoke(inventoryIdentifier);

        // Returning the old item
        return oldItem;
    }

    public void SetItem(int slot, InventoryItem item){
        items[slot] = item;
        OnInventoryChanged?.Invoke(inventoryIdentifier);
    }

    public void AddItem(int slot, InventoryItem item)
    {
        if (items.ContainsKey(slot))
        {
            Debug.Log($"Slot {slot} is already occupied.");
            return;
        }
        items[slot] = item;
        Debug.Log("Item added, invoking OnInventoryChanged.");
        OnInventoryChanged?.Invoke(inventoryIdentifier);
    }

    // public InventoryItem AddItemOrSwap(int slot, InventoryItem item)
    // {
    //     if (items.ContainsKey(slot))
    //     {
    //         Debug.Log($"Slot {slot} is occupied. Swapping items.");
    //         InventoryItem oldItem = items[slot].Clone();
    //         items[slot] = item.Clone();

    //         // Handle the old item if necessary
    //         return oldItem;

    //         Debug.Log("Item swapped, invoking OnInventoryChanged.");
    //         OnInventoryChanged?.Invoke(inventoryIdentifier);

    //     }
    //     else
    //     {
    //         items[slot] = item.Clone();
    //         return null;
    //         OnInventoryChanged?.Invoke(inventoryIdentifier);

    //     }

    //     Debug.Log("Item added or swapped, invoking OnInventoryChanged.");
    // }

    public bool TryAddItem(string itemName)
    {
        InventoryItem item = InventoryItemDatabase.GetItem(itemName).Clone();

        if (item == null)
        {
            Debug.Log($"Item {itemName} does not exist.");
            return false;
        }

        return TryAddItemToInventory(item);
    }

    public bool TryAddItem(InventoryItem item)
    {
        return TryAddItemToInventory(item);
    }

    public bool TryAddItemToInventory(InventoryItem item)
    {
        // Check for existing stackable items
        foreach (var kvp in items)
        {
            InventoryItem slotItem = kvp.Value;

            if (slotItem == null){
                continue;
            }
            if (kvp.Value.Name == item.Name && kvp.Value.Amount < kvp.Value.StackAmount)
            {
                var remainingSpace = kvp.Value.StackAmount - kvp.Value.Amount;
                if (remainingSpace >= item.Amount)
                {
                    kvp.Value.Amount += item.Amount;
                    Debug.Log("Item stacked, invoking OnInventoryChanged.");
                    OnInventoryChanged?.Invoke(inventoryIdentifier);
                    return true;
                }
                else
                {
                    kvp.Value.Amount += remainingSpace;
                    item.Amount -= remainingSpace;
                }
            }
        }

        // If no available stack was found or couldn't fully stack, find an empty slot.
        for (int i = 0; i < inventorySize; i++)
        {
            if (!items.ContainsKey(i))
            {
                items[i] = item;
                // Debug.Log("Item added in first available slot, invoking OnInventoryChanged.");
                OnInventoryChanged?.Invoke(inventoryIdentifier);
                return true;
            }
        }

        Debug.LogWarning("No empty slot available to add the item.");
        return false;
    }

    public void SyncInventory(){
        OnInventoryChanged?.Invoke(inventoryIdentifier);
    }

    public bool RemoveItem(int slot)
    {
        if (items.Remove(slot))
        {
            OnInventoryChanged?.Invoke(inventoryIdentifier);
            return true;
        }
        OnInventoryChanged?.Invoke(inventoryIdentifier);

        return false;
    }

   public bool RemoveItem(InventoryItem item)
    {
        int slot = -1;
        foreach (var kvp in items)
        {
            if (kvp.Value == item)
            {
                slot = kvp.Key;
                break;
            }
        }

        if (slot != -1)
        {
            items.Remove(slot);
            OnInventoryChanged?.Invoke(inventoryIdentifier);
            return true;
        }

        return false;
    }

    public InventoryItem GetEquippedItem()
    {
        return null;  // Return equipped item logic here
    }

    public bool CheckIfItemsExistAndRemove(string[] titles, int[] amounts = null)
    {
        Dictionary<string, int> itemCount = new Dictionary<string, int>();

        // Initialize item count dictionary
        foreach (var item in items.Values)
        {
            if (itemCount.ContainsKey(item.Name))
            {
                itemCount[item.Name]++;
            }
            else
            {
                itemCount[item.Name] = 1;
            }
        }

        // Validate existence of required items
        for (int i = 0; i < titles.Length; i++)
        {
            string title = titles[i];
            int requiredAmount = (amounts != null && amounts.Length > i) ? amounts[i] : 1;

            if (!itemCount.ContainsKey(title) || itemCount[title] < requiredAmount)
            {
                return false;
            }
        }

        // Remove required items after successful validation
        for (int i = 0; i < titles.Length; i++)
        {
            string title = titles[i];
            int amountToRemove = (amounts != null && amounts.Length > i) ? amounts[i] : 1;

            // Using a list to collect items to be removed first
            List<int> slotsToRemove = new List<int>();

            for (int j = 0; j < amountToRemove; j++)
            {
                foreach (var kvp in items)
                {
                    if (kvp.Value.Name == title)
                    {
                        slotsToRemove.Add(kvp.Key);
                        break;
                    }
                }
            }

            // Now actually removing the items
            foreach (var slot in slotsToRemove)
            {
                RemoveItem(slot);
            }
        }

        OnInventoryChanged?.Invoke(inventoryIdentifier);
        return true;
    }

    // Other inventory-related methods...
}
