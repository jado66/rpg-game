using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string type;
    public virtual void onCharacterInteract(){
        // Debug.Log("Player interacted with a "+type);
    }

   

     public virtual void onCharacterInteract(CharacterActions interaction){
        // Debug.Log("Player interacted with a "+type);
    }
}
