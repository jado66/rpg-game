using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;
using System.IO;


public class GameSaver : MonoBehaviour
{
    Character character;
    SceneManager sceneManager;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("Player1").GetComponent<Character>(); 
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>(); 
        // SaveGame();
    }

    // Update is called once per frame
    

    GameData collectGameData(){

        string characterName = character.playerName;
        // int realGameSecondsPlayed = character.realGameSecondsPlayed;

        // int characterLevel = character.level;

        // string currentRealm = sceneManager.realmName;

        // int numberOfRealmsDiscovered = character.howManyRealmsHasCharacterDiscovered(true);
        // int numberOfMiniRealmsDiscovered = character.howManyRealmsHasCharacterDiscovered(false);

        GameData gameData = new GameData(characterName, 0, 0, "", 0, 0);

        return gameData;
    }

    void SaveGame(){
        string characterName = character.playerName;

        GameData gameData =  collectGameData();

        if(!Directory.Exists(Application.persistentDataPath + "/SavedGames"))
        {    
            //if it doesn't, create it
            Directory.CreateDirectory(Application.persistentDataPath + "/SavedGames");
        
        }

        if(!Directory.Exists(Application.persistentDataPath + "/SavedGames/"+characterName))
        {    
            //if it doesn't, create it
            Directory.CreateDirectory(Application.persistentDataPath + "/SavedGames/"+characterName);
        
        }

        string filePath = Application.persistentDataPath + "/SavedGames"+
                          string.Format("/{0}/gameData.save",characterName);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        Debug.Log("Trying to save "+filePath);

        bf.Serialize(file, gameData);
        file.Close();
    }
}
