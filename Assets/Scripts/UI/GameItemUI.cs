using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameItemUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private CharacterInventoryUI characterInventoryUI;

    public string gameItemName = "New";

    public bool isGhostItem = false; 
    public InventoryItem Item { get; private set; }

    public Image spriteImage;

    public Text amountText;

    public bool isSelected = false; 
    public TooltipUI tooltip;

    public GameItemUI selectedGameItemUI;
    public InventoryItem selectedInventoryItem;

    // public GameItemUI selectedStoreItem;


    void Awake()
    {
        characterInventoryUI = GameObject.FindObjectOfType<CharacterInventoryUI>();
        tooltip = GameObject.Find("Tooltip").GetComponent<TooltipUI>(); // Assuming there's a Tooltip script/component
        SelectedItemUI selectedItemUI = GameObject.Find("SelectedItem").GetComponent<SelectedItemUI>();
        // selectedStoreItem = GameObject.Find("SelectedStoreItem").GetComponent<GameItemUI>();

        selectedGameItemUI = selectedItemUI.selectedGameItemUI;
        selectedInventoryItem = selectedItemUI.selectedInventoryItem;
        spriteImage = GetComponent<Image>();
        
        if (amountText == null)
        {
            amountText = transform.Find("Amount").GetComponent<Text>(); // Assuming there's a Text component for the amount
            amountText.gameObject.SetActive(false);
        }

        UpdateGameItem(null);
    }


    public void UpdateGameItem(InventoryItem item)
    {
        Item = item;

        if (item != null)
        {
            spriteImage.sprite = Resources.Load<Sprite>("Sprites/Items/" + item.Name);
            amountText.text = item.Amount.ToString();
            amountText.gameObject.SetActive(item.Amount > 1); // Show amount text only if amount > 1
            Color imageColor = spriteImage.color;
            imageColor.a = 1.0f; // Set alpha to 1 (fully opaque)
            spriteImage.color = imageColor;
        }
        else
        {
            spriteImage.sprite = null;//Resources.Load<Sprite>("Sprites/Default"); // Set a default placeholder sprite
            amountText.text = "";
            amountText.gameObject.SetActive(false); // Hide amount text
            Color imageColor = spriteImage.color;
            imageColor.a = 0.0f; // Set alpha to 0 (fully transparent)
            spriteImage.color = imageColor;
        }
    }

    public void SelectItem(){
        selectedGameItemUI = this;
        selectedInventoryItem = Item;

        isSelected = true;

        if (spriteImage != null){
                Color imageColor = spriteImage.color;
                imageColor.a = 0.5f; // Set alpha to 0.5 (slightly transparent)
                spriteImage.color = imageColor;
        }
    }

    public void UnselectItem(){
        

        isSelected = false;

        if (spriteImage != null)
        {
            Color imageColor = spriteImage.color;
            imageColor.a = 1.0f; // Set alpha to 1 (fully opaque)
            spriteImage.color = imageColor;
        }
    }

    public void PlaceSelectedItem(){
        UpdateGameItem(selectedInventoryItem);
        selectedGameItemUI.UpdateGameItem(null);        
        selectedGameItemUI.UnselectItem();

        SelectItem();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Item != null){
            SelectItem();
        }
        else if (ReferenceEquals(Item, selectedGameItemUI)){
            selectedGameItemUI = null;
            selectedInventoryItem = null;
            UnselectItem();
        }
        else if (Item == null && selectedInventoryItem != null){
            PlaceSelectedItem();
        }
    }
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item != null)
        {
            tooltip.GenerateTooltip(Item); // Assuming GenerateTooltip method works with GameItem object
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }

    // Methods to set up the UIItem based on data from CharacterInventoryUI
    public void SetHotbarSlot(KeyCode key)
    {
        InventoryItem item = characterInventoryUI.GetHotbarItem(key);
        UpdateGameItem(item);
    }

    public void SetInventorySlot(int slotIndex)
    {
        InventoryItem item = characterInventoryUI.GetInventorySlotItem(slotIndex);
        UpdateGameItem(item);
    }

    public void SetEquipmentSlot(string slot)
    {
        InventoryItem item = characterInventoryUI.GetEquipmentSlotItem(slot);
        UpdateGameItem(item);
    }

    public void UpdateItemAmount()
    {
        if (Item != null)
        {
            int amount = Item.Amount;
            amountText.text = amount.ToString();
            amountText.gameObject.SetActive(amount > 1);
        }
    }
}
