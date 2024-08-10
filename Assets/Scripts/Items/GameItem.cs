using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class GameItem
{
    public string Id;
    public string Name;
    public string Description;
    public Uses UseType;
    public Dictionary<string, int> Stats;
    public int Value;
    public List<string> StrongConsumers;
    public List<string> WeakConsumers;
    public int Amount;
    public Sprite Icon;


    public GameItem(string id, string name, string description, Uses useType,
                    Dictionary<string, int> stats, int value, 
                    List<string> strongConsumers, List<string> weakConsumers, int amount = 1)
    {
        Id = id;
        Name = name;
        Description = description;
        UseType = useType;
        Stats = stats;
        Value = value;
        StrongConsumers = strongConsumers;
        WeakConsumers = weakConsumers;
        Amount = amount;
        Icon = Resources.Load<Sprite>("Sprites/Items/" + name);

    }

    public enum Uses
    {
        RegularItem,
        Tool,
        Interactable,
        Consumable,
        Spell,
        Wearable,
        // Add other uses here
    }
}
