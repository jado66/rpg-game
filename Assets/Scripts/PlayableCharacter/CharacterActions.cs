using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobileController;


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

    public CharacterBuilding building;
    private CharacterWorldInteraction worldInteraction;

    private float[] keyCount = new float[10];

    private List<KeyCode> buttonsPressed = new List<KeyCode>();

    private CharacterHotbar hotbar;

    Vector2 change;

    public bool isMobileControlled = false;

    public Analog mobileJoystick;
    public Button ltButton;
    public Button rtButton;

    public Button zButton;
    public Button xButton;
    public Button cButton;


    public void InitializeComponents(Character characterRef){
        character = characterRef;
        movement = character.GetMovement();
        building = character.GetBuilding();
        worldInteraction = character.GetWorldInteraction();
        sceneManager = character.GetSceneManager();
        hotbar = character.GetHotbar();
    }

    public void FixedUpdate(){

        if (!character.characterIsInControl){
            FixedUpdateClone();
            return;
        }

        characterMoveData = new CharacterMoveData();

        change = Vector2.zero;

        if (isMobileControlled){
            Vector2 joystickPosition = mobileJoystick.GetStickPosition();
            change.x = joystickPosition.x;
            change.y = joystickPosition.y;
        }
        else{
            change.x = Input.GetAxisRaw("Horizontal");
            change.y = Input.GetAxisRaw("Vertical");
        }

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
        
        if (buttonsPressed == null || buttonsPressed.Count == 0) {
            return;
        }

        if (buttonsPressed.Contains(KeyCode.E)) {
            buttonsPressed.Remove(KeyCode.E);

            if (building.isBuilding){
                Debug.Log("Try build from Chr actions");
                TryBuild();
            }
            else{
                Interact();
            }
        }

        var hotbarKeys = new Dictionary<KeyCode, int> {
            { KeyCode.Z, 0 },
            { KeyCode.X, 1 },
            { KeyCode.C, 2 },
            { KeyCode.V, 3 }
        };

        var hotbarButtons = buttonsPressed.Where(button => hotbarKeys.ContainsKey(button)).ToList();
        
        foreach (var button in hotbarButtons) {
            // Debug.Log($"Handling {button} buttons");

            int index = hotbarKeys[button];
            InventoryItem item = hotbar.GetHotbarItem(index);
            // We need to assign the hotbar slot
            // if (character.selectedItem != null){ //TODO this needs fixing
            //     HandleHobarItemAssignement(index);
            //     break;
            // }

            if (item != null) {
                Debug.Log($"Using {item.Name}");
                // usedItems.Add(item.Clone()); //TODO fix me
                item.Use(character);
                hotbar.SyncInventory();

            }
            else{
                Debug.Log($"No item in slot {index}");
            }
        }
    }
    private void ReplayItemUsages(List<InventoryItem> usedItems){
        if(usedItems != null) {
            foreach(var item in usedItems) {
                if (item != null){
                    item.Use(character);
                    hotbar.SyncInventory();
                }
            }
        }
    }

    public List<KeyCode> GatherButtonsInputs(){
        List<KeyCode> newButtonsPressed = new List<KeyCode>();

        // Press and hold keys 
        if (isMobileControlled){ 
            if (ltButton.GetButtonState() == Button.ButtonState.hold || ltButton.GetButtonState() == Button.ButtonState.tap){
                newButtonsPressed.Add(KeyCode.LeftShift);
            }
        } else
        {
            if (Input.GetKey(KeyCode.LeftShift)){
                newButtonsPressed.Add(KeyCode.LeftShift);
            }
        }

        // HOTKEYS
        if (isMobileControlled){
            if (zButton.GetButtonState() == Button.ButtonState.tap && keyCount[0] == 0) {
                newButtonsPressed.Add(KeyCode.Z);
                // Debug.Log("torch");
                keyCount[0]++;
            }
            if (xButton.GetButtonState() == Button.ButtonState.tap && keyCount[0] == 0) {
                newButtonsPressed.Add(KeyCode.X);
                // Debug.Log("torch");
                keyCount[0]++;
            }
            if (cButton.GetButtonState() == Button.ButtonState.tap && keyCount[0] == 0) {
                newButtonsPressed.Add(KeyCode.C);
                // Debug.Log("torch");
                keyCount[0]++;
            }
        } 
        else {
            if (Input.GetKey(KeyCode.Z) && keyCount[0] == 0) {
                newButtonsPressed.Add(KeyCode.Z);
                // Debug.Log("torch");
                keyCount[0]++;
            }

            if (Input.GetKey(KeyCode.X) && keyCount[0] == 0) {
                newButtonsPressed.Add(KeyCode.X);
                // Debug.Log("torch");
                keyCount[0]++;
            }

            if (Input.GetKey(KeyCode.C) && keyCount[0] == 0) {
                newButtonsPressed.Add(KeyCode.C);
                // Debug.Log("torch");
                keyCount[0]++;
            }

            if (Input.GetKey(KeyCode.V) && keyCount[1] == 0) {
                newButtonsPressed.Add(KeyCode.V);
                keyCount[1]++;
            }
        }

        // Interact
        if (isMobileControlled){
            if (rtButton.GetButtonState() == Button.ButtonState.tap && keyCount[0] == 0) {
                newButtonsPressed.Add(KeyCode.E);
                // Debug.Log("torch");
                keyCount[0]++;
            }
        } 
        else {

            if (Input.GetKey(KeyCode.E) && keyCount[1] == 0) {
                newButtonsPressed.Add(KeyCode.E); 
                keyCount[1]++;
            }
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

    private void TryBuild(){
        StartCoroutine(building.TryBuild());         
    }
    private void Interact()
    {
        StartCoroutine(worldInteraction.Interact());         
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