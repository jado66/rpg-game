using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Merchant : NPC
{
    SceneManager sceneManager;
    public string storeType; 

    Inventory inventory;

    bool storeOpen;

    public int money;

    int level; // I need to level items... yuck

    int baseWealth; // This will determine the size of their inventory, the inventory will populate based
                    // off of their base wealth. (similar to the astriod resource allocation) 

    /* Blacksmith, alchemist, farmer, artison/craftsman, collector/trader 
    Blacksmith
        Sells: Weapons, Armors, and Tools
        Buys at premium price: Wood, Ores, Rock
        Buys at discount: What it sells
    Potions dealer
        Sells: Health potions, mana (or whatever) potions, stamina potions etc
        Buys at premium price: Odd crops, mushrooms, specific animal parts, other oddidies
        Buys at discount: What it sells
    Farmers R Us (title pending)
        Sells: Crops, Seeds of sorts, farming tools, wood, any other farming needs
        Buys at premium price:
        Buys at discount: What it sells
    Construction worker R Us (working title)
        Sells: Certian blueprints, wood, rock, ores (Ingots?), etc
        Buys at premium price: 
        Buys at discount: What it sells
    Oddities and stuff
        Sells: Rare items, shards, odd blueprints, info?
        Buys at premium price: Anything odd
        Buys at discount:
    */  

    // Dialog Options buy/sell - ask a specific question to teach player

    public void Start(){
        inventory = GetComponent<Inventory>();
        player = GameObject.Find("Character").GetComponent<Player>();
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
    }

    public override void onPlayerInteract(){
        // Open dialog box
        
        storeOpen = !storeOpen;
        if (storeOpen)
           sceneManager.playerUI.buyNSellMenuPanel.GetComponent<StoreUiInventory>().merchant = this;
        else 
            sceneManager.playerUI.buyNSellMenuPanel.GetComponent<StoreUiInventory>().merchant = null;

        sceneManager.loadAndUnloadStoreInventory(inventory,storeOpen);
        
    }
    // Types off NPC's

    // Quest/Contract givers
    
    // Buy/Sell Merchants

    // Advice givers

    // Attack player?

    // Follow player

    // Make any npc can do all of the above if they want, mak 

    public void refreshInventory(){
        
        List<Item> storeAppropriateItems;

            switch(storeType){
                case "Blacksmith":
                    storeAppropriateItems = ItemDatabase.blacksmithItems;
                    break;
                case "Alchemist":
                    storeAppropriateItems = ItemDatabase.alchemistItems;
                    break;
                case "Famer":
                    storeAppropriateItems = ItemDatabase.farmerItems;
                    break;
                case "Craftsman":
                    storeAppropriateItems = ItemDatabase.craftsmanItems;
                    break;
                case "General":
                default:
                    storeAppropriateItems = ItemDatabase.generalItems;
                    break;
            }

        // turnover inventory
        foreach(var inventoryItem in inventory.items){
            if (storeAppropriateItems.Find(item => item.title == inventoryItem.title)!=null){
                if (UnityEngine.Random.Range(0.0f,10)<.35f)
                    {
                        //switch it out
                    }
            }
            else{
                if (UnityEngine.Random.Range(0.0f,10)<.85f)
                    {
                        //switch it out
                    }
            }

        for (int i = 0; i <6;i++){
            inventory.GiveItem(storeAppropriateItems[UnityEngine.Random.Range(0,storeAppropriateItems.Count)].title);
        }
            // if item belongs to item type (smaller chance of cycling through say 35%)

            // if item does not belong to item group 75% chance of getting rid of it
            









        }
    }
}
