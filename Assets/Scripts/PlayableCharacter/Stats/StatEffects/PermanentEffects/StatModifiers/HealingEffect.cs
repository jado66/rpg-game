public class HealingEffect : PermanentEffect
{
    private float amount;

    public HealingEffect(float amount) : base("Health Potion")
    {
        this.amount = amount;
    }

    public override void Apply(CharacterStats character)
    {
        character.Heal(amount);
    }

    public override void Remove(CharacterStats character)
    {
        // No removal logic needed here, since it's a one-time effect
    }
}
