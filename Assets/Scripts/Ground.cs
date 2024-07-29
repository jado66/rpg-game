using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collider){
        
        if (collider.tag == "Player"){
            
            // Debug.Log("Player entered water");
            Player player = collider.GetComponent<Player>();
            player.inWater =true;
            if (!player.onBoat)
                player.animator.SetBool("swimming",true);
        }
    }
    void OnTriggerEnter2D(Collider2D collider){
        if (collider.tag == "Player"){
            
            Player player = collider.GetComponent<Player>();
            player.inWater =false;
            // Debug.Log("Player exited water");
            if (!player.onBoat)
                player.animator.SetBool("swimming",false);
        }
    }
}
