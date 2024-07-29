using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;



public class SavedGameManager: MonoBehaviour
{
    public GameObject savedGamesGrid;

    public GameObject savedGameButton;

    GameData[] savedGames;

    public Tooltip tooltip;
    void Start(){
        // Load up saved gamesDatabase
        savedGames = loadSavedGames();

        foreach (var gameData in savedGames)
        {
            SavedGameButton newButton = Instantiate(savedGameButton,Vector3.zero,Quaternion.identity,savedGamesGrid.transform).GetComponent<SavedGameButton>();
            newButton.loadGameData(gameData);
            newButton.tooltip = tooltip;
        }
    }

    GameData[] loadSavedGames(){

        string[] directories = Directory.GetDirectories(Application.persistentDataPath + "/SavedGames");
        
        Debug.Log("Printing directories");
        for (int i = 0; i < directories.Length; i++)
        {
            directories[i] = new DirectoryInfo(System.IO.Path.GetDirectoryName(directories[i]+"/gameData.save")).Name;
            Debug.Log(directories[i]);
        }


        // foreach(var dir in directories)
        // {
        //     Debug.Log(dir.ToString());
        // }

        GameData[] games = new GameData[directories.Length];

        for (int i = 0; i < directories.Length; i++){
            string playerName = directories[i];
            string filePath = Application.persistentDataPath + "/SavedGames"+
                          string.Format("/{0}/gameData.save",playerName);
       
            Debug.Log("Trying to access "+filePath+"/gameData.save");
            GameData save;
            if (File.Exists(filePath))
            {
                
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(filePath, FileMode.Open);
                // save = new GameData("FileNotFound",0,0,"Error",0,0);
                save = (GameData)bf.Deserialize(file);
                file.Close();

            }
            else{
                save = new GameData("FileNotFound",0,0,"Error",0,0);
                Debug.Log("File not found"+ filePath);
            }
            games[i] = save;
        }
        return games;
    }
}

