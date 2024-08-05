public class ModifyMaxHealthEffect : PermanentEffect
{
    private float healthIncreaseAmount;

    // Constructor with corrected name
    public ModifyMaxHealthEffect(float amount) : base("Permanent Health Max Modification")
    {
        healthIncreaseAmount = amount;
    }

    // Apply method to override the base class method, no return type needed (void)
    public override void Apply(CharacterStats characterStats)
    {
        characterStats.ModifyMaxHealth(healthIncreaseAmount);  // Increase max health
        // character.Health += healthIncreaseAmount;     // Optionally increase current health to match max health increase
    }

    // Remove method to override the base class method, no return type needed (void)
    public override void Remove(CharacterStats character)
    {
        // Permanent effects do not need to remove themselves
    }
}
