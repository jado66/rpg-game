using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Sign : Interactable
{   
   
    public bool customizable;

    Player player;

    PlayerUI playerUi;



    public string message = "Hello world";

    void Start(){
        player = GameObject.Find("player1").GetComponent<Player>();
        playerUi = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
        
    }
    public override void OnCharacterInteract(){
        base.OnCharacterInteract();
        
        Debug.Log("Interacted with sign");
        if (playerUi.customSignBox.activeSelf && playerUi.customSignBox.GetComponent<InputField>().isFocused){
            Debug.Log("We shouldn't be doing anything");
            return;}
        //Lets worry about the player state
        if (playerUi.dialogBox.activeSelf || playerUi.customSignBox.activeSelf)
            player.currentState = PlayerState.walk;
        else{
            player.currentState = PlayerState.standby;
        }

        if (!customizable){
            playerUi.dialogBoxText.text = message;
            playerUi.dialogBox.SetActive(!playerUi.dialogBox.activeSelf);
        }
        else{
            if (playerUi.customSignBox.activeSelf)
                message = playerUi.customSignText.text.ToString();
            else
                playerUi.customSignText.text = message;
            playerUi.customSignBox.SetActive(!playerUi.customSignBox.activeSelf);
        }
        // GameObject dialogBox = GameObject.Find("DialogBox");
        // Debug.Log(dialogBox == null?"No dialog box":"Found dialog box");
        // dialogBox.SetActive(!dialogBox.activeSelf);
        
    }
}
