using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

public class CharacterInventoryUI : MonoBehaviour
{
    private Character character;
    private CharacterInventory inventory;

    [SerializeField] private Dictionary<KeyCode, InventoryItem> hotbarSlots = new Dictionary<KeyCode, InventoryItem>();
    [SerializeField] private Dictionary<string, InventoryItem> equipmentSlots = new Dictionary<string, InventoryItem>();
    [SerializeField] private Dictionary<int, InventoryItem> inventorySlots = new Dictionary<int, InventoryItem>();

    public void InitializeComponents(Character characterRef){
        character = characterRef;
        // Initialize equipment slots
        if (equipmentSlots == null)
        {
            equipmentSlots = new Dictionary<string, InventoryItem>();
        }

        // Initialize equipment slots only if they don't already exist
        if (!equipmentSlots.ContainsKey("head")){
            equipmentSlots["head"] = null;
        }
        if (!equipmentSlots.ContainsKey("chest")){
            equipmentSlots["chest"] = null;
        }
        if (!equipmentSlots.ContainsKey("ring")){
            equipmentSlots["ring"] = null;
        }
        if (!equipmentSlots.ContainsKey("pants")){
            equipmentSlots["pants"] = null;
        }
        // Maybe more?

        inventory = character.GetInventory();

        LoadInventoryData();

    }

    public void AssignHotbarSlotsItem(KeyCode key, InventoryItem item)
    {
        if (hotbarSlots.ContainsKey(key))
            hotbarSlots[key] = item;
        else
            hotbarSlots.Add(key, item);
    }

    public InventoryItem GetHotbarItem(KeyCode key)
    {
        if (hotbarSlots.ContainsKey(key))
        {
            return hotbarSlots[key];
        }
        return null;
    }

    public void EquipItem(string slot, InventoryItem item)
    {
        if (equipmentSlots.ContainsKey(slot))
        {
            UnequipItem(slot);
            equipmentSlots[slot] = item;
            ApplyEquipmentProperties(item);
        }
    }

    public void UnequipItem(string slot)
    {
        if (equipmentSlots.ContainsKey(slot))
        {
            InventoryItem currentItem = equipmentSlots[slot];
            if (currentItem != null)
            {
                RemoveEquipmentProperties(currentItem);
            }
            equipmentSlots[slot] = null;
        }
    }

    private void ApplyEquipmentProperties(InventoryItem item)
    {
        Debug.Log("Unequipting");
    }

    private void RemoveEquipmentProperties(InventoryItem item)
    {
        Debug.Log("Equipting");
    }

    public InventoryItem GetInventorySlotItem(int slotIndex)
    {
        if (inventorySlots.ContainsKey(slotIndex))
        {
            return inventorySlots[slotIndex];
        }
        return null;
    }

    public void SaveInventoryData()
    {
        PlayerPrefs.SetInt("InventorySlotCount", inventorySlots.Count);
        int index = 0;
        foreach (var kvp in inventorySlots)
        {
            PlayerPrefs.SetInt("SlotIndex" + index, kvp.Key);
            PlayerPrefs.SetString("SlotItem" + index, kvp.Value.Id); // Assuming InventoryItem has an ItemID property
            index++;
        }
        PlayerPrefs.Save();
    }

    public void LoadInventoryData()
    {
        int slotCount = PlayerPrefs.GetInt("InventorySlotCount", 0);
        inventorySlots.Clear();
        for (int i = 0; i < slotCount; i++)
        {
            int slotIndex = PlayerPrefs.GetInt("SlotIndex" + i);
            string itemId = PlayerPrefs.GetString("SlotItem" + i);
            InventoryItem item = InventoryItemDatabase.GetItemByID(itemId).Clone(); // Assuming you have a way to get a InventoryItem by ID
            inventorySlots[slotIndex] = item;
        }
    }

    public InventoryItem GetEquipmentSlotItem(string slot)
    {
        if (equipmentSlots.ContainsKey(slot))
        {
            return equipmentSlots[slot];
        }
        return null;
    }
    // Make sure to save inventory data when the game exits or scene changes
    private void OnDestroy()
    {
        SaveInventoryData();
    }
}
