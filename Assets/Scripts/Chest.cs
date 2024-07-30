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

    void Start(){
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        player = GameObject.Find("Character").GetComponent<Player>();
        animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();

        if (inventory.inventoryUI == null){
            GameObject externalInventoryPanel = GameObject.Find("ExternalInventoryPanel");
            UIInventory uiInventory = externalInventoryPanel.GetComponent<UIInventory>();
            inventory.SetInventoryUI(uiInventory);
        }
    }
    public override void onPlayerInteract(){
        base.onPlayerInteract();
        
        animator.SetBool("IsOpen",!isOpen);
        
        isOpen = !isOpen;
        StartCoroutine(playerInteract());
        // animator.SetBool("IsOpen",!isOpen);
    }

    public IEnumerator playerInteract(){
        sceneManager.loadAndUnloadChest(inventory,isOpen);
        yield return null;
    }
}
