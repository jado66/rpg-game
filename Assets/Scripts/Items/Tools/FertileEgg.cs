using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FertileEgg : InventoryItem
{
    // Constructor with all parameters
    public FertileEgg(string id, string name, string description, Uses useType,
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

        return new FertileEgg(
            Id, Name, Description, UseType, clonedStats, Value, clonedStrongConsumers, clonedWeakConsumers, Amount, StackAmount);
    }
            
    // Simplified constructor for a small health potion
    public static FertileEgg ChickenEgg() 
    {
        return new FertileEgg(
            "egg",
            "Chicken Egg",
            "I don't think you should eat this one.",
            Uses.Consumable, 
            new Dictionary<string, int>{}, 
            150, 
            new List<string>(), 
            new List<string>(),
            1,
            15
        );
    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} used by {character.playerName}.");
        Amount--;

        // Get the character's position
        Vector3 characterPosition = character.transform.position;


        BuildablePrefabs prefabs = UnityEngine.Object.FindObjectOfType<BuildablePrefabs>();

        // Instantiate the prefab at the character's position
        UnityEngine.Object.Instantiate(prefabs.chick, characterPosition, Quaternion.identity);
        
        if (Amount <= 0)
        {
            // Remove the item from the character's inventory if its amount is zero
            RemoveFromInventory(character);
        }
    }
}
