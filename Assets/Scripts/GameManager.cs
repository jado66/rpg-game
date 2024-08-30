using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public GameObject cameraTarget;

    private float zPos = -15f;

    void Start()
    {
        DontDestroyOnLoad(this);
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public void SetZPos(float newZPos){
        zPos = newZPos;
    }

    // Update is called once per frame
    void Update()
    {

        if (cameraTarget == null){
            cameraTarget=GameObject.FindWithTag("Character");
        }

        Vector3 position = transform.position;
        position.x = cameraTarget.transform.position.x;
        position.y = cameraTarget.transform.position.y;
        position.z = zPos;
        transform.position = position;    
    }
}
