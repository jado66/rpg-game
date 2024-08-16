using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CharacterInventory : MonoBehaviour
{
    [SerializeField] 
    private Dictionary<int, InventoryItem> items = new Dictionary<int, InventoryItem>();

    // Public getter for Items dictionary
    public Dictionary<int, InventoryItem> Items => items;

    public int inventorySize;

    public string inventoryIdentifier;

    public bool isDebug;

    public event System.Action<string> OnInventoryChanged; // Changed to include identifier

    [SerializeField]
    private List<ItemEntry> startingItems; // List to hold starting item names


    public void InitializeComponents(Character character)
    {
        // Initialization logic here
    }

    void Start()
    {
        // This needs save game names for this to work
        // bool loadSuccess = LoadInventory();
        // if (!loadSuccess){
        PopulateStartingItems();
        // }
    }

    private void PopulateStartingItems()
    {
        foreach (var entry in startingItems)
        {
            TryAddItem(entry.itemName, entry.amount);
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
        SyncInventory();

        // Returning the old item
        return oldItem;
    }

    public void SetItem(int slot, InventoryItem item){
        items[slot] = item;
        SyncInventory();
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
        SyncInventory();
    }

    public bool TryAddItem(string itemName, int amount = 1)
    {
        InventoryItem item = InventoryItemDatabase.GetItem(itemName).Clone();
        item.Amount = amount;

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
                    SyncInventory();
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
                SyncInventory();
                return true;
            }
        }

        Debug.LogWarning("No empty slot available to add the item.");
        return false;
    }

   

    public void SyncInventory(){
        OnInventoryChanged?.Invoke(inventoryIdentifier);
        SaveInventory();
    }

    public bool RemoveItem(int slot)
    {
        if (items.Remove(slot))
        {
            SyncInventory();
            return true;
        }
        SyncInventory();

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
                itemCount[item.Name] += item.Amount;
            }
            else
            {
                itemCount[item.Name] = item.Amount;
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

            // Collect list of slots and how much to remove from each.
            List<(int slot, int amount)> slotsToModify = new List<(int slot, int amount)>();
            
            foreach (var kvp in items)
            {
                if (kvp.Value.Name == title)
                {
                    if (amountToRemove <= 0)
                        break;

                    int removableAmount = Mathf.Min(amountToRemove, kvp.Value.Amount);
                    slotsToModify.Add((kvp.Key, removableAmount));
                    amountToRemove -= removableAmount;
                }
            }

            // Now actually removing the items or reducing their amount
            foreach (var entry in slotsToModify)
            {
                if (items[entry.slot].Amount <= entry.amount)
                {
                    items.Remove(entry.slot);
                }
                else
                {
                    items[entry.slot].Amount -= entry.amount;
                }
            }
        }

        OnInventoryChanged?.Invoke(inventoryIdentifier);
        return true;
    }
    public void SaveInventory()
    {
        string jsonString = JsonHelper.ToJson(items);
        PlayerPrefs.SetString($"chest-{inventoryIdentifier}", jsonString);
        PlayerPrefs.Save();
    }

    // Load the inventory from PlayerPrefs
    public bool LoadInventory()
    {
        if (PlayerPrefs.HasKey($"chest-{inventoryIdentifier}"))
        {
            string jsonString = PlayerPrefs.GetString($"chest-{inventoryIdentifier}");
            items = JsonHelper.FromJson<Dictionary<int, InventoryItem>>(jsonString);
            SyncInventory();
            Debug.Log("load success");
            return true;
        }
        return false;
    }

    void OnDisable()
    {
        SaveInventory();
    }

    // void OnApplicationQuit()
    // {
    //     SaveInventory();
    // }
    public void GiveItem(string itemInfo) //item:amount its annoying
    {
        string[] parts = itemInfo.Split(':');
        if (parts.Length != 2 || !int.TryParse(parts[1], out int amount))
        {
            Debug.LogError("Invalid item info format. Use 'ItemName:Amount'");
            return;
        }

        string itemName = parts[0];
        bool success = TryAddItem(itemName, amount);
        if (success)
        {
            ToastNotification.Instance.Toast("got-item", $"You received {amount} {itemName}");

            Debug.Log($"Successfully added {amount} of {itemName} to inventory.");
        }
        else
        {
            ToastNotification.Instance.Toast("no-space", "Not enough space in your inventory");

            Debug.LogError($"Failed to add {itemName} to inventory.");
        }
    }
    
}



[System.Serializable]
public class ItemEntry
{
    public string itemName;
    public int amount;

    public ItemEntry(string itemName, int amount)
    {
        this.itemName = itemName;
        this.amount = amount;
    }
}