using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bed : Interactable
{   
    public override void onPlayerInteract(){
        base.onPlayerInteract();
        
        SceneManager sceneManagerScript = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        sceneManagerScript.playerSleeps();
        }
}
