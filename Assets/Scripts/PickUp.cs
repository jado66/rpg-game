using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // Start is called before the first frame update
    public string type;
    void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.name == "Character" || collider.gameObject.name == "Character(Clone)" ){
            if (collider.isTrigger){
                Player playerScript = collider.GetComponent<Player>();
                if (type == "GoldCoin")
                    playerScript.money+= 100;
                else if (type == "Heart"){
                    if (playerScript.health< playerScript.healthMax)
                        playerScript.health++;
                }
                Destroy(gameObject);
                // Debug.Log("I should be destroyed");
            }
        }
    }   
}
