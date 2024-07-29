using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInducer : MonoBehaviour
{
    public int damageDealt;
    public float recoilForce;
    // Start is called before the first frame update
    
    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag == "Player"){
            try{   
                GetComponent<Animator>().SetTrigger("attack");
                
            }
            catch{}
            
            Debug.Log("Collision");
            collision.gameObject.GetComponent<Player>().takeDamage(damageDealt);
            collision.gameObject.GetComponent<Player>().recoil(recoilForce,transform.position);
        }
    }

}
