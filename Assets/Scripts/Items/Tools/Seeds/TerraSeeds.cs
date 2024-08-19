using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraSeeds : InventoryItem
{
    public string PlantName { get; private set; }

    // Constructor with all parameters
    public TerraSeeds(string id, string name, string description, Uses useType,
                      Dictionary<string, int> stats, int value, List<string> strongConsumers,
                      List<string> weakConsumers, string plantName, int amount = 1, int stackAmount = 99)
        : base(id, name, description, useType, stats, value, strongConsumers, weakConsumers, amount, stackAmount)
    {
        PlantName = plantName;
    }

    public override InventoryItem Clone()
    {
        var clonedStats = new Dictionary<string, int>(Stats);
        var clonedStrongConsumers = new List<string>(StrongConsumers);
        var clonedWeakConsumers = new List<string>(WeakConsumers);

        return new TerraSeeds(
            Id, Name, Description, UseType, clonedStats, Value, clonedStrongConsumers, clonedWeakConsumers, PlantName, Amount, StackAmount);
    }

    // Simplified constructor for a tree sapling
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
            "Tree",
            1,
            25
        );
    }

    public static TerraSeeds AppleSapling() 
    {
        return new TerraSeeds(
            "apple-sapling",
            "Apple Sapling",
            "It most certainly will kill you.",
            Uses.Tool, 
            new Dictionary<string, int>(), 
            150, 
            new List<string>(), 
            new List<string>(),
            "Apple Tree",
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
            "Bush",
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
            "Tomato",
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
            "Carrot",
            1,
            99
        );
    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} used by {character.playerName}.");
        character.StartCoroutine(UseSeedCoroutine(character));
    }

    private IEnumerator UseSeedCoroutine(Character character)
    {
        bool plantingSuccess = false;
        yield return character.StartCoroutine(PlantAndGetResult(character, result => plantingSuccess = result));

        if (plantingSuccess)
        {
            Amount--;
            Debug.Log($"{PlantName} planted successfully. Remaining {Name}: {Amount}");

            if (Amount <= 0)
            {
                Debug.Log($"Removing {Name} from inventory");
                RemoveFromInventory(character);
            }
        }
        else
        {
            Debug.Log($"Failed to plant {PlantName}. No {Name} consumed.");
        }
    }

    private IEnumerator PlantAndGetResult(Character character, System.Action<bool> callback)
    {
        bool result = false;
        yield return character.StartCoroutine(character.Plant(PlantName, success => result = success));
        callback(result);
    }
}