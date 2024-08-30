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
    public string IconName; // Stores the path to the icon

    public GameItem(string id, string name, string description, Uses useType,
                    Dictionary<string, int> stats, int value, 
                    List<string> strongConsumers, List<string> weakConsumers, 
                    int amount = 1,  string iconName = null)
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
        IconName = iconName != null ? $"Sprites/Items/{iconName}" : $"Sprites/Items/{name}"; // Use default path if not provided
        LoadIcon();
    }

    private void LoadIcon()
    {
        Icon = Resources.Load<Sprite>(IconName);
        if (Icon == null)
        {
            Debug.LogWarning($"Failed to load icon for item {Name} at path: {IconName}");
        }
    }

    public enum Uses
    {
        RegularItem,
        Tool,
        Interactable,
        Weapon,
        Consumable,
        Spell,
        Wearable,
        // Add other uses here
    }
}