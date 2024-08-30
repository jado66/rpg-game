using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : Interactable
{   

    SceneManager sceneManager;
    public StoreInventory inventory;

    public bool isOpen = false;
    void Start(){
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
    }
    public override void OnCharacterInteract(CharacterWorldInteraction character){
        Debug.Log("Made it here - Store");

        if (!isOpen){
            Debug.Log("Tying to open");
            character.OpenStore(this);
            inventory.OpenStoreInventory();
            isOpen = true;

        }
        else{
            character.CloseOpenStore();
            inventory.CloseStoreInventory();
        }
    }

    public void CloseStore() {
        if(isOpen) {
            Debug.Log("Closing the chest.");
            
            inventory.CloseStoreInventory();
            isOpen = false;
        } else {
            Debug.Log("Chest is already closed.");
        }
    }

    
}
