using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SavedGame
{
    string name;
    int gameSeed;

    int[] charLocation = new int[3];

    int day = 0;
    int normalizedHour = 0;

    int realGameSecondsPlayed;

    int characterLevel;

    string currentRealm;

    int numberOfRealmsDiscovered;
    int numberOfMiniRealmsDiscovered;

    CharacterSaveData characterSaveData;

    SavedGame(){
        
    
        SceneManager sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        this.day = sceneManager.day;
        this.normalizedHour = (int)(sceneManager.normalizedHour);

        saveCharacterData(); 
    }

    public void LoadGame(){
        SceneManager sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        sceneManager.day = this.day;
        sceneManager.normalizedHour = this.normalizedHour;
        currentRealm = sceneManager.realmName;

        loadCharacterData(); 
    }

    public void saveCharacterData(){
        Player player = GameObject.Find("Character").GetComponent<Player>();
        
        int[] position = new int[]{(int)(player.transform.position.x),
                                   (int)(player.transform.position.y),
                                   (int)(player.transform.position.z)};

        int[] monitorStats = new int[]{(int)(player.health),(int)(player.mana)};

        characterSaveData = new CharacterSaveData(position,1,monitorStats, player.money);
    
        realGameSecondsPlayed = player.realGameSecondsPlayed;

        characterLevel = player.level;

        numberOfRealmsDiscovered = player.howManyRealmsHasPlayerDiscovered(true);
        numberOfMiniRealmsDiscovered = player.howManyRealmsHasPlayerDiscovered(false);
    }

    public void loadCharacterData(){
        Player player = GameObject.Find("Character").GetComponent<Player>();

        player.transform.position = new Vector3(characterSaveData.charLocation[0],characterSaveData.charLocation[1],characterSaveData.charLocation[2]);
        player.health = characterSaveData.health;
        player.mana = characterSaveData.mana;
        player.money = characterSaveData.money;
    }
}



   