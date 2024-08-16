using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CharacterStats : MonoBehaviour
{

    [SerializeField]
    private float money;

    [SerializeField]
    private float health;
    
    [SerializeField]
    private float maxHealth;
    
    [SerializeField]
    private float mana;
    
    [SerializeField]
    private float maxMana;
    
    [SerializeField]
    private float stamina;

    [SerializeField]
    private float staminaDrainPerSecond;
    
    [SerializeField]
    private float maxStamina;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float swimSpeed;

    [SerializeField]
    private float runSpeedMultiplier;

    [SerializeField]
    private float fastSwimSpeedMultiplier;

    private Character character;

    private List<Effect> activeEffects = new List<Effect>();

    public float Money
    {
        get => money;
        private set => money = value; 
    }

    public void AddMoney(float amount)
    {
        Money += amount; // This will use the property setter, including clamping
    }

    public void SubtractMoney(float amount)
    {
        Money -= amount; // This will use the property setter, including clamping
    }

    public float Health
    {
        get => health;
        private set => health = Mathf.Clamp(value, 0, MaxHealth); 
    }

    public void Heal(float amount)
    {
        Health += amount; // This will use the property setter, including clamping
    }

    public void TakeDamage(float amount){
        Health -= amount;
        if (Health <= 0)
        {
            character.Die();
        }
    }

    public float MaxHealth
    {
        get => maxHealth;
        private set => maxHealth = value; 
    }

    public void ModifyMaxHealth(float amount)
    {
        MaxHealth += amount;
    }
    
    public float Mana
    {
        get => mana;
        private set => mana = Mathf.Clamp(value, 0, MaxMana);
    }

    public void ModifyMana(float amount)
    {
        Mana += amount;
    }

    public void RegainMana(float amount)
    {
        Mana += amount; // This will use the property setter, including clamping
    }

    public void UseMana(float amount)
    {
        Mana -= amount; // This will use the property setter, including clamping
    }
    

    public float MaxMana
    {
        get => maxMana;
        private set => maxMana = value; 
    }

    public void ModifyMaxMana(float amount)
    {
        MaxMana += amount;
    }

    public float Stamina
    {
        get => stamina;
        private set => stamina = Mathf.Clamp(value, 0, MaxStamina);
    }

    

    public void DepleteStamina(float amount)
    {
        Stamina -= amount; // This will use the property setter, including clamping
    }

    public void RegainStamina(float amount)
    {
        Stamina += amount; // This will use the property setter, including clamping
    }
    

    public void ModifyStamina(float amount)
    {
        Stamina += amount; // This will use the property setter, including clamping
    }
    public float MaxStamina
    {
        get => maxStamina;
        private set => maxStamina = value; 
    }

    public void ModifyMaxStamina(float amount)
    {
        MaxStamina += amount; // This will use the property setter, including clamping
    }

    public float StaminaDrainPerSecond
    {
        get => staminaDrainPerSecond;
        private set => staminaDrainPerSecond = value; 
    }

    public float Speed
    {
        get => speed;
        private set => speed = value; 
    }

    public void ModifySpeed(float amount){
        Speed += amount;
    }

    public float SwimSpeed
    {
        get => swimSpeed;
        private set => swimSpeed = value; 
    }

    public void ModifySwimSpeed(float amount){
        SwimSpeed += amount;
    }

    public float RunSpeedMultiplier
    {
        get => runSpeedMultiplier;
        private set => runSpeedMultiplier = value; 
    }

    public float FastSwimSpeedMultiplier
    {
        get => fastSwimSpeedMultiplier;
        private set => fastSwimSpeedMultiplier = value; 
    }

    public void ApplyEffect(Effect effect)
    {
        effect.Apply(this);
        if (effect.Duration > 0)
        {
            activeEffects.Add(effect);
            StartCoroutine(RemoveEffectAfterDuration(effect));
        }
    }

    private IEnumerator RemoveEffectAfterDuration(Effect effect)
    {
        yield return new WaitForSeconds(effect.Duration);
        effect.Remove(this);
        activeEffects.Remove(effect);
    }

    public void InitializeComponents(Character characterRef){
        character = characterRef;
    }
}