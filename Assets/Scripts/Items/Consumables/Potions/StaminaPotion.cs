using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPotion : InventoryItem
{
    // Constructor with all parameters
    public StaminaPotion(string id, string name, string description, Uses useType,
                        Dictionary<string, int> stats, int value, List<string> strongConsumers,
                        List<string> weakConsumers, int amount = 1, int stackAmount = 5)
        : base(id, name, description, useType, stats, value, strongConsumers, weakConsumers, amount, stackAmount)
    {
    }

    public override InventoryItem Clone()
    {
        var clonedStats = new Dictionary<string, int>(Stats);
        var clonedStrongConsumers = new List<string>(StrongConsumers);
        var clonedWeakConsumers = new List<string>(WeakConsumers);

        return new StaminaPotion(
            Id, Name, Description, UseType, clonedStats, Value, clonedStrongConsumers, clonedWeakConsumers, Amount, StackAmount);
    }

    // Simplified constructor for a small health potion
    public static StaminaPotion SmallPotion() 
    {
        return new StaminaPotion(
            "sp-1",
            "Small Stamina Potion",
            "A small energy boost",
            Uses.Consumable, 
            new Dictionary<string, int>{{"RegainStamina", 10}}, 
            25, 
            new List<string>(), 
            new List<string>()
        );
    }

    // Simplified constructor for a medium health potion
    public static StaminaPotion MediumPotion() 
    {
        return new StaminaPotion(
            "sp-2",
            "Medium Stamina Potion",
            "A good energy boost",
            Uses.Consumable, 
            new Dictionary<string, int>{{"RegainStamina", 25}}, 
            50, 
            new List<string>(), 
            new List<string>()
        );
    }

    // Simplified constructor for a large health potion
    public static StaminaPotion LargePotion() 
    {
        return new StaminaPotion(
            "sp-3",
            "Large Stamina Potion",
            "A huge energy boost",
            Uses.Consumable, 
            new Dictionary<string, int>{{"RegainStamina", 50}}, 
            75, 
            new List<string>(), 
            new List<string>()
        );
    }

    // Simplified constructor for a full health potion
    public static StaminaPotion FullPotion() 
    {
        return new StaminaPotion(
            "sp-4",
            "Full Stamina Potion",
            "Fully energy recovery",
            Uses.Consumable, 
            new Dictionary<string, int>{{"RegainStamina", 10000}}, 
            250, 
            new List<string>(), 
            new List<string>()
        );
    }

    public override void Use(Character character)
    {
        AddressableAudioPlayer player = Object.FindObjectOfType<AddressableAudioPlayer>();

        Debug.Log($"{Name} used by {character.playerName}.");
        CharacterStats stats = character.GetStats();

        if (stats.Stamina >= stats.MaxStamina)
        {
            Debug.Log("Stamina is already at maximum. No need to use the item.");
            return;
        }

        if (Stats.ContainsKey("RegainStamina"))
        {
            stats.RegainStamina(Stats["RegainStamina"]);
            player.PlayAddressableSound("Assets/SFX/Effects/inventory/bottle.wav");
        }

        Amount--;
        
        if (Amount <= 0)
        {
            // Remove the item from the character's inventory if its amount is zero
            Debug.Log("Removing stamina potion from inventory");
            RemoveFromInventory(character);
        }
    }
}
