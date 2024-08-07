using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// This class deal with world manipulation and building
public class CharacterWorldInteraction: MonoBehaviour
{
    [SerializeField] private bool isBuilding = false;

    private bool canModifyWorldAtBuildSquare = false;
    
    private CharacterInventory inventory;

    public GameObject buildSquare;
    public GameObject noBuildSquare;

    private Character character;
    private CharacterMovement movement;

    private TilePalette tilePalette;

    // World variables 
    private Vector3Int buildSquareCellLocation;

    private int interactLayer; 
    private int choppableLayer; // Ray will only hit objects on the interact layer

    private int groundLayer;

    private int defaultLayer;
    private GridLayout grid;

    private Vector3 playerFacingDirection;

    public void InitializeComponents(Character characterRef){
        character = characterRef;
        movement = character.GetMovement();
        inventory = character.GetInventory();

        // Will I need to refetch this on scene changes?
        tilePalette = GameObject.Find("TilePalette").GetComponent<TilePalette>();
        interactLayer = LayerMask.NameToLayer("Interactable");
        choppableLayer = LayerMask.NameToLayer("Choppable");
        groundLayer = LayerMask.NameToLayer("Ground");
        defaultLayer = LayerMask.NameToLayer("Default");
        grid = GameObject.Find("Grid").GetComponent<GridLayout>();

    }
    public IEnumerator Chop()
    {
        buildSquareCellLocation = grid.WorldToCell(movement.characterCenter+movement.playerFacingDirection);

        if(tilePalette.choppable.GetTile(buildSquareCellLocation)!=null){ 
                
            Debug.Log("We hit a tree tile at " + buildSquareCellLocation.ToString());
            // These need to be params of the object if (Random.Range(0,5) == 4){ 
            tilePalette.choppable.SetTile(buildSquareCellLocation,null);
            inventory.TryAddItem("Wood");
                // Debug.DrawRay(movement.characterCenter+movement.playerFacingDirection*.75f-perpendicularDirection*.3f,perpendicularDirection*.6f,Color.red,1f);
            if (Random.Range(0, 10) == 0)
            {
                inventory.TryAddItem("Tree Sapling");
            }
        }
        else{
            Debug.Log("Nothing to chop");
            // Debug.DrawRay(movement.characterCenter+movement.playerFacingDirection*.75f-perpendicularDirection*.3f,perpendicularDirection*.6f,Color.green,1f);
        }
        yield return null;
    }

    public IEnumerator Mine()
    {
        buildSquareCellLocation = grid.WorldToCell(movement.characterCenter+movement.playerFacingDirection);

        var minableTile = tilePalette.minable.GetTile(buildSquareCellLocation);
        var groundTile = tilePalette.ground.GetTile(buildSquareCellLocation);

        var minableItemMap = new Dictionary<TileBase, string>
        {
            { tilePalette.forestRock, "Rock" },
            { tilePalette.minableRock, "Rock" },
            { tilePalette.minableIron, "Iron Ore" },
            { tilePalette.minableGold, "Gold Ore" }
        };

        if (minableItemMap.ContainsKey(minableTile))
        {
            tilePalette.minable.SetTile(buildSquareCellLocation, null);
            inventory.TryAddItem(minableItemMap[minableTile]);
        }
        else if (groundTile == tilePalette.cobbleStonePath)
        {
            tilePalette.ground.SetTile(buildSquareCellLocation, tilePalette.grass);
            inventory.TryAddItem("Rock");
        }

        yield return null;
    }

    public IEnumerator TillGround()
    {
        buildSquareCellLocation = grid.WorldToCell(movement.characterCenter+movement.playerFacingDirection);

        Debug.Log("We are attempting to dig at "+buildSquareCellLocation.ToString());
        TileBase tile = tilePalette.ground.GetTile(buildSquareCellLocation);

        if (IsValidDigTile(tile))
        {
            Debug.Log("Grass is here. Digging at");
            tilePalette.ground.SetTile(buildSquareCellLocation,tilePalette.dirt);
        }
        yield return null;
    }
    
    private bool IsValidDigTile(TileBase tile)
    {
        buildSquareCellLocation = grid.WorldToCell(movement.characterCenter+movement.playerFacingDirection);

        return tile == tilePalette.grass || 
            tile == tilePalette.dirt || 
            tile == tilePalette.ploughedDirt;
    }

    public IEnumerator Plant()
    {
        buildSquareCellLocation = grid.WorldToCell(movement.characterCenter+movement.playerFacingDirection);

        if (tilePalette.ground.GetTile(buildSquareCellLocation) == tilePalette.dirt){
            tilePalette.ground.SetTile(buildSquareCellLocation,tilePalette.ploughedDirt);
        }
        yield return null;
    }

    public IEnumerator IrrigateGround()
    {            
        buildSquareCellLocation = grid.WorldToCell(movement.characterCenter+movement.playerFacingDirection);

        foreach (var direction in DirectionalPointers.Pointers)
        {
            if (tilePalette.ground.GetTile(buildSquareCellLocation + direction) == tilePalette.water)
            {
                tilePalette.ground.SetTile(buildSquareCellLocation, tilePalette.water);
                tilePalette.decor.SetTile(buildSquareCellLocation, null);
                break;
            }
        }

        yield return null;
    }
}