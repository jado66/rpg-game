using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelItem : InventoryItem
{
    public ShovelItem(string id, string name, string description, Uses useType,
                      Dictionary<string, int> stats, int value, List<string> strongConsumers,
                      List<string> weakConsumers, int amount = 1, int stackAmount = 1)
        : base(id, name, description, useType, stats, value, strongConsumers, weakConsumers, amount, stackAmount)
    {
        // Initialize specific tool properties if needed
    }

    public override InventoryItem Clone()
    {
        var clonedStats = new Dictionary<string, int>(Stats);
        var clonedStrongConsumers = new List<string>(StrongConsumers);
        var clonedWeakConsumers = new List<string>(WeakConsumers);

        return new ShovelItem(
            Id, Name, Description, UseType, clonedStats, Value, clonedStrongConsumers, clonedWeakConsumers, Amount, StackAmount);
    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} used by {character.playerName}.");
        character.IrrigateGround();
    }
}