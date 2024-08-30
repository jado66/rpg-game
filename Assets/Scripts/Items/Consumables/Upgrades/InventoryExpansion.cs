using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for InventoryExpansion
public class InventoryExpansion : InventoryItem
{
    protected int totalSlots;

    // Constructor with all parameters
    public InventoryExpansion(string id, string name, string description, Uses useType,
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

        return new InventoryExpansion(
            Id, Name, Description, UseType, clonedStats, Value, clonedStrongConsumers, clonedWeakConsumers, totalSlots, Amount, StackAmount);
    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} used by {character.playerName}.");
        character.ExpandInventory(totalSlots);
        
        Amount--;
        if (Amount <= 0)
        {
            RemoveFromInventory(character);
        }
    }
}

// Derived class for MediumBackpack
public class MediumBackpack : InventoryExpansion
{
    public MediumBackpack() 
        : base("mp-1", "Medium Backpack", "Can hold 9 items.", Uses.Consumable, 
               new Dictionary<string, int>(), 300, new List<string>(), new List<string>(), 9)
    {
    }

    public override InventoryItem Clone()
    {
        return new MediumBackpack()
        {
            Amount = this.Amount,
            StackAmount = this.StackAmount
        };
    }
}

// Derived class for LargeBackpack
public class LargeBackpack : InventoryExpansion
{
    public LargeBackpack() 
        : base("lp-1", "Large Backpack", "Can hold 12 items.", Uses.Consumable, 
               new Dictionary<string, int>(), 500, new List<string>(), new List<string>(), 12)
    {
    }

    public override InventoryItem Clone()
    {
        return new LargeBackpack()
        {
            Amount = this.Amount,
            StackAmount = this.StackAmount
        };
    }
}

// Derived class for ExtraLargeBackpack
public class ExtraLargeBackpack : InventoryExpansion
{
    public ExtraLargeBackpack() 
        : base("elp-1", "Extra Large Backpack", "Can hold 15 items.", Uses.Consumable, 
               new Dictionary<string, int>(), 700, new List<string>(), new List<string>(), 15)
    {
    }

    public override InventoryItem Clone()
    {
        return new ExtraLargeBackpack()
        {
            Amount = this.Amount,
            StackAmount = this.StackAmount
        };
    }
}

public class SpellInventoryExpansion : InventoryExpansion
{
    public SpellInventoryExpansion() 
        : base("inv-exp", "Spell of Inventory Expansion", "Increases inventory space by 1 up to three times.", Uses.Consumable, 
               new Dictionary<string, int>(), 700, new List<string>(), new List<string>(), 1)
    {
    }

    public override InventoryItem Clone()
    {
        return new SpellInventoryExpansion()
        {
            Amount = this.Amount,
            StackAmount = this.StackAmount
        };
    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} used by {character.playerName}.");
        int currentSpaces = character.GetEnhancedInventorySpaces();

        if (currentSpaces >= 3)
        {
            ToastNotification.Instance.Toast("inv-enhanced-max", $"You can't use this spell anymore.");
            Debug.Log("Cannot use this spell anymore. Maximum enhanced inventory spaces reached.");
            return;
        }

        character.SetEnhancedInventorySpaces(currentSpaces + 1);

        Amount--;
        if (Amount <= 0)
        {
            RemoveFromInventory(character);
        }
    }
}

public class StrongSpellInventoryExpansion : InventoryExpansion
{
    public StrongSpellInventoryExpansion() 
        : base("inv-exp2", "Strong Spell of Inventory Expansion", "Increases inventory space by 1 up to three times.", Uses.Consumable, 
               new Dictionary<string, int>(), 700, new List<string>(), new List<string>(), 1)
    {
    }

    public override InventoryItem Clone()
    {
        return new StrongSpellInventoryExpansion()
        {
            Amount = this.Amount,
            StackAmount = this.StackAmount
        };
    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} used by {character.playerName}.");
        int currentSpaces = character.GetEnhancedInventorySpaces();

        if (currentSpaces < 3)
        {
            ToastNotification.Instance.Toast("inv-enhanced-not-ready", "You can't use this spell.");
            Debug.Log("Cannot use this spell anymore. Maximum enhanced inventory spaces reached.");
            return;
        }

        if (currentSpaces >= 6)
        {
            ToastNotification.Instance.Toast("inv-enhanced-max", "You can't use this spell anymore.");
            Debug.Log("Cannot use this spell anymore. Maximum enhanced inventory spaces reached.");
            return;
        }
        character.SetEnhancedInventorySpaces(currentSpaces + 1);

        Amount--;
        if (Amount <= 0)
        {
            RemoveFromInventory(character);
        }
    }
}
