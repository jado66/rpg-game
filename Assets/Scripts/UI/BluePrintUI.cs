using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BluePrintUI: MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler  {
    public string title;
    public string description;
    public Image icon;

    public TooltipUI tooltip;
                                    
    public Dictionary<string, int> price = new Dictionary<string, int>();

    public BluePrint bluePrint;

    void Awake()
    {
        tooltip = GameObject.Find("Tooltip").GetComponent<TooltipUI>();
        icon = GetComponent<Image>();
        // Debug.Log("Setting item to null");
        this.bluePrint = null;
        LoadBluePrint(this.bluePrint);
        

    }

    public void LoadBluePrint(string key)
    {
        
        icon = GetComponent<Image>();
        
        this.bluePrint = InventoryItemDatabase.bluePrintDatabase[key];
        // Debug.Log("Updating blueprint to"+(this.bluePrint== null?" null":this.bluePrint.title));
        if (this.bluePrint != null)
        {
            // Debug.Log("No blueprint found");
            icon.color = Color.white;
            icon.sprite = bluePrint.icon;

        }
        else
        {
            Debug.Log($"could not find item {key}");
            icon.color = Color.clear;
            
        }
    }
    public void LoadBluePrint(BluePrint bluePrint)
    {
        
        icon = GetComponent<Image>();
        
        this.bluePrint = bluePrint;
        // Debug.Log("Updating blueprint to"+(this.bluePrint== null?" null":this.bluePrint.title));
        if (this.bluePrint != null)
        {
            transform.parent.GetChild(0).gameObject.SetActive(true);
            Debug.Log("No blueprint found");
            icon.color = Color.white;
            icon.sprite = bluePrint.icon;
            

        }
        else
        {
            icon.color = Color.clear;
            transform.parent.GetChild(0).gameObject.SetActive(false);
            
        }
    }

    public void TryFixBlueprint(){
        // Debug.Log("Trying to fix item");
        // Fix all items

        // Debug.Log("this.item == null = "+(this.item == null?"True":"False"));
        // Debug.Log("spriteImage.color != Color.clear "+(spriteImage.color != Color.clear?"True":"False"));


        if (this.bluePrint == null & icon != null){
            // get item name from sprite
            
            try{
                string spriteName = icon.sprite.name;
                Debug.Log("Fixing item"+spriteName);
            
            
                this.bluePrint = InventoryItemDatabase.bluePrintDatabase[spriteName];
                
                
                // item.amount = (amount.text != "1" ?int.Parse(amount.text):1);
                // this.item = new Item(item);
                // parentInventory.items.Add(this.item);
            }
            catch{
                return;
            }
            
        } 
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (bluePrint == null && icon.sprite != null){
            TryFixBlueprint();
            return;
        }
        if (tooltip==null)
            tooltip = GameObject.Find("Tooltip").GetComponent<TooltipUI>();

        
        tooltip.GenerateTooltip(bluePrint);
        Debug.Log("Enter");
        // transform.parent.GetComponent<Image>().color = new Color(.549f,.549f,.549f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (bluePrint == null){
            return;
        }
        tooltip.HideTooltip();
        Debug.Log("Exit");
        // transform.parent.GetComponent<Image>().color = new Color(.2627f,.2627f,.2627f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BluePrintUI[] buildSlots = FindObjectsOfType<BluePrintUI>();
        foreach( var buildSlot in buildSlots){
            buildSlot.transform.parent.GetComponent<Image>().color = new Color(.2627f,.2627f,.2627f); 
        }
        transform.parent.GetComponent<Image>().color = new Color(.549f,.549f,.549f);   
        GameObject.Find("Player1").GetComponent<CharacterBuilding>().activeBlueprint = this.bluePrint;
        
        }

        
}
