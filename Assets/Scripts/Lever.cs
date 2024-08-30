using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Lever : Interactable
{   
   
    bool leverDown;
    public Sprite leverDownSprite;

    public Sprite leverUpSprite;

    public virtual void toggleLever(){
        leverDown = !leverDown;
        Debug.Log("leverDown");
        if (leverDown){
            GetComponent<SpriteRenderer>().sprite = leverDownSprite;
        }
        else{
            GetComponent<SpriteRenderer>().sprite = leverUpSprite;
        }

    }
    public override void OnCharacterInteract(CharacterWorldInteraction interaction){
        
        toggleLever();
    }
}
