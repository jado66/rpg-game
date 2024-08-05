using System;
using System.Collections;
using System.Collections.Generic;
public class TemporaryEffect : Effect
{
    public TemporaryEffect(string name, float duration) : base(name, duration) { }

    public override void Apply(CharacterStats character)
    {
        // Implementation of applying the effect (e.g., freezing)
    }

    public override void Remove(CharacterStats character)
    {
        // Implementation of removing the effect (e.g., unfreezing)
    }
}