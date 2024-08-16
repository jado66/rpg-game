using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bed : Interactable
{   
    public override void OnCharacterInteract(){
        base.OnCharacterInteract();
        
        SceneManager sceneManagerScript = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        sceneManagerScript.PlayerSleeps();
        }
}
