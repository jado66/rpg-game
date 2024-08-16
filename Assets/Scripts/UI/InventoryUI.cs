using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public List<GameItemUI> uiItems = new List<GameItemUI>();
    public GameObject slotPrefab;
    public Transform slotPanel;
    public string inventoryIdentifier;

    public GameObject parent;
    private CharacterInventory currentInventory;
    private int currentInventorySize = 0;

    private Dictionary<int, InventoryItem> localItems = new Dictionary<int, InventoryItem>();

    public bool subscribeToChanges = true;

    private void Start()
    {
        if (currentInventory == null && parent != null)
        {
            CharacterInventory[] inventories = parent.GetComponents<CharacterInventory>();
            foreach (CharacterInventory inv in inventories)
            {
                if (inv.inventoryIdentifier == inventoryIdentifier)
                {
                    SetInventory(inv);
                    break;
                }
            }
        }
    }

    public void SetInventory(CharacterInventory newInventory)
    {
        if (currentInventory != null && subscribeToChanges)
        {
            currentInventory.OnInventoryChanged -= OnInventoryChangedHandler;
        }

        currentInventory = newInventory;
        inventoryIdentifier = currentInventory.inventoryIdentifier;

        if (subscribeToChanges)
        {
            currentInventory.OnInventoryChanged += OnInventoryChangedHandler;
        }

        UpdateInventorySize();
        UpdateUI();
    }

    private void UpdateInventorySize()
    {
        int newSize = currentInventory.inventorySize;

        if (newSize != currentInventorySize)
        {
            currentInventorySize = newSize;
            ResizeUISlots();
        }
    }

    private void ResizeUISlots()
    {
        // Clear all existing slots
        foreach (Transform child in slotPanel)
        {
            Destroy(child.gameObject);
        }
        uiItems.Clear();

        // Create new slots
        for (int i = 0; i < currentInventorySize; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotPanel);
            slotGO.transform.localPosition = Vector3.zero;

            GameItemUI uiItem = slotGO.GetComponentInChildren<GameItemUI>();
            uiItem.slotIndex = i;
            uiItem.parentInventory = currentInventory;

            uiItems.Add(uiItem);
        }
    }

    private void OnInventoryChangedHandler(string identifier)
    {
        if (currentInventory.inventoryIdentifier == identifier)
        {
            UpdateUI();
        }
    }

    private void OnDestroy()
    {
        if (currentInventory != null && subscribeToChanges)
        {
            currentInventory.OnInventoryChanged -= OnInventoryChangedHandler;
        }
    }

    private void UpdateUI()
    {
        Dictionary<int, InventoryItem> itemsToDisplay = currentInventory != null ? currentInventory.Items : localItems;

        for (int i = 0; i < uiItems.Count; i++)
        {
            if (itemsToDisplay.TryGetValue(i, out InventoryItem item))
            {
                if (uiItems[i] != null)
                {
                    if (item == null || item.Amount <= 0)
                    {
                        uiItems[i].UpdateGameItem(null);
                        itemsToDisplay.Remove(i);
                    }
                    else
                    {
                        uiItems[i].UpdateGameItem(item);
                    }
                }
                else
                {
                    Debug.LogError($"uiItems[{i}] is null during UpdateUI.");
                }
            }
            else
            {
                if (uiItems[i] != null)
                {
                    uiItems[i].UpdateGameItem(null);
                }
                else
                {
                    Debug.LogError($"uiItems[{i}] is null during UpdateUI.");
                }
            }
        }
    }

    public void UpdateItemAmounts()
    {
        foreach (var item in uiItems)
        {
            item.UpdateItemAmount();
        }
    }

    public void UpdateItemAmount(InventoryItem item)
    {
        int slot = -1;
        foreach (var kvp in localItems)
        {
            if (kvp.Value == item)
            {
                slot = kvp.Key;
                break;
            }
        }

        if (slot >= 0)
        {
            uiItems[slot].UpdateItemAmount();
        }
    }

    public void UpdateSlot(int slot, InventoryItem item)
    {
        if (slot >= 0 && slot < uiItems.Count)
        {
            uiItems[slot].UpdateGameItem(item);

            if (currentInventory == null)
            {
                if (item != null)
                {
                    localItems[slot] = item;
                }
                else
                {
                    localItems.Remove(slot);
                }
            }
            else
            {
                currentInventory.AddItem(slot, item);
            }
        }
    }

    public void AddNewItem(int slot, InventoryItem item)
    {
        if (currentInventory != null)
        {
            currentInventory.AddItem(slot, item);
        }
        else
        {
            if (!localItems.ContainsKey(slot) && localItems.Count < currentInventory.inventorySize)
            {
                UpdateSlot(slot, item);
            }
        }
    }

    public void RemoveItem(int slot)
    {
        if (currentInventory != null)
        {
            currentInventory.RemoveItem(slot);
        }
        else
        {
            if (localItems.ContainsKey(slot))
            {
                UpdateSlot(slot, null);
            }
        }
    }

    public Dictionary<int, InventoryItem> GetItems()
    {
        return currentInventory != null ? currentInventory.Items : localItems;
    }
}
