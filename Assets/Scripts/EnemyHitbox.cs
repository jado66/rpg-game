using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{

    ArmedMonster enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = transform.parent.GetComponent<ArmedMonster>();
    }

    // Update is called once per frame
    
    
    void OnTriggerEnter2D(Collider2D collision){
        if ((collision.gameObject.GetComponent("Player") as Player) != null){
            Debug.Log("hit from enemy");
            Player player = collision.gameObject.GetComponent<Player>();
            
            int force = enemy.recoilForce;
            Vector3 direction = (collision.transform.position-enemy.transform.position).normalized;
            Rigidbody2D otherBody = collision.gameObject.GetComponent<Rigidbody2D>();
            otherBody.isKinematic = false;
            otherBody.AddForce(direction*force,ForceMode2D.Impulse);
            StartCoroutine(KnockbackCo(otherBody));
            player.takeDamage(enemy.damageDealt);
        }
        
    }

    private IEnumerator KnockbackCo(Rigidbody2D otherBody){
        if (otherBody != null){
            yield return new WaitForSeconds(.2f);
            otherBody.velocity = Vector2.zero;
            otherBody.bodyType = RigidbodyType2D.Dynamic;

        }
    }
}
