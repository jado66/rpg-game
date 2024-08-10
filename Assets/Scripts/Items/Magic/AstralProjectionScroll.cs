using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralProjectionScroll : InventoryItem
{
    public AstralProjectionScroll(int amount = 1, int stackAmount = 1)
        : base(
            id: "astral_scroll_01",
            name: "Astral Projection Scroll",
            description: "A scroll that allows temporary astral projection.",
            useType: Uses.Spell,
            stats: new Dictionary<string, int>
            {
                { "MagicDamage", 50 },
                { "ManaCost", 20 }
            },
            value: 1000,
            strongConsumers: new List<string> { "Mage", "Sorcerer" },
            weakConsumers: new List<string> { "Warrior", "Rogue" },
            amount: amount,
            stackAmount: stackAmount)
    {
        // Initialize specific tool properties if needed
    }
    

    public override InventoryItem Clone()
    {
        var clonedStats = new Dictionary<string, int>(Stats);
        var clonedStrongConsumers = new List<string>(StrongConsumers);
        var clonedWeakConsumers = new List<string>(WeakConsumers);

        return new AstralProjectionScroll(Amount, StackAmount);

    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} used by {character.playerName}.");
    }
}