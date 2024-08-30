using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class
public class MonsterParts : InventoryItem
{
    // Constructor with all parameters
    public MonsterParts(string id, string name, string description, Uses useType,
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

        return new MonsterParts(
            Id, Name, Description, UseType, clonedStats, Value, clonedStrongConsumers, clonedWeakConsumers, Amount, StackAmount);
    }
}

// Derived class for DragonScale
public class Bones : MonsterParts
{
    public Bones() 
        : base("mp-1", "Bones", "Bones from a monster.", Uses.RegularItem, 
               new Dictionary<string, int>(), 10, new List<string>(), new List<string>(), 1, 10)
    {
    }

    public override InventoryItem Clone()
    {
        return new Bones()
        {
            Amount = this.Amount,
            StackAmount = this.StackAmount
        };
    }
}

public class Skull : MonsterParts
{
    public Skull() 
        : base("mp-2", "Skull", "Skeleton skull.", Uses.RegularItem, 
               new Dictionary<string, int>(), 50, new List<string>(), new List<string>(), 1, 5)
    {
    }

    public override InventoryItem Clone()
    {
        return new Skull()
        {
            Amount = this.Amount,
            StackAmount = this.StackAmount
        };
    }
}

public class GrassSlimeball : MonsterParts
{
    public GrassSlimeball() 
        : base("mp-3", "Grass Slimeball", "Piece of a slime.", Uses.RegularItem, 
               new Dictionary<string, int>(), 50, new List<string>(), new List<string>(), 1, 99)
    {
    }

    public override InventoryItem Clone()
    {
        return new GrassSlimeball()
        {
            Amount = this.Amount,
            StackAmount = this.StackAmount
        };
    }
}

// Derived class for GoblinEar
// public class GoblinEar : MonsterParts
// {
//     public GoblinEar() 
//         : base("mp-2", "Goblin Ear", "An ear cut off from a goblin.", Uses.RegularItem, 
//                new Dictionary<string, int>(), 50, new List<string>(), new List<string>())
//     {
//     }

//     public override InventoryItem Clone()
//     {
//         return new GoblinEar()
//         {
//             Amount = this.Amount,
//             StackAmount = this.StackAmount
//         };
//     }
// }

// Derived class for VampireFang
// public class VampireFang : MonsterParts
// {
//     public VampireFang() 
//         : base("mp-3", "Vampire Fang", "A sharp fang from a vampire.", Uses.RegularItem, 
//                new Dictionary<string, int>(), 500, new List<string>(), new List<string>())
//     {
//     }

//     public override InventoryItem Clone()
//     {
//         return new VampireFang()
//         {
//             Amount = this.Amount,
//             StackAmount = this.StackAmount
//         };
//     }
// }

// Derived class for WerewolfClaw
// public class WerewolfClaw : MonsterParts
// {
//     public WerewolfClaw() 
//        : base("mp-4", "Werewolf Claw", "A claw taken from a werewolf.", Uses.RegularItem, 
//               new Dictionary<string, int>(), 200, new List<string>(), new List<string>())
//     {
//     }

//     public override InventoryItem Clone()
//     {
//         return new WerewolfClaw()
//         {
//             Amount = this.Amount,
//             StackAmount = this.StackAmount
//         };
//     }
// }

// Derived class for PhoenixFeather
// public class PhoenixFeather : MonsterParts
// {
//     public PhoenixFeather() 
//         : base("mp-5", "Phoenix Feather", "A feather from the mythical phoenix.", Uses.RegularItem, 
//                new Dictionary<string, int>(), 1000, new List<string>(), new List<string>())
//     {
//     }

//     public override InventoryItem Clone()
//     {
//         return new PhoenixFeather()
//         {
//             Amount = this.Amount,
//             StackAmount = this.StackAmount
//         };
//     }
// }
