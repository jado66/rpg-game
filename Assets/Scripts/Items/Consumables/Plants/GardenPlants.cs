using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenPlant : InventoryItem
{
    // Constructor with all parameters
    public GardenPlant(string id, string name, string description, Uses useType,
                        Dictionary<string, int> stats, int value, List<string> strongConsumers,
                        List<string> weakConsumers, int amount = 1, int stackAmount = 25)
        : base(id, name, description, useType, stats, value, strongConsumers, weakConsumers, amount, stackAmount)
    {
    }

    public override InventoryItem Clone()
    {
        var clonedStats = new Dictionary<string, int>(Stats);
        var clonedStrongConsumers = new List<string>(StrongConsumers);
        var clonedWeakConsumers = new List<string>(WeakConsumers);

        return new GardenPlant(
            Id, Name, Description, UseType, clonedStats, Value, clonedStrongConsumers, clonedWeakConsumers, Amount, StackAmount);
    }
            
    // Simplified constructor for a small health potion
    public static GardenPlant Carrot() 
    {
        return new GardenPlant(
            "carrot",
            "Carrot",
            "For horses.",
            Uses.Consumable, 
            new Dictionary<string, int>{{"Heals", 10},{"RegainStamina", 10}}, 
            5, 
            new List<string>(), 
            new List<string>()
        );
    }

    // Simplified constructor for a medium health potion
    public static GardenPlant Tomato() 
    {
        return new GardenPlant(
            "tomato",
            "Tomato",
            "Ripe from the vine",
            Uses.Consumable, 
            new Dictionary<string, int>{{"Heals", 25},{"RegainStamina", 10}}, 
            3, 
            new List<string>(), 
            new List<string>()
        );
    }

    // Simplified constructor for a large health potion
    public static GardenPlant WhiteMushroom() 
    {
        return new GardenPlant(
            "white-mushroom",
            "White Mushroom",
            "A pretty rare mushroom.",
            Uses.Consumable, 
            new Dictionary<string, int>{{"Heals", 10},{"RegainStamina", 10}}, 
            50, 
            new List<string>(), 
            new List<string>()
        );
    }

    public static GardenPlant PinkMushroom() 
    {
        return new GardenPlant(
            "pink-mushroom",
            "Mushroom",
            "Squishy.",
            Uses.Consumable, 
            new Dictionary<string, int>{{"Heals", -30}}, 
            5, 
            new List<string>(), 
            new List<string>()
        );
    }

    public static GardenPlant RedMushroom() 
    {
        return new GardenPlant(
            "red-mushroom",
            "Red Mushroom",
            "Looks tasty.",
            Uses.Consumable, 
            new Dictionary<string, int>{{"RegainStamina", 40}}, 
            25, 
            new List<string>(), 
            new List<string>()
        );
    }

    // Simplified constructor for a full health potion
    public static GardenPlant Apple() 
    {
        return new GardenPlant(
            "apple",
            "Apple",
            "Keeps doctors away",
            Uses.Consumable, 
            new Dictionary<string, int>{{"Heals", 10},{"RegainStamina", 10}}, 
            5, 
            new List<string>(), 
            new List<string>()
        );
    }

    public static GardenPlant UnripeApple() 
    {
        return new GardenPlant(
            "unripe-apple",
            "Unripe Apple",
            "Doesn't keeps doctors away",
            Uses.Consumable, 
            new Dictionary<string, int>{{"Heals", -2},{"RegainStamina", -10}}, 
            0, 
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

        if (Stats.ContainsKey("RegainStamina"))
        {
            stats.RegainStamina(Stats["RegainStamina"]);
        }

        if (Amount <= 0)
        {
            // Remove the item from the character's inventory if its amount is zero
            Debug.Log("Removing potion from inventory");
            RemoveFromInventory(character);
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
