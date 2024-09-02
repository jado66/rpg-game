using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : InventoryItem
{
    // Constructor with all parameters
    public float LightTime { get; set; }

     public Torch(string id, string name, string description, Uses useType,
                 Dictionary<string, int> stats, int value, float lightTime, List<string> strongConsumers,
                 List<string> weakConsumers,  int amount = 1, int stackAmount = 5)
        : base(id, name, description, useType, stats, value, strongConsumers, weakConsumers, amount, stackAmount)
    {
        LightTime = lightTime;
    }

    public override InventoryItem Clone()
    {
        var clonedStats = new Dictionary<string, int>(Stats);
        var clonedStrongConsumers = new List<string>(StrongConsumers);
        var clonedWeakConsumers = new List<string>(WeakConsumers);

        return new Torch(
            Id, Name, Description, UseType, clonedStats, Value, LightTime, clonedStrongConsumers, clonedWeakConsumers, Amount, StackAmount);
    }
            
    // Simplified constructor for a small health potion
    public static Torch SmallTorch() 
    {
        return new Torch(
            "small-torch",
            "Torch",
            "Keeps the darkness away..",
            Uses.Consumable, 
            new Dictionary<string, int>(), 
            10, 
            20f, // default light time in seconds
            new List<string>(), 
            new List<string>()
        );
    }

    public override void Use(Character character)
    {
        
        bool success = character.ToggleTorch(true, 10);

        if (!success){
            return;
        }

        SceneManager sceneManager = GameObject.FindObjectOfType<SceneManager>();
        bool isDay = sceneManager.IsDay();

        // if (isDay)
        // {
        //     ToastNotification.Instance.Toast("not-day", "You can't use this in the day.");
        //     return;
        // }

        Amount--;
        
        if (Amount <= 0)
        {
            // Remove the item from the character's inventory if its amount is zero
            Debug.Log("Removing potion from inventory");
            RemoveFromInventory(character);
        }

        character.StartCoroutine(LightTimeCountdown(character));

    }

    private IEnumerator LightTimeCountdown(Character character)
    {
        yield return new WaitForSeconds(LightTime);
        character.ToggleTorch(false, 10);
        Debug.Log("Torch light time ended");
    }
}
