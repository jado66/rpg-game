using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryCategory
{
    public string categoryName;
    public List<string> items;
}
public class PlayerInventory : Inventory {
    public UIInventory hotItemsUI;

    public UIInventory externalInventory;

    public StoreUiInventory storeUiInventory;

    [SerializeField]
    public List<InventoryCategory> inventoryMap = new List<InventoryCategory>();

    void Start()
    {
        // GiveItem("Torch L1",false,5);
        // GiveItem("Health Potion L1",false,5);
        // GiveItem("Mana Potion L1",false,5);
        // GiveItem("Stamina Potion L1",false,5);
        // GiveItem("Garden Shovel");
        // GiveItem("");
        // GiveItem("Carrot Seed", false,99);
        // GiveItem("Tomato Seed",false,99);
        // GiveItem("Bush Sapling",false,99);
        // GiveItem("Tree Sapling",false,99);
        // GiveItem("Pickaxe");
    }

    public override void GiveItem(string itemName, bool wholeStack = false, int amount = 1)
    {
        
        Item itemToAdd = ItemDatabase.GetItem(itemName);
        if (amount != 1){
            itemToAdd.amount = amount;
            items.Add(itemToAdd);
            try{inventoryUI.AddNewItem(itemToAdd);}
                catch(Exception e){
                    Debug.Log(e);
                    hotItemsUI.AddNewItem(itemToAdd);
                }
            return;
        }
        // Check if we can stack items
        List<Item> matches = items.FindAll(item => item.title == itemName);
        
        bool canStack = false;
        foreach (var item in matches){
            if (item.amount <item.stackAmount){
                canStack = true;
                // update item stack
                item.amount ++;    
                inventoryUI.UpdateItemAmounts();
                hotItemsUI.UpdateItemAmounts();
                // Debug.Log("Item amount increased to: " + itemToAdd.title);        
                break;
            }
        }

        if (!canStack){
            if (items.Count >= inventoryUI.inventorySize + hotItemsUI.inventorySize){
                Debug.Log("Inventory is full");
            }
            else{
                // Check if inventory UI is full and if so move to hot items.
                itemToAdd.amount = 1;

                Item newItem = new Item(itemToAdd);
                items.Add(newItem);
                try{inventoryUI.AddNewItem(newItem);}
                catch{
                    // Debug.Log(e);
                    try{hotItemsUI.AddNewItem(newItem);}
                    catch{
                        Debug.Log("Inventory Full or error");
                    }
                }
                // Debug.Log("Added item: " + newItem.title);

            }
        }
        
    }

    public override void RemoveItem(string name, bool wholeStack = false)
    {
        // Debug.Log("Removing Item");
        List<Item> matches = items.FindAll(item => item.title == name);
        
        
        Item minItem = null;

        int min = 100;
        foreach (var item in matches){
            if (item.amount < min)
                minItem = item;
        }

        minItem.amount --;
        inventoryUI.UpdateItemAmounts();
        hotItemsUI.UpdateItemAmounts();

        if (minItem.amount <= 0)
            items.Remove(minItem);
    }
}
