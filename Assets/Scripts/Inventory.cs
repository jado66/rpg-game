using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public List<Item> items = new List<Item>();
    public UIInventory inventoryUI;

    public List<string> startingItems;
    public List<int> startingAmounts;

    void Start()
    {
        for(int i = 0; i<startingItems.Count;i++){
            int amount = startingAmounts[i];
            StartWithItem(startingItems[i],amount);
        }
    }

    public void SetInventoryUI(UIInventory ui) {
        this.inventoryUI = ui;
    }
    public virtual void GiveItem(int id)
    {
        if (items.Count >= inventoryUI.inventorySize){
            Debug.Log("Inventory is full");
        }
        else{
            Item itemToAdd = ItemDatabase.GetItem(id);
            items.Add(itemToAdd);
            if(inventoryUI!= null)
                inventoryUI.AddNewItem(itemToAdd);
            Debug.Log("Added item: " + itemToAdd.title);
        }
    }

    public void StartWithItem(string itemName, int amount ){
        Item itemToAdd = ItemDatabase.GetItem(itemName);
        Item newItem = new Item(itemToAdd);
        newItem.amount = amount;
        items.Add(newItem);
        // Debug.Log("Added "+amount.ToString()+ " " + newItem.title+(amount>1?"s":""));
    }

    public virtual void GiveItem(string itemName, bool wholeStack = false, int amount = 1)
    {

        // Debug.Log("Giving "+amount.ToString()+" "+itemName+" to chest");
        Item itemToAdd = ItemDatabase.GetItem(itemName);

        // Make this do math
        if (amount != 1){
            
            itemToAdd.amount = amount;
            items.Add(itemToAdd);
            if(inventoryUI!= null)
                inventoryUI.AddNewItem(itemToAdd);
        }

        // Check if we can stack items
        List<Item> matches = items.FindAll(item => item.title == itemName);
        
        bool canStack = false;
        foreach (var item in matches){
            if (item.amount <item.stackAmount){
                canStack = true;
                // update item stack
                item.amount ++;   
                if(inventoryUI!= null) 
                    inventoryUI.UpdateItemAmounts();
                Debug.Log("Item amount increased to: " + itemToAdd.title);        
                break;
            }
        }

        if (!canStack){
            if (inventoryUI != null){
                if (items.Count >= inventoryUI.inventorySize){
                    Debug.Log("Inventory is full");
                }
                else{
                    itemToAdd.amount = 1;

                    Item newItem = new Item(itemToAdd);
                    items.Add(newItem);
                    if(inventoryUI!= null)
                    inventoryUI.AddNewItem(newItem);
                    Debug.Log("Added item: " + newItem.title);
                }
            }
            
        }
        
    }

    public string GetStringifiedInventory() {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (var item in items) {
            sb.AppendLine($"Item: {item.title}, Amount: {item.amount}");
        }
        return sb.ToString();
    }

    public bool checkIfItemsExistsAndRemove(string[] titles, int[] amount = null){
        bool allItemsExist = true;     
        foreach(var title in titles)
        {
            allItemsExist = ChickIfItemExists(title);
            if (!allItemsExist)
                return false;
        }
        //Add Item amount
        foreach(var title in titles)
        {
            RemoveItem(title);
        }
        
        return true;
    }
    // public void GiveItem(string itemName)
    // {
    //     Item itemToAdd = itemDatabase.GetItem(itemName);

    //     // Check if we can stack items
    //     bool canStack = false;

    //     var items = items.FindAll(item => item.title == itemName);
    //     List<int> indexes = new List<int>();

    //     foreach (var item in items)
    //     {
    //         indexes.Add(items.IndexOf(item));
    //         Debug.Log(items.IndexOf(item));
    //     }

    //     if (indexes.Count > 0){
    //         foreach (var index in indexes)
    //         {
    //             if (items[index].amount < items[index].stackAmount)
    //             {
    //                 items[index].amount ++;
    //                 inventoryUI.UpdateItemAmounts();
    //                 canStack = true;
    //                 break;
    //             }
    //         }
    //     }
        
    //     if (!canStack){
    //         // itemToAdd.amount = 1;
    //         items.Add(itemToAdd);
    //         inventoryUI.AddNewItem(itemToAdd);
    //         Debug.Log("Added item: " + itemToAdd.title);
    //     }
        
    // }


    public Item CheckForItem(int id)
    {
        return items.Find(item => item.id == id);
    }

    public Item CheckForItem(string name)
    {
        return items.Find(item => item.title == name);
    }

    public bool ChickIfItemExists(string name)
    {
        // Debug.Log(name+((CheckForItem(name) != null)?" does":" ")+" exist in player inventory");
        return CheckForItem(name) != null;
    }

    public void RemoveItem(int id)
    {   // Probably need to get rid of this method
        Debug.Log("Removing Item");
        Item itemToRemove = CheckForItem(id);
        if (itemToRemove != null)
        {
            items.Remove(itemToRemove);
            inventoryUI.RemoveItem(itemToRemove);
            Debug.Log("Removed item: " + itemToRemove.title);
        }
    }

    public void RemoveSpecificSlot(Item itemToRemove){
        for(int i = 0; i< items.Count;i++){
            if (items[i].title == itemToRemove.title && items[i].amount == itemToRemove.amount){
                items.RemoveAt(i);
                break;
            }
        }
    }

    public void overrideInventory(List<Item> newItems){
        items.Clear();
        foreach(var item in newItems){
            items.Add(item);
        }
    }
    public virtual void RemoveItem(string name, bool wholeStack = false)
    {
        Debug.Log("Removing Item");
        List<Item> matches = items.FindAll(item => item.title == name);
        
        Item minItem = null;

        int min = 100;
        foreach (var item in matches){
            if (item.amount < min)
                minItem = item;
        }

        minItem.amount --;
        inventoryUI.UpdateItemAmounts();  
        
        // bool allAreStacked = false;
        
        // if (!wholeStack){
        //     foreach (var item in matches){
        //         if (item.amount == 1){
        //             allAreStacked = true;
        //             // update item stack
        //             item.amount --;    
        //             inventoryUI.UpdateItemAmounts();    
        //             break;
        //         }
        //     }
        // }

        // if (!allAreStacked){
        //     Item itemToRemove = CheckForItem(name);
        //     if (itemToRemove != null)
        //     {
        //         items.Remove(itemToRemove);

        //         inventoryUI.RemoveItem(itemToRemove);
        //         Debug.Log("Removed item: " + itemToRemove.title);
                
                
        //     }
        // }

        // bool allAreStacked = true;
        


        // if (!wholeStack){
        //     foreach (var item in matches){
        //         if (item.amount == 1){
        //             allAreStacked = false;
        //             // update item stack
        //             Item itemToRemove = CheckForItem(name);
        //             items.Remove(itemToRemove);
        //             inventoryUI.RemoveItem(itemToRemove);    
        //             break;
        //         }
        //     }
        // }

        // if (!allAreStacked){
        //     Item itemToRemove = CheckForItem(name);
        //     if (itemToRemove != null)
        //     {
        //         foreach (var item in matches){
        //             if (item.amount == 1){
        //                 allAreStacked = true;
        //                 // update item stack
        //                 item.amount --;    
        //                 inventoryUI.UpdateItemAmounts();    
        //                 break;
        //             }
        //         }
        //     }
        // }
    }
}
