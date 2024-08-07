using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : GameItem
{
    public int StackAmount { get; set; }

    public InventoryItem(string id, string name, string description, Uses useType,
                         Dictionary<string, int> stats, int value, 
                         List<string> strongConsumers, List<string> weakConsumers,
                         int amount = 1, int stackAmount = 1)
        : base(id, name, description, useType, stats, value, strongConsumers, weakConsumers, amount)
    {
        StackAmount = stackAmount;
    }
    
    

    public virtual void Use(Character character)
    {
        // Perform the item's effect on the character here

        Debug.Log($"{Name} is used.");

        // If the item is consumable, reduce its amount
        if (UseType == Uses.Consumable && Amount > 0)
        {
            Amount--;
            if (Amount <= 0)
            {
                // Remove the item from the character's inventory if its amount is zero
                RemoveFromInventory(character);
            }
        }
    }

    private void ApplyEffect(Character character)
    {
        // Define what happens when the item is used on the character here
        Debug.Log($"Applying {Name}'s effect on {character.name}");
    }

    protected void RemoveFromInventory(Character character)
    {
        CharacterInventory inventory = character.GetHotbar();
        if (inventory != null)
        {
            inventory.RemoveItem(this);
        }
    }

    public virtual InventoryItem Clone()
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