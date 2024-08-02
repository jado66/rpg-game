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

    public int health;

    public int maxHealth;

    void Start(){
        alive = true;
        // healthbarObject.SetActive(false);
    }

    public virtual void takeDamage(int amount){
        Debug.Log("Taking damage from player");
        // if (!alive)
        //     return;

        
        health -= amount;

        if (health <= 0){
            kill();
        }
        else{
            if (healthbarObject != null && !healthbarObject.activeSelf){
                healthbar.fillAmount = (float)(health)/(float)(maxHealth);
                healthbarObject.SetActive(true);
            }
        }
    }

    protected virtual void kill(){
        Debug.Log("Killing "+ name);
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
        Destroy(this.gameObject);
    }
}
