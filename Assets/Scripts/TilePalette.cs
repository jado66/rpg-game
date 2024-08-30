using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public struct TileModification{
    public string layer { get; set; }
    public int xCoord { get; set; }
    public int yCoord { get; set; }
    public string tileName { get; set; }

    public TileModification(string layer, int xCoord, int yCoord, string tileName){
        this.layer = layer;
        this.xCoord = xCoord;
        this.yCoord = yCoord;
        this.tileName = tileName;
    }
}

[Serializable]
public struct TileMapData{
    public int startX  { get; set; }
    public int startY  { get; set; }
    public int lengthX  { get; set; }
    public string[] tileNames  { get; set; }

    public TileMapData(int startX, int startY, int lengthX, string[] tileNames){
        this.startX = startX;
        this.startY = startY;
        this.lengthX = lengthX;
        this.tileNames = tileNames;
    }
}
public class TilePalette : MonoBehaviour
{
    private static TilePalette _instance;

    public static TilePalette Instance { get { return _instance; } }
    
    public List<TileModification> modifications = new List<TileModification>();
    int interactLayer; 
    int choppableLayer; // Ray will only hit objects on the interact layer

    int groundLayer;

    public GridLayout grid;

    public Tilemap choppable;

    public Tilemap collidable;
    public Tilemap ground;

    public Tilemap minable;

    public Tilemap decor;

    public Tilemap interactable;

    public Tilemap noBuildZone;

    public AdvancedRuleTile dirt;
    public AdvancedRuleTile grass;

    public AdvancedRuleTile hole;

    public AdvancedRuleTile water;


    public AdvancedRuleTile fence;

    public AdvancedRuleTile gateOpen;

    public AdvancedRuleTile gateClosed;

    public AdvancedRuleTile mushroomTile;
    public AdvancedRuleTile mushroomTileRed;

    public AdvancedRuleTile mushroomTileWhite;


    public AdvancedRuleTile forestFlower;

    public AdvancedRuleTile forestFlowerPink;

    public AdvancedRuleTile forestFlowerPurple;


    public AdvancedRuleTile forestRock;

    public AdvancedRuleTile minableRock;
    public AdvancedRuleTile minableIron;
    public AdvancedRuleTile minableGold;
    public AdvancedRuleTile carrot;
    public AdvancedRuleTile tomato;

    public AdvancedRuleTile tree;
    public AdvancedRuleTile bush;

    public AdvancedRuleTile appleTree;

    public AdvancedRuleTile appleTreeEmpty;

    public AdvancedRuleTile ploughedDirt;

    public AdvancedRuleTile sapling;

    public AdvancedRuleTile cobbleStonePath;
    
    // Start is called before the first frame update
    void Awake()
    {
        interactLayer = LayerMask.NameToLayer("Interactable");
        choppableLayer = LayerMask.NameToLayer("Choppable");
        groundLayer = LayerMask.NameToLayer("Ground");

        try{ grid = GameObject.Find("Grid").GetComponent<GridLayout>(); } 
        catch (Exception e){
            Debug.LogException(e,this);
            Debug.Log("Problem finding grid in TilePallet");
        }
        try{ noBuildZone = GameObject.Find("NoBuildZone").GetComponent<Tilemap>(); } 
        catch (Exception e){
            Debug.LogException(e,this);
            Debug.Log("Problem finding noBuildZone in TilePallet");
        }
        try{ minable = GameObject.Find("Minable").GetComponent<Tilemap>(); } 
        catch (Exception e){
            Debug.LogException(e,this);
            Debug.Log("Problem finding minable in TilePallet");
        }
        try{ collidable = GameObject.Find("Collidable").GetComponent<Tilemap>(); } 
        catch (Exception e){
            Debug.LogException(e,this);
            Debug.Log("Problem finding collidable in TilePallet");
        }
        try{ decor = GameObject.Find("Decor").GetComponent<Tilemap>(); } 
        catch (Exception e){
            Debug.LogException(e,this);
            Debug.Log("Problem finding decor in TilePallet"); 
        }
        try{ interactable = GameObject.Find("Interactable").GetComponent<Tilemap>(); } 
        catch (Exception e){
            Debug.LogException(e,this);
            Debug.Log("Problem finding interactable in TilePallet"); 
        }
        try{ choppable = GameObject.Find("Choppable").GetComponent<Tilemap>(); } 
        catch (Exception e){
            Debug.LogException(e,this);
            Debug.Log("Problem finding choppable in TilePallet"); 
        }
        try{ ground = GameObject.Find("Ground").GetComponent<Tilemap>(); } 
        catch (Exception e){
            Debug.LogException(e,this);
            Debug.LogError("Problem finding ground in TilePallet");
        }
        // 
        // try{ dirt = GameObject.Find("Dirt").GetComponent<Tilemap>(); } 
        // catch (Exception e){
        //     Debug.LogException(e,this);
        //     Debug.LogError("Problem finding dirt in TilePallet");
        // }
        
        // DontDestroyOnLoad(this);
        // if (_instance != null && _instance != this)
        // {
        //     Destroy(this.gameObject);
        // } else {  
        //     _instance = this;
        // }      
        
    }

    public bool IsCollidableAtTile(Vector3Int tilePos)
    {
        // Check if the position is within bounds of any of the collidable Tilemaps
        if (collidable.HasTile(tilePos) || minable.HasTile(tilePos) ||
            choppable.HasTile(tilePos) || interactable.HasTile(tilePos))
        {
            return true;
        }
        return false;
    }

    void Init(){
        
    }

}
