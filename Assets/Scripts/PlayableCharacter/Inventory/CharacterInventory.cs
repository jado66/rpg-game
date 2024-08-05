using UnityEngine;
using System.Collections.Generic;

public class CharacterInventory : MonoBehaviour
{
    [SerializeField] 
    private List<GameItem> items = new List<GameItem>();

    // Public getter for Items list
    public List<GameItem> Items => items;

    public void InitializeComponents(Character character)
    {
        // Initialization logic here
    }

    public void AddItem(GameItem item)
    {
        items.Add(item);
    }

    public void AddItem(string itemName)
    {
        GameItem itemToAdd = GameItemDatabase.GetItem(itemName);
        if (itemToAdd != null)
        {
            AddItem(itemToAdd);
        }
        else
        {
            Debug.LogWarning($"Item '{itemName}' not found in database.");
        }
    }

    public bool RemoveItem(GameItem item)
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
            List<GameItem> itemsToRemove = new List<GameItem>();

            for (int j = 0; j < amountToRemove; j++)
            {
                GameItem itemToRemove = items.Find(item => item.Name == title);
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

        return true;
    }

    // Other inventory-related methods...
}
