using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeSwitch : MonoBehaviour
{
    GameObject player;
    SceneManager sceneManager;
    public float time;
    public bool freezeTime;

    void Start()
    {

        player = GameObject.FindWithTag("Player");
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collider){
        sceneManager.pauseTimeSpell(time);
    }
}
