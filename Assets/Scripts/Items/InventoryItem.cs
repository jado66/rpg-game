using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : GameItem
{
    public int StackAmount { get; set; }

    public InventoryItem(string id, string name, string description, Uses useType,
                         Dictionary<string, int> stats, int value, 
                         List<string> strongConsumers, List<string> weakConsumers,
                         int amount = 1, int stackAmount = 0)
        : base(id, name, description, useType, stats, value, strongConsumers, weakConsumers, amount)
    {
        StackAmount = stackAmount;
    }
    
    public virtual void Use()
    {
        Debug.Log($"{Name} is used.");
    }

    public virtual void Use(Character character)
    {
        Debug.Log($"{character.playerName} used {Name}.");
    }

    public InventoryItem Clone()
    {
        // Creating deep copies of lists and dictionary to ensure the cloned item is fully independent
        var clonedStats = new Dictionary<string, int>(Stats);
        var clonedStrongConsumers = new List<string>(StrongConsumers);
        var clonedWeakConsumers = new List<string>(WeakConsumers);

        return new InventoryItem(
            Id, Name, Description, UseType, clonedStats, Value, clonedStrongConsumers, clonedWeakConsumers, Amount, StackAmount);
    }

    public InventoryItem Merge(InventoryItem other)
    {
        if (other == null || other.Id != Id)
        {
            return other;
        }

        // If this item is already at max stack amount, return the other item unchanged
        if (Amount == StackAmount)
        {
            return other;
        }

        // Calculate maximum transferable amount
        int transferableAmount = Mathf.Min(StackAmount - Amount, other.Amount);

        // Adjust the amounts in both items
        Amount += transferableAmount;
        other.Amount -= transferableAmount;

        // If the other item's amount is reduced to zero, return null
        if (other.Amount == 0)
        {
            return null;
        }

        // Return the modified other item
        return other;
    }

}