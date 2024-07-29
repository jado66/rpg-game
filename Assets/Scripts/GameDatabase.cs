using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GameData{
    public string name;
    public int realGameSecondsPlayed;

    public int characterLevel;

    public string currentRealm;

    public int numberOfRealmsDiscovered;
    public int numberOfMiniRealmsDiscovered;

    public GameData(string name,int realGameSecondsPlayed, int characterLevel, string currentRealm, int numberOfMiniRealmsDiscovered, int numberOfRealmsDiscovered){
        this.characterLevel = characterLevel;
        this.currentRealm = currentRealm;
        this.numberOfMiniRealmsDiscovered = numberOfMiniRealmsDiscovered;
        this.numberOfRealmsDiscovered = numberOfRealmsDiscovered;
        this.name = name;
        this.realGameSecondsPlayed = realGameSecondsPlayed;

    }
}

[System.Serializable]
public class GameDatabase
{
    GameData[] savedGames;

    // settings

    GameDatabase(GameData[] savedGames){
        this.savedGames = savedGames;

    }
}


// [System.Serializable]
// public class SavedGamesDatabase
// {
//     public string[] savedGameNames;

//     public int numberOfSavedGames;

//     public SavedGamesDatabase(int numberOfSavedGames, string[] savedGameNames){
        
//         this.numberOfSavedGames = numberOfSavedGames; 

//         this.savedGameNames = savedGameNames;    
//     }
// }


   
   