using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LootItem
{
    public string itemName;
    public float chance;
    public int minAmount;
    public int maxAmount;
}

public class MonsterLoot : StoreInventory
{
    [SerializeField] private List<LootItem> possibleLoot = new List<LootItem>();

    private System.Random random = new System.Random();

    protected new void Start()
    {

        CharacterUI playerUi = FindObjectOfType<CharacterUI>();

        StoreInventoryGui = playerUi.externalInventoryGui;
        StoreInventoryPanel = playerUi.externalInventoryPanels;
        inventoryUI = playerUi.externalInventoryPanels.GetComponent<InventoryUI>();
    

        base.Start();
        GenerateLoot();
    }

    private void GenerateLoot()
    {
        ClearItems(); // Clear existing items before generating new loot

        foreach (var lootItem in possibleLoot)
        {
            if (random.NextDouble() <= lootItem.chance)
            {
                int amount = random.Next(lootItem.minAmount, lootItem.maxAmount + 1);
                TryAddItem(lootItem.itemName, amount);
            }
        }

        SyncInventory();
    }
}