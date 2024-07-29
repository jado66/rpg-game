using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDamageInducer : DamageInducer
{
    void OnTriggerExit2D(Collider2D collider){
        if (collider.tag == "Player"){
            collider.gameObject.GetComponent<Player>().takeDamage(damageDealt);
            // collision.gameObject.GetComponent<Player>().recoil(recoilForce,transform.position);
        }
    }
}
