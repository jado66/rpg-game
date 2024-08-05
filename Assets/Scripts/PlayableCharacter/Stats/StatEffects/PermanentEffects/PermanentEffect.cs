public class PermanentEffect : Effect
{
    public PermanentEffect(string name) : base(name, 0) { }

    public override void Apply(CharacterStats character)
    {
        // Implementation of applying the permanent effect (e.g., increased strength)
    }

    public override void Remove(CharacterStats character)
    {
        // Usually, permanent effects don't need to be removed; this might remain empty or handle exceptions
    }
}
