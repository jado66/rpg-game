using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collider){
        if (collider.tag == "Player"){
            collider.GetComponent<Player>().fallOffPlatform();
        }
    }
}
