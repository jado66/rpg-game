using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArmourItemUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private CharacterInventoryUI characterInventoryUI;
    public GameItem gameItem;
    public Image spriteImage;
    public Text amountText;
    public TooltipUI tooltip;

    void Awake()
    {
        characterInventoryUI = GameObject.FindObjectOfType<CharacterInventoryUI>();
        tooltip = GameObject.Find("Tooltip").GetComponent<TooltipUI>(); // Assuming there's a Tooltip script/component

        spriteImage = GetComponent<Image>();
        if (amountText == null){
            amountText = transform.Find("Amount").GetComponent<Text>(); // Assuming there's a Text component for the amount
        }

        UpdateGameItem(null);
    }

    public void UpdateGameItem(GameItem item)
    {
        this.gameItem = item;
        if (this.gameItem != null)
        {
            spriteImage.color = Color.white;
            spriteImage.sprite = item.icon;
            amountText.text = this.gameItem.amount.ToString();
            amountText.gameObject.SetActive(this.gameItem.amount > 1);
        }
        else
        {
            amountText.gameObject.SetActive(false);
            spriteImage.color = Color.clear;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameItem != null)
        {
            // Handle item selection logic here
            Debug.Log($"Clicked on item: {gameItem.Name}");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameItem != null)
        {
            tooltip.GenerateTooltip(gameItem); // Assuming GenerateTooltip method works with GameItem object
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }

    // Methods to set up the UIItem based on data from CharacterInventoryUI
    public void SetHotbarSlot(KeyCode key)
    {
        GameItem item = characterInventoryUI.GetHotbarItem(key);
        UpdateGameItem(item);
    }

    public void SetInventorySlot(int slotIndex)
    {
        GameItem item = characterInventoryUI.GetInventorySlotItem(slotIndex);
        UpdateGameItem(item);
    }

    public void SetEquipmentSlot(string slot)
    {
        GameItem item = characterInventoryUI.GetEquipmentSlotItem(slot);
        UpdateGameItem(item);
    }
}
