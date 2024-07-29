using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;
using System.IO;


public class GameSaver : MonoBehaviour
{
    Player player;
    SceneManager sceneManager;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Character").GetComponent<Player>(); 
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>(); 
        // SaveGame();
    }

    // Update is called once per frame
    

    GameData collectGameData(){

        string playerName = player.playerName;
        int realGameSecondsPlayed = player.realGameSecondsPlayed;

        int characterLevel = player.level;

        string currentRealm = sceneManager.realmName;

        int numberOfRealmsDiscovered = player.howManyRealmsHasPlayerDiscovered(true);
        int numberOfMiniRealmsDiscovered = player.howManyRealmsHasPlayerDiscovered(false);

        GameData gameData = new GameData(playerName, realGameSecondsPlayed, characterLevel, currentRealm, numberOfMiniRealmsDiscovered, numberOfRealmsDiscovered);

        return gameData;
    }

    void SaveGame(){
        string playerName = player.playerName;

        GameData gameData =  collectGameData();

        if(!Directory.Exists(Application.persistentDataPath + "/SavedGames"))
        {    
            //if it doesn't, create it
            Directory.CreateDirectory(Application.persistentDataPath + "/SavedGames");
        
        }

        if(!Directory.Exists(Application.persistentDataPath + "/SavedGames/"+playerName))
        {    
            //if it doesn't, create it
            Directory.CreateDirectory(Application.persistentDataPath + "/SavedGames/"+playerName);
        
        }

        string filePath = Application.persistentDataPath + "/SavedGames"+
                          string.Format("/{0}/gameData.save",playerName);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        Debug.Log("Trying to save "+filePath);

        bf.Serialize(file, gameData);
        file.Close();
    }
}
