using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour
{
    public bool alive;
    public GameObject healthbarObject;
    public Image healthbar;
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
        healthbar.fillAmount = (float)(health)/(float)(maxHealth);

        if (health <= 0){
            kill();
        }
        else{
            if (!healthbarObject.activeSelf)
                healthbarObject.SetActive(true);
        }
    }

    protected virtual void kill(){
        Debug.Log("Killing "+ name);
        healthbarObject.SetActive(false);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        alive = false;
        
        StartCoroutine(WaitAndDestroy());
    }

    protected IEnumerator WaitAndDestroy(){
        yield return new WaitForSeconds(150);
        Destroy(this.gameObject);
    }
}
