using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PlantData{
    public string name;

    public int[] growthProgression; // {Saplings appear, fruits appear, death, }

    public bool isPerrinial; // come back each year


    public bool isPermanent; // like a tree

    public int howFarFromWaterToGrow;



    public PlantData(string name, int[] growthProgression, bool isPerrinial, bool isPermanent, int howFarFromWaterToGrow = 5)
    {
        this.name = name;
        this.growthProgression = growthProgression;
        this.isPerrinial = isPerrinial;
        this.isPermanent = isPermanent;
        this.howFarFromWaterToGrow = howFarFromWaterToGrow;
    }
}

public static class ItemDatabase{
    static List<Item> items = new List<Item>();

    public static List<Item> blacksmithItems = new List<Item>();
    public static List<Item> alchemistItems = new List<Item>();
    public static List<Item> farmerItems = new List<Item>();
    public static List<Item> craftsmanItems = new List<Item>();
    public static List<Item> collectorItems = new List<Item>();

    public static List<Item> generalItems = new List<Item>();


    public static Dictionary<string, PlantData> plantDatabase;
    public static Dictionary<string, BluePrint> bluePrintDatabase;

    // public static int value;

    static ItemDatabase()
    {
        BuildDatabase();
        BuildPlantDatabase();
        BuildBluePrintDatabase();
        BuildStoreSellItems(); 
    }

    public static Item GetItem(int id)
    {
        return items.Find(item=> item.id == id);
    }

    public static Item GetItem(string itemName)
    {
        return items.Find(item => item.title == itemName);
    }

    static void BuildDatabase()
    {
        var tool = Item.Uses.tool;
        var interactable = Item.Uses.interactable; // Used when pressing interact button
        var consumable = Item.Uses.consumable; // Used when pressing drink/eat button
        var wearable = Item.Uses.wearable; // Can equipt... I might want to make this more complicated...
        var regularItem = Item.Uses.regularItem;

        items = new List<Item>() {
            // Resources
            new Item(0, "Wood", "Not the morning kind",5,regularItem,
            new Dictionary<string, int> {
                
            },10,
            new List<string>(){},new List<string>(){}),
            new Item(1, "Rock", "Not the morning kind",15,regularItem,
            new Dictionary<string, int> {
                
            },15,new List<string>(){},new List<string>(){}),
            new Item(2, "Iron Ore", "Let's pump some",15,regularItem,
            new Dictionary<string, int> {
                
            },350,new List<string>(){},new List<string>(){}),
            new Item(3, "Gold Ore", "Oooh ahh",15,regularItem,
            new Dictionary<string, int> {
                
            },750,new List<string>(){},new List<string>(){}),
            new Item(4, "Coins", "Mooooooneeey",99,regularItem,
            new Dictionary<string, int> {
                
            },10,new List<string>(){},new List<string>(){}),
            new Item(5, "Tree Sapling", "A baby tree",99,interactable,
            new Dictionary<string, int> {
                
            },16,new List<string>(){},new List<string>(){}),
            // new Item(6, "Apple Tree Sapling", "A baby apple tree",99,interactable,
            // new Dictionary<string, int> {
                
            // },new List<string>(){},new List<string>(){}),
            new Item(7, "Bush Sapling", "A baby bush",99,interactable,
            new Dictionary<string, int> {
                
            },13,new List<string>(){},new List<string>(){}),
            new Item(8, "Tomato Seed", "A baby tomato",99,interactable,
            new Dictionary<string, int> {
                
            },5,new List<string>(){},new List<string>(){}),
            new Item(9, "Carrot Seed", "A baby carrot",99,interactable,
            new Dictionary<string, int> {
                
            },5,new List<string>(){},new List<string>(){}),
            // new Item(10, "Unknown Carrot Seed", "You don't know what kind of seed it is",99,interactable,
            // new Dictionary<string, int> {
                
            // },1,"Seed",new List<string>(){},new List<string>(){}),
            // new Item(11, "Unknown Tomato Seed", "You don't know what kind of seed it is",99,interactable,
            // new Dictionary<string, int> {
                
            // },1,"Seed",new List<string>(){},new List<string>(){}),
            // new Item(12, "Unknown Bush Seed", "You don't know what kind of seed it is",99,interactable,
            // new Dictionary<string, int> {
                
            // },1,"Seed",new List<string>(){},new List<string>(){}),
            // new Item(13, "Unknown Sapling Seed", "You don't know what kind of seed it is",99,interactable,
            // new Dictionary<string, int> {
                
            // },1,"Seed",new List<string>(){},new List<string>(){}),
            // new Item(14, "Unknown Apple Tree Seed", "You don't know what kind of seed it is",99,interactable,
            // new Dictionary<string, int> {
                
            // },1,"Seed",new List<string>(){},new List<string>(){}),
            // new Item(15, "Key", "Probably goes to a door",20,interactable,
            // new Dictionary<string, int> {
                
            // },new List<string>(){},new List<string>(){}),
            // },1,"Seed",new List<string>(){},new List<string>(){}),
            new Item(199, "Skull", "Creepers",5,regularItem,
            new Dictionary<string, int> {
                
            },150,new List<string>(){},new List<string>(){}),
            

            
            //Food
            new Item(20, "Carrot", "For horses.",20,consumable,
            new Dictionary<string, int> {
                
            },4,new List<string>(){},new List<string>(){}),
            new Item(21, "Tomato", "Ripe from the vine",20,consumable,
            new Dictionary<string, int> {
                
            },3,new List<string>(){},new List<string>(){}),
            new Item(22, "Mushroom", "Don't get high",40,consumable,
            new Dictionary<string, int> {
                
            },25,new List<string>(){},new List<string>(){}),

            new Item(23, "Apple", "You know what they say",20,consumable,
            new Dictionary<string, int> {

            },5,new List<string>(){},new List<string>(){}),

            new Item(24, "Health Potion L1", "Heals you right up",5,consumable,
            new Dictionary<string, int> {
                {"Heals:",15}
            },150,new List<string>(){},new List<string>(){}, 1,"Health Remedy"),

            new Item(26, "Mana Potion L1", "A what?",5,consumable,
            new Dictionary<string, int> {

            },225,new List<string>(){},new List<string>(){},1,"Magic Elixir"),

            new Item(27, "Stamina Potion L1", "Juicing isn't good for you.",20,consumable,
            new Dictionary<string, int> {

            },225,new List<string>(){},new List<string>(){},1,"Stamina Booster"),

            new Item(28, "Meat", "Yum yum",10,consumable,
            new Dictionary<string, int> {

            },15,new List<string>(){},new List<string>(){}),
            new Item(28, "Egg", "Yum yum",5250,consumable,
            new Dictionary<string, int> {

            },5,new List<string>(){},new List<string>(){}),
            new Item(28, "Feather", "Really light",999,regularItem,
            new Dictionary<string, int> {

            },1,new List<string>(){},new List<string>(){}),
            // Tools
            new Item(33, "Hammer", "Used to construct whatever is in the build menu?",1,tool,
            new Dictionary<string, int> {

            },125,new List<string>(){},new List<string>(){}),
            new Item(34, "Garden Shovel", "For digging gardens",1,tool,
            new Dictionary<string, int> {

            },150,new List<string>(){},new List<string>(){}),
            new Item(35, "Trench Shovel", "For digging trenches",1,tool,
            new Dictionary<string, int> {

            },350,new List<string>(){},new List<string>(){}),
            new Item(36, "Axe", "For chopping all things choppable",1,tool,
            new Dictionary<string, int> {

            },550,new List<string>(){},new List<string>(){}),
            new Item(37, "Pickaxe", "It's off to work",1,tool,
            new Dictionary<string, int> {

            },650,new List<string>(){},new List<string>(){}),
            new Item(38, "Sword", "A sword",1,tool,
            new Dictionary<string, int> {

            },650,new List<string>(){},new List<string>(){}),
            new Item(39, "Shield", "Block attacks",1,tool,
            new Dictionary<string, int> {

            },650,new List<string>(){},new List<string>(){}),
            new Item(39, "Fishpole", "Catches fish",1,tool,
            new Dictionary<string, int> {

            },650,new List<string>(){},new List<string>(){}),
            new Item(39, "Iron Helmet", "Protects your head",1,wearable,
            new Dictionary<string, int> {

            },650,new List<string>(){},new List<string>(){}),
            new Item(39, "Iron Chestplate", "Protects your chest",1,wearable,
            new Dictionary<string, int> {

            },650,new List<string>(){},new List<string>(){}),
            new Item(39, "Iron Gauntlets", "Protects your gauntlets",1,wearable,
            new Dictionary<string, int> {

            },650,new List<string>(){},new List<string>(){}),
            new Item(39, "Iron Boots", "Protects your toes",1,wearable,
            new Dictionary<string, int> {

            },650,new List<string>(){},new List<string>(){}),
            new Item(39, "Ruby Ring", "Protects your finger",1,wearable,
            new Dictionary<string, int> {

            },650,new List<string>(){},new List<string>(){}),
        };
    }

    static void BuildBluePrintDatabase(){
        bluePrintDatabase = new Dictionary<string, BluePrint>();

        bluePrintDatabase.Add("Fence",new BluePrint("Fence","Something you build",new Dictionary<string, int>(){
            {"Wood",1}},new string[1]{"Gate"}));
        bluePrintDatabase.Add("Cobblestone Path",new BluePrint("Cobblestone Path","A path",new Dictionary<string, int>(){
            {"Rock",1}}));
        bluePrintDatabase.Add("Sign",new BluePrint("Sign","A marker you can cusomize",new Dictionary<string, int>(){
            {"Wood",1}}));
        bluePrintDatabase.Add("Gate",new BluePrint("Gate","A fence door",new Dictionary<string, int>(){
            {"Wood",1}},new string[1]{"Fence"}));
        bluePrintDatabase.Add("Sack",new BluePrint("Sack","For potatoes",new Dictionary<string, int>(){
            {"Wood",1}}));
        // bluePrintDatabase.Add("Crate",new BluePrint("Crate","Hold my fruit",new Dictionary<string, int>(){
        //     {"Wood",1}}));
        bluePrintDatabase.Add("Chest",new BluePrint("Chest","Yes indeed",new Dictionary<string, int>(){
            {"Wood",3},
            {"Iron Ore",2}}));
        bluePrintDatabase.Add("Combat Dummy",new BluePrint("Combat Dummy","Violence is okay.",new Dictionary<string, int>(){
            {"Wood",3},
            {"Iron Ore",3}}));
        bluePrintDatabase.Add("Torch x 3",new BluePrint("Torch x 3","Helps this with the dark.",new Dictionary<string, int>(){
            {"Wood",3},
            {"Iron Ore",3}}));
        bluePrintDatabase.Add("Single Bed",new BluePrint("Single Bed","I'm so lonely.",new Dictionary<string, int>(){
            {"Wood",3},
            {"Iron Ore",3}}));
        bluePrintDatabase.Add("Double Bed",new BluePrint("Double Bed","I'm so happy.",new Dictionary<string, int>(){
            {"Wood",3},
            {"Iron Ore",3}}));
        bluePrintDatabase.Add("Rug",new BluePrint("Rug","So soft",new Dictionary<string, int>(){
            {"Wood",3},
            {"Iron Ore",3}}));
        bluePrintDatabase.Add("Chair",new BluePrint("Chair","Sit on me please",new Dictionary<string, int>(){
            {"Wood",3},
            {"Iron Ore",3}}));
        bluePrintDatabase.Add("Table",new BluePrint("Table","Sit on me please",new Dictionary<string, int>(){
            {"Wood",3},
            {"Iron Ore",3}}));
        bluePrintDatabase.Add("Decorated Table",new BluePrint("Decorated Table","Sit on me please",new Dictionary<string, int>(){
            {"Wood",3},
            {"Iron Ore",3}}));
    }
    static void BuildPlantDatabase(){
        plantDatabase = new Dictionary<string, PlantData>();

        plantDatabase.Add("tomato",new PlantData("Tomato",new int[]{1,2,3},false,false,4));
        plantDatabase.Add("carrot",new PlantData("Carrot",new int[]{1,2,3},false,false));
        plantDatabase.Add("bushSapling",new PlantData("Bush",new int[]{1,2,3},false,true,0));
        plantDatabase.Add("appleTreeSapling",new PlantData("AppleTreeSapling",new int[]{1,2,3},false,true,0));
        plantDatabase.Add("treeSapling",new PlantData("TreeSapling",new int[]{1,2,3},false,true,0));
        plantDatabase.Add("appleTree",new PlantData("AppleTree",new int[]{-1,3,-1,},true,true,0));
    }

    static void BuildStoreSellItems(){
        
        // Blacksmith
        blacksmithItems.Add(GetItem("Axe"));
        blacksmithItems.Add(GetItem("Hammer"));
        blacksmithItems.Add(GetItem("Garden Shovel"));
        blacksmithItems.Add(GetItem("Trench Shovel"));
        blacksmithItems.Add(GetItem("Pickaxe"));
        // blacksmithItems.Add(GetItem(""));
        // blacksmithItems.Add(GetItem(""));
        // blacksmithItems.Add(GetItem(""));
        // blacksmithItems.Add(GetItem(""));
        // blacksmithItems.Add(GetItem(""));
        // blacksmithItems.Add(GetItem(""));


        // Alchemist
        // alchemistItems.Add(GetItem(""));
        // alchemistItems.Add(GetItem(""));
        // alchemistItems.Add(GetItem(""));
        // alchemistItems.Add(GetItem(""));
        // alchemistItems.Add(GetItem(""));
        // alchemistItems.Add(GetItem(""));
        // alchemistItems.Add(GetItem(""));

        // Farmer
        farmerItems.Add(GetItem("Tomato Seed"));
        farmerItems.Add(GetItem("Carrot Seed"));
        farmerItems.Add(GetItem("Bush Sapling"));
        farmerItems.Add(GetItem("Tree Sapling"));
        farmerItems.Add(GetItem("Garden Shovel"));
        farmerItems.Add(GetItem("Trench Shovel"));
        farmerItems.Add(GetItem("Apple"));
        farmerItems.Add(GetItem("Carrot"));
        farmerItems.Add(GetItem("Tomato"));

        // Craftsman
        craftsmanItems.Add(GetItem("Hammer"));
        craftsmanItems.Add(GetItem("Rock"));
        craftsmanItems.Add(GetItem("Wood"));
        craftsmanItems.Add(GetItem("Iron Ore"));
        craftsmanItems.Add(GetItem("Gold Ore"));
        // craftsmanItems.Add(GetItem(""));
        // craftsmanItems.Add(GetItem(""));
        // craftsmanItems.Add(GetItem(""));
        // craftsmanItems.Add(GetItem(""));

        // Collector
        // collectorItems.Add(GetItem(""));
        // collectorItems.Add(GetItem(""));
        // collectorItems.Add(GetItem(""));
        // collectorItems.Add(GetItem(""));
        // collectorItems.Add(GetItem(""));
        // collectorItems.Add(GetItem(""));
        // collectorItems.Add(GetItem(""));
        // collectorItems.Add(GetItem(""));
        // collectorItems.Add(GetItem(""));

        // General
        generalItems.Add(GetItem("Mushroom"));
        // generalItems.Add(GetItem(""));
        // generalItems.Add(GetItem(""));
        // generalItems.Add(GetItem(""));
        // generalItems.Add(GetItem(""));
        // generalItems.Add(GetItem(""));
        // generalItems.Add(GetItem(""));
        // generalItems.Add(GetItem(""));
        // generalItems.Add(GetItem(""));
        
        

    }
}
