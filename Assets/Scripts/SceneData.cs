using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;



[Serializable]
public class SceneData
{
    public TileMapData choppableTileMap;

    public TileMapData collidableTileMap;

    public TileMapData groundTileMap;

    public TileMapData minableTileMap;

    public TileMapData decorTileMap;

    public TileMapData interactableTileMap;
    public TileMapData noBuildZoneTileMap;

    public SceneData(TileMapData choppableTileMap,TileMapData collidableTileMap,TileMapData groundTileMap,TileMapData minableTileMap,TileMapData decorTileMap,TileMapData interactableTileMap,TileMapData noBuildZoneTileMap){
        this.choppableTileMap = choppableTileMap;
        this.collidableTileMap = collidableTileMap;
        this.decorTileMap = decorTileMap;
        this.interactableTileMap = interactableTileMap;
        this.noBuildZoneTileMap = noBuildZoneTileMap;
        this.minableTileMap = minableTileMap;
        this.groundTileMap = groundTileMap;
    }
    
}
