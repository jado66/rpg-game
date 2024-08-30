using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class
public class MagicSpells : InventoryItem
{
    // Constructor with all parameters
    public MagicSpells(string id, string name, string description, Uses useType,
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

        return new MagicSpells(
            Id, Name, Description, UseType, clonedStats, Value, clonedStrongConsumers, clonedWeakConsumers, Amount, StackAmount);
    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} used by {character.playerName}.");
    }
}

// Derived class for MysteriousPotion
public class MysteriousPotion : MagicSpells
{
    public MysteriousPotion() 
        : base("sp-1", "Mysterious Potion", "This looks dangerous", Uses.Consumable, 
               new Dictionary<string, int>(), 150, new List<string>(), new List<string>())
    {
    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} mysterious potion used by {character.playerName}.");

        // Define the specific effects of using a MysteriousPotion here
        CharacterStats stats = character.GetStats();

        Amount--;
        stats.UseMana(25f);
        ToastNotification.Instance.Toast("wonder", "Hmmmm I wonder what this does?");

        if (Amount <= 0)
        {
            Debug.Log("Removing mysterious potion from inventory");
            RemoveFromInventory(character);
        }
    }

    public override InventoryItem Clone()
    {
        return new MysteriousPotion()
        {
            Amount = this.Amount,
            StackAmount = this.StackAmount
        };
    }
}

// Derived class for DaySpell
public class DaySpell : MagicSpells
{
    public DaySpell() 
        : base("day-spell", "Day Spell", "Turn the night into day", Uses.Consumable, 
               new Dictionary<string, int>(), 500, new List<string>(), new List<string>())
    {
    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} day spell used by {character.playerName}.");
        
        // Define the specific effects of using a Day Spell here
        CharacterStats stats = character.GetStats();

        if (stats.Mana < 25f){
            ToastNotification.Instance.Toast("not-enough-mana", "Not enough mana.");         
            return;
        }

        SceneManager sceneManager = GameObject.FindObjectOfType<SceneManager>();

        bool isSuccess = sceneManager.TryTransitionToDay();

        if (!isSuccess){
            ToastNotification.Instance.Toast("not-day", "You can't use this in the day.");
            return;
        }

        Amount--;
        stats.UseMana(25f);

        if (Amount <= 0)
        {
            Debug.Log("Removing day spell from inventory");
            RemoveFromInventory(character);
        }
    }

    public override InventoryItem Clone()
    {
        return new DaySpell()
        {
            Amount = this.Amount,
            StackAmount = this.StackAmount
        };
    }
}

public class GrowthSpell : MagicSpells
{
    public GrowthSpell() 
        : base("growth-spell", "Growth Spell", "Makes all plants grow.", Uses.Consumable, 
               new Dictionary<string, int>(), 2500, new List<string>(), new List<string>())
    {
    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} spell used by {character.playerName}.");
        
        // Define the specific effects of using a Day Spell here
        CharacterStats stats = character.GetStats();

        if (stats.Mana < 50f){
            ToastNotification.Instance.Toast("not-enough-mana", "Not enough mana.");         
            return;
        }
        
        SceneManager sceneManager = GameObject.FindObjectOfType<SceneManager>();

        sceneManager.GrowAllPlants();

        ToastNotification.Instance.Toast("all-plants-grow", "All crops have grown!");         

        Amount--;
        stats.UseMana(50f);

        if (Amount <= 0)
        {
            RemoveFromInventory(character);
        }
    }

    public override InventoryItem Clone()
    {
        return new GrowthSpell()
        {
            Amount = this.Amount,
            StackAmount = this.StackAmount
        };
    }
}

// Derived class for NightSpell
public class NightSpell : MagicSpells
{
    public NightSpell() 
       : base("night-spell", "Night Spell", "Turn the day into night", Uses.Consumable, 
              new Dictionary<string, int>(), 500, new List<string>(), new List<string>())
    {
    }

    public override void Use(Character character)
    {
        Debug.Log("NightSpell.Use method called");
        // Define the specific effects of using a Night Spell here
        
        CharacterStats stats = character.GetStats();

        if (stats.Mana < 25f){
            ToastNotification.Instance.Toast("not-enough-mana", "Not enough mana.");         
            return;
        }

        SceneManager sceneManager = GameObject.FindObjectOfType<SceneManager>();

        bool isSuccess = sceneManager.TryTransitionToNight();

        if (!isSuccess){
            ToastNotification.Instance.Toast("not-night", "You can't use this at night.");
            return;
        }



        Amount--;
        stats.UseMana(25f);

        if (Amount <= 0)
        {
            Debug.Log("Removing night spell from inventory");
            RemoveFromInventory(character);
        }
    }

    public override InventoryItem Clone()
    {
        return new NightSpell()
        {
            Amount = this.Amount,
            StackAmount = this.StackAmount
        };
    }
}

public class IlluminationSpell : MagicSpells
{
    private Coroutine currentCoroutine;

    public IlluminationSpell() 
        : base("illumination-spell", "Illumination Spell", "Works like a torch.", Uses.Consumable, 
               new Dictionary<string, int>(), 250, new List<string>(), new List<string>())
    {
    }

    public override void Use(Character character)
    {
        Debug.Log("IlluminationSpell.Use method called");
        
        CharacterStats stats = character.GetStats();

        SceneManager sceneManager = GameObject.FindObjectOfType<SceneManager>();
        bool isDay = sceneManager.IsDay();

        if (isDay)
        {
            ToastNotification.Instance.Toast("not-day", "You can't use this in the day.");
            return;
        }

        if (stats.Mana <= 0f)
        {
            ToastNotification.Instance.Toast("not-enough-mana", "Not enough mana.");         
            return;
        }

        // If there's an active coroutine, stop it and decrease the item count
        if (currentCoroutine != null)
        {
            character.StopCoroutine(currentCoroutine);
            currentCoroutine = null;
            DecreaseItemCount(character);
            character.ToggleTorch(false, 10);
            return;
        }

        bool success = character.ToggleTorch(true, 35);

        if (!success)
        {
            ToastNotification.Instance.Toast("torch-failed", "Failed to activate torch.");
            return;
        }

        currentCoroutine = character.StartCoroutine(IlluminationCoroutine(character));
    }

    public override InventoryItem Clone()
    {
        return new IlluminationSpell()
        {
            Amount = this.Amount,
            StackAmount = this.StackAmount
        };
    }

    private IEnumerator IlluminationCoroutine(Character character)
    {
        CharacterStats stats = character.GetStats();
        
        while (stats.Mana > 0f)
        {
            yield return null; // Wait for the next frame
            stats.UseMana(1f * Time.deltaTime); // Drain mana over time
        }

        character.ToggleTorch(false, 10);
        Debug.Log("Illumination spell ended due to lack of mana");
        
        DecreaseItemCount(character);
        currentCoroutine = null;
    }

    private void DecreaseItemCount(Character character)
    {
        Amount--;
        if (Amount <= 0)
        {
            Debug.Log("Removing illumination spell from inventory");
            RemoveFromInventory(character);
        }
    }
}