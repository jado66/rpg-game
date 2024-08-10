using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteriousPotion : InventoryItem
{
    // Constructor with all parameters
    public MysteriousPotion(string id, string name, string description, Uses useType,
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
    public static MysteriousPotion MysteriousPotion1() 
    {
        return new MysteriousPotion(
            "mysterious-potion",
            "Mysterious Potion",
            "It most certainly will kill you.",
            Uses.Consumable, 
            new Dictionary<string, int>(), 
            150, 
            new List<string>(), 
            new List<string>()
        );
    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} used by {character.playerName}.");

        Amount--;
        
        if (Amount <= 0)
        {
            // Remove the item from the character's inventory if its amount is zero
            Debug.Log("Removing potion from inventory");
            RemoveFromInventory(character);
        }
    }
}
