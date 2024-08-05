using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

    string Name { get; }
    public int id;
    public string title;
    public string titleToDisplay;

    public int stackAmount;
    public int amount;

    public int value;
    public string description;
    public Sprite icon;
                                    //tool, interactable, consumable, wearable
    public enum Uses {tool,interactable,consumable,wearable,regularItem};

    public Uses use; 
    public Dictionary<string, int> stats = new Dictionary<string, int>();

    public List<string> strongConsumers;
    public List<string> weakConsumers;


    public Item(int id, string title, string description, int stackAmount, Uses use,Dictionary<string, int> stats, int value, List<string> strongConsumers, List<string>  weakConsumers, int amount = 1, string titleToDisplay = null)
    {
        this.id = id;
        this.title = title;
        if (titleToDisplay == null)
            this.titleToDisplay = title;
        else
            this.titleToDisplay = titleToDisplay;
        this.description = description;
        this.icon = Resources.Load<Sprite>("Sprites/Items/" + title);
        this.stats = stats;
        this.use = use;
        this.value = value;
        this.stackAmount = stackAmount;
        this.amount = amount;

        this.strongConsumers = strongConsumers;
        this.weakConsumers = weakConsumers;
    }

    public Item(Item item)
    {
        this.id = item.id;
        this.title = item.title;
        this.description = item.description;
        this.icon = Resources.Load<Sprite>("Sprites/Items/" + item.title);
        this.stackAmount = item.stackAmount;
        this.stats = item.stats;
        this.use = item.use;
        this.amount = item.amount;
        this.titleToDisplay = item.titleToDisplay;
        this.value = item.value;
        this.strongConsumers = new List<string>();
        foreach(var name in strongConsumers){
            this.strongConsumers.Add(name);
        }
        this.weakConsumers = new List<string>();
        foreach(var name in weakConsumers){
            this.weakConsumers.Add(name);
        }
    }

    public virtual void Use(){

    }
}
