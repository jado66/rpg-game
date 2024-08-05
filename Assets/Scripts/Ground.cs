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

        if (collider.tag == "Character"){
            Debug.Log("Player1 exited water");

            CharacterMovement character = collider.GetComponent<CharacterMovement>();
            character.ExitWater();
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

        if (collider.tag == "Character"){
            
            CharacterMovement character = collider.GetComponent<CharacterMovement>();
            character.EnterWater();
        }
    }
}
