using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public CharacterCombat character;
    private Dictionary<GameObject, float> hitCooldowns = new Dictionary<GameObject, float>();
    public float hitCooldownDuration = 0.05f; // Adjust this value as needed

    void Start()
    {
        if (character == null)
        {
            Transform parentOfParent = transform.parent.parent;

            character = parentOfParent.GetComponent<CharacterCombat>();
            if (character == null)
            {
                Debug.LogError("CharacterHitbox: Character component not found!");
            }
        }
    }

    void Update()
    {
        // Update cooldowns using LINQ
        hitCooldowns = hitCooldowns
            .Select(kvp => new { 
                Key = kvp.Key, 
                Value = kvp.Value - Time.deltaTime 
            })
            .Where(x => x.Value > 0)
            .ToDictionary(x => x.Key, x => x.Value);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject hitObject = collision.gameObject;

        if (!hitCooldowns.ContainsKey(hitObject) || hitCooldowns[hitObject] <= 0)
        {
            if (hitObject.TryGetComponent(out LivingEntity livingEntity))
            {
                ApplyKnockbackToEnemy(hitObject, livingEntity);
            }
            else if (hitObject.name == "CombatDummy")
            {
                ApplyKnockbackToDummy(hitObject);
            }
            else if (hitObject.TryGetComponent(out Pot pot))
            {
                pot.OnHit();
            }
            // Add more conditions here for other types of hittable objects

            // Set cooldown for the hit object
            hitCooldowns[hitObject] = hitCooldownDuration;
        }
    }

    private void ApplyKnockbackToEnemy(GameObject target, LivingEntity livingEntity)
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        float force = character.recoilForce;

        if (target.TryGetComponent(out Rigidbody2D rb))
        {
            ApplyKnockback(rb, direction, force);
        }

        livingEntity.TakeDamage(character.damageDealt);
    }

    private void ApplyKnockbackToDummy(GameObject dummy)
    {
        Debug.Log("Hit from character");
        if (dummy.TryGetComponent(out CombatDummy combatDummy))
        {
            combatDummy.health -= character.damageDealt;
        }

        Vector2 direction = (dummy.transform.position - character.transform.position).normalized;
        float force = character.recoilForce;

        if (dummy.TryGetComponent(out Rigidbody2D rb))
        {
            ApplyKnockback(rb, direction, force);
        }
    }

    private void ApplyKnockback(Rigidbody2D rb, Vector2 direction, float force)
    {
        rb.velocity = Vector2.zero; // Reset velocity before applying new force
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        StartCoroutine(KnockbackResetRoutine(rb));
    }

    private IEnumerator KnockbackResetRoutine(Rigidbody2D rb)
    {
        if (rb != null)
        {
            yield return new WaitForSeconds(character.knockTime);
            
            // Gradually reduce velocity instead of setting it to zero instantly
            float reductionTime = 0.15f; // Time over which to reduce velocity
            Vector2 initialVelocity = rb.velocity;
            
            for (float t = 0; t < reductionTime; t += Time.deltaTime)
            {
                rb.velocity = Vector2.Lerp(initialVelocity, Vector2.zero, t / reductionTime);
                yield return null;
            }
            
            rb.velocity = Vector2.zero;
        }
    }
}