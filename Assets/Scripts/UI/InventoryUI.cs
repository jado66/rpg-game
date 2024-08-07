using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public List<GameItemUI> uiItems = new List<GameItemUI>();
    public GameObject slotPrefab;
    public Transform slotPanel;
    public string inventoryIdentifier;  // Add this line

    public GameObject parent;
    public CharacterInventory parentInventory;
    public int inventorySize = 10;

    private Dictionary<int, InventoryItem> localItems = new Dictionary<int, InventoryItem>();

    public bool subscribeToChanges = false;

    private float[,] inventoryGuiTransformPoints = new float[4, 4]
    {
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
        CharacterInventory[] inventories = parent.GetComponents<CharacterInventory>();
        foreach (CharacterInventory inv in inventories)
        {
            if (inv.inventoryIdentifier == inventoryIdentifier)
            {
                Debug.Log($"{inv.inventoryIdentifier} synced with {inventoryIdentifier}");
                parentInventory = inv;
                break;
            }
        }

        // Call Initialize during Awake
        if (parentInventory != null)
        {
            Debug.Log($"Initializing InventoryUI in Awake for {inventoryIdentifier}");
            Initialize(parentInventory);
        }
        else
        {
            Debug.LogWarning($"Parent inventory is null in Awake for {inventoryIdentifier}.");
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
                parentInventory.OnInventoryChanged += OnInventoryChangedHandler;
            }
        }
        else
        {
            Debug.LogWarning("Passed inventory is null during initialization.");
        }
        UpdateUI();
    }

    private void OnInventoryChangedHandler(string identifier)
    {
        if (parentInventory.inventoryIdentifier == identifier)
        {
            UpdateUI();
        }
    }

    private void OnDestroy()
    {
        if (parentInventory != null)
        {
            Debug.Log("Unsubscribing from OnInventoryChanged.");
            parentInventory.OnInventoryChanged -= OnInventoryChangedHandler;
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
        Dictionary<int, InventoryItem> itemsToDisplay = parentInventory != null ? parentInventory.Items : localItems;

        for (int i = 0; i < uiItems.Count; i++)
        {
            if (itemsToDisplay.TryGetValue(i, out InventoryItem item))
            {
                if (uiItems[i] != null)
                {
                    if (item.Amount <= 0)
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

            if (parentInventory == null)
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
                parentInventory.AddItem(slot, item);
            }
        }
    }

    public void AddNewItem(int slot, InventoryItem item)
    {
        if (parentInventory != null)
        {
            parentInventory.AddItem(slot, item);
        }
        else
        {
            if (!localItems.ContainsKey(slot) && localItems.Count < inventorySize)
            {
                UpdateSlot(slot, item);
            }
        }
    }

    public void RemoveItem(int slot)
    {
        if (parentInventory != null)
        {
            parentInventory.RemoveItem(slot);
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
        return parentInventory != null ? parentInventory.Items : localItems;
    }
}
