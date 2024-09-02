using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : InventoryItem
{
    // Constructor with all parameters
    public Ingredient(string id, string name, string description, Uses useType,
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

        return new Ingredient(
            Id, Name, Description, UseType, clonedStats, Value, clonedStrongConsumers, clonedWeakConsumers, Amount, StackAmount);
    }
            
    // Simplified constructor for a small health potion
    public static Ingredient Egg() 
    {
        return new Ingredient(
            "egg",
            "Egg",
            "From chickens",
            Uses.Consumable, 
            new Dictionary<string, int>{}, 
            25, 
            new List<string>(), 
            new List<string>(),
            1,
            15
        );
    }

    // Simplified constructor for a medium health potion
    public static Ingredient Meat() 
    {
        return new Ingredient(
            "meat",
            "Meat",
            "Dead chicken",
            Uses.Consumable, 
            new Dictionary<string, int>{}, 
            25, 
            new List<string>(), 
            new List<string>(),
            1,
            3
        );
    }

    public static Ingredient Feather() 
    {
        return new Ingredient(
            "feather",
            "Feather",
            "Chicken Feather",
            Uses.RegularItem, 
            new Dictionary<string, int>{}, 
            3, 
            new List<string>(), 
            new List<string>(),
            1,
            99
        );
    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} used by {character.playerName}.");
       
    }
}
