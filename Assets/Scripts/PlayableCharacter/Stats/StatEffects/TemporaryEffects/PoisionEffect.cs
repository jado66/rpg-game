using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : Effect
{
    private float damagePerTick;
    private float tickInterval;

    public PoisonEffect(float damagePerTick, float tickInterval, float duration) : base("Poison", duration)
    {
        this.damagePerTick = damagePerTick;
        this.tickInterval = tickInterval;
    }

    public override void Apply(CharacterStats character)
    {
        character.StartCoroutine(ApplyPoison(character));
    }

    private IEnumerator ApplyPoison(CharacterStats character)
    {
        float elapsedTime = 0;

        while (elapsedTime < Duration)
        {
            character.TakeDamage(damagePerTick);
            elapsedTime += tickInterval;

            yield return new WaitForSeconds(tickInterval);
        }

        Remove(character); // Clean up when the poison effect ends
    }

    public override void Remove(CharacterStats character)
    {
        // Optionally handle any cleanup if necessary.
    }
}
