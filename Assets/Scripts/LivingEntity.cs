using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : Interactable
{
    public bool alive;
    public GameObject healthbarObject;
    public Image healthbar;

    public float deathVanishDelay = 5.0f;

    public float health;

    public float maxHealth;

    public EntitySpawner spawner;

    void Start(){
        alive = true;
        // healthbarObject.SetActive(false);
    }

    public virtual void TakeDamage(int amount){
    // Cast int to float and call the float version of the method
        TakeDamage((float)amount);
    }

    public virtual void TakeDamage(float amount){
        // Debug.Log("Taking damage from player");
        // if (!alive)
        //     return;

        
        health -= amount;
        Debug.Log($"{type} took {(float)amount} damage");

        if (health <= 0){
            kill();
        }
        else{
            if (healthbarObject != null){

                float fillAmount = (float)(health)/(float)(maxHealth);
                // Debug.Log($"Entity hit. Health percentage fill is {fillAmount}");
                healthbar.fillAmount = fillAmount;
                healthbarObject.SetActive(true);
            }
        }
    }

    protected virtual void kill(){

        if (!alive){
            return;
        }

        if (healthbarObject != null){
            healthbarObject.SetActive(false);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        alive = false;
        
        StartCoroutine(WaitAndDestroy());
    }

    protected IEnumerator WaitAndDestroy(){
        yield return new WaitForSeconds(deathVanishDelay);
        if (spawner != null){
            spawner.RemoveDeadEntity(this.gameObject);
        }
        Destroy(this.gameObject);
    }
}
