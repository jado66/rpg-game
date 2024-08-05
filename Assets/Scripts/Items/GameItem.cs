using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class GameItem : IItem
{
    public string Name; // Removed 'protected set' to make it serializable
    public string id; // Removed 'protected set' to make it serializable
    public string description; // Removed 'protected set' to make it serializable
    public int amount; // Removed 'protected set' to make it serializable
    public int value; // Removed 'protected set' to make it serializable
    public Sprite icon; // Removed 'protected set' to make it serializable
    
    [Serializable]
    public enum Uses {
        tool,
        interactable,
        consumable,
        wearable,
        regularItem
    };

    public Uses use; // Removed 'protected set' to make it serializable
    public Dictionary<string, int> stats = new Dictionary<string, int>(); // Removed 'protected set' to make it serializable
    public List<string> strongConsumers = new List<string>(); // Removed 'protected set' to make it serializable
    public List<string> weakConsumers = new List<string>(); // Removed 'protected set' to make it serializable

    protected GameItem(string id, string name, string description, Uses use, 
                        Dictionary<string, int> stats, int value, 
                        List<string> strongConsumers, List<string> weakConsumers, int amount = 1)
    {
        this.id = id;
        this.Name = name;
        this.description = description;
        this.icon = Resources.Load<Sprite>("Sprites/Items/" + name);
        this.use = use;
        this.stats = stats ?? new Dictionary<string, int>();
        this.value = value;
        this.amount = amount;
        this.strongConsumers = new List<string>(strongConsumers ?? new List<string>());
        this.weakConsumers = new List<string>(weakConsumers ?? new List<string>());
    }

    public abstract void Use();

    public virtual void Use(Character character)
    {
        // Implement default use logic here, if necessary.
    }
}
