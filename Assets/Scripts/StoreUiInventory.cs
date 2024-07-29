using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUiInventory : MonoBehaviour {
    public List<StoreItem> uiItems = new List<StoreItem>();
    public GameObject storeSlotPrefab;
    public Transform slotPanel;

    public Merchant merchant;

    public Inventory parentInventory;

    public int inventorySize;

                                        // With top left anchor:  Posx, Posy, Width, Height 
    private float[,] inventoryGuiTransformPoints = new float[4,4]{{134.44f,-87.1f,38.36f,186.75f}, //Stores 5
                                                                  {148.34f,-87.1f,73.12f,186.75f}, //Stores 10
                                                                  {0.1f,0.1f,0.1f,0.1f},
                                                                  {0.1f,0.1f,0.1f,0.1f}};

    void Awake()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject instance = Instantiate(storeSlotPrefab);
            instance.transform.SetParent(slotPanel);
            // instance.GetComponent<UIItem>().setParent(parentInventory);
            uiItems.Add(instance.GetComponentInChildren<StoreItem>());
        }

        // foreach(var item in uiItems){
        //     item.setParent(parentInventory);
        // }
    }

    public void UpdateItemAmounts()
    {
        foreach (var item in uiItems){
            item.UpdateAmount();
        }
    }

    public void UpdateItemAmount(Item item)
    {
        int slot = uiItems.FindIndex(i=> i.item == item);
        uiItems[slot].UpdateAmount();
    }
    public void UpdateSlot(int slot, Item item)
    {
        uiItems[slot].UpdateItem(item);
    }

    public void AddNewItem(Item item)
    {
        UpdateSlot(uiItems.FindIndex(i=> i.item == null), item);
    }

    public void RemoveItem(Item item)
    {
        Debug.Log("Updating slots");
        UpdateSlot(uiItems.FindIndex(i=> i.item == item), null);
    }

    public void UpdateMerchant(Merchant merchant){
        this.merchant = merchant;
    }
}
