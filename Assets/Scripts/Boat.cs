using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Boat : Interactable
{
    Player player;

    TilePalette tilePalette;
    void Start(){
        tilePalette = GameObject.Find("TilePalette").GetComponent<TilePalette>();
        player = GameObject.Find("Character").GetComponent<Player>();
    }
    // Start is called before the first frame update
    public override void OnCharacterInteract(){
        player.boat = this;
        player.onBoat = !player.onBoat;
        // On boat
        if (player.currentState != PlayerState.standby){
            GetComponent<BoxCollider2D>().isTrigger = false;
            tilePalette.ground.GetComponent<TilemapCollider2D>().isTrigger = false;
            player.currentState = PlayerState.standby;
            if (player.inWater)
                player.animator.SetBool("swimming",false);
                player.animator.SetBool("moving",false);
            
            }
        // Off boat
        else{
            // needs to be coroutine
            GetComponent<BoxCollider2D>().isTrigger = true;

            tilePalette.ground.GetComponent<TilemapCollider2D>().isTrigger = true;

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
