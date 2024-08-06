using UnityEngine;
using System;
using System.Collections.Generic;

public struct CharacterMoveData{

    public int day; //This helps us to find the right time; Find the day and time and then cut the queue there
    public float time;  
    public Vector2 movement;
    public List<KeyCode> buttonsPressed;
    public CharacterState cloneState; 

    public List<InventoryItem> itemsUsed; //TODO change to item
}