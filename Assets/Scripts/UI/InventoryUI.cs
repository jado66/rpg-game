using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public List<GameItemUI> uiItems = new List<GameItemUI>();
    public GameObject slotPrefab;
    public Transform slotPanel;
    
    public CharacterInventory parentInventory;
    public int inventorySize = 10;
    
    private List<InventoryItem> localItems = new List<InventoryItem>();

    public bool subscribeToChanges = false;

    private float[,] inventoryGuiTransformPoints = new float[4, 4] {
        {134.44f, -87.1f, 38.36f, 186.75f}, // Stores 5
        {148.34f, -87.1f, 73.12f, 186.75f}, // Stores 10
        {0.1f, 0.1f, 0.1f, 0.1f},
        {0.1f, 0.1f, 0.1f, 0.1f}
    };

    private void Start()
    {
        if (parentInventory == null)
        {
            // Debug.LogWarning("Parent inventory is null on start.");
        }
        SetupInventoryUI();
    }

     private void Awake()
    {
        // Call Initialize during Awake
        if (parentInventory != null)
        {
            Debug.Log("Initializing InventoryUI in Awake.");
            Initialize(parentInventory);
        }
        else
        {
            Debug.LogWarning("Parent inventory is null in Awake.");
        }
    }

    public void Initialize(CharacterInventory inventory = null)
    {
        if (inventory != null)
        {
            parentInventory = inventory;
            if (subscribeToChanges)
            {
                Debug.Log("Subscribing to OnInventoryChanged.");
                parentInventory.OnInventoryChanged += UpdateUI;
            }
        }
        else
        {
            Debug.LogWarning("Passed inventory is null during initialization.");
        }
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (parentInventory != null)
        {
            Debug.Log("Unsubscribing from OnInventoryChanged.");
            parentInventory.OnInventoryChanged -= UpdateUI;
        }
    }

    private void SetupInventoryUI()
    {
        foreach (var uiItem in uiItems)
        {
            Destroy(uiItem.gameObject);
        }
        uiItems.Clear();

        for (int i = 0; i < inventorySize; i++)
        {
            // Instantiate the slot prefab and set its parent to slotPanel
            GameObject slotGO = Instantiate(slotPrefab, slotPanel);

            // Adjust the local position if necessary:
            slotGO.transform.localPosition = Vector3.zero;

            // Get the GameItemUI component
            GameItemUI uiItem = slotGO.GetComponentInChildren<GameItemUI>();
            
            // Add to the list of UI items
            uiItems.Add(uiItem);
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        Debug.Log("Items being updated");
        List<InventoryItem> itemsToDisplay = parentInventory != null ? parentInventory.Items : localItems;

        for (int i = 0; i < uiItems.Count; i++)
        {
            if (i < itemsToDisplay.Count)
            {
                if (uiItems[i] != null)
                {
                    uiItems[i].UpdateGameItem(itemsToDisplay[i]);
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
        int slot = uiItems.FindIndex(i => i.Item == item);
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
            
            if (parentInventory == null)
            {
                if (item != null)
                {
                    if (slot < localItems.Count)
                    {
                        localItems[slot] = item;
                    }
                    else
                    {
                        localItems.Add(item);
                    }
                }
                else if (slot < localItems.Count)
                {
                    localItems.RemoveAt(slot);
                }
            }
        }
    }

    public void AddNewItem(InventoryItem item)
    {
        if (parentInventory != null)
        {
            parentInventory.AddItem(item);
        }
        else
        {
            int emptySlot = localItems.FindIndex(i => i == null);
            if (emptySlot == -1 && localItems.Count < inventorySize)
            {
                emptySlot = localItems.Count;
            }
            
            if (emptySlot != -1)
            {
                UpdateSlot(emptySlot, item);
            }
        }
    }

    public void RemoveItem(InventoryItem item)
    {
        if (parentInventory != null)
        {
            parentInventory.RemoveItem(item);
        }
        else
        {
            int slot = localItems.IndexOf(item);
            if (slot != -1)
            {
                UpdateSlot(slot, null);
            }
        }
    }

    public List<InventoryItem> GetItems()
    {
        return parentInventory != null ? parentInventory.Items : localItems;
    }
}

// Definitions for UIItem, IItem, and InventoryItem are required for this script to compile and function.
