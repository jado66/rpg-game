using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{   
    private Animator animator;

    SceneManager sceneManager;
    public ExternalInventory inventory;

    Player player;
    bool isOpen;

    public bool isLocked;

    void Start(){
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        player = GameObject.Find("Character").GetComponent<Player>();
        animator = GetComponent<Animator>();
        inventory = GetComponent<ExternalInventory>();

        if (inventory == null){
            return;
        }

        if (inventory.inventoryUI == null){
            PlayerUI playerUi = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
            GameObject externalInventoryPanels = playerUi.externalInventoryPanels;
            UIInventory uiInventory = externalInventoryPanels.GetComponent<UIInventory>();
            inventory.SetInventoryUI(uiInventory);
        }
    }
    public override void onCharacterInteract(){
        base.onCharacterInteract();
        Debug.Log("Interact with chest.");

        animator.SetBool("IsOpen",!isOpen);
        
         if (isLocked && !isOpen)
        {
            Debug.Log("Chest is locked.");
            return;
        }

        isOpen = !isOpen;

        if (isOpen){
            inventory.OpenExternalInventory();
        }
        else{
            inventory.CloseExternalInventory();
        }
        
        
        // animator.SetBool("IsOpen",!isOpen);
    }



    
}
