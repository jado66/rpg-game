using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{

    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Character").GetComponent<Player>();
    }

    // Update is called once per frame
    
    
    void OnTriggerEnter2D(Collider2D collision){
        if ((collision.gameObject.GetComponent("LivingEntity") as LivingEntity) != null){
            
            LivingEntity livingEntity = collision.gameObject.GetComponent<LivingEntity>();
            
            int force = player.recoilForce;
            Vector3 direction = (collision.transform.position-player.transform.position).normalized;
            Rigidbody2D otherBody = collision.gameObject.GetComponent<Rigidbody2D>();
            otherBody.isKinematic = false;
            otherBody.AddForce(direction*force,ForceMode2D.Impulse);
            StartCoroutine(KnockbackCo(otherBody));
            livingEntity.takeDamage(player.damageDealt);
        }
        else if (collision.gameObject.name == "CombatDummy"){
            Debug.Log("hit from player");
            CombatDummy dummy = collision.gameObject.GetComponent<CombatDummy>();
            dummy.health -= player.damageDealt;
            int force = player.recoilForce;
            Vector3 direction = (collision.transform.position-player.transform.position).normalized;
            Rigidbody2D otherBody = collision.gameObject.GetComponent<Rigidbody2D>();
            otherBody.isKinematic = false;
            otherBody.AddForce(direction*force,ForceMode2D.Impulse);
            StartCoroutine(KnockbackCo(otherBody));
        }
        else if ((collision.gameObject.GetComponent("Pot") as Pot) != null){
            
            collision.gameObject.GetComponent<Pot>().OnHit();
        }
            
        
    
    }

    private IEnumerator KnockbackCo(Rigidbody2D otherBody){
        if (otherBody != null){
            yield return new WaitForSeconds(player.knockTime);
            otherBody.velocity = Vector2.zero;
            otherBody.bodyType = RigidbodyType2D.Dynamic;

        }
    }
}
