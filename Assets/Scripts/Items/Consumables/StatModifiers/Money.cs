using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for Money
public class Money : InventoryItem
{
    protected int totalSlots;

    // Constructor with all parameters
    public Money(string id, string name, string description, Uses useType,
                    Dictionary<string, int> stats, int value, List<string> strongConsumers,
                    List<string> weakConsumers, int totalSlots, int amount = 1, int stackAmount = 1)
        : base(id, name, description, useType, stats, value, strongConsumers, weakConsumers, amount, stackAmount)
    {
        this.totalSlots = totalSlots;
    }

    public override InventoryItem Clone()
    {
        var clonedStats = new Dictionary<string, int>(Stats);
        var clonedStrongConsumers = new List<string>(StrongConsumers);
        var clonedWeakConsumers = new List<string>(WeakConsumers);

        return new Money(
            Id, Name, Description, UseType, clonedStats, Value, clonedStrongConsumers, clonedWeakConsumers, totalSlots, Amount, StackAmount);
    }

    public override void Use(Character character)
    {
        Debug.Log($"Giving {Amount} coins to player.");
        character.GiveMoney(Amount);
        
        Amount = 0;
        if (Amount <= 0)
        {
            RemoveFromInventory(character);
        }
    }

    public static Money Coins() 
    {
        return new Money(
            "coins",
            "Coins",
            "Money",
            Uses.Consumable, 
            new Dictionary<string, int>(), 
            1, 
            new List<string>(), 
            new List<string>(),
            1,
            999
        );
    }
}

