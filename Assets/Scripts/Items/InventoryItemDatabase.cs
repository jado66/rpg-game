using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventoryItemDatabase
{
    static List<InventoryItem> items = new List<InventoryItem>();

    public static List<InventoryItem> blacksmithItems = new List<InventoryItem>();
    public static List<InventoryItem> alchemistItems = new List<InventoryItem>();
    public static List<InventoryItem> farmerItems = new List<InventoryItem>();
    public static List<InventoryItem> craftsmanItems = new List<InventoryItem>();
    public static List<InventoryItem> collectorItems = new List<InventoryItem>();
    public static List<InventoryItem> generalItems = new List<InventoryItem>();

    public static Dictionary<string, PlantData> plantDatabase;
    public static Dictionary<string, BluePrint> bluePrintDatabase;

    static InventoryItemDatabase()
    {
        BuildDatabase();
        BuildPlantDatabase();
        BuildBluePrintDatabase();
        BuildStoreSellItems();
    }

    public static InventoryItem GetItemByID(string id)
    {
        return items.Find(item => item.Id == id);
    }

    public static InventoryItem GetItem(string itemName)
    {
        return items.Find(item => item.Name == itemName);
    }

    static void BuildDatabase()
    {
        var tool = InventoryItem.Uses.Tool;
        var interactable = InventoryItem.Uses.Interactable;
        var consumable = InventoryItem.Uses.Consumable;
        var wearable = InventoryItem.Uses.Wearable;
        var regularItem = InventoryItem.Uses.RegularItem;

        items = new List<InventoryItem> {
            new RegularItem("0", "Wood", "Not the morning kind", 99, regularItem, new Dictionary<string, int>(), 10, new List<string>(), new List<string>()),
            new RegularItem("1", "Rock", "Not the morning kind", 99, regularItem, new Dictionary<string, int>(), 15, new List<string>(), new List<string>()),
            new RegularItem("2", "Iron Ore", "Let's pump some", 99, regularItem, new Dictionary<string, int>(), 350, new List<string>(), new List<string>()),
            new RegularItem("3", "Gold Ore", "Oooh ahh", 99, regularItem, new Dictionary<string, int>(), 750, new List<string>(), new List<string>()),
            new RegularItem("4", "Coins", "Mooooooneeey", 999, regularItem, new Dictionary<string, int>(), 10, new List<string>(), new List<string>()),
            new InteractableItem("5", "Tree Sapling", "A baby tree", 99, interactable, new Dictionary<string, int>(), 16, new List<string>(), new List<string>()),
            new InteractableItem("7", "Bush Sapling", "A baby bush", 99, interactable, new Dictionary<string, int>(), 13, new List<string>(), new List<string>()),
            new InteractableItem("8", "Tomato Seed", "A baby tomato", 99, interactable, new Dictionary<string, int>(), 5, new List<string>(), new List<string>()),
            new InteractableItem("9", "Carrot Seed", "A baby carrot", 99, interactable, new Dictionary<string, int>(), 5, new List<string>(), new List<string>()),
            // new ConsumableItem("20", "Carrot", "For horses.", 20, consumable, new Dictionary<string, int>(), 4, new List<string>(), new List<string>()),
            // new ConsumableItem("21", "Tomato", "Ripe from the vine", 20, consumable, new Dictionary<string, int>(), 3, new List<string>(), new List<string>()),
            // new ConsumableItem("22", "Mushroom", "Don't get high", 40, consumable, new Dictionary<string, int>(), 25, new List<string>(), new List<string>()),
            // new ConsumableItem("23", "Apple", "You know what they say", 20, consumable, new Dictionary<string, int>(), 5, new List<string>(), new List<string>()),
            // new ConsumableItem("24", "Health Potion L1", "Heals you right up", 5, consumable, new Dictionary<string, int>{{"Heals:", 15}}, 150, new List<string>(), new List<string>(), 1,"Health Remedy"),
            // new ConsumableItem("26", "Mana Potion L1", "A what?", 5, consumable, new Dictionary<string, int>(), 225, new List<string>(), new List<string>(), 1,"Magic Elixir"),
            // new ConsumableItem("27", "Stamina Potion L1", "Juicing isn't good for you.", 20, consumable, new Dictionary<string, int>(), 225, new List<string>(), new List<string>(), 1,"Stamina Booster"),
            // new ConsumableItem("28", "Meat", "Yum yum", 10, consumable, new Dictionary<string, int>(), 15, new List<string>(), new List<string>()),
            // new ConsumableItem("29", "Egg", "Yum yum", 525, consumable, new Dictionary<string, int>(), 5, new List<string>(), new List<string>()),
            new RegularItem("30", "Feather", "Really light", 999, regularItem, new Dictionary<string, int>(), 1, new List<string>(), new List<string>()),
            new ToolItem("33", "Hammer", "Used to construct whatever is in the build menu?", 1, tool, new Dictionary<string, int>(), 125, new List<string>(), new List<string>()),
            new ToolItem("34", "Garden Shovel", "For digging gardens", 1, tool, new Dictionary<string, int>(), 150, new List<string>(), new List<string>()),
            new ToolItem("35", "Trench Shovel", "For digging trenches", 1, tool, new Dictionary<string, int>(), 350, new List<string>(), new List<string>()),
            new ToolItem("36", "Axe", "For chopping all things choppable", 1, tool, new Dictionary<string, int>(), 550, new List<string>(), new List<string>()),
            new ToolItem("37", "Pickaxe", "It's off to work", 1, tool, new Dictionary<string, int>(), 650, new List<string>(), new List<string>()),
            new ToolItem("38", "Sword", "A sword", 1, tool, new Dictionary<string, int>(), 650, new List<string>(), new List<string>()),
            new ToolItem("39", "Shield", "Block attacks", 1, tool, new Dictionary<string, int>(), 650, new List<string>(), new List<string>()),
            new ToolItem("40", "Fishpole", "Catches fish", 1, tool, new Dictionary<string, int>(), 650, new List<string>(), new List<string>()),
            new WearableItem("41", "Iron Helmet", "Protects your head", 1, wearable, new Dictionary<string, int>(), 650, new List<string>(), new List<string>()),
            new WearableItem("42", "Iron Chestplate", "Protects your chest", 1, wearable, new Dictionary<string, int>(), 650, new List<string>(), new List<string>()),
            new WearableItem("43", "Iron Gauntlets", "Protects your hands", 1, wearable, new Dictionary<string, int>(), 650, new List<string>(), new List<string>()),
            new WearableItem("44", "Iron Boots", "Protects your toes", 1, wearable, new Dictionary<string, int>(), 650, new List<string>(), new List<string>()),
            new WearableItem("45", "Ruby Ring", "Protects your finger", 1, wearable, new Dictionary<string, int>(), 650, new List<string>(), new List<string>())
        };
    }

    static void BuildBluePrintDatabase()
    {
        bluePrintDatabase = new Dictionary<string, BluePrint>
        {
            { "Fence", new BluePrint("Fence", "Something you build", new Dictionary<string, int> { { "Wood", 1 } }, new string[1] { "Gate" }) },
            { "Cobblestone Path", new BluePrint("Cobblestone Path", "A path", new Dictionary<string, int> { { "Rock", 1 } }) },
            { "Sign", new BluePrint("Sign", "A marker you can customize", new Dictionary<string, int> { { "Wood", 1 } }) },
            { "Gate", new BluePrint("Gate", "A fence door", new Dictionary<string, int> { { "Wood", 1 } }, new string[1] { "Fence" }) },
            { "Sack", new BluePrint("Sack", "For potatoes", new Dictionary<string, int> { { "Wood", 1 } }) },
            { "Chest", new BluePrint("Chest", "Yes indeed", new Dictionary<string, int> { { "Wood", 3 }, { "Iron Ore", 2 } }) },
            { "Combat Dummy", new BluePrint("Combat Dummy", "Violence is okay.", new Dictionary<string, int> { { "Wood", 3 }, { "Iron Ore", 3 } }) },
            { "Torch x 3", new BluePrint("Torch x 3", "Helps with the dark.", new Dictionary<string, int> { { "Wood", 3 }, { "Iron Ore", 3 } }) },
            { "Single Bed", new BluePrint("Single Bed", "I'm so lonely.", new Dictionary<string, int> { { "Wood", 3 }, { "Iron Ore", 3 } }) },
            { "Double Bed", new BluePrint("Double Bed", "I'm so happy.", new Dictionary<string, int> { { "Wood", 3 }, { "Iron Ore", 3 } }) },
            { "Rug", new BluePrint("Rug", "So soft", new Dictionary<string, int> { { "Wood", 3 }, { "Iron Ore", 3 } }) },
            { "Chair", new BluePrint("Chair", "Sit on me please", new Dictionary<string, int> { { "Wood", 3 }, { "Iron Ore", 3 } }) },
            { "Table", new BluePrint("Table", "Sit on me please", new Dictionary<string, int> { { "Wood", 3 }, { "Iron Ore", 3 } }) },
            { "Decorated Table", new BluePrint("Decorated Table", "Sit on me please", new Dictionary<string, int> { { "Wood", 3 }, { "Iron Ore", 3 } }) }
        };
    }

    static void BuildPlantDatabase()
    {
        plantDatabase = new Dictionary<string, PlantData>
        {
            { "tomato", new PlantData("Tomato", new int[] { 1, 2, 3 }, false, false, 4) },
            { "carrot", new PlantData("Carrot", new int[] { 1, 2, 3 }, false, false) },
            { "bushSapling", new PlantData("BushSapling", new int[] { 1, 2, 3 }, false, true, 0) },
            { "appleTreeSapling", new PlantData("AppleTreeSapling", new int[] { 1, 2, 3 }, false, true, 0) },
            { "treeSapling", new PlantData("TreeSapling", new int[] { 1, 2, 3 }, false, true, 0) },
            { "appleTree", new PlantData("AppleTree", new int[] { -1, 3, -1 }, true, true, 0) }
        };
    }

    static void BuildStoreSellItems()
    {
        // Blacksmith
        blacksmithItems.Add(GetItemByID("36")); // Axe
        blacksmithItems.Add(GetItemByID("33")); // Hammer
        blacksmithItems.Add(GetItemByID("34")); // Garden Shovel
        blacksmithItems.Add(GetItemByID("35")); // Trench Shovel
        blacksmithItems.Add(GetItemByID("37")); // Pickaxe

        // Alchemist: Fill in as needed

        // Farmer
        farmerItems.Add(GetItemByID("8")); // Tomato Seed
        farmerItems.Add(GetItemByID("9")); // Carrot Seed
        farmerItems.Add(GetItemByID("7")); // Bush Sapling
        farmerItems.Add(GetItemByID("5")); // Tree Sapling
        farmerItems.Add(GetItemByID("34")); // Garden Shovel
        farmerItems.Add(GetItemByID("35")); // Trench Shovel
        farmerItems.Add(GetItemByID("23")); // Apple
        farmerItems.Add(GetItemByID("20")); // Carrot
        farmerItems.Add(GetItemByID("21")); // Tomato

        // Craftsman
        craftsmanItems.Add(GetItemByID("33")); // Hammer
        craftsmanItems.Add(GetItemByID("1"));  // Rock
        craftsmanItems.Add(GetItemByID("0"));  // Wood
        craftsmanItems.Add(GetItemByID("2"));  // Iron Ore
        craftsmanItems.Add(GetItemByID("3"));  // Gold Ore

        // Collector: Fill in as needed

        // General
        generalItems.Add(GetItemByID("22")); // Mushroom
    }
}

// Definitions for InventoryItem subclasses
public class RegularItem : InventoryItem
{
    public RegularItem(string id, string name, string description, int amount, Uses use, Dictionary<string, int> stats, int value, List<string> strongConsumers, List<string> weakConsumers)
        : base(id, name, description, use, stats, value, strongConsumers, weakConsumers, amount) { }

    public override void Use()
    {
        // Implement regular item usage logic here
    }
}

public class InteractableItem : InventoryItem
{
    public InteractableItem(string id, string name, string description, int amount, Uses use, Dictionary<string, int> stats, int value, List<string> strongConsumers, List<string> weakConsumers)
        : base(id, name, description, use, stats, value, strongConsumers, weakConsumers, amount) { }

    public override void Use()
    {
        // Implement interactable item usage logic here
    }
}

public class ConsumableItem : InventoryItem
{
    public ConsumableItem(string id, string name, string description, int amount, Uses use, Dictionary<string, int> stats, int value, List<string> strongConsumers, List<string> weakConsumers)
        : base(id, name, description, use, stats, value, strongConsumers, weakConsumers, amount) { }

    public override void Use()
    {
        // Implement consumable item usage logic here
    }
}

public class ToolItem : InventoryItem
{
    public ToolItem(string id, string name, string description, int amount, Uses use, Dictionary<string, int> stats, int value, List<string> strongConsumers, List<string> weakConsumers)
        : base(id, name, description, use, stats, value, strongConsumers, weakConsumers, amount) { }

    public override void Use()
    {
        // Implement tool item usage logic here
    }
}

public class WearableItem : InventoryItem
{
    public WearableItem(string id, string name, string description, int amount, Uses use, Dictionary<string, int> stats, int value, List<string> strongConsumers, List<string> weakConsumers)
        : base(id, name, description, use, stats, value, strongConsumers, weakConsumers, amount) { }

    public override void Use()
    {
        // Implement wearable item usage logic here
    }
}