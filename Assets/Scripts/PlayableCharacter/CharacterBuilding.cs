using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class CharacterBuilding : MonoBehaviour
{
    [SerializeField] private bool isBuilding = false;

    public GameObject buildSquare;
    public GameObject noBuildSquare;

    private CharacterMovement movement;
    private Character character;
    private CharacterInventory inventory;
    private TilePalette tilePalette;

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
    
    public void ToggleBuilding(bool callFromSceneManager = false)
    {
        isBuilding = !isBuilding;

        if (!callFromSceneManager){
            sceneManager.ToggleBuildMenu();
        }

        if (isBuilding){
            UpdateBuildSquareVisibility();
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

    private IEnumerator Build()
    {
        Debug.Log("Initiating build process...");

        TileBase groundTile = tilePalette.ground.GetTile(buildSquareCellLocation);
        if (IsWaterTile(groundTile) || !canModifyWorldAtBuildSquare)
        {
            Debug.Log("Can't build here");
            yield break;
        }

        HandleChoppableDetection();

        Debug.Log($"Trying to build a {activeBlueprint.title}");

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
                BuildObject(activeBlueprint.title);
                break;
            case "Cobblestone Path":
                BuildPath();
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
            BuildTile("Fence", tilePalette.fence);
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
            BuildTile("Gate", tilePalette.gateClosed);
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
            BuildTile("Cobblestone Path", tilePalette.cobbleStonePath);
        }
        else
        {
            Debug.LogWarning("Cannot build Cobblestone Path on water");
        }
    }

    private void BuildObject(string itemName)
    {
        GameObject prefab = GetPrefabForItem(itemName);
        if (prefab != null)
        {
            Instantiate(prefab, tilePalette.grid.CellToWorld(buildSquareCellLocation) + new Vector3(.5f, .5f, 0), Quaternion.identity);
            Debug.Log($"Built {itemName}");
        }
    }

    private GameObject GetPrefabForItem(string itemName)
{
    switch (itemName)
    {
        case "Chest":
            return tilePalette.chest;
        default:
            return null;
    }
}

    private void BuildTile(string itemName, TileBase targetTile)
    {
        tilePalette.choppable.SetTile(buildSquareCellLocation, targetTile);
        Debug.Log($"Built {itemName}");
    }

    private void ReplaceCurrentTileWith(string itemName, TileBase targetTile)
    {
        tilePalette.choppable.SetTile(buildSquareCellLocation, targetTile);
        Debug.Log($"Replaced current tile with {itemName}");
    }
}
