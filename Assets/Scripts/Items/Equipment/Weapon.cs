using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : InventoryItem
{
    // Constructor with all parameters
    public Weapon(string id, string name, string description, Uses useType,
                        Dictionary<string, int> stats, int value, List<string> strongConsumers,
                        List<string> weakConsumers, int amount = 1, int stackAmount = 1)
        : base(id, name, description, useType, stats, value, strongConsumers, weakConsumers, amount, stackAmount)
    {
    }

    public override InventoryItem Clone()
    {
        var clonedStats = new Dictionary<string, int>(Stats);
        var clonedStrongConsumers = new List<string>(StrongConsumers);
        var clonedWeakConsumers = new List<string>(WeakConsumers);

        return new Weapon(
            Id, Name, Description, UseType, clonedStats, Value, clonedStrongConsumers, clonedWeakConsumers, Amount, StackAmount);
    }

    // Simplified constructor for a small health potion
    public static Weapon Sword() 
    {
        return new Weapon(
            "sword",
            "Sword",
            "This looks pointy",
            Uses.Weapon, 
            new Dictionary<string, int>{}, 
            250, 
            new List<string>(), 
            new List<string>()
        );
    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} used.");
        
        character.Attack();
    }
}
