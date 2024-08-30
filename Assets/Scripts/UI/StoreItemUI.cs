using UnityEngine;
using UnityEngine.UI;

public class StoreItemUI : ItemUI
{
    public override void SelectItem()
    {
        if (character == null)
        {
            character = GameObject.FindWithTag("Character").GetComponent<Character>();
        }

        if (Item != null){

            if (character.selectedItem){
                character.selectedItem.UnselectItem();
            }
            if (character.selectedStoreItem){
                character.selectedStoreItem.UnselectItem();
            }

            isSelected = true; 
            character.selectedStoreItem = this;

            background.color = new Color(0.89f, 0.6747f, 0.4685f);
        }
        // Selling
        else if (Item == null && character.selectedItem != null){
            int sellValue = character.selectedItem.Item.Value * character.selectedItem.Item.Amount;
            character.GiveMoney(sellValue);
            // merchant.SpendMoney(sellValue);
            UpdateGameItem(character.selectedItem.Item);
            character.selectedItem.UpdateGameItem(null, true);
            character.selectedItem.UnselectItem();
        }
        
    }

    public override void UnselectItem()
    {
        if (character == null)
        {
            character = GameObject.FindWithTag("Character").GetComponent<Character>();
        }
        
        isSelected = false;
        background.color = new Color(0.3301887f, 0.3301887f, 0.3301887f);  
        character.selectedStoreItem = null;
    }  
}