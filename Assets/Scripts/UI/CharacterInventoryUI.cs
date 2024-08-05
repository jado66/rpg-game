using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

public class CharacterInventoryUI : MonoBehaviour
{
    private Character character;
    private CharacterInventory inventory;

    [SerializeField] private Dictionary<KeyCode, GameItem> hotbarSlots = new Dictionary<KeyCode, GameItem>();
    [SerializeField] private Dictionary<string, GameItem> equipmentSlots = new Dictionary<string, GameItem>();
    [SerializeField] private Dictionary<int, GameItem> inventorySlots = new Dictionary<int, GameItem>();

    public void InitializeComponents(Character characterRef){
        character = characterRef;
        // Initialize equipment slots
        if (equipmentSlots == null)
        {
            equipmentSlots = new Dictionary<string, GameItem>();
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

    public void AssignHotbarSlotsItem(KeyCode key, GameItem item)
    {
        if (hotbarSlots.ContainsKey(key))
            hotbarSlots[key] = item;
        else
            hotbarSlots.Add(key, item);
    }

    public GameItem GetHotbarItem(KeyCode key)
    {
        if (hotbarSlots.ContainsKey(key))
        {
            return hotbarSlots[key];
        }
        return null;
    }

    public void EquipItem(string slot, GameItem item)
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
            GameItem currentItem = equipmentSlots[slot];
            if (currentItem != null)
            {
                RemoveEquipmentProperties(currentItem);
            }
            equipmentSlots[slot] = null;
        }
    }

    private void ApplyEquipmentProperties(GameItem item)
    {
        Debug.Log("Unequipting");
    }

    private void RemoveEquipmentProperties(GameItem item)
    {
        Debug.Log("Equipting");
    }

    public GameItem GetInventorySlotItem(int slotIndex)
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
            PlayerPrefs.SetString("SlotItem" + index, kvp.Value.id); // Assuming GameItem has an ItemID property
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
            GameItem item = GameItemDatabase.GetItemByID(itemId); // Assuming you have a way to get a GameItem by ID
            inventorySlots[slotIndex] = item;
        }
    }

    public GameItem GetEquipmentSlotItem(string slot)
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
