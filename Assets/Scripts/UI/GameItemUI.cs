using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameItemUI : ItemUI
{
    public override void SelectItem()
    {
        if (character == null)
        {
            character = GameObject.FindWithTag("Character").GetComponent<Character>();
        }

        // Buying or trying
        if (character.selectedStoreItem != null){
            Debug.Log("Store item selected");
            if (Item != null){
                character.selectedStoreItem.UnselectItem();
                character.selectedItem = this;
                background.color = new Color(0.89f, 0.6747f, 0.4685f);
                return;
            }

            StoreItemUI storeItem = character.selectedStoreItem;
            if (character.GetMoneyAmount() >= storeItem.Item.Value * storeItem.Item.Amount)
            {
                character.SpendMoney(storeItem.Item.Value * storeItem.Item.Amount);
                UpdateGameItem(storeItem.Item, true);
                storeItem.UpdateGameItem(null);
                character.selectedItem = null;
                character.selectedStoreItem.UnselectItem();
            }
            else
            {
                ToastNotification.Instance.Toast("not-enough-money", "Not enough money");
                Debug.Log("Not enough money to buy this item.");
            }
            return;
        }

        if (character.selectedItem == null)
        {
            Debug.Log("Selected GameItem UI - Unselect Store Item");

            if (Item == null)
            {
                return;
            }

            isSelected = true;
            
            if (character.selectedItem != null)
            {
                character.selectedItem.UnselectItem();
            }
            character.selectedItem = this;

            background.color = new Color(0.89f, 0.6747f, 0.4685f);
        }
        else
        {
            Debug.Log("Selected GameItem UI - Swap");
            SwapItem();
        }
    }

    public void SwapItem()
    {
        InventoryItem selectedItem = character.selectedItem.Item != null ? character.selectedItem.Item.Clone() : null;
        GameItemUI selectedItemUI = character.selectedItem;

        if (Item != null && selectedItem != null && Item.Id == selectedItem.Id)
        {
            int totalAmount = Item.Amount + selectedItem.Amount;
            int maxStackSize = Item.StackAmount;

            if (totalAmount <= maxStackSize)
            {
                Item.Amount = totalAmount;
                UpdateGameItem(Item, true);
                selectedItemUI.UpdateGameItem(null);
            }
            else
            {
                Item.Amount = maxStackSize;
                UpdateGameItem(Item, true);
                selectedItem.Amount = totalAmount - maxStackSize;
                selectedItemUI.UpdateGameItem(selectedItem, true);
            }
        }
        else
        {
            InventoryItem tempItem = Item != null ? Item.Clone() : null;
            UpdateGameItem(selectedItem != null ? selectedItem.Clone() : null, true);
            selectedItemUI.UpdateGameItem(tempItem, true);
        }

        selectedItemUI.UnselectItem();
        
        if (character != null)
        {
            character.selectedItem = null;
        }

        UnselectItem();
    }

    public override void UnselectItem()
    {
        if (character == null)
        {
            character = GameObject.FindWithTag("Character").GetComponent<Character>();
        }
        
        isSelected = false;
        background.color = new Color(0.3301887f, 0.3301887f, 0.3301887f);  
        character.selectedItem = null;
    }  
}