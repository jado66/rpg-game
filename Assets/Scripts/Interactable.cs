using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string type;
    public virtual void onPlayerInteract(){
        // Debug.Log("Player interacted with a "+type);
    }
}
