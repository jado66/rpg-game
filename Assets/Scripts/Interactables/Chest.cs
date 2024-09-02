using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{   
    public Animator animator;

    SceneManager sceneManager;
    public StoreInventory inventory;

    bool isOpen;

    public bool isLocked;

    void Start(){
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        // if (inventory == null){
        //     return;
        // }

        // if (inventory.inventoryUI == null){
        //     PlayerUI playerUi = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
        //     GameObject externalInventoryPanels = playerUi.externalInventoryPanels;
        //     UIInventory uiInventory = externalInventoryPanels.GetComponent<UIInventory>();
        //     inventory.SetInventoryUI(uiInventory);
        // }
    }
    public override void OnCharacterInteract(CharacterWorldInteraction character){
        // base.OnCharacterInteract();

        if (animator != null){
            animator.SetBool("IsOpen",!isOpen);
        }
        Debug.Log("Made it here");

        // if (isLocked && !isOpen)
        // {
        //     Debug.Log("Chest is locked.");
        //     return;
        // }

        isOpen = !isOpen;

        if (isOpen && !isLocked){
            Debug.Log("Tying to open");
            character.OpenChest(this);
            inventory.OpenStoreInventory();
        }
        else{
            Debug.Log("Tying to open");
            character.CloseOpenChest();
            inventory.CloseStoreInventory();
        }
        
        
        
        // animator.SetBool("IsOpen",!isOpen);
    }

    public void CloseChest() {
        if(isOpen) {
            Debug.Log("Closing the chest.");
            
            if(animator != null) {
                animator.SetBool("IsOpen", false);
            }

            inventory.CloseStoreInventory();
            isOpen = false;
        } else {
            Debug.Log("Chest is already closed.");
        }
    }

    void OnDestroy() {
        CloseChest();
    }

    
}
