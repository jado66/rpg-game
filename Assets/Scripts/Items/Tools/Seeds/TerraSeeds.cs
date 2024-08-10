using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraSeeds : InventoryItem
{
    // Constructor with all parameters
    public TerraSeeds(string id, string name, string description, Uses useType,
                        Dictionary<string, int> stats, int value, List<string> strongConsumers,
                        List<string> weakConsumers, int amount = 1, int stackAmount = 99)
        : base(id, name, description, useType, stats, value, strongConsumers, weakConsumers, amount, stackAmount)
    {
    }

    public override InventoryItem Clone()
    {
        var clonedStats = new Dictionary<string, int>(Stats);
        var clonedStrongConsumers = new List<string>(StrongConsumers);
        var clonedWeakConsumers = new List<string>(WeakConsumers);

        return new TerraSeeds(
            Id, Name, Description, UseType, clonedStats, Value, clonedStrongConsumers, clonedWeakConsumers, Amount, StackAmount);
    }

    // Simplified constructor for a small health potion
    public static TerraSeeds TreeSapling() 
    {
        return new TerraSeeds(
            "tree-sapling",
            "Tree Sapling",
            "It most certainly will kill you.",
            Uses.Tool, 
            new Dictionary<string, int>(), 
            150, 
            new List<string>(), 
            new List<string>(),
            1,
            25
        );
    }

    // Simplified constructor for a bush sapling
    public static TerraSeeds BushSapling() 
    {
        return new TerraSeeds(
            "bush-sapling",
            "Bush Sapling",
            "A baby bush",
            Uses.Tool, 
            new Dictionary<string, int>(), 
            13, 
            new List<string>(), 
            new List<string>(),
            1,
            25
        );
    }

    // Simplified constructor for tomato seed
    public static TerraSeeds TomatoSeed() 
    {
        return new TerraSeeds(
            "tomato-seed",
            "Tomato Seed",
            "A baby tomato",
            Uses.Tool, 
            new Dictionary<string, int>(), 
            5, 
            new List<string>(), 
            new List<string>(),
            1,
            99
        );
    }

    // Simplified constructor for carrot seed
    public static TerraSeeds CarrotSeed() 
    {
        return new TerraSeeds(
            "carrot-seed",
            "Carrot Seed",
            "A baby carrot",
            Uses.Tool, 
            new Dictionary<string, int>(), 
            5, 
            new List<string>(), 
            new List<string>(),
            1,
            99
        );
    }

    public override void Use(Character character)
    {
        Debug.Log($"Sapling used by {character.playerName}.");

        Amount--;

        character.Plant();

        
        if (Amount <= 0)
        {
            // Remove the item from the character's inventory if its amount is zero
            Debug.Log("Removing sapling from inventory");
            RemoveFromInventory(character);
        }
    }
}