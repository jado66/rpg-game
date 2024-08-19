using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameItemUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private CharacterInventoryUI characterInventoryUI;

    public CharacterInventory parentInventory;

    public string gameItemName = "New";

    public bool isGhostItem = false; 
    public InventoryItem Item { get; private set; }

    public int slotIndex;

    public Image spriteImage;

    public Text amountText;

    public bool isSelected = false; 
    public TooltipUI tooltip;

    public GameItemUI selectedGameItemUI;
    public InventoryItem selectedInventoryItem;

    public Character character;

    // public GameItemUI selectedStoreItem;
    AddressableAudioPlayer effectsPlayer;

    private string ClickSoundUrl = "Assets/SFX/Effects/UI/click3.wav";


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

        UpdateGameItem(null, true);


        // Sound effects 
        effectsPlayer = Object.FindObjectOfType<AddressableAudioPlayer>();
        effectsPlayer.PreloadSound(ClickSoundUrl);


    }


    public void UpdateGameItem(InventoryItem item, bool topDown = false)
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

        if (topDown && parentInventory != null){
            parentInventory.SetItem(slotIndex,item);
        }
    }

    public void SelectItem(){

        if (character == null){
            character = GameObject.FindWithTag("Character").GetComponent<Character>();
        }


        if (character.selectedItem == null){
            if (Item == null){
                return;
            }

            isSelected = true;

            
            
            if (character.selectedItem != null){
                character.selectedItem.UnselectItem();
            }
            character.selectedItem = this;


            transform.parent.GetComponent<Image>().color = new Color(0.89f,0.6747f,0.4685f);
        }
        else{
            SwapItem();
        }

           
    }

    public void SwapItem()
    {
        InventoryItem selectedItem = character.selectedItem.Item != null ? character.selectedItem.Item.Clone() : null;
        GameItemUI selectedItemUI = character.selectedItem;

        if (Item != null && selectedItem != null && Item.Id == selectedItem.Id)
        {
            // Items are the same type, attempt to stack
            int totalAmount = Item.Amount + selectedItem.Amount;
            int maxStackSize = Item.StackAmount; // Using the StackAmount property



            if (totalAmount <= maxStackSize)
            {
                // Full stack in current slot
                Item.Amount = totalAmount;
                UpdateGameItem(Item, true);
                selectedItemUI.UpdateGameItem(null);
            }
            else
            {
                // Partial stack
                Item.Amount = maxStackSize;
                UpdateGameItem(Item, true);
                selectedItem.Amount = totalAmount - maxStackSize;
                selectedItemUI.UpdateGameItem(selectedItem, true);
            }
        }
        else
        {
            // Different items or one slot is empty, perform regular swap
            InventoryItem tempItem = Item != null ? Item.Clone() : null;
            UpdateGameItem(selectedItem != null ? selectedItem.Clone() : null, true);
            selectedItemUI.UpdateGameItem(tempItem, true);
        }

        selectedItemUI.UnselectItem();
        
        // Update character's selected item reference
        if (character != null)
        {
            character.selectedItem = null;
        }

        UnselectItem();
    }

    public void UnselectItem(){
        isSelected = false;
        transform.parent.GetComponent<Image>().color = new Color(0.3867f,0.3867f,0.3867f);  
        character.selectedItem = null;
 
    }

    public void ToggleSelected(){
        if (isSelected){
            UnselectItem();
        } else {
            SelectItem();
        }
    }

    public void PlaceSelectedItem(){
        UpdateGameItem(selectedInventoryItem, true);
        selectedGameItemUI.UpdateGameItem(null, true);        
        selectedGameItemUI.UnselectItem();

        SelectItem();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        // BluePrintUI[] buildSlots = FindObjectsOfType<BluePrintUI>();
        // foreach( var buildSlot in buildSlots){
            // buildSlot.transform.parent.GetComponent<Image>().color = new Color(.2627f,.2627f,.2627f); 
        // }231, 144, 105
        // Debug.Log("TEST");
        ToggleSelected();   
        effectsPlayer.PlayAddressableSound(ClickSoundUrl);
        // GameObject.Find("Character").GetComponent<Player>().activeBlueprint = this.bluePrint;E79069
        
        }
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item != null)
        {
            // Debug.Log("generating tooltip");
            tooltip.GenerateTooltip(Item); // Assuming GenerateTooltip method works with GameItem object
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Item != null)
        {
            tooltip.HideTooltip();
        }
        
    }

    // Methods to set up the UIItem based on data from CharacterInventoryUI
    public void SetHotbarSlot(KeyCode key)
    {
        InventoryItem item = characterInventoryUI.GetHotbarItem(key);
        UpdateGameItem(item, true);
    }

    public void SetInventorySlot(int slotIndex)
    {
        InventoryItem item = characterInventoryUI.GetInventorySlotItem(slotIndex);
        UpdateGameItem(item, true);
    }

    public void SetEquipmentSlot(string slot)
    {
        InventoryItem item = characterInventoryUI.GetEquipmentSlotItem(slot);
        UpdateGameItem(item, true);
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
