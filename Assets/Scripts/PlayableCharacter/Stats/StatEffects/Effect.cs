public abstract class Effect
{
    public string Name { get; }
    public float Duration { get; protected set; } // Duration in seconds, 0 means permanent

    public Effect(string name, float duration)
    {
        Name = name;
        Duration = duration;
    }

    public abstract void Apply(CharacterStats character);
    public abstract void Remove(CharacterStats character); // Called for temporary effects when duration ends
}