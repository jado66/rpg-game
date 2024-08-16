using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public ArmedMonster enemyEntity;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.5f;

    void Start()
    {
        if (enemyEntity == null)
        {
            enemyEntity = GetComponentInParent<ArmedMonster>();
            if (enemyEntity == null)
            {
                Debug.LogError("EnemyHitbox: LivingEntity component not found!");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"EnemyHitbox: Collision detected with {collision.gameObject.name}");

        if (collision.CompareTag("Character"))
        {
            ApplyKnockbackToPlayer(collision.gameObject);
            collision.GetComponent<CharacterStats>().TakeDamage(2f);
        }
    }

    private void ApplyKnockbackToPlayer(GameObject player)
    {
        if (player.TryGetComponent(out Rigidbody2D playerRb) && player.TryGetComponent(out Player playerScript))
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            playerRb.velocity = Vector2.zero;
            playerRb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
            StartCoroutine(KnockbackResetRoutine(playerRb));

            // Apply damage to the player
            playerScript.TakeDamage(enemyEntity.damageDealt);

            Debug.Log($"EnemyHitbox: Applied knockback and {enemyEntity.damageDealt} damage to player");
        }
    }

    private IEnumerator KnockbackResetRoutine(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(knockbackDuration);
        
        float reductionTime = 0.2f;
        Vector2 initialVelocity = rb.velocity;
        
        for (float t = 0; t < reductionTime; t += Time.deltaTime)
        {
            rb.velocity = Vector2.Lerp(initialVelocity, Vector2.zero, t / reductionTime);
            yield return null;
        }
        
        rb.velocity = Vector2.zero;
        Debug.Log($"EnemyHitbox: Finished knockback reset for player");
    }
}