using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{   
    private Animator animator;

    SceneManager sceneManager;
    public Inventory inventory;

    Player player;
    bool isOpen;

    public bool isLocked;

    void Start(){
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        player = GameObject.Find("Character").GetComponent<Player>();
        animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();

        if (inventory.inventoryUI == null){
            PlayerUI playerUi = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
            GameObject externalInventoryPanels = playerUi.externalInventoryPanels;
            UIInventory uiInventory = externalInventoryPanels.GetComponent<UIInventory>();
            inventory.SetInventoryUI(uiInventory);
        }
    }
    public override void onPlayerInteract(){
        base.onPlayerInteract();
        Debug.Log("Interact with chest.");

        animator.SetBool("IsOpen",!isOpen);
        
         if (isLocked && !isOpen)
        {
            Debug.Log("Chest is locked.");
            return;
        }

        isOpen = !isOpen;
        StartCoroutine(playerInteract());
        // animator.SetBool("IsOpen",!isOpen);
    }

    public IEnumerator playerInteract(){
        sceneManager.loadAndUnloadChest(inventory,isOpen);
        yield return null;
    }

    void OnDestroy()
    {
        // Ensure chest is unloaded if it was open
        if (isOpen)
        {
            sceneManager.loadAndUnloadChest(inventory, false);
        }
    }
}
