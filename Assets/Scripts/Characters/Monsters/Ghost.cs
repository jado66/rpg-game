using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghost : NighttimeMonster
{

    public int damageDealt;
    public float fadeSpeed = 2f;
    // public GameObject particlePrefab; // TODO later
    // public int particleCount = 20;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Set ghost-specific properties

        // Make the ghost semi-transparent
        Color ghostColor = spriteRenderer.color;
        ghostColor.a = 0.5f;
        spriteRenderer.color = ghostColor;
    }

   

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!alive)
            return;

        if (collision.gameObject.CompareTag("Character"))
        {
           
            Character character = collision.gameObject.GetComponent<Character>();
            character.TakeDamage(damageDealt);
            character.ToggleTorch(false);
            DisappearViolently();
        
        }
    }

    void DisappearViolently()
    {

        kill();
        // StartCoroutine(FadeOutCoroutine());
        // SpawnParticles();
    }

    // IEnumerator FadeOutCoroutine()
    // {
    //     Color originalColor = spriteRenderer.color;
    //     float elapsedTime = 0f;

    //     while (elapsedTime < .11f)
    //     {
    //         elapsedTime += Time.deltaTime * fadeSpeed;
    //         float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime);
    //         spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    //         yield return null;
    //     }

    // }

    // void SpawnParticles()
    // {
    //     if (particlePrefab != null)
    //     {
    //         for (int i = 0; i < particleCount; i++)
    //         {
    //             GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
    //             Rigidbody2D rb = particle.GetComponent<Rigidbody2D>();
    //             if (rb != null)
    //             {
    //                 Vector2 randomDirection = Random.insideUnitCircle.normalized;
    //                 rb.AddForce(randomDirection * Random.Range(1f, 3f), ForceMode2D.Impulse);
    //             }
    //             Destroy(particle, 2f); // Destroy particle after 2 seconds
    //         }
    //     }
    // }

    protected override void kill()
    {
        gameObject.SetActive(false);
        // base.kill();
    }
}