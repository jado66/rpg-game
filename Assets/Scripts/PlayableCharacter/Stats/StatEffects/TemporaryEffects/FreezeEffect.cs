using System;
using System.Collections;
using System.Collections.Generic;

public class FreezeEffect : TemporaryEffect
{
    private float originalSpeed;

    public FreezeEffect(float duration) : base("Freezing", duration) {}

    public override void Apply(CharacterStats character)
    {
        originalSpeed = character.Speed;
        character.ModifySpeed(0); // Freezes the character by setting speed to 0
    }

    public override void Remove(CharacterStats character)
    {
        character.ModifySpeed(originalSpeed); // Restore the original speed
    }
}
