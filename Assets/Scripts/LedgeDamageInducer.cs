using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDamageInducer : DamageInducer
{
    void OnTriggerEnter2D(Collider2D collider){
        
        Debug.Log("Entered ledge");
        if (collider.tag == "Character"){
            
            CharacterMovement character = collider.GetComponent<CharacterMovement>();
            character.EnterWater();
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if (collider.tag == "Player"){
            collider.gameObject.GetComponent<Player>().TakeDamage(damageDealt);
            // collision.gameObject.GetComponent<Player>().recoil(recoilForce,transform.position);
        }
    }
}
