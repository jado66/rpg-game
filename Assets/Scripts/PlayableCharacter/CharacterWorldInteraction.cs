using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using PlantSystem; 

// This class deal with world manipulation and building
public class CharacterWorldInteraction: MonoBehaviour
{
    private bool canModifyWorldAtBuildSquare = false;

    private CharacterStats stats;
    
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
        stats = character.GetStats();
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

    public IEnumerator Interact(){
        Debug.Log($"We are actually interacting - interact layer {interactLayer}");
       
        buildSquareCellLocation = grid.WorldToCell(movement.characterCenter+movement.playerFacingDirection);

        Vector2 rayOrigin = movement.characterCenter;
        Vector2 rayDirection = movement.playerFacingDirection;
        // Cast the ray and get the hit information
        RaycastHit2D interactableHit = Physics2D.Raycast(rayOrigin, rayDirection, 1f, 1 << interactLayer);

        // Draw the ray for visualization
        Debug.DrawRay(rayOrigin, rayDirection * 1f, Color.red, 2f); // The ray will be drawn in red for 2 seconds
        
        
        // Debug.DrawRay(movement.characterCenter+playerFacingDirection*.5f,perpendicularDirection*.5f,Color.red);


        if(interactableHit.collider != null){ 
            Debug.Log("We hit something");
            try{
                Debug.Log("Player interacted with "+interactableHit.collider.gameObject.GetComponent<Interactable>().type);
                var interactables = interactableHit.collider.gameObject.GetComponents<Interactable>();
                foreach (var interactable in interactables) {
                    interactable.OnCharacterInteract();
                }
            }
            catch {
                // Check for mushrooms
                
                
            }
        }
         else{
            Debug.Log("Nothing on the interact layer");
            // Check for fence
            if (tilePalette.interactable.GetTile(buildSquareCellLocation)==tilePalette.mushroomTile){
                Debug.Log("Hit a mushroom");
                tilePalette.interactable.SetTile(buildSquareCellLocation,null);

                float randomChance = Random.value; // Returns a value between 0.0 and 1.0
                if (randomChance < 0.1f) // 50% chance for Green Mushroom
                {
                    inventory.TryAddItem("Green Mushroom");
                }
                else if (randomChance < 0.3f)// 50% chance for Red Mushroom
                {
                    inventory.TryAddItem("Red Mushroom");
                }
                else{
                    inventory.TryAddItem("Mushroom");

                }
            }
            else if (tilePalette.decor.GetTile(buildSquareCellLocation)==tilePalette.forestFlower)
                tilePalette.decor.SetTile(buildSquareCellLocation,null);
            else if (tilePalette.choppable.GetTile(buildSquareCellLocation) == tilePalette.gateOpen)
                tilePalette.choppable.SetTile(buildSquareCellLocation,tilePalette.gateClosed);
            else if (tilePalette.choppable.GetTile(buildSquareCellLocation) == tilePalette.gateClosed)
                tilePalette.choppable.SetTile(buildSquareCellLocation,tilePalette.gateOpen);
            else if (tilePalette.choppable.GetTile(buildSquareCellLocation)==tilePalette.appleTree){
                if (stats.Stamina < .5f){
                    character.ToastStaminaMessage();
                    yield return null;
                }
                tilePalette.choppable.SetTile(buildSquareCellLocation,tilePalette.appleTreeEmpty);
                float randomChance = Random.value; // Returns a value between 0.0 and 1.0
                if (randomChance < 0.1f) // 50% chance for Green Mushroom
                    inventory.TryAddItem("Unripe Apple");
                else{
                    inventory.TryAddItem("Apple");
                }
                stats.DepleteStamina(.5f);
            }
            else if (tilePalette.ground.GetTile(buildSquareCellLocation)==tilePalette.tomato)
            {
                PickUpCrop("Tomato", buildSquareCellLocation);
            }
            else if (tilePalette.ground.GetTile(buildSquareCellLocation)==tilePalette.carrot)
            {
                PickUpCrop("Carrot", buildSquareCellLocation);
            }
        }
        yield return null;

    }


    public void PickUpCrop(string itemName, Vector3Int location){

        if (stats.Stamina < .5f){
            character.ToastStaminaMessage();
            return;
        }

        if (Random.value < 0.5f) // 50% chance for Green Mushroom
        {
            inventory.TryAddItem($"{itemName} Seed");
        }

        bool success = inventory.TryAddItem(itemName);
        
        if (success){
           PlantTileManager.Instance.HarvestPlant(buildSquareCellLocation);
        }

        stats.DepleteStamina(.5f);
    
    }

    public IEnumerator Chop()
    {
        buildSquareCellLocation = grid.WorldToCell(movement.characterCenter + movement.playerFacingDirection);

        Debug.DrawRay(movement.characterCenter, movement.playerFacingDirection * 1f, Color.green, 2f);

        var tile = tilePalette.choppable.GetTile(buildSquareCellLocation);

        if (tile != null)
        {
            Debug.Log("We hit a tree tile at " + buildSquareCellLocation.ToString());

            // Check if the tile is an apple tree
            if (tile == tilePalette.appleTree)
            {
                Debug.Log("It's an apple tree!");

                tilePalette.choppable.SetTile(buildSquareCellLocation, null);
                inventory.TryAddItem("Wood");

                // Drop multiple apples if not already empty
                int appleCount = Random.Range(0, 5); // Range is inclusive of the minimum value and exclusive of the maximum value
                int unRipeAppleCount = Random.Range(5, 15); // Range is inclusive of the minimum value and exclusive of the maximum value

                for (int i = 0; i < appleCount; i++)
                {
                    inventory.TryAddItem("Apple");
                }
                for (int i = 0; i < unRipeAppleCount; i++)
                {
                    inventory.TryAddItem("Unripe Apple");
                }
            }
            else if (tile == tilePalette.appleTreeEmpty)
            {
                Debug.Log("It's an empty apple tree!");
                tilePalette.choppable.SetTile(buildSquareCellLocation, null);
                inventory.TryAddItem("Wood");
            }
            else
            {
                // Handle other trees normally
                tilePalette.choppable.SetTile(buildSquareCellLocation, null);
                inventory.TryAddItem("Wood");
            }

            // Random chance to drop a Tree Sapling
            if (Random.Range(0, 10) == 0)
            {
                inventory.TryAddItem("Tree Sapling");
            }
        }
        else
        {
            Debug.Log("Nothing to chop");
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
            
            float randomChance = Random.value; // Returns a value between 0.0 and 1.0
            if (randomChance < 0.01f) // 1% chance for Green Mushroom
            {
                inventory.TryAddItem("Gem");
            }

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

    public IEnumerator Plant(string plantName, System.Action<bool> callback)
    {
        buildSquareCellLocation = grid.WorldToCell(movement.characterCenter + movement.playerFacingDirection);
        Debug.Log($"Attempting to plant {plantName}");

        if (tilePalette.ground.GetTile(buildSquareCellLocation) == tilePalette.dirt)
        {
            Debug.Log("Suitable location for planting");
            bool planted = PlantTileManager.Instance.TryPlantSeed(buildSquareCellLocation, plantName);
            
            if (planted)
            {
                Debug.Log($"Successfully planted {plantName}");
                callback(true);
            }
            else
            {
                Debug.Log($"Failed to plant {plantName}. Location might be occupied.");
                callback(false);
            }
        }
        else
        {
            Debug.Log("Cannot plant here. Need ploughable dirt.");
            callback(false);
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