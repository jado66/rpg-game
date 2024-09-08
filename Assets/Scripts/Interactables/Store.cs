using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Store : Interactable
{   
    public SceneManager sceneManager;
    public StoreInventory inventory;
    public bool isOpen = false;
    public bool autoOpen = true;  // New bool to determine if store should auto-open
    public GameObject dialogGameObject;
    private DialogManager dialogManager;         
    public UnityEvent onFinish;

    private CharacterWorldInteraction character;

    void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        dialogManager = GetComponent<DialogManager>();
    }

    public override void OnCharacterInteract(CharacterWorldInteraction newCharacter)
    {

        character = newCharacter;

        if (isOpen){
            CloseStore();
        }
        else{
            if (autoOpen)
            {
                OpenStore();
            }
            else if (dialogManager != null)
            {
                dialogManager.StartDialog();
            }
        }
    }

    public void OpenStore()
    {
        Debug.Log("Trying to open store");
        character.OpenStore(this);
        inventory.OpenStoreInventory();
        isOpen = true;
    }

    public void CloseStore() 
    {
        if(isOpen) {
            Debug.Log("Closing the chest.");
            
            inventory.CloseStoreInventory();
            isOpen = false;
        } else {
            Debug.Log("Chest is already closed.");
        }
    }

}