using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StoreItem : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {
    public Item item;

    private Player player;
    public Image spriteImage;
    private UIItem selectedItem;
    private StoreItem selectedStoreItem;

    private StoreUiInventory storeUiInventory;

    public List<Item> parentList;
    public Tooltip tooltip;

    public GameObject textAmount;

    public Text amount;

    Inventory parentInventory;
    void Awake()
    {
        player = GameObject.Find("Character").GetComponent<Player>();
        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
        selectedStoreItem = GameObject.Find("SelectedStoreItem").GetComponent<StoreItem>();
        storeUiInventory = GameObject.Find("BuyNSellPanel").GetComponent<StoreUiInventory>();
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
        spriteImage = GetComponent<Image>();
        // Debug.Log("Setting item to null");
        UpdateItem(null);
    }

    public void setParent(Inventory parent){
        this.parentInventory = parent;
    }
    public void UpdateAmount(){
        if (item == null)
            return;
        if (this.item.amount >1){
                textAmount.SetActive(true);
                amount.text = this.item.amount.ToString();
            }
        else
            textAmount.SetActive(false);

        if (item.amount ==0){
            UpdateItem(null);
        }
    }
    public void UpdateItem(Item item)
    {
        // Debug.Log("Updating item to"+(item== null?" null":item.title));
        this.item = item;
        if (this.item != null)
        {
            spriteImage.color = Color.white;
            spriteImage.sprite = item.icon;
            UpdateAmount();

        }
        else
        {
            textAmount.SetActive(false);
            spriteImage.color = Color.clear;
            
        }
    }

    public void TryFixItem(){
        // Debug.Log("Trying to fix item");
        // Fix all items

        // Debug.Log("this.item == null = "+(this.item == null?"True":"False"));
        // Debug.Log("spriteImage.color != Color.clear "+(spriteImage.color != Color.clear?"True":"False"));


        if (this.item == null & spriteImage != null){
            // get item name from sprite
            
            try{
                string spriteName = spriteImage.sprite.name;
                Debug.Log("Fixing item"+spriteName);
            
            
                Item item = ItemDatabase.GetItem(spriteName);
                
                
                item.amount = (amount.text != "1" ?int.Parse(amount.text):1);
                this.item = new Item(item);
                parentInventory.items.Add(this.item);
            }
            catch{
                return;
            }
            
        } 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        selectedItem.TryFixItem();


        if (this.item != null)
        {
            
            if (player.money < item.value)
                return;
            if (selectedItem.item != null) //selling whats on hand and grabbing store item
            {
                Item clone = new Item(selectedItem.item);
                player.money+=selectedItem.item.value*selectedItem.item.amount;
                try{
                    storeUiInventory.merchant.money-=selectedItem.item.value*selectedItem.item.amount;
                }
                catch{
                    Debug.Log("Missing merchant maybe");
                }
                selectedStoreItem.UpdateItem(this.item);
                selectedItem.UpdateItem(null);

                // Give player whatever money the selected item was worth

                UpdateItem(clone);
            }
            else if (selectedStoreItem.item != null) //swapping store item
            {
                Item clone = new Item(selectedStoreItem.item);
                selectedItem.UpdateItem(null);
                selectedStoreItem.UpdateItem(this.item);
                UpdateItem(clone);
            }
            else // grabbing store item
            {
                selectedStoreItem.UpdateItem(this.item);
                selectedItem.UpdateItem(null);
                UpdateItem(null);
            }
        }
        else if (selectedItem.item != null) // selling whats on hand
        {
            
            player.money+=selectedItem.item.value*selectedItem.item.amount;
            try{
                storeUiInventory.merchant.money-=selectedItem.item.value*selectedItem.item.amount;
            }
            catch{
                Debug.Log("Missing merchant maybe");
            }
            // Give player whatever money the selected item was worth
            UpdateItem(selectedItem.item);
            selectedItem.UpdateItem(null);
        }
        else if (selectedStoreItem.item != null) // putting back store item
        {
            UpdateItem(selectedStoreItem.item);
            selectedStoreItem.UpdateItem(null);
            selectedItem.UpdateItem(null);
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       if (this.spriteImage.color != Color.clear)
       {
        //    if (this.item == null){
        //        TryFixItem();
        //    }
           //Generate buy tooltip
           tooltip.GenerateBuyTooltip(this.item);
       }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
