using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : InventoryItem
{
    // Constructor with all parameters
    public HealthPotion(string id, string name, string description, Uses useType,
                        Dictionary<string, int> stats, int value, List<string> strongConsumers,
                        List<string> weakConsumers, int amount = 1, int stackAmount = 0)
        : base(id, name, description, useType, stats, value, strongConsumers, weakConsumers, amount, stackAmount)
    {
    }

    public override InventoryItem Clone()
    {
        var clonedStats = new Dictionary<string, int>(Stats);
        var clonedStrongConsumers = new List<string>(StrongConsumers);
        var clonedWeakConsumers = new List<string>(WeakConsumers);

        return new HealthPotion(
            Id, Name, Description, UseType, clonedStats, Value, clonedStrongConsumers, clonedWeakConsumers, Amount, StackAmount);
    }
    

    // Simplified constructor for a small health potion
    public static HealthPotion SmallPotion() 
    {
        return new HealthPotion(
            "24",
            "Small Health Potion",
            "Heals you right up",
            Uses.Consumable, 
            new Dictionary<string, int>{{"Heals", 10}}, 
            150, 
            new List<string>(), 
            new List<string>()
        );
    }

    // Simplified constructor for a medium health potion
    public static HealthPotion MediumPotion() 
    {
        return new HealthPotion(
            "25",
            "Medium Health Potion",
            "Heals you well",
            Uses.Consumable, 
            new Dictionary<string, int>{{"Heals", 25}}, 
            300, 
            new List<string>(), 
            new List<string>()
        );
    }

    // Simplified constructor for a large health potion
    public static HealthPotion LargePotion() 
    {
        return new HealthPotion(
            "26",
            "Large Health Potion",
            "Heals you greatly",
            Uses.Consumable, 
            new Dictionary<string, int>{{"Heals", 50}}, 
            500, 
            new List<string>(), 
            new List<string>()
        );
    }

    // Simplified constructor for a full health potion
    public static HealthPotion FullPotion() 
    {
        return new HealthPotion(
            "27",
            "Full Health Potion",
            "Fully heals you",
            Uses.Consumable, 
            new Dictionary<string, int>{{"Heals", 10000}}, 
            1000, 
            new List<string>(), 
            new List<string>()
        );
    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} used by {character.playerName}.");
        CharacterStats stats = character.GetStats();

        if (Stats.ContainsKey("Heals"))
        {
            stats.Heal(Stats["Heals"]);
        }

        Amount--;
        
        if (Amount <= 0)
        {
            // Remove the item from the character's inventory if its amount is zero
            Debug.Log("Removing potion from inventory");
            RemoveFromInventory(character);
        }
    }
}
