using UnityEngine;
using System.Collections;

public class ArmedMonster : Monster
{
    public int damageDealt;
    public int recoilForce;
    public int timeBetweenSwings;
    public Chest chest;

    private int swingTimer;
    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();
        InitializeArmedMonster();
    }

    private void InitializeArmedMonster()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        chest = gameObject.AddComponent<Chest>();
        chest.inventory = gameObject.GetComponent<ExternalInventory>();
        chest.isLocked = true;
    }

    protected override void Update()
    {
        base.Update();
        UpdateHealthbar();
        UpdateSwingTimer();
    }

    private void UpdateHealthbar()
    {
        if (healthbar != null)
        {
            healthbar.fillAmount = (float)(health) / (float)(maxHealth);
        }
    }

    private void UpdateSwingTimer()
    {
        if (swingTimer > 0)
        {
            swingTimer++;
            if (swingTimer >= timeBetweenSwings)
                swingTimer = 0;
        }
    }

    protected override void HandleMovement()
    {
        base.HandleMovement();

        if (animators.Count > 0 && animators[0].GetBool("attacking"))
        {
            animators[0].SetBool("attacking", false);
        }
    }

    private IEnumerator AttackCo()
    {
        Debug.Log("Attacking");
        yield return null; // Wait a frame
        if (animators.Count > 0)
        {
            animators[0].SetTrigger("attack");
        }
        yield return new WaitForSeconds(0.2f);
        yield return null;
    }

    public void Attack()
    {
        StartCoroutine(AttackCo());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!alive) return;
        
        Debug.Log("Collision");
        if (collision.gameObject.CompareTag("Character"))
        {
            StartCoroutine(AttackCo());
        }
    }

    protected override void kill()
    {
        Debug.Log("Killing " + name);
        healthbarObject.SetActive(false);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        alive = false;
        chest.isLocked = false;

        if (animators.Count > 0)
        {
            animators[0].SetTrigger("die");
        }

        if (spriteRenderer != null)
        {
            StartCoroutine(FadeAndDestroy());
        }
        else
        {
            StartCoroutine(WaitAndDestroy());
        }
    }

    private IEnumerator FadeAndDestroy()
    {
        Debug.Log("Fading");
        float elapsedTime = 0f;
        yield return new WaitForSeconds(deathVanishDelay);

        Color originalColor = spriteRenderer.color;

        while (elapsedTime < 5.0f)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / 5.0f);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!alive) return;

        if (collision.CompareTag("Player") && swingTimer <= 0)
        {
            StartCoroutine(AttackCo());
            swingTimer++;
        }
    }

    // Add this method if it's not in the base Monster class
    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(deathVanishDelay + 5.0f);
        Destroy(gameObject);
    }
}