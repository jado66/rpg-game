using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string type;
    public virtual void OnCharacterInteract(){
        // Debug.Log("Player interacted with a "+type);
    }

   

     public virtual void OnCharacterInteract(CharacterActions interaction){
        // Debug.Log("Player interacted with a "+type);
    }
}
