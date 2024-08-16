using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    public Player player;

    public string characterName;

    

    // Dialog Options buy/sell - ask a specific question to teach player

    

    public override void OnCharacterInteract(){
        // Open dialog box
        player = GameObject.Find("Character").GetComponent<Player>();
    }
    // Types off NPC's

    // Quest/Contract givers
    
    // Buy/Sell Merchants

    // Advice givers

    // Attack player?

    // Follow player

    // Make any npc can do all of the above if they want, mak 
}
