using UnityEngine;
using System.Collections.Generic;

public class CharacterInventory : MonoBehaviour
{
    [SerializeField] 
    private List<InventoryItem> items = new List<InventoryItem>();

    // Public getter for Items list
    public List<InventoryItem> Items => items;

    public event System.Action OnInventoryChanged;

    public void InitializeComponents(Character character)
    {
        // Initialization logic here
    }

    public void AddItem(InventoryItem item)
    {
        items.Add(item);
        Debug.Log("Item added, invoking OnInventoryChanged.");
        OnInventoryChanged?.Invoke();
    }

    public void AddItem(string itemName)
    {
        InventoryItem itemToAdd = InventoryItemDatabase.GetItem(itemName);
        if (itemToAdd != null)
        {
            AddItem(itemToAdd);
            OnInventoryChanged?.Invoke();
        }
        else
        {
            Debug.LogWarning($"Item '{itemName}' not found in database.");
        }
    }

    public bool RemoveItem(InventoryItem item)
    {
        return items.Remove(item);
        
    }

    public Item GetEquippedItem()
    {
        return null;  // Return equipped item logic here
    }

    public bool CheckIfItemsExistAndRemove(string[] titles, int[] amounts = null)
    {
        Dictionary<string, int> itemCount = new Dictionary<string, int>();

        // Initialize item count dictionary
        foreach (var item in items)
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
            List<InventoryItem> itemsToRemove = new List<InventoryItem>();

            for (int j = 0; j < amountToRemove; j++)
            {
                InventoryItem itemToRemove = items.Find(item => item.Name == title);
                if (itemToRemove != null)
                {
                    itemsToRemove.Add(itemToRemove);
                }
            }

            // Now actually removing the items
            foreach (var item in itemsToRemove)
            {

                RemoveItem(item);
            }
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    // Other inventory-related methods...
}
