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
                            // id,  name,  description, int stackAmount, Uses use, Dictionary<string, int> stats, int value, List<string> strongConsumers, List<string> weakConsumers, string iconPath = nul            
            new RegularItem("0", "Wood", "Can be used for building", 10, regularItem, new Dictionary<string, int>(), 5, new List<string>(), new List<string>()),
            new RegularItem("1", "Rock", "Not the morning kind", 2, regularItem, new Dictionary<string, int>(), 3, new List<string>(), new List<string>()),
            new RegularItem("2", "Iron Ore", "Let's pump some", 10, regularItem, new Dictionary<string, int>(), 150, new List<string>(), new List<string>()),
            new RegularItem("3", "Gold Ore", "Oooh ahh", 10, regularItem, new Dictionary<string, int>(), 500, new List<string>(), new List<string>()),
            // new RegularItem("4", "Coins", "Mooooooneeey", 999, regularItem, new Dictionary<string, int>(), 10, new List<string>(), new List<string>()),
            new RegularItem("5", "Wild Flower", "A wild flower", 99, regularItem, new Dictionary<string, int>(), 3, new List<string>(), new List<string>()),
            new RegularItem("6", "Pink Wild Flower", "A pink wild flower", 99, regularItem, new Dictionary<string, int>(), 25, new List<string>(), new List<string>()),
            new RegularItem("6", "Purple Wild Flower", "A purple wild flower", 99, regularItem, new Dictionary<string, int>(), 25, new List<string>(), new List<string>()),

            new RegularItem("gem", "Gem", "Gem gem ", 10, regularItem, new Dictionary<string, int>(), 850, new List<string>(), new List<string>()),
            new RegularItem("grass-shard", "Grass Shard", "A rare crystal filled with energy", 750, regularItem, new Dictionary<string, int>(), 10, new List<string>(), new List<string>()),
            new RegularItem("fire-shard", "Fire Shard", "A rare crystal filled with energy", 1250, regularItem, new Dictionary<string, int>(), 10, new List<string>(), new List<string>()),
            new RegularItem("ice-shard", "Ice Shard", "A rare crystal filled with energy", 1250, regularItem, new Dictionary<string, int>(), 10, new List<string>(), new List<string>()),
            new RegularItem("silver-key", "Silver Key", "It must open something.", 1, regularItem, new Dictionary<string, int>(), 10, new List<string>(), new List<string>()),
            new RegularItem("gold-key", "Gold Key", "It must open something.", 1, regularItem, new Dictionary<string, int>(), 10, new List<string>(), new List<string>()),
            new Key("woods-house-key", "Woods House Key", "It must open something.",750,"wk-1"),
            new Key("house-key-2", "Woods Mansion Key", "It must open something.",5500,"wk-2"),
            new Key("house-key-3", "Castle House Key", "It must open something.",3500,"ck-1"),
            new Key("house-key-4", "Castle Mansion Key", "It must open something.",9500,"ck-2"),
            // new Key(id: "woods-house-key",name: "Woods House Key",description: "This key unlocks the house in the woods",keyId: "woods-house"),

            // Seeds
            TerraSeeds.TreeSapling(),
            TerraSeeds.AppleSapling(),
            TerraSeeds.BushSapling(),
            TerraSeeds.TomatoSeed(),
            TerraSeeds.CarrotSeed(),

            // Potions
            HealthPotion.SmallPotion(),
            HealthPotion.MediumPotion(),
            HealthPotion.LargePotion(),
            HealthPotion.FullPotion(),
            StaminaPotion.SmallPotion(),
            StaminaPotion.MediumPotion(),
            StaminaPotion.LargePotion(),
            StaminaPotion.FullPotion(),
            ManaPotion.SmallPotion(),
            ManaPotion.MediumPotion(),
            ManaPotion.LargePotion(),
            ManaPotion.FullPotion(),

            // Torches
            Torch.SmallTorch(),

            // Plants
            GardenPlant.Carrot(),
            GardenPlant.Tomato(),
            GardenPlant.RedMushroom(),
            GardenPlant.PinkMushroom(),
            GardenPlant.WhiteMushroom(),
            GardenPlant.Apple(),
            GardenPlant.UnripeApple(),
            GardenPlant.UnripeApple(),
            new MysteriousPotion(),    // Creating instance of MysteriousPotion
            new NightSpell(),          // Creating instance of NightSpell
            new DaySpell(),
            new GrowthSpell(),
            new IlluminationSpell(),
            new MediumBackpack(),
            new LargeBackpack(),
            new ExtraLargeBackpack(),
            new StrongSpellInventoryExpansion(),
            new SpellInventoryExpansion(),
            new Skull(),
            new Bones(),
            new GrassSlimeball(),
            Money.Coins(),
            // new AstralProjectionScroll(),

            // new ConsumableItem("26", "Mana Potion L1", "A what?", 5, consumable, new Dictionary<string, int>(), 225, new List<string>(), new List<string>(), 1,"Magic Elixir"),
            // new ConsumableItem("27", "Stamina Potion L1", "Juicing isn't good for you.", 20, consumable, new Dictionary<string, int>(), 225, new List<string>(), new List<string>(), 1,"Stamina Booster"),
            // new ConsumableItem("28", "Meat", "Yum yum", 10, consumable, new Dictionary<string, int>(), 15, new List<string>(), new List<string>(), 5),
            // new ConsumableItem("29", "Egg", "Yum yum", 525, consumable, new Dictionary<string, int>(), 5, new List<string>(), new List<string>(), 10),
            Ingredient.Egg(),
            Ingredient.Meat(),
            Ingredient.Feather(),
            FertileEgg.ChickenEgg(),

            // new RegularItem("30", "Feather", "Really light", 999, regularItem, new Dictionary<string, int>(), 1, new List<string>(), new List<string>(), 99),
            new PickaxeItem("31", "Pickaxe", "It's off to work", tool, new Dictionary<string, int>(), 750, new List<string>(), new List<string>(), 1),
            new HoeItem("32", "Hoe", "For tilling ground", tool, new Dictionary<string, int>(), 150, new List<string>(), new List<string>(), 1),
            new AxeItem("33", "Axe", "For chopping all things choppable", tool, new Dictionary<string, int>(), 550, new List<string>(), new List<string>(), 1),
            new ShovelItem("34", "Shovel", "For digging gardens", tool, new Dictionary<string, int>(), 125, new List<string>(), new List<string>(), 1),
            new ToolItem("astral-scroll", "Astral Projection Scroll", "Used to temporarily separate your spirit from body.", 1, tool, new Dictionary<string, int>(), 125, new List<string>(), new List<string>()),
            new ToolItem("35", "Hammer", "Used to construct whatever is in the build menu?", 1, tool, new Dictionary<string, int>(), 125, new List<string>(), new List<string>()),
            
            // Weapons
            Weapon.Sword(),
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
            { "Torch x 3", new BluePrint("Torch x 3", "Helps with the dark.", new Dictionary<string, int> { { "Wood", 3 }, { "Iron Ore", 1 } }) },
            { "Tan Single Bed", new BluePrint("Tan Single Bed", "I'm so lonely.", new Dictionary<string, int> { { "Wood", 8 } }) },
            { "Tan Double Bed", new BluePrint("Tan Double Bed", "I'm so happy.", new Dictionary<string, int> { { "Wood", 15 } }) },
            { "Lavender Single Bed", new BluePrint("Lavender Single Bed", "I'm so lonely.", new Dictionary<string, int> { { "Wood", 8 } }) },
            { "Lavender Double Bed", new BluePrint("Lavender Double Bed", "I'm so happy.", new Dictionary<string, int> { { "Wood", 15 } }) },
            { "Rug", new BluePrint("Rug", "So soft", new Dictionary<string, int> { { "Wood", 3 }, { "Iron Ore", 3 } }) },
            { "Chair", new BluePrint("Chair", "Sit on me please", new Dictionary<string, int> { { "Wood", 3 }, { "Iron Ore", 1 } }) },
            { "Table", new BluePrint("Table", "Sit on me please", new Dictionary<string, int> { { "Wood", 3 }, { "Iron Ore", 3 } }) },
            { "Covered Table", new BluePrint("Covered Table", "Sit on me please", new Dictionary<string, int> { { "Wood", 3 }, { "Iron Ore", 1 } }) }
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
        blacksmithItems.Add(GetItemByID("33")); // Axe
        blacksmithItems.Add(GetItemByID("35")); // Hammer
        blacksmithItems.Add(GetItemByID("32")); // Garden Shovel
        blacksmithItems.Add(GetItemByID("34")); // Trench Shovel
        blacksmithItems.Add(GetItemByID("31")); // Pickaxe

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
    public RegularItem(string id, string name, string description, int stackAmount, Uses use, Dictionary<string, int> stats, int value, List<string> strongConsumers, List<string> weakConsumers, string iconPath = null)
        : base(id, name, description, use, stats, value, strongConsumers, weakConsumers, 1, stackAmount, iconPath) { }

    
}

public class InteractableItem : InventoryItem
{
    public InteractableItem(string id, string name, string description, int stackAmount, Uses use, Dictionary<string, int> stats, int value, List<string> strongConsumers, List<string> weakConsumers, string iconPath = null)
        : base(id, name, description, use, stats, value, strongConsumers, weakConsumers, 1, stackAmount, iconPath) { }

   
}

public class ConsumableItem : InventoryItem
{
    public ConsumableItem(string id, string name, string description, int stackAmount, Uses use, Dictionary<string, int> stats, int value, List<string> strongConsumers, List<string> weakConsumers, string iconPath = null)
        : base(id, name, description, use, stats, value, strongConsumers, weakConsumers, 1, stackAmount, iconPath) { }

   
}

public class ToolItem : InventoryItem
{
    public ToolItem(string id, string name, string description, int stackAmount, Uses use, Dictionary<string, int> stats, int value, List<string> strongConsumers, List<string> weakConsumers, string iconPath = null)
        : base(id, name, description, use, stats, value, strongConsumers, weakConsumers, 1, stackAmount, iconPath) { }

   
}

public class WearableItem : InventoryItem
{
    public WearableItem(string id, string name, string description, int stackAmount, Uses use, Dictionary<string, int> stats, int value, List<string> strongConsumers, List<string> weakConsumers, string iconPath = null)
        : base(id, name, description, use, stats, value, strongConsumers, weakConsumers, 1, stackAmount, iconPath) { }

    
}
