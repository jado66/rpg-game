using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    bool buttonDown;
    public Sprite buttonDownSprite;

    public Sprite buttonUpSprite;

    // Start is called before the first frame update


    void toggleButton(){
        buttonDown = !buttonDown;
        Debug.Log("buttonDown");
        if (buttonDown){
            GetComponent<SpriteRenderer>().sprite = buttonDownSprite;
        }
        else{
            GetComponent<SpriteRenderer>().sprite = buttonUpSprite;
        }

    }
    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collider2D){
        
        if ((collider2D.gameObject.GetComponent("Player") as Player)!= null){
            if (!buttonDown){
                toggleButton();
            }
        }
    }
    void OnTriggerExit2D(Collider2D collider2D){
        if ((collider2D.gameObject.GetComponent("Player") as Player)!= null){
            if (buttonDown){
                toggleButton();
            }
        }
    }
}
