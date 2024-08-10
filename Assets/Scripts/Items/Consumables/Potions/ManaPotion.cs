using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : InventoryItem
{
    // Constructor with all parameters
    public ManaPotion(string id, string name, string description, Uses useType,
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

        return new ManaPotion(
            Id, Name, Description, UseType, clonedStats, Value, clonedStrongConsumers, clonedWeakConsumers, Amount, StackAmount);
    }

    // Simplified constructor for a small health potion
    public static ManaPotion SmallPotion() 
    {
        return new ManaPotion(
            "sp-1",
            "Small Mana Potion",
            "A small mana boost",
            Uses.Consumable, 
            new Dictionary<string, int>{{"RegainMana", 10}}, 
            150, 
            new List<string>(), 
            new List<string>()
        );
    }

    // Simplified constructor for a medium health potion
    public static ManaPotion MediumPotion() 
    {
        return new ManaPotion(
            "sp-2",
            "Medium Mana Potion",
            "A good mana boost",
            Uses.Consumable, 
            new Dictionary<string, int>{{"RegainMana", 25}}, 
            300, 
            new List<string>(), 
            new List<string>()
        );
    }

    // Simplified constructor for a large health potion
    public static ManaPotion LargePotion() 
    {
        return new ManaPotion(
            "sp-3",
            "Large Mana Potion",
            "A huge mana boost",
            Uses.Consumable, 
            new Dictionary<string, int>{{"RegainMana", 50}}, 
            500, 
            new List<string>(), 
            new List<string>()
        );
    }

    // Simplified constructor for a full health potion
    public static ManaPotion FullPotion() 
    {
        return new ManaPotion(
            "sp-4",
            "Full Mana Potion",
            "Fully mana recovery",
            Uses.Consumable, 
            new Dictionary<string, int>{{"RegainMana", 10000}}, 
            1000, 
            new List<string>(), 
            new List<string>()
        );
    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} mana potion used by {character.playerName}.");
        CharacterStats stats = character.GetStats();

        if (Stats.ContainsKey("RegainMana"))
        {
            stats.RegainMana(Stats["RegainMana"]);
        }

        Amount--;
        
        if (Amount <= 0)
        {
            // Remove the item from the character's inventory if its amount is zero
            Debug.Log("Removing mana  potion from inventory");
            RemoveFromInventory(character);
        }
    }
}
