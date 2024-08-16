using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharacterBuilding : MonoBehaviour
{
    [SerializeField] public bool isBuilding = false;

    public GameObject buildSquare;
    public GameObject noBuildSquare;


    private CharacterMovement movement;
    private Character character;
    private CharacterInventory inventory;
    private TilePalette tilePalette;

    private BuildablePrefabs prefabs;

    // World variables 
    private Vector3Int buildSquareCellLocation;

    [SerializeField] private SceneManager sceneManager;
    private bool canModifyWorldAtBuildSquare = false;

    private int interactLayer;
    private int choppableLayer; // Ray will only hit objects on the interact layer

    private GridLayout grid;
    private RaycastHit2D choppableHit;

    public BluePrint activeBlueprint;
    

    public void InitializeComponents(Character characterRef){
        character = characterRef;
        inventory = character.GetInventory();
        movement = character.GetMovement();

        sceneManager = character.GetSceneManager();

        tilePalette = FindObjectOfType<TilePalette>();
        prefabs = FindObjectOfType<BuildablePrefabs>();

        interactLayer = LayerMask.NameToLayer("Interactable");
        choppableLayer = LayerMask.NameToLayer("Choppable");

        grid = GameObject.Find("Grid").GetComponent<GridLayout>();
    }

    void FixedUpdate()
    {
        UpdateBuildSquaresPosition();
    }

    private void UpdateBuildSquaresPosition()
    {
        buildSquareCellLocation = tilePalette.grid.WorldToCell(movement.characterCenter + movement.playerFacingDirection);
        Vector3 squareWorldPos = tilePalette.grid.CellToWorld(buildSquareCellLocation) + new Vector3(.5f, .5f);

        buildSquare.transform.position = squareWorldPos;
        noBuildSquare.transform.position = squareWorldPos;
    }
    
    public void ToggleBuilding()
    {   
        isBuilding = !isBuilding;
        sceneManager.ToggleBuildMenu(isBuilding);

        if (isBuilding){
            UpdateBuildSquareVisibility();
        }
        else{
            HideBuildSquare();
        }
    }

    private void HideBuildSquare(){
        buildSquare.SetActive(false);
        noBuildSquare.SetActive(false);
    }

    private void UpdateBuildSquareVisibility()
    {
        if (tilePalette.noBuildZone.GetTile(buildSquareCellLocation) == null)
        {
            buildSquare.SetActive(true);
            noBuildSquare.SetActive(false);
            canModifyWorldAtBuildSquare = true;
        }
        else
        {
            buildSquare.SetActive(false);
            noBuildSquare.SetActive(true);
            canModifyWorldAtBuildSquare = false;
        }
    }

    public IEnumerator TryBuild()
    {
        Debug.Log("Initiating build process...");

        TileBase groundTile = tilePalette.ground.GetTile(buildSquareCellLocation);
        if (IsWaterTile(groundTile) || !canModifyWorldAtBuildSquare)
        {
            Debug.Log("Can't build here");
            yield break;
        }

        HandleChoppableDetection();

        string[] requiredItemTitles = new string[activeBlueprint.price.Count];
        int[] requiredItemAmounts = new int[activeBlueprint.price.Count];

        int index = 0;
        foreach (var kvp in activeBlueprint.price)
        {
            requiredItemTitles[index] = kvp.Key;
            requiredItemAmounts[index] = kvp.Value;
            index++;
        }

        // Call the CheckIfItemsExistAndRemove method with the required parameters
        if (!inventory.CheckIfItemsExistAndRemove(requiredItemTitles, requiredItemAmounts))
        {
            ToastNotification.Instance.Toast("need-items-building", "You need more resources.");
            Debug.LogWarning($"Not enough items to build {activeBlueprint.title}");
            yield break;
        }

        BuildBasedOnBlueprint();
        yield return null;
    }

    private bool IsWaterTile(TileBase groundTile)
    {
        if (groundTile == tilePalette.water)
        {
            Debug.LogWarning("Build square is water. Cannot build here.");
            return true;
        }
        return false;
    }

    private void HandleChoppableDetection()
    {
        choppableHit = Physics2D.Raycast(buildSquare.transform.position, -Vector3.forward, .6f, 1 << choppableLayer);
        if (choppableHit.collider != null)
        {
            Debug.Log("Choppable object hit detected. Still building...");
        }
    }

    

    private void BuildBasedOnBlueprint()
    {
        TileBase currentTile = tilePalette.choppable.GetTile(buildSquareCellLocation);
        bool isTileBuildable = CheckIfSpaceIsClearForBuilding();
 
        switch (activeBlueprint.title)
        {
            case "Fence":
                BuildFence(currentTile, isTileBuildable);
                break;
            case "Gate":
                BuildGate(currentTile, isTileBuildable);
                break;
            case "Sack":
            case "Sign":
            case "Chest":
            case "Tan Single Bed": //TODO remove duplicate code
            case "Tan Double Bed": //TODO remove duplicate code
            case "Lavender Single Bed": //TODO remove duplicate code
            case "Lavender Double Bed": //TODO remove duplicate code
            case "Table": //TODO remove duplicate code
            case "Covered Table": //TODO remove duplicate code
                BuildObject(activeBlueprint.title);
                break;
            case "Torch":
                BuildItem(activeBlueprint.title);
                break;
            case "Cobblestone Path":
                BuildPath();
                break;
            case "Torch x 3":
                BuildItem(activeBlueprint.title);
                break;
            default:
                Debug.LogError($"Unknown blueprint title: {activeBlueprint.title}");
                break;
        }
    }

    private bool CheckIfSpaceIsClearForBuilding()
    {
        return tilePalette.choppable.GetTile(buildSquareCellLocation) == null &&
               tilePalette.minable.GetTile(buildSquareCellLocation) == null &&
               tilePalette.collidable.GetTile(buildSquareCellLocation) == null;
    }

    private void BuildFence(TileBase currentTile, bool isTileBuildable)
    {
        if (isTileBuildable)
        {
            BuildChoppableTile("Fence", tilePalette.fence);
        }
        else if (currentTile == tilePalette.gateOpen || currentTile == tilePalette.gateClosed)
        {
            ReplaceCurrentTileWith("Fence", tilePalette.fence);
        }
    }

    private void BuildGate(TileBase currentTile, bool isTileBuildable)
    {
        if (isTileBuildable)
        {
            BuildChoppableTile("Gate", tilePalette.gateClosed);
        }
        else if (currentTile == tilePalette.fence)
        {
            ReplaceCurrentTileWith("Gate", tilePalette.gateClosed);
        }
    }

    private void BuildPath()
    {
        TileBase groundTile = tilePalette.ground.GetTile(buildSquareCellLocation);
        if (groundTile != tilePalette.water)
        {
            BuildGroundTile("Cobblestone Path", tilePalette.cobbleStonePath);
        }
        else
        {
            Debug.LogWarning("Cannot build Cobblestone Path on water");
        }
    }

    private void BuildItem(string itemName){
        string[] parts = itemName.Split(new string[] { " x " }, StringSplitOptions.None);
    
        string item;
        int amount;

        if (parts.Length == 2 && int.TryParse(parts[1], out amount)) {
            item = parts[0];
        } else {
            // If the split did not result in two parts, use the original itemName and set amount to 1
            item = itemName;
            amount = 1;
        }

        inventory.TryAddItem(item, amount);
        ToastNotification.Instance.Toast($"Added-{item}", $"Added {amount} {item}(s) to inventory.");
    }

    private void BuildObject(string itemName)
    {
        CharacterUI characterUI = character.GetSceneManager().characterUI; 

        Debug.Log($"trying to build {itemName}");

        switch (itemName)
        {
            case "Chest":
                GameObject newChest = Instantiate(prefabs.chest,tilePalette.grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,0),Quaternion.identity);
                ExternalInventory newChestScript = newChest.GetComponent<ExternalInventory>();

                newChestScript.inventoryIdentifier = "red-chest"+Guid.NewGuid().ToString();
                newChestScript.ExternalInventoryGui = characterUI.externalInventoryGui;
                newChestScript.ExternalInventoryPanel = characterUI.externalInventoryPanels;
                newChestScript.inventoryUI = characterUI.externalInventoryPanels.GetComponent<InventoryUI>();
                
                return ;
            case "Sack": //TODO remove duplicate code
                GameObject newSack = Instantiate(prefabs.sack,tilePalette.grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,0),Quaternion.identity);
                ExternalInventory newSackScript = newSack.GetComponent<ExternalInventory>();
                newSackScript.inventoryIdentifier = "red-chest"+Guid.NewGuid().ToString();
                newSackScript.ExternalInventoryGui = characterUI.externalInventoryGui;
                newSackScript.ExternalInventoryPanel = characterUI.externalInventoryPanels;
                newSackScript.inventoryUI = characterUI.externalInventoryPanels.GetComponent<InventoryUI>();

                return;
            case "Tan Single Bed": //TODO remove duplicate code
                Instantiate(prefabs.tanSingleBed,tilePalette.grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,0),Quaternion.identity);
                return;
            case "Tan Double Bed": //TODO remove duplicate code
                Instantiate(prefabs.tanDoubleBed,tilePalette.grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,0),Quaternion.identity);
                return;
            case "Lavender Single Bed": //TODO remove duplicate code
                Instantiate(prefabs.lavenderSingleBed,tilePalette.grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,0),Quaternion.identity);
                return;
            case "Lavender Double Bed": //TODO remove duplicate code
                Instantiate(prefabs.lavenderDoubleBed,tilePalette.grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,0),Quaternion.identity);
                return;
            case "Table": //TODO remove duplicate code
                Instantiate(prefabs.table,tilePalette.grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,0),Quaternion.identity);
                return;
            case "Covered Table": //TODO remove duplicate code
                Instantiate(prefabs.coveredTable,tilePalette.grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,0),Quaternion.identity);
                return;
            case "Sign":
                return ;
            default:
                ToastNotification.Instance.Toast("debug", "No prefab =(");
                return;
        }
    }

   

    private void BuildChoppableTile(string itemName, TileBase targetTile)
    {
        tilePalette.choppable.SetTile(buildSquareCellLocation, targetTile);
        Debug.Log($"Built {itemName}");
    }

    private void BuildGroundTile(string itemName, TileBase targetTile)
    {
        tilePalette.ground.SetTile(buildSquareCellLocation, targetTile);
        Debug.Log($"Built {itemName}");
    }

    private void ReplaceCurrentTileWith(string itemName, TileBase targetTile)
    {
        tilePalette.choppable.SetTile(buildSquareCellLocation, targetTile);
        Debug.Log($"Replaced current tile with {itemName}");
    }
}
