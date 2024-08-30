using UnityEngine;
using UnityEngine.UI;

public abstract class ItemUI : MonoBehaviour
{
    public CharacterInventory parentInventory;
    public InventoryItem Item { get; protected set; }
    public int slotIndex;
    public Image spriteImage;
    public Text amountText;
    public bool isSelected = false;
    public TooltipUI tooltip;
    public Character character;

    protected AddressableAudioPlayer effectsPlayer;
    protected string ClickSoundUrl = "Assets/SFX/Effects/UI/click3.wav";

    public Image background;

    protected virtual void Awake()
    {
        InitializeComponents();
    }

    protected virtual void InitializeComponents()
    {
        tooltip = GameObject.Find("Tooltip").GetComponent<TooltipUI>();

        spriteImage = GetComponent<Image>();
        
        if (amountText == null)
        {
            amountText = transform.Find("Amount").GetComponent<Text>();
            amountText.gameObject.SetActive(false);
        }

        UpdateGameItem(null, true);

        effectsPlayer = FindObjectOfType<AddressableAudioPlayer>();
        effectsPlayer.PreloadSound(ClickSoundUrl);

        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.raycastTarget = true;
        }
    }

    public void UpdateGameItem(InventoryItem item, bool topDown = false)
    {
        Item = item;

        if (item != null)
        {
            spriteImage.sprite = Resources.Load<Sprite>("Sprites/Items/" + item.Name);
            amountText.text = item.Amount.ToString();
            amountText.gameObject.SetActive(item.Amount > 1);
            spriteImage.color = new Color(spriteImage.color.r, spriteImage.color.g, spriteImage.color.b, 1.0f);
        }
        else
        {
            spriteImage.sprite = null;
            amountText.text = "";
            amountText.gameObject.SetActive(false);
            spriteImage.color = new Color(spriteImage.color.r, spriteImage.color.g, spriteImage.color.b, 0.0f);
        }

        if (topDown && parentInventory != null)
        {
            parentInventory.SetItem(slotIndex, item);
        }
    }

    public abstract void SelectItem();
    public abstract void UnselectItem();

    public void ToggleSelected()
    {
        if (isSelected)
        {
            UnselectItem();
        }
        else
        {
            SelectItem();
        }
    }

    public virtual void OnItemPointerEnter()
    {
        if (Item != null)
        {
            tooltip.GenerateTooltip(Item);
        }
    }

    public virtual void OnItemPointerExit()
    {
        if (Item != null)
        {
            tooltip.HideTooltip();
        }
    }

    public virtual void UpdateItemAmount()
    {
        if (Item != null)
        {
            int amount = Item.Amount;
            amountText.text = amount.ToString();
            amountText.gameObject.SetActive(amount > 1);
        }
    }

    private void HandleInteraction()
    {
        ToggleSelected();   
        effectsPlayer.PlayAddressableSound(ClickSoundUrl);
    }

   
}