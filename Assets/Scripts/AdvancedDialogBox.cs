using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AdvancedDialogBox : MonoBehaviour
{   
   
    public bool customizable;

    Player player;

    GameObject buyMenu;

    GameObject sellMenu;

    int entityMoney;

    // Merchant type: a blacksmith might buy ore, but not weapons and definetally not carrots
    // Merchant quirks// say a specific merchant like carrots, well he'll pay more for them
    //      this can be a list of specific items with a +/- difference.

    void Start(){
        player = GameObject.Find("Character").GetComponent<Player>();
    }
    
    public void onBuyOptionClicked(){
        buyMenu.SetActive(true);
    }
    public void onSellOptionClicked(){
        sellMenu.SetActive(true);
    }

    public bool onSellAttempt(){
        // item is what and item value is what
        
        int itemValue = 0; 
        if (entityMoney > itemValue){ // also check that item is aligned to merchant type.
            entityMoney -= itemValue;
            return true;
            //remove item and give item
        }
        else{
            return false;
        }
    }
    public bool onBuyAttempt(){
        // item is what and item value is what
        
        int itemValue = 0; 
        if (player.money > itemValue){ 
            player.money -= itemValue;
            return true;
            //remove item and give item
        }
        else{
            return false;
        }
    }

    public void replenishStock(){
        // using type of merchant
        // we need a score for how valueable the items are for example a 10 star merchant wont be selling rock but will be selling the most precious ores

        // randomize items based on type and merchant score
        // randomly keep some old items, so if you sell a *** ton of carrots there is a chance they still have them in stock.
        
        // there is a probability to get rarer things in the stock

        // using the quirks we can add more of the negatives, so if carrots are cheaper to sell here they probably have a lot of carrots


        // blacksmith sells weapons and armor etc and buys ores/wood and hides at a good price and weapons and hides at a discounted price

        // farm store sells seeds and buys seeds at a discounted price

        // general store buys everything general at a discounted price

        // contractors (new name please) will contract with you a specific yield for a specific price, failing to provide adequate yeild results in a 
        //          a much smaller to no reward, and the trust they have for you will go down so they will offer contracts less often or smaller contracts 
        //          until trust is gained back. There will be no limit to who can be contractors. The blacksmith might ask you for metal, or a general store
        //          manager might ask you to retrive a small amount of a specific item/items. These can be considered quests. 

    }

    /*

    The buy menu should be have the following setup a potentially horizontal inventory,
    where the player can hover their cursor over items, a tooltip will show the item and the price with description

    picking up the item costs nothing until it is placed in one's inventory.  (For this to work i need picking things up
    in the inventory to delete them from the inventory and placing them adds them)

    the sell menu might look different? Not quite sure how but when the player hovers over items in his invenotry
    the tooltip will display the sell value to the specific vendor. The prices may be different. For certain items
    the vendors will act like trash cans, (ie. a blacksmith will accept carrots and give nothing in return)  

    if the items are ones that the vendor sells they will apear in their inventory for a temporary time.  



    */


}
