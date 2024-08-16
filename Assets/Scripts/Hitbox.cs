using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public CharacterCombat character;
    
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"CharacterHitbox: Collision detected with {collision.gameObject.name}");

        if (collision.TryGetComponent(out LivingEntity livingEntity))
        {
            ApplyKnockbackToEnemy(collision.gameObject, livingEntity);
        }
        else if (collision.gameObject.name == "CombatDummy")
        {
            ApplyKnockbackToDummy(collision.gameObject);
        }
        else if (collision.TryGetComponent(out Pot pot))
        {
            pot.OnHit();
        }
    }

    private void ApplyKnockbackToEnemy(GameObject target, LivingEntity livingEntity)
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        float force = character.recoilForce;

        if (target.TryGetComponent(out Rigidbody2D rb))
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(direction * force, ForceMode2D.Impulse);
            StartCoroutine(KnockbackResetRoutine(rb));
            Debug.Log($"CharacterHitbox: Applied force {direction * force} to {target.name}");
        }

        livingEntity.TakeDamage(character.damageDealt);
        Debug.Log($"CharacterHitbox: Dealt {character.damageDealt} damage to {target.name}");
    }

    private void ApplyKnockbackToDummy(GameObject dummy)
    {
        Debug.Log("hit from character");
        if (dummy.TryGetComponent(out CombatDummy combatDummy))
        {
            combatDummy.health -= character.damageDealt;
        }

        Vector2 direction = (dummy.transform.position - character.transform.position).normalized;
        float force = character.recoilForce;

        if (dummy.TryGetComponent(out Rigidbody2D rb))
        {
            rb.velocity = Vector2.zero; // Reset velocity before applying new force
            rb.AddForce(direction * force, ForceMode2D.Impulse);
            StartCoroutine(KnockbackResetRoutine(rb));
        }
    }

    private IEnumerator KnockbackResetRoutine(Rigidbody2D rb)
    {
        if (rb != null)
        {
            yield return new WaitForSeconds(character.knockTime);
            
            // Gradually reduce velocity instead of setting it to zero instantly
            float reductionTime = 0.5f; // Time over which to reduce velocity
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