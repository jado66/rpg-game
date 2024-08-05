using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public GameObject openSprite;

    public GameObject closedSprite;
    public GameObject linkedTeleport;


    public bool startOpen;

    public bool isLocked;

    public int keyID;
    bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = startOpen;

        if (isOpen){
            closedSprite.SetActive(false);
            openSprite.SetActive(true);
            linkedTeleport.GetComponent<BoxCollider2D>().enabled = true;
        }
        else{
            closedSprite.SetActive(true);
            openSprite.SetActive(false);
            linkedTeleport.GetComponent<BoxCollider2D>().enabled = false;
        }

    }

    void toggleDoor(){
        isOpen = !isOpen;
        if (isOpen){
            closedSprite.SetActive(false);
            openSprite.SetActive(true);
            linkedTeleport.GetComponent<BoxCollider2D>().enabled = true;
        }
        else{
            closedSprite.SetActive(true);
            openSprite.SetActive(false);
            linkedTeleport.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    // Update is called once per frame
    public override void onCharacterInteract(){
        if (!isLocked || isOpen ){
            toggleDoor();
        }
        // Debug.Log("Player interacted with a "+type);
    }
}
