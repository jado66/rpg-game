using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class deals with character actions, combat, interactions with objects
public class CharacterActions : MonoBehaviour
{
    [SerializeField] private float interactionRange = 2f;

    [SerializeField] private Character character;

    private CharacterMovement movement;


    private CharacterState currentState;

    private SceneManager sceneManager;

    private CharacterMoveData characterMoveData;

    static Queue<CharacterMoveData> characterMoves = new Queue<CharacterMoveData>();

    private CharacterBuilding building;
    private CharacterWorldInteraction worldInteraction;

    private float[] keyCount = new float[10];

    private List<KeyCode> buttonsPressed = new List<KeyCode>();

    private CharacterInventoryUI inventoryUI;

    Vector2 change;

    public void InitializeComponents(Character characterRef){
        character = characterRef;
        movement = character.GetMovement();
        building = character.GetBuilding();
        worldInteraction = character.GetWorldInteraction();
        sceneManager = character.GetSceneManager();
    }

    public void FixedUpdate(){

        if (!character.characterIsInControl){
            FixedUpdateClone();
            return;
        }

        characterMoveData = new CharacterMoveData();

        change = Vector2.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        buttonsPressed = GatherButtonsInputs();

        bool isLeftShiftPressed = buttonsPressed.Contains(KeyCode.LeftShift);
        movement.HandleMovement(change, isLeftShiftPressed);
        HandleButtonPresses(buttonsPressed, characterMoveData.itemsUsed);

        characterMoveData.movement = change;
        characterMoveData.buttonsPressed = new List<KeyCode>(buttonsPressed); // Shallow copy
        characterMoveData.cloneState = currentState;

        buttonsPressed.Clear();

        characterMoves.Enqueue(characterMoveData);

        HandleKeyCounts();
    }
    public void FixedUpdateClone(){

        // Clear buttons pressed
        buttonsPressed.Clear();

        

        if (characterMoves.Count > 0){
            characterMoveData = characterMoves.Dequeue();
            change = characterMoveData.movement;
            buttonsPressed = new List<KeyCode>(characterMoveData.buttonsPressed); // Shallow copy
        
            bool isLeftShiftPressed = buttonsPressed.Contains(KeyCode.LeftShift);
            movement.HandleMovement(change, isLeftShiftPressed);
            ReplayItemUsages(characterMoveData.itemsUsed); // Replay item usage
        }
    }

    private void HandleButtonPresses(List<KeyCode> buttonsPressed, List<InventoryItem> usedItems) {
        // Check if Keycode.E exists in the buttonsPressed list and remove it if it does
        if (buttonsPressed.Contains(KeyCode.E)) {
            buttonsPressed.Remove(KeyCode.E);
            Interact();
        }

        var hotbarKeys = new HashSet<KeyCode> { KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V };

        var hotbarButtons = buttonsPressed.Where(button => hotbarKeys.Contains(button)).ToList();
        
        foreach (var button in hotbarButtons){
            InventoryItem item = inventoryUI.GetHotbarItem(button);
            if (item != null){
                item.Use(character);
                usedItems.Add(item);
            }
        }
    }

    private void ReplayItemUsages(List<InventoryItem> usedItems){
        if(usedItems != null) {
            foreach(var item in usedItems) {
                if (item != null){
                    item.Use(character);
                }
            }
        }
    }

    // public IEnumerator HandleInteractionRoutine() {
    //     Vector2 perpendicularDirection = new Vector2(transform.up.y, -transform.up.x);
    //     Debug.DrawRay(characterCenter + transform.up * 0.5f, perpendicularDirection * 0.5f, Color.red);
    //     yield return null; // Assuming you want to yield something for the coroutine
    // }

    public List<KeyCode> GatherButtonsInputs(){
        List<KeyCode> newButtonsPressed = new List<KeyCode>();

        // Press and hold keys 
        if (Input.GetKey(KeyCode.LeftShift)){
            newButtonsPressed.Add(KeyCode.LeftShift);
        }

        // Delay keys
        if (Input.GetKey(KeyCode.T) && keyCount[0] == 0 && currentState == CharacterState.walk) {
            newButtonsPressed.Add(KeyCode.T);
            // Debug.Log("torch");
            keyCount[0]++;
        }

        if (Input.GetKey(KeyCode.E) && keyCount[1] == 0) {
            newButtonsPressed.Add(KeyCode.E);
            keyCount[1]++;
        }

        if (Input.GetKey(KeyCode.B) && keyCount[1] == 0) {
            building.ToggleBuilding();
            keyCount[1]++;
        }

        if (Input.GetKey(KeyCode.I) && keyCount[1] == 0) {
            sceneManager.ToggleInventory();
            keyCount[1]++;
        }

        if (Input.GetKey(KeyCode.P) && keyCount[1] == 0) { 
            newButtonsPressed.Add(KeyCode.P);
            // Pause game
            keyCount[1]++;
        }

        return newButtonsPressed;
    }

    private void Interact()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionRange);
        foreach (Collider2D collider in colliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.onCharacterInteract(this);
                break;
            }
        }
    }

    private void HandleKeyCounts(){
         for (int i = 0; i < 10; i++){
            if (keyCount[i] != 0)
                keyCount[i] += 1;
            if (keyCount[i] >= 15)
                keyCount[i] = 0;
        }
    }
}