using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItem : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {
    
    private Player player;

    PlayerUI playerUi;
    public Item item;
    public Image spriteImage;
    public UIItem selectedItem;
    private StoreItem selectedStoreItem;

    public List<Item> parentList;
    public TooltipUI tooltip;

    public GameObject textAmount;

    public Text amount;

    Inventory parentInventory;
    void Awake()
    {
        transform.parent.GetComponent<RectTransform>().localScale = new Vector3(1.25f,1.25f,1.25f);

        playerUi = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
        player = GameObject.Find("Character").GetComponent<Player>();
        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
        selectedStoreItem = GameObject.Find("SelectedStoreItem").GetComponent<StoreItem>();
        tooltip = GameObject.Find("TooltipUI").GetComponent<TooltipUI>();
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
        selectedStoreItem.TryFixItem();
        // Debug.Log("Pressed Down"+(this.item == null?" on empty":"on "+this.item.title));
        if (this.item != null)
        {
            Debug.Log("Pressed Down");
            if (selectedItem.item != null) // Slot full and item in hand
            {
                if (selectedItem.item.title == this.item.title){
                    int currentAmount = this.item.amount;
                    int addedAmount = selectedItem.item.amount;
                    if (currentAmount+addedAmount>this.item.stackAmount){
                        
                        this.item.amount = this.item.stackAmount;
                        selectedItem.item.amount -= currentAmount+addedAmount-this.item.stackAmount;
                        selectedItem.UpdateAmount();
                    }
                    else{
                        this.item.amount += addedAmount;
                        selectedItem.UpdateItem(null);
                    }
                    UpdateAmount();
                    // parentInventory.GiveItem(selectedItem.item.title,false,selectedItem.item.amount);
                    
                }
                else{
                    Item clone = new Item(selectedItem.item);
                    selectedItem.UpdateItem(this.item);
                    UpdateItem(clone);
                }
            }
            else if (selectedStoreItem.item != null) // Buying store item but grabbing item
            {
                // Subtract player money
                player.money -= selectedStoreItem.item.value;
                playerUi.storeUi.merchant.money += selectedStoreItem.item.value;
                Item clone = new Item(selectedStoreItem.item);
                selectedItem.UpdateItem(this.item);
                selectedStoreItem.UpdateItem(null);
                UpdateItem(clone);
            }
            else
            {
                selectedItem.UpdateItem(this.item);
                UpdateItem(null);
            }
        }
        else if (selectedItem.item != null)
        {
            UpdateItem(selectedItem.item);
            selectedItem.UpdateItem(null);
        }
        else if (selectedStoreItem.item != null)
        {
            player.money -= selectedStoreItem.item.value;
            try{
                playerUi.storeUi.merchant.money += selectedStoreItem.item.value;
            }
            catch{
                Debug.Log("Missing merchant maybe");
            }
            UpdateItem(selectedStoreItem.item);
            selectedStoreItem.UpdateItem(null);
        }
        // else if (selectedItem.item == null){
        //     Debug.Log("This item is null");
        //     TryFixItem();

        //     if (this.item != null && selectedItem.item != null){
        //         selectedItem.UpdateItem(this.item);
        //         UpdateItem(null);
        //     }
        // }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.spriteImage.color != Color.clear)
        {
            if (this.item == null){
                TryFixItem();
                player.FixInventory();
            }
            // if (playerUi.buyNSellMenu.activeSelf) // if store is open generate sell tooltip
            //     // tooltip.GenerateSellTooltip(this.item);
            // else
            //     tooltip.GenerateTooltipUI(this.item);
        }
    }

    public void UpdateItemFromString(string itemTitle)
    {
        Item item = ItemDatabase.GetItem(itemTitle);
        if (item != null)
        {
            Debug.Log($"Updating item to {item.title}");
            UpdateItem(item);
        }
        else
        {
            Debug.LogWarning($"Item with title '{itemTitle}' not found in database.");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }

    public override string ToString() {
        return $"UIItem: {item?.title ?? "None"}, Amount: {item?.amount ?? 0}";
    }
}
