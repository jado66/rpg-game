using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public struct tileMapData
{
    public int startX { get; set; }
    public int startY { get; set; }
    public int lengthX { get; set; }
    public string[] tiles { get; set; }    

    
}


public class SceneSaver : MonoBehaviour
{
    public bool MasterSave; 
    public SceneManager sceneManager;
    public TilePalette tilePalette;

    int keyCount = 0;

    void Start(){
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        tilePalette = sceneManager.tilePalette;

        if (MasterSave){
            SaveMasterScene();
        }
    }

    void Update(){
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.S) && keyCount ==0)
            {
                keyCount ++;
                SaveMasterScene();
            }

    }
    public void SaveMasterScene(){
        StartCoroutine(SaveMasterSceneCo());
    }

    private IEnumerator SaveLocalScene(){
        string playerName = sceneManager.player1.playerName;
        string filePath = Application.persistentDataPath + "/SavedGames"+
                            string.Format("/{0}/{1}-Data.save",playerName,sceneManager.getSceneName());
        

        SceneData sceneData = null;
        // Open file and save as SceneData
        if (File.Exists(filePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(filePath, FileMode.Open);
                // save = new GameData("FileNotFound",0,0,"Error",0,0);
                sceneData = (SceneData)bf.Deserialize(file);
                file.Close();

            }
        else{
            string masterFilePath = Application.persistentDataPath + "/SavedGames"+
                        string.Format("/{0}/{1}-Data.save",playerName,sceneManager.getSceneName());
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(masterFilePath, FileMode.Open);
            // save = new GameData("FileNotFound",0,0,"Error",0,0);
            sceneData = (SceneData)bf.Deserialize(file);
            file.Close();
        }

        // Take modifications from sceneManager

        int row;
        int col;
        int tileIndex;

        foreach (var modification in tilePalette.modifications)
        {
            

            // choppableTileMap, collidableTileMap, groundTileMap, minableTileMap, decorTileMap, interactableTileMap, noBuildZoneTileMap
            switch (modification.layer)
            {
                // These might be faster as Enums
                case "choppable":
                    row = (modification.yCoord-sceneData.choppableTileMap.startY)-1;
                    col = (modification.xCoord - sceneData.choppableTileMap.startX);
                    tileIndex = row*sceneData.choppableTileMap.lengthX + col;
                    sceneData.choppableTileMap.tileNames[tileIndex] = modification.tileName;
                    break;
                case "collidable":
                    row = (modification.yCoord-sceneData.collidableTileMap.startY)-1;
                    col = (modification.xCoord - sceneData.collidableTileMap.startX);
                    tileIndex = row*sceneData.collidableTileMap.lengthX + col;
                    sceneData.collidableTileMap.tileNames[tileIndex] = modification.tileName;
                    break;
                case "ground":
                    row = (modification.yCoord-sceneData.groundTileMap.startY)-1;
                    col = (modification.xCoord - sceneData.groundTileMap.startX);
                    tileIndex = row*sceneData.groundTileMap.lengthX + col;
                    sceneData.groundTileMap.tileNames[tileIndex] = modification.tileName;
                    break;
                case "minable":
                    row = (modification.yCoord-sceneData.minableTileMap.startY)-1;
                    col = (modification.xCoord - sceneData.minableTileMap.startX);
                    tileIndex = row*sceneData.minableTileMap.lengthX + col;
                    sceneData.minableTileMap.tileNames[tileIndex] = modification.tileName;
                    break;
                case "decor":
                    row = (modification.yCoord-sceneData.decorTileMap.startY)-1;
                    col = (modification.xCoord - sceneData.decorTileMap.startX);
                    tileIndex = row*sceneData.decorTileMap.lengthX + col;
                    sceneData.decorTileMap.tileNames[tileIndex] = modification.tileName;
                    break;
                case "interactable":
                    row = (modification.yCoord-sceneData.interactableTileMap.startY)-1;
                    col = (modification.xCoord - sceneData.interactableTileMap.startX);
                    tileIndex = row*sceneData.interactableTileMap.lengthX + col;
                    sceneData.interactableTileMap.tileNames[tileIndex] = modification.tileName;
                    break;
                case "noBuildZone":
                    row = (modification.yCoord-sceneData.noBuildZoneTileMap.startY)-1;
                    col = (modification.xCoord - sceneData.noBuildZoneTileMap.startX);
                    tileIndex = row*sceneData.noBuildZoneTileMap.lengthX + col;
                    sceneData.noBuildZoneTileMap.tileNames[tileIndex] = modification.tileName;
                    break;
                default:  
                    break;
            }
        }

        yield return null;
        //Save back to file
    }


    private IEnumerator SaveMasterSceneCo(){
        // Choppable
        yield return new WaitForSeconds(1);
        Debug.Log("Saving Master Scene");

        BoundsInt choppableBounds = tilePalette.choppable.cellBounds;
        TileBase[] choppableTiles = tilePalette.choppable.GetTilesBlock(choppableBounds);
        TileMapData choppableTileMap = new TileMapData();

        Debug.Log("Trying to save choppable");
        choppableTileMap.startX = tilePalette.choppable.cellBounds.xMin;
        choppableTileMap.startY = tilePalette.choppable.cellBounds.yMin;
        choppableTileMap.lengthX = choppableBounds.size.x;
        string[] tileNames = new string[choppableTiles.Length];

        Debug.Log("Choppable has "+ choppableTiles.Length.ToString()+" tiles");
        for(int i =0;i<choppableTiles.Length;i++){
            if (choppableTiles[i] != null){
                // Debug.Log("Choppable Tiles = "+choppableTiles[i].name);
                tileNames[i] = choppableTiles[i].name;
                }
            else
                tileNames[i] = "null";
        }

        // Collidable
        BoundsInt collidableBounds = tilePalette.collidable.cellBounds;
        TileBase[] collidableTiles = tilePalette.collidable.GetTilesBlock(collidableBounds);
        TileMapData collidableTileMap = new TileMapData();
        collidableTileMap.startX = tilePalette.collidable.cellBounds.xMin;
        collidableTileMap.startY = tilePalette.collidable.cellBounds.yMin;
        collidableTileMap.lengthX = collidableBounds.size.x;
        tileNames = new string[collidableTiles.Length];

        Debug.Log("Collidable has "+ collidableTiles.Length.ToString()+" tiles");

        for(int i =0;i<collidableTiles.Length;i++){
            if (collidableTiles[i] != null){
                // Debug.Log("Collidable Tiles = "+collidableTiles[i].name);
                tileNames[i] = collidableTiles[i].name;
                }
            else
                tileNames[i] = "null";
        }

        // Ground
        BoundsInt groundBounds = tilePalette.ground.cellBounds;
        TileBase[] groundTiles = tilePalette.ground.GetTilesBlock(groundBounds);
        TileMapData groundTileMap = new TileMapData();
        groundTileMap.startX = tilePalette.ground.cellBounds.xMin;
        groundTileMap.startY = tilePalette.ground.cellBounds.yMin;
        groundTileMap.lengthX = groundBounds.size.x;
        tileNames = new string[groundTiles.Length];

        Debug.Log("Ground has "+ groundTiles.Length.ToString()+" tiles");

        for(int i =0;i<groundTiles.Length;i++){
            if (groundTiles[i] != null){
                // Debug.Log("Ground Tiles = "+groundTiles[i].name);
                tileNames[i] = groundTiles[i].name;
                }
            else
                tileNames[i] = "null";
        }

        // minable
        BoundsInt minableBounds = tilePalette.minable.cellBounds;
        TileBase[] minableTiles = tilePalette.minable.GetTilesBlock(minableBounds);
        TileMapData minableTileMap = new TileMapData();
        minableTileMap.startX = tilePalette.minable.cellBounds.xMin;
        minableTileMap.startY = tilePalette.minable.cellBounds.yMin;
        minableTileMap.lengthX = minableBounds.size.x;
        tileNames = new string[minableTiles.Length];

        Debug.Log("Minable has "+ minableTiles.Length.ToString()+" tiles");
        for(int i =0;i<minableTiles.Length;i++){
            if (minableTiles[i] != null){
                // Debug.Log("Minable Tiles = "+minableTiles[i].name);
                tileNames[i] = minableTiles[i].name;
                }
            else
                tileNames[i] = "null";
        }

        //Decor
        BoundsInt decorBounds = tilePalette.decor.cellBounds;
        TileBase[] decorTiles = tilePalette.decor.GetTilesBlock(decorBounds);
        TileMapData decorTileMap = new TileMapData();
        decorTileMap.startX = tilePalette.decor.cellBounds.xMin;
        decorTileMap.startY = tilePalette.decor.cellBounds.yMin;
        decorTileMap.lengthX = decorBounds.size.x;
        tileNames = new string[decorTiles.Length];

        Debug.Log("Decor has "+ decorTiles.Length.ToString()+" tiles");
        for(int i =0;i<decorTiles.Length;i++){
            if (decorTiles[i] != null){
                // Debug.Log("Decor Tiles = "+decorTiles[i].name);
                tileNames[i] = decorTiles[i].name;
                }
            else
                tileNames[i] = "null";
        }

        // interactable
        BoundsInt interactableBounds = tilePalette.interactable.cellBounds;
        TileBase[] interactableTiles = tilePalette.interactable.GetTilesBlock(interactableBounds);
        TileMapData interactableTileMap = new TileMapData();
        interactableTileMap.startX = tilePalette.interactable.cellBounds.xMin;
        interactableTileMap.startY = tilePalette.interactable.cellBounds.yMin;
        interactableTileMap.lengthX = interactableBounds.size.x;
        tileNames = new string[interactableTiles.Length];

        Debug.Log("Interactable has "+ interactableTiles.Length.ToString()+" tiles");
        for(int i =0;i<interactableTiles.Length;i++){
            if (interactableTiles[i] != null){
                // Debug.Log("Interactable Tiles = "+interactableTiles[i].name);
                tileNames[i] = interactableTiles[i].name;
                }
            else
                tileNames[i] = "null";
        }
        
        // noBuildZone
        BoundsInt noBuildZoneBounds = tilePalette.noBuildZone.cellBounds;
        TileBase[] noBuildZoneTiles = tilePalette.noBuildZone.GetTilesBlock(noBuildZoneBounds);
        TileMapData noBuildZoneTileMap = new TileMapData();
        noBuildZoneTileMap.startX = tilePalette.noBuildZone.cellBounds.xMin;
        noBuildZoneTileMap.startY = tilePalette.noBuildZone.cellBounds.yMin;
        noBuildZoneTileMap.lengthX = noBuildZoneBounds.size.x;
        tileNames = new string[noBuildZoneTiles.Length];

        Debug.Log("Nobuild has "+ noBuildZoneTiles.Length.ToString()+" tiles");

        for(int i =0;i<noBuildZoneTiles.Length;i++){
            if (noBuildZoneTiles[i] != null){
                // Debug.Log("Nobuild Tiles = "+noBuildZoneTiles[i].name);
                tileNames[i] = noBuildZoneTiles[i].name;
                }
            else
                tileNames[i] = "null";
        }

        SceneData sceneData = new SceneData(choppableTileMap,collidableTileMap,groundTileMap,minableTileMap,decorTileMap,interactableTileMap,noBuildZoneTileMap);

        if (MasterSave){
            if(!Directory.Exists(Application.persistentDataPath + "/MasterSceneData"))
            {
                //if it doesn't, create it
                Directory.CreateDirectory(Application.persistentDataPath + "/MasterSceneData");

            }

            string filePath = Application.persistentDataPath + "/MasterSceneData"+
                            string.Format("/{0}MasterData.save",sceneManager.getSceneName());

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(filePath);

            Debug.Log("Trying to save "+filePath);

            bf.Serialize(file, sceneData);
            file.Close();}
        else
        {
            Debug.Log("We probably want to just keep track of specific tiles we've modified\nrather than loading everything");
        }

        keyCount =0;
        yield return null;
    }

    
}
