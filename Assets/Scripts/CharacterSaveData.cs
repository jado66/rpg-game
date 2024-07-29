using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSaveData
{
    public int[] charLocation = new int[3]; 

    public int level;
    public int health;
    public int mana;
    public int money;

    public CharacterSaveData(int[] charLocation, int level, int[] monitorStats, int money){
        this.charLocation = charLocation;
        this.level = level;
        this.health = monitorStats[0];
        this.mana = monitorStats[1];
        this.money = money;
    }
}



   