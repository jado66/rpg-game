using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Boat : Interactable
{
    Player player;

    TilePallete tilePallete;
    void Start(){
        tilePallete = GameObject.Find("TilePallete").GetComponent<TilePallete>();
        player = GameObject.Find("Character").GetComponent<Player>();
    }
    // Start is called before the first frame update
    public override void onPlayerInteract(){
        player.boat = this;
        player.onBoat = !player.onBoat;
        // On boat
        if (player.currentState != PlayerState.standby){
            GetComponent<BoxCollider2D>().isTrigger = false;
            tilePallete.ground.GetComponent<TilemapCollider2D>().isTrigger = false;
            player.currentState = PlayerState.standby;
            if (player.inWater)
                player.animator.SetBool("swimming",false);
                player.animator.SetBool("moving",false);
            
            }
        // Off boat
        else{
            // needs to be coroutine
            GetComponent<BoxCollider2D>().isTrigger = true;

            tilePallete.ground.GetComponent<TilemapCollider2D>().isTrigger = true;

            if (player.inWater){
                player.currentState = PlayerState.swim;
                player.animator.SetBool("swimming",true);
            }
            else{
                player.currentState = PlayerState.walk;
                // player.animator.SetBool("swimming",true);
            }
        }

        player.transform.position = transform.position + new Vector3(.2f,1,0);
        // Debug.Log("Player interacted with a "+type);
    }
}
