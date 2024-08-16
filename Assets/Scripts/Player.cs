using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;




// Make sure all item copies are not pointers

struct PlayerMovesData{

    public int day; //This helps us to find the right time; Find the day and time and then cut the queue there
    public float time;  
    public Vector3 movement;
    public List<Enum> buttonsPressed;
    public CloneState cloneState;
}
public enum PlayerState
{
    walk,
    run,
    attack,
    swim,
    interact,
    
    standby
}

public enum CloneState
{
    walk,
    run,
    attack,
    swim,
    interact,
    standby
}

public class Player : MonoBehaviour
{
    
    private Interactable objectBeingCarried;

    public void SetObjectBeingCarried(Interactable newObject)
    {
        objectBeingCarried = newObject;
    }
    public int lightCount;
    bool staminaBoosted;
    public Item attackWeapon;
    private static Player _instance;

    public static Player Instance { get { return _instance; } }
    // public List<BluePrint> bluePrints;
    public BluePrint activeBlueprint;
    public GameObject projection;


    public int damageDealt;

    public int recoilForce;

    public float knockTime;
    

    public SceneManager sceneManager;

    private bool astralProjecting;
    private PlayerMovesData playerMoveData;
    public PlayerState currentState;
    public CloneState cloneState;
    

    public TilePalette tilePalette;
    public Animator animator;

    Animator projectionAnimator;
    public GameObject spriteAndAnimator;
    Vector3 change;

    Vector3 playerFacingDirection;

    Color rayColor = Color.green;

    RaycastHit2D interactableHit;

    RaycastHit2D defaultHit;
    RaycastHit2D choppableHit;
    RaycastHit2D interactableHit2;
    RaycastHit2D choppableHit2;

    RaycastHit2D groundHit;


    Vector3 playerCenter;

    int interactLayer; 
    int choppableLayer; // Ray will only hit objects on the interact layer

    int groundLayer;

    int defaultLayer;
    GridLayout grid;

    
    public int wood;

    public int mushrooms;

    public int level;
    public float healthMax;
    public float health;

    public float mana;

    public float manaMax;

    public float staminaMax;
    public float stamina;

    public int money;

    public GameObject torch;

    // public bool torchIsLit;
    public float speed = .01f;

    public static bool playerExists;

    private float[] keyCount = new float[10];

    private List<Enum> buttonsPressed = new List<Enum>();

    Rigidbody2D rigidbody2D;

    bool showBuildSquare = false;

    bool canModifyWorldAtBuildSquare;

    public GameObject buildSquare;
    public GameObject noBuildSquare;

    Vector3Int buildSquareCellLocation;

    Vector3Int[] directionalPointers = new Vector3Int[4]{new Vector3Int(0,1,0),
                                                         new Vector3Int(0,-1,0),
                                                         new Vector3Int(1,0,0),
                                                         new Vector3Int(-1,0,0)};

    public PlayerInventory inventory;


  
    static Queue<PlayerMovesData> playerMoves = new Queue<PlayerMovesData>();

    public bool playerIsInControl = false;

    // Maybe hotkeys for a bunch of 
    public Item activeTool = null;
    public Item activeConsumable = null;

    bool inventoryEmpty = false;

    public List<GameObject> followingObjects = new List<GameObject>();

    bool playerHasStartedTheGame;

    List<ShadowMonster> shadows;

    public bool shadowsFollowing;

    public bool onBoat;

    public bool inWater;

    public Boat boat;

    List<string> realmsDiscovered = new List<string>();
    List<string> miniRealmsDiscovered = new List<string>();

    public int realGameSecondsPlayed;

    public string playerName = "Player";

    public GameObject upHitBox;
    public GameObject downHitBox;

    public GameObject rightHitBox;

    public GameObject leftHitBox;


    void Awake(){
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        torch.SetActive(false);
         
        if (PlayerPrefs.GetString("PlayerName")!=""){
            playerName = PlayerPrefs.GetString("PlayerName");
            PlayerPrefs.SetString("PlayerName","");
            PlayerPrefs.Save();
        }

        inventory = GetComponent<PlayerInventory>();

    }

    public bool hasPlayerStartedTheGame(){return playerHasStartedTheGame;}

    
    public void addOrRemoveFollower(GameObject follower,bool add){
        if (add){

            followingObjects.Add(follower);
            Debug.Log("Added "+follower.name + " to followers");
        }
        else{
            followingObjects.Remove(follower);
        }
    }
    public void LinkPlayerToUI(){
        Debug.Log("Attempting to fix connections");
        
        tilePalette = GameObject.Find("TilePalette").GetComponent<TilePalette>();
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        // bool inventoryActiveSelf = sceneManager.playerUI.inventoryGui.activeSelf;
        // sceneManager.playerUI.inventoryGui.SetActive(true);
        inventory.inventoryUI = GameObject.Find("InventoryPanel").GetComponent<UIInventory>();
        inventory.hotItemsUI = GameObject.Find("HotItemsPanel").GetComponent<UIInventory>();
        // sceneManager.playerUI.inventoryGui.SetActive(inventoryActiveSelf);


        int i = 0;
        Debug.Log($"Item Count: {inventory.items.Count}");

        foreach(var item in inventory.items){
            if (i<10){
                inventory.inventoryUI.uiItems[i].UpdateItem(item);
            }
            else
            {
                inventory.hotItemsUI.uiItems[i].UpdateItem(item);
            }
            i++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        shadows = GameObject.FindObjectsOfType<ShadowMonster>().ToList();
        foreach(var shadow in shadows){
                shadow.gameObject.SetActive(false);
            }

        currentState = PlayerState.walk;

        buildSquare.SetActive(false);
        // buildSquare = GameObject.Find("BuildSquare");
        // Make it so the raycast doesn't collide with player
        interactLayer = LayerMask.NameToLayer("Interactable");
        choppableLayer = LayerMask.NameToLayer("Choppable");
        groundLayer = LayerMask.NameToLayer("Ground");
        defaultLayer = LayerMask.NameToLayer("Default");

       

        try{ grid = GameObject.Find("Grid").GetComponent<GridLayout>(); } 
        catch (Exception e){
            Debug.LogException(e,this);
            Debug.Log("Problem finding grid in TilePallet");
        }
        
        // try{ tilePalette.choppable = GameObject.Find("Choppable").GetComponent<Tilemap>(); } 
        // catch (Exception e){
        //     Debug.LogException(e,this);
        //     Debug.Log("Problem finding choppable in TilePallet"); 
        // }
        // try{ tilePalette.ground = GameObject.Find("Ground").GetComponent<Tilemap>(); } 
        // catch (Exception e){
        //     Debug.LogException(e,this);
        //     Debug.LogError("Problem finding ground in TilePallet");
        // }
        // try{ grid = GameObject.Find("Grid").GetComponent<GridLayout>(); } 
        // catch (Exception e){
        //     Debug.LogException(e,this);
        //     Debug.LogError("Problem finding grid in TilePallet");
        // }
        // try{ tilePalette.dirt = GameObject.Find("Dirt").GetComponent<Tilemap>(); } 
        // catch (Exception e){
        //     Debug.LogException(e,this);
        //     Debug.LogError("Problem finding dirt in TilePallet");
        // }
        // if (grid == null)
                    
        rigidbody2D = GetComponent<Rigidbody2D>();
        // interactLayer = ~interactLayer;
        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        // interactLayer = ~interactLayer;

        // playerCenter = gameObject.GetComponent<BoxCollider2D>().bounds.center;
        animator = spriteAndAnimator.GetComponent<Animator>();
        projectionAnimator = projection.GetComponent<Animator>();
        torch.SetActive(false);
        DontDestroyOnLoad(this);
        // if (_instance != null && _instance != this )//&& playerIsInControl)
        // {
        //     Destroy(this.gameObject);
        // } else {  
        //     _instance = this;
        // }

        
        // Debug.Log(PlayerPrefs.GetString("teleportTo"));
    }

    

    void FixedUpdateClone(){
        playerMoveData = playerMoves.Dequeue();
        change = playerMoveData.movement;
        buttonsPressed.Clear();
        buttonsPressed = playerMoveData.buttonsPressed.GetRange(0, playerMoveData.buttonsPressed.Count); // Shallow copy
        cloneState = playerMoveData.cloneState;

        if (inventory.items.Count == 0){
            if (!inventoryEmpty ){
                Debug.Log("Fixed inventory at "+sceneManager.normalizedHour.ToString());
                // fix inventory and update amounts
                foreach(var uiItem in inventory.inventoryUI.uiItems){
                    uiItem.TryFixItem();
                }
                foreach(var uiItem in inventory.hotItemsUI.uiItems){
                    uiItem.TryFixItem();
                }
                inventoryEmpty = true;

            }

        }
        else
            inventoryEmpty = false;

        
        if (cloneState == CloneState.attack && currentState != PlayerState.run && currentState != PlayerState.attack 
                                          && currentState != PlayerState.swim && currentState != PlayerState.standby && keyCount[0] == 0){
            StartCoroutine(AttackCo());
            keyCount[0] ++;
        }
        else if (animator.GetBool("swimming")){
            animator.SetFloat("moveX",change.x);
            animator.SetFloat("moveY",change.y);
            animator.SetBool("moving",true);
            movePlayer(false);
            playerFacingDirection.x = change.x;
            playerFacingDirection.y = change.y;
            playerFacingDirection.Normalize();
        }
        else if (change != Vector3.zero &&  currentState != PlayerState.standby &&(currentState == PlayerState.walk || currentState == PlayerState.run )){
            // Debug.Log("Change = ("+change.x.ToString()+","+change.y.ToString());
            if (currentState == PlayerState.standby){
                // close any external inventory 
                // switchCameraToFollowPlayer();
            }
            animator.SetFloat("moveX",change.x);
            animator.SetFloat("moveY",change.y);
            animator.SetBool("moving",true);
            movePlayer(cloneState == CloneState.run);
            cloneState = (Input.GetKey(KeyCode.LeftShift)? CloneState.run : CloneState.walk);
            playerFacingDirection.x = change.x;
            playerFacingDirection.y = change.y;
            playerFacingDirection.Normalize();
        }
        else if (currentState != PlayerState.standby){
            currentState = PlayerState.walk;
            animator.SetBool("moving",false);
            animator.SetBool("running",false);
        }
        else if (currentState == PlayerState.standby){
            currentState = PlayerState.standby;
            animator.SetBool("moving",false);
            animator.SetBool("running",false);

            if (astralProjecting){
                moveAstralProjection(change);
            }
        }

        buildSquareCellLocation = grid.WorldToCell(playerCenter+playerFacingDirection);
        buildSquare.transform.position = grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,1);
        noBuildSquare.transform.position = grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,1);

        StartCoroutine("handleButtonPressesCo");
    }


   
    void FixedUpdate(){
        if (currentState == PlayerState.run && ! staminaBoosted)
            stamina -= .1f;
        else{
            if (stamina < staminaMax){
                    stamina += .02f;      
            }     
        }
        playerHasStartedTheGame = true;

        if (!playerIsInControl){
            FixedUpdateClone();
            return;
        }
        else{
            change = Vector3.zero;
            change.x = Input.GetAxisRaw("Horizontal");
            change.y = Input.GetAxisRaw("Vertical");
            
        
        // Handle button inputs
        buttonsPressed.Clear();


        if (Input.GetKey(KeyCode.Y) && keyCount[0] == 0 && currentState == PlayerState.walk){
            buttonsPressed.Add(KeyCode.Y);
            LinkPlayerToUI();
            keyCount[0]+=1;
        }
        if (Input.GetKey(KeyCode.T) && keyCount[0] == 0 && currentState == PlayerState.walk){
            buttonsPressed.Add(KeyCode.T);
            // Debug.Log("torch");
            keyCount[0]+=1;
        }
        if (Input.GetKey(KeyCode.E) && keyCount[1] == 0){
            buttonsPressed.Add(KeyCode.E);
            keyCount[1]+=1;
        }

        

        
        // if (Input.GetKey(KeyCode.Q) && keyCount[1] == 0 && currentState == PlayerState.walk){
        //     buttonsPressed.Add(KeyCode.Q);
        //     keyCount[1]+=1;
        // }
        // if (Input.GetKey(KeyCode.C) && keyCount[1] == 0 && currentState == PlayerState.walk){
        //     buttonsPressed.Add(KeyCode.C);
        //     keyCount[1]+=1;
        //     Debug.Log("chop");
        // }
        // if (Input.GetKey(KeyCode.F) && keyCount[1] == 0 && currentState == PlayerState.walk){
        //     buttonsPressed.Add(KeyCode.F);
        //     keyCount[1]+=1;
        //     // Debug.Log("Build");
        // }
        if (Input.GetKey(KeyCode.M) && keyCount[1] == 0 && currentState == PlayerState.walk){
            buttonsPressed.Add(KeyCode.M);
            keyCount[1]+=1;
        }
        if (Input.GetKey(KeyCode.B) && keyCount[1] == 0){
            buttonsPressed.Add(KeyCode.B);
            // sceneManager.ToggleBuildMenu();
            keyCount[1]+=1;
        }
        if (Input.GetKey(KeyCode.P) && keyCount[1] == 0){ 
            // == 0 && currentState == PlayerState.walk
            Debug.Log("PPP");
            buttonsPressed.Add(KeyCode.P);
            keyCount[1]+=1;
        }

        // if (inventory.hotItemsUI == null || inventory==null)
        //         Debug.Log("Houston");


        if (Input.GetKey(KeyCode.Z) && keyCount[1] == 0 ){

            Debug.Log(inventory.hotItemsUI.uiItems[0].item);

            if (inventory.hotItemsUI.uiItems[0].item ==null){
                
                // if (inventory == null)
                //     Debug.Log("Inventory is null");
                // if (inventory.hotItemsUI == null)
                //     Debug.Log("Hot Items is null");
                // if (inventory.hotItemsUI.uiItems == null)
                //     Debug.Log("Hot Item's uiItems list is null");
                // if (inventory.hotItemsUI.uiItems[0] == null)
                //     Debug.Log("Hot Item's uiItems[0] list is null");
                // if (inventory.hotItemsUI.uiItems[0].item == null)
                //     Debug.Log("Just Z Item is null");

                    
                // Debug.Log("The itemDatabase has "+inventory.itemDatabase.items.Count.ToString()+" values");

                // for (int i = 0; i<inventory.itemDatabase.items.Count;i++){
                //     Debug.Log(i.ToString()+"= "+inventory.items[i].title);
                // }


                return;
            }
            keyCount[1]+=1;
            
            useItem(inventory.hotItemsUI.uiItems[0].item);
            
        }
        if (Input.GetKey(KeyCode.X) && keyCount[2] == 0 && inventory.hotItemsUI.uiItems[1].item !=null){
            
            keyCount[2]+=1;
            useItem(inventory.hotItemsUI.uiItems[1].item);
            
        }
        if (Input.GetKey(KeyCode.C) && keyCount[3] == 0 && inventory.hotItemsUI.uiItems[2].item !=null){
            
            keyCount[3]+=1;
            useItem(inventory.hotItemsUI.uiItems[2].item);
            
        }
        if (Input.GetKey(KeyCode.V) && keyCount[4] == 0 && inventory.hotItemsUI.uiItems[3].item !=null){
            
            keyCount[4]+=1;
            useItem(inventory.hotItemsUI.uiItems[3].item);
            
        }
        // Debug.Log(inventory.hotItemsUI.uiItems[3].item.ToString());

        // I need an enum for player state, but differen than current state

        
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack && currentState != PlayerState.standby 
                                          && currentState != PlayerState.swim && keyCount[0] == 0){
            cloneState = CloneState.attack;
            StartCoroutine(AttackCo());
            keyCount[0] ++;
        }
        else if (currentState == PlayerState.standby && onBoat){
            Debug.Log("Move boat");
            moveBoat();
        }
        else if (animator.GetBool("swimming")){
            cloneState = CloneState.swim;
            animator.SetFloat("moveX",change.x);
            animator.SetFloat("moveY",change.y);
            animator.SetBool("moving",true);
            movePlayer(false);
            playerFacingDirection.x = change.x;
            playerFacingDirection.y = change.y;
            playerFacingDirection.Normalize();
        }
        else if (change != Vector3.zero && currentState != PlayerState.standby && (currentState == PlayerState.walk || currentState == PlayerState.run)){
            // Debug.Log("Change = ("+change.x.ToString()+","+change.y.ToString());
            animator.SetFloat("moveX",change.x);
            animator.SetFloat("moveY",change.y);
            animator.SetBool("moving",true);
            movePlayer(Input.GetKey(KeyCode.LeftShift)&&(stamina >= 1));
            cloneState = (Input.GetKey(KeyCode.LeftShift)? CloneState.run : CloneState.walk);
            playerFacingDirection.x = change.x;
            playerFacingDirection.y = change.y;
            playerFacingDirection.Normalize();
        }
        
        else if (currentState != PlayerState.standby){
            cloneState = CloneState.walk;
            currentState = PlayerState.walk;
            animator.SetBool("moving",false);
            animator.SetBool("running",false);
        }
        else if (currentState == PlayerState.standby){
            // control bird
            if (astralProjecting){
                moveAstralProjection(change);
            }
        }
        
        playerMoveData.movement = change;
        // playerMoveData.buttonsPressed.Clear();
        playerMoveData.buttonsPressed = buttonsPressed.GetRange(0, buttonsPressed.Count); // Shallow copy
        playerMoveData.cloneState = cloneState;
        playerMoves.Enqueue(playerMoveData);

        try{
            buildSquareCellLocation = tilePalette.grid.WorldToCell(playerCenter+playerFacingDirection);
            buildSquare.transform.position = tilePalette.grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,1);
            noBuildSquare.transform.position = tilePalette.grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,1);
        }
        catch{
            Debug.Log("Grid missing");
        }
        StartCoroutine("handleButtonPressesCo");

        }

    }
    // Update is called once per frame
    public void FixInventory(){
        
        foreach(var uiItem in inventory.inventoryUI.uiItems){
            uiItem.TryFixItem();
        }
        foreach(var uiItem in inventory.hotItemsUI.uiItems){
            uiItem.TryFixItem();
        }
        inventory.inventoryUI.uiItems[0].selectedItem.TryFixItem();
    }
    
    void Update()
    {
        

        playerCenter = gameObject.GetComponent<BoxCollider2D>().bounds.center;        

        if (rigidbody2D == null)
            rigidbody2D = GetComponent<Rigidbody2D>();
        if (rigidbody2D.velocity.magnitude > 0)
            rigidbody2D.velocity *= .75f;
        // if (PlayerPrefs.GetInt("teleportTo") != 0){
        //     int frequency = PlayerPrefs.GetInt("teleportTo");
        //     Vector3 newPosition = transform.position;
        //     foreach(var teleport in GameObject.FindGameObjectsWithTag("Teleport")){
        //         if (teleport.GetComponent<teleporter>().recievingFrequency == frequency)
        //             newPosition = teleport.transform.position;
        //             // Debug.Log("Teleport position is at "+newPosition.ToString());
        //             break;
        //     }
        //     transform.position = newPosition;

            
        //     PlayerPrefs.SetInt("teleportTo",0);
        //     PlayerPrefs.Save();
            // Debug.Log("Putting player at "+newPosition.ToString());
        

        
        
        if (tilePalette.noBuildZone.GetTile(buildSquareCellLocation)== null){
            if (showBuildSquare){
                buildSquare.SetActive(true);
                noBuildSquare.SetActive(false);
            }
            canModifyWorldAtBuildSquare = true;    
        }
            
        else{
            if (showBuildSquare){
                buildSquare.SetActive(false);
                noBuildSquare.SetActive(true);
            }
            canModifyWorldAtBuildSquare = false;    
            }
    }

    
    private IEnumerator handleButtonPressesCo(){



        foreach (var button in buttonsPressed){
            // Debug.Log("button pressed");

            switch(button){
                case KeyCode.T:
                    Debug.Log("torch");
                    torch.SetActive(!torch.activeSelf); 
                    setEnabledShadows(lightCount == 0 && !torch.activeSelf);
                    break;
                case KeyCode.E:
                    if (onBoat){
                        boat.OnCharacterInteract();
                    }
                    else
                        StartCoroutine(interact("interact"));
                    break;
                case KeyCode.Q:
                    StartCoroutine(interact("dig"));
                    break;
                case KeyCode.C:
                    StartCoroutine(interact("chop"));
                    break;
                case KeyCode.F:
                    StartCoroutine(interact("build"));
                    break;
                case KeyCode.M:
                    StartCoroutine(interact("mine"));
                    break;
                case KeyCode.P:
                    Debug.Log("astralProjecting");
                    if (!astralProjecting){
                       astralProject(); 
                    }
                    else
                        returnProjectionToBody();
                    break;
            }
        }

        // reset button presses
        for (int i = 0; i <10; i++){
            if (keyCount[i] != 0)
                keyCount[i]+=1;
            if (keyCount[i]>=15)
                keyCount[i] = 0;
        }

        yield return null;
    }

    public void toggleBuildSquare(bool isToggleOn){
        showBuildSquare = isToggleOn;
        if (!showBuildSquare){
            buildSquare.SetActive(false);
            noBuildSquare.SetActive(false);
        }
        else{
            buildSquare.SetActive(true);
            noBuildSquare.SetActive(true);
        }
    }

    void useItem(Item item, int buttonIndex = -1){
        if (item.use == Item.Uses.tool){
            Debug.Log("Using "+item.title);
            // if (item.use == Item.Uses.tool){
            switch(item.title){
                case "Axe":
                    StartCoroutine(interact("chop"));
                    break;
                case "Garden Shovel":
                    StartCoroutine(interact("dig"));
                    break;
                case "Trench Shovel":
                    StartCoroutine(interact("irrigate"));
                    break;
                case "Pickaxe":
                    StartCoroutine(interact("mine"));
                    break;
                case "Hammer":
                    StartCoroutine(interact("build"));
                    break;
                case "Sword":
                    if (currentState != PlayerState.standby && currentState != PlayerState.swim && keyCount[0] == 0)
                        StartCoroutine(AttackCo());
                    break;
                }
        }
        else if (item.use == Item.Uses.consumable){
            switch(item.title){
                case "Torch L1":
                    if (!torch.activeSelf && sceneManager.isDark())
                        StartCoroutine(igniteTorch(1));
                    break;
                case "Torch L2":
                    if (!torch.activeSelf  && sceneManager.isDark())
                        StartCoroutine(igniteTorch(2));
                    break;
                case "Torch L3":
                    if (!torch.activeSelf  && sceneManager.isDark())
                        StartCoroutine(igniteTorch(3));
                    break;
                case "Health Potion L1":
                    StartCoroutine(drinkPotion(item.title,"health",1));
                    break;
                case "Health Potion L2":
                    StartCoroutine(drinkPotion(item.title,"health",2));
                    break;
                case "Health Potion L3":
                    StartCoroutine(drinkPotion(item.title,"health",3));
                    break;
                case "Health Potion L4":
                    StartCoroutine(drinkPotion(item.title,"health",4));
                    break;
                case "Health Potion L5":
                    StartCoroutine(drinkPotion(item.title,"health",5));
                    break;
                case "Stamina Potion L1":
                    inventory.RemoveItem(item.title);
                    StartCoroutine(staminaBoost(1));
                    break;

            }
        
        }
        else if (item.use == Item.Uses.interactable){
            // Debug.Log("Using "+item.title);
            switch(item.title){
                
                case "Carrot Seed":
                    if (checkIfCanPlant()){
                        StartCoroutine(interact("plant"));
                        // StartCoroutine(sceneManager.addPlantTile(buildSquareCellLocation,"carrot"));
                        inventory.RemoveItem(item.title);
                    }
                    break;
                case "Tomato Seed":
                    if (checkIfCanPlant()){
                        StartCoroutine(interact("plant"));
                        // StartCoroutine(sceneManager.addPlantTile(buildSquareCellLocation,"tomato"));
                        inventory.RemoveItem(item.title);
                    }
                    break;
                case "Bush Sapling":
                    if (checkIfCanPlant()){
                    StartCoroutine(interact("plant"));
                    // StartCoroutine(sceneManager.addPlantTile(buildSquareCellLocation,"bushSapling"));
                    inventory.RemoveItem(item.title);
                    }
                    break;
                case "Tree Sapling":
                    if (checkIfCanPlant()){
                    StartCoroutine(interact("plant"));
                    // StartCoroutine(sceneManager.addPlantTile(buildSquareCellLocation,"treeSapling"));
                    inventory.RemoveItem(item.title);
                    }
                    break;
            }
        }
    }

    public void healPlayer(int amount){
        health += amount;
        if (health > healthMax) 
            health = healthMax;
    }
    private IEnumerator drinkPotion(string title, string type, int level){
        

        inventory.RemoveItem(title);
        if (type == "health"){
            switch (level){
                case 1:
                    healPlayer(15);
                    break;
                case 2:
                    healPlayer(25);
                    break;
                case 3:
                    healPlayer(35);
                    break;
                case 4:
                    healPlayer(55);
                    break;
                case 5:
                    healPlayer(70);
                    break;
            }
        }    
        yield return null;
    }

    private IEnumerator staminaBoost(int level){
        staminaBoosted = true;
        stamina = staminaMax;
        switch (level){
            case 1:
                yield return new WaitForSeconds(5);
                break;
            case 2:
                yield return new WaitForSeconds(10);
                break;
            case 3:
                yield return new WaitForSeconds(15);
                break;
        }

        staminaBoosted = false;
            
    }

    private IEnumerator igniteTorch(int level){
        
        setEnabledShadows(false);
        switch (level){
            case 1:
                inventory.RemoveItem("Torch L1");
                torch.GetComponent<Light>().range = 25;
                torch.GetComponent<Light>().intensity = 15;
                torch.SetActive(true);
                yield return new WaitForSeconds(25);
                break;
            case 2:
                inventory.RemoveItem("Torch L2");
                torch.GetComponent<Light>().range = 35;
                torch.GetComponent<Light>().intensity = 25;
                torch.SetActive(true);
                yield return new WaitForSeconds(45);
                break;
            case 3:
                inventory.RemoveItem("Torch L3");
                torch.GetComponent<Light>().range = 45;
                torch.GetComponent<Light>().intensity = 35;
                torch.SetActive(true);
                yield return new WaitForSeconds(75);
                break;
        }

        torch.SetActive(false);
        if (lightCount == 0)
            setEnabledShadows(true);
        yield return null;
    }
    bool checkIfCanPlant(){
        return ( tilePalette.ground.GetTile(buildSquareCellLocation) == tilePalette.dirt);
    }

    void moveBoat(){
        Vector3 position = transform.position;
        
        position+= change.normalized * speed * Time.deltaTime;
        boat.gameObject.transform.position += change.normalized * speed * Time.deltaTime;
        transform.position = position;

        

        // boat.gameObject.transform.position = playerCenter;
        
    }
    void movePlayer(bool running){

        Vector3 position = transform.position;
        
        if (running && animator.GetBool("swimming") != true){
            animator.SetBool("running",true);
            currentState = PlayerState.run;
            position+= 2* change.normalized * speed * Time.deltaTime;
            foreach(var shadow in shadows){
                if (shadow){
                    shadow.transform.position -=  change.normalized * speed * Time.deltaTime;
                }
            }
        }
        else if (animator.GetBool("swimming") != true){
            animator.SetBool("running",false) ;
            currentState = PlayerState.walk;
            position+= change.normalized * speed * Time.deltaTime;
            foreach(var shadow in shadows){
                if (shadow){
                    shadow.transform.position -= change.normalized * speed/2 * Time.deltaTime;
                }
            }
        }
        else if (animator.GetBool("swimming") == true)
        {
            position+= change.normalized * .35f * speed * Time.deltaTime;
            foreach(var shadow in shadows){
                if (shadow){
                    shadow.transform.position -= change.normalized * .35f/2* speed * Time.deltaTime;
                }
            }
        }
        transform.position = position;
    }

    void moveAstralProjection(Vector3 change){
        if (change != Vector3.zero){
            projectionAnimator.SetBool("moving",true);
            projectionAnimator.SetFloat("moveX",change.x);
            projectionAnimator.SetFloat("moveY",change.y);
            Vector3 position = projection.transform.position;

            position+= change.normalized * 2.2f * speed * Time.deltaTime;

            projection.transform.position = position;
        }
        else{
            projectionAnimator.SetBool("moving",false);
        }
    }

    void astralProject(){
        animator.SetBool("moving",false);
        astralProjecting = true;
        projection.SetActive(true);
        currentState = PlayerState.standby;
        if (playerIsInControl)
            GameObject.Find("MainCamera").GetComponent<GameManager>().cameraTarget = projection;
    }

    void returnProjectionToBody(){
        
        astralProjecting = false;
        projection.transform.position = transform.position;
        projection.SetActive(false);
        currentState = PlayerState.walk;
        if (playerIsInControl)
            GameObject.Find("MainCamera").GetComponent<GameManager>().cameraTarget = this.gameObject;
    }

    public void getInventoryFromGui(){
        foreach(var uiitem in inventory.inventoryUI.uiItems){
            try{
                if (uiitem.spriteImage != null && uiitem.item != null){
                    Debug.Log(uiitem.spriteImage.sprite.name+":"+uiitem.item.amount.ToString());
                    Item item = ItemDatabase.GetItem(uiitem.spriteImage.sprite.name);
                    Item itemToAdd = new Item(item);
                    itemToAdd.amount = uiitem.item.amount;
                    
                    inventory.items.Add(itemToAdd);
                }
            }
            catch{
                
            }
        }
        foreach(var uiitem in inventory.hotItemsUI.uiItems){
            try{
                if (uiitem.spriteImage != null && uiitem.item != null){
                    // Debug.Log(uiitem.spriteImage.sprite.name+":"+uiitem.item.amount.ToString());
                    Item item = ItemDatabase.GetItem(uiitem.spriteImage.sprite.name);
                    Item itemToAdd = new Item(item);
                    itemToAdd.amount = uiitem.item.amount;
                    inventory.items.Add(itemToAdd);
                }
            }
            catch{
                
            }
        }
    }
    
    
    
    private IEnumerator AttackCo(){
    animator.SetTrigger("attack");

    // Deactivate all hitboxes first
    upHitBox.SetActive(false);
    rightHitBox.SetActive(false);
    downHitBox.SetActive(false);
    leftHitBox.SetActive(false);

    // Activate the appropriate hitbox based on playerFacingDirection
    if (playerFacingDirection.x > 0) {
        rightHitBox.SetActive(true);
    } else if (playerFacingDirection.x < 0) {
        leftHitBox.SetActive(true);
    } else if (playerFacingDirection.y > 0) {
        upHitBox.SetActive(true);
    } else if (playerFacingDirection.y < 0) {
        downHitBox.SetActive(true);
    }

    currentState = PlayerState.attack;
    yield return null; //Wait a frame
    yield return new WaitForSeconds(.2f);
    currentState = PlayerState.walk;

    // Deactivate all hitboxes after attack
    upHitBox.SetActive(false);
    rightHitBox.SetActive(false);
    downHitBox.SetActive(false);
    leftHitBox.SetActive(false);
}


    

    public void interfaceStoreInventoryAndUi(Inventory storeInventory, bool storeIsOpening){
        
        
    }


    public IEnumerator interact(string type){
        // Debug.Log("interact");
        //Start with four playerFacingDirections
        if (objectBeingCarried != null){
            objectBeingCarried.OnCharacterInteract();
            yield break;  // This signals the end of the IEnumerator
        }
        
        playerCenter = gameObject.GetComponent<BoxCollider2D>().bounds.center;
        Vector3 perpendicularDirection = new Vector3(playerFacingDirection.y,-playerFacingDirection.x,0);
        Debug.DrawRay(playerCenter+playerFacingDirection*.5f,perpendicularDirection*.5f,Color.red);

        // combine the layers later
        if (type == "interact"){
            // Debug.Log("Interact");

            interactableHit = Physics2D.Raycast(playerCenter+playerFacingDirection*.5f-perpendicularDirection*.3f,perpendicularDirection,.6f,1<<interactLayer);
            

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
                
                
                Debug.DrawRay(playerCenter+playerFacingDirection*.5f-perpendicularDirection*.3f,perpendicularDirection*.6f,Color.red,1f);
            
                
            }
            else{
                // Debug.Log("Nothing on the interact layer");
                // Check for fence
                if (tilePalette.interactable.GetTile(buildSquareCellLocation)==tilePalette.mushroomTile){
                        tilePalette.interactable.SetTile(buildSquareCellLocation,null);
                        inventory.GiveItem("Mushroom");;
                    }
                else if (tilePalette.decor.GetTile(buildSquareCellLocation)==tilePalette.forestFlower)
                    tilePalette.decor.SetTile(buildSquareCellLocation,null);
                else if (tilePalette.choppable.GetTile(buildSquareCellLocation) == tilePalette.gateOpen)
                    tilePalette.choppable.SetTile(buildSquareCellLocation,tilePalette.gateClosed);
                else if (tilePalette.choppable.GetTile(buildSquareCellLocation) == tilePalette.gateClosed)
                    tilePalette.choppable.SetTile(buildSquareCellLocation,tilePalette.gateOpen);
                else if (tilePalette.choppable.GetTile(buildSquareCellLocation)==tilePalette.appleTree){
                    tilePalette.choppable.SetTile(buildSquareCellLocation,tilePalette.appleTreeEmpty);
                    inventory.GiveItem("Apple");
                }
                else if (tilePalette.ground.GetTile(buildSquareCellLocation)==tilePalette.tomato)
                {
                    tilePalette.ground.SetTile(buildSquareCellLocation,tilePalette.dirt);
                    inventory.GiveItem("Tomato");
                }
                else if (tilePalette.ground.GetTile(buildSquareCellLocation)==tilePalette.carrot)
                {
                    tilePalette.ground.SetTile(buildSquareCellLocation,tilePalette.dirt);
                    inventory.GiveItem("Carrot");
                }

                    //Give player apple
            }
        }

        if (currentState == PlayerState.standby){
            yield return null;
        }

        if (type == "chop" && canModifyWorldAtBuildSquare){
            // choppableHit = Physics2D.Raycast(buildSquare.transform.position,-Vector3.forward,.6f,1<<choppableLayer);
            
            if(tilePalette.choppable.GetTile(buildSquareCellLocation)!=null){ 
                
                // Debug.Log("We hit a tree tile at " + buildSquareCellLocation.ToString());

                // if (Random.Range(0,5) == 4){
                    
                    tilePalette.choppable.SetTile(buildSquareCellLocation,null);
                    // tilePalette.choppable.GetComponent<TilemapCollider2D>().enabled = false;
                    // tilePalette.choppable.GetComponent<TilemapCollider2D>().enabled = true;
                    inventory.GiveItem("Wood");
                // }
                // interactableHit.collider.gameObject.GetComponent<Interactable>().OnCharacterInteract();
                // Debug.DrawRay(playerCenter+playerFacingDirection*.75f-perpendicularDirection*.3f,perpendicularDirection*.6f,Color.red,1f);
            }
            else{
                // Debug.Log("Nothing to chop");
                Debug.DrawRay(playerCenter+playerFacingDirection*.75f-perpendicularDirection*.3f,perpendicularDirection*.6f,Color.green,1f);
            }
        }

        else if (type == "mine" && canModifyWorldAtBuildSquare){
            // choppableHit = Physics2D.Raycast(buildSquare.transform.position,-Vector3.forward,.6f,1<<choppableLayer);
            
            if(tilePalette.minable.GetTile(buildSquareCellLocation)==tilePalette.forestRock ||
               tilePalette.minable.GetTile(buildSquareCellLocation)==tilePalette.minableRock)
               {
                    tilePalette.minable.SetTile(buildSquareCellLocation,null);
                    inventory.GiveItem("Rock");

               } 
            else if (tilePalette.minable.GetTile(buildSquareCellLocation)==tilePalette.minableIron){
                tilePalette.minable.SetTile(buildSquareCellLocation,null);
                inventory.GiveItem("Iron Ore");

            }   
            else if (tilePalette.minable.GetTile(buildSquareCellLocation)==tilePalette.minableGold){
                tilePalette.minable.SetTile(buildSquareCellLocation,null);
                inventory.GiveItem("Gold Ore");

            }
            else if (tilePalette.ground.GetTile(buildSquareCellLocation)==tilePalette.cobbleStonePath){
                tilePalette.ground.SetTile(buildSquareCellLocation,tilePalette.grass);
                inventory.GiveItem("Rock");

            }
                
            
        }

        //Change choppable
        else if (type == "build" && canModifyWorldAtBuildSquare)
        {
            Debug.Log("Initiating build process...");
            
            if (tilePalette.ground.GetTile(buildSquareCellLocation) == tilePalette.water)
            {
                Debug.LogWarning("Build square is water. Cannot build here.");
                yield return null;
            }

            choppableHit = Physics2D.Raycast(buildSquare.transform.position, -Vector3.forward, .6f, 1 << choppableLayer);
            Debug.Log($"Trying to build a {activeBlueprint.title}");

            bool hasAllItems = true;
            string[] keys = new string[activeBlueprint.price.Keys.Count];
            activeBlueprint.price.Keys.CopyTo(keys, 0);

            TileBase currentTile = tilePalette.choppable.GetTile(buildSquareCellLocation);
            TileBase groundTile = tilePalette.ground.GetTile(buildSquareCellLocation);

            bool isTileBuildable =  tilePalette.choppable.GetTile(buildSquareCellLocation) ==null &&
                                    tilePalette.minable.GetTile(buildSquareCellLocation) == null &&
                                    tilePalette.collidable.GetTile(buildSquareCellLocation) == null;

            switch (activeBlueprint.title)
            {
                case "Fence":
                    if (isTileBuildable)
                    {
                        hasAllItems = inventory.checkIfItemsExistsAndRemove(keys);
                        if (hasAllItems)
                        {
                            tilePalette.choppable.SetTile(buildSquareCellLocation, tilePalette.fence);
                            Debug.Log("Built Fence");
                        }
                        else
                        {
                            Debug.LogWarning("Not enough items to build Fence");
                        }
                    }
                    else if (currentTile == tilePalette.gateOpen || currentTile == tilePalette.gateClosed)
                    {
                        tilePalette.choppable.SetTile(buildSquareCellLocation, tilePalette.fence);
                        Debug.Log("Replaced Gate with Fence");
                    }
                    break;

                case "Gate":
                    if (isTileBuildable)
                    {
                        hasAllItems = inventory.checkIfItemsExistsAndRemove(keys);
                        if (hasAllItems)
                        {
                            tilePalette.choppable.SetTile(buildSquareCellLocation, tilePalette.gateClosed);
                            Debug.Log("Built Gate");
                        }
                        else
                        {
                            Debug.LogWarning("Not enough items to build Gate");
                        }
                    }
                    else if (currentTile == tilePalette.fence)
                    {
                        tilePalette.choppable.SetTile(buildSquareCellLocation, tilePalette.gateClosed);
                        Debug.Log("Replaced Fence with Gate");
                    }
                    break;
                case "Sack":
                   if (isTileBuildable){
                        hasAllItems = inventory.checkIfItemsExistsAndRemove(keys);
                        if (hasAllItems)
                        {
                            // Instantiate(tilePalette.sack,tilePalette.grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,0),Quaternion.identity);
                            Debug.Log("Built Sack");
                        }
                        else
                        {
                            Debug.LogWarning("Not enough items to build Sack");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Cannot build Sack because the space isn't clear");
                    }
                    break;
                case "Sign":
                    if (isTileBuildable){
                        hasAllItems = inventory.checkIfItemsExistsAndRemove(keys);
                        if (hasAllItems)
                        {
                            // GameObject sign = Instantiate(tilePalette.sign,tilePalette.grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,0),Quaternion.identity);
                            // sign.GetComponent<Sign>().customizable = true;
                            // Debug.Log("Built Sign");
                        }
                        else
                        {
                            Debug.LogWarning("Not enough items to build Sign");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Cannot build Sign because the space isn't clear");
                    }
                    break;
                case "Chest":
                    if (isTileBuildable){
                        hasAllItems = inventory.checkIfItemsExistsAndRemove(keys);
                        if (hasAllItems)
                        {
                            // Instantiate(tilePalette.chest,tilePalette.grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,0),Quaternion.identity);
                            Debug.Log("Built Chest");
                        }
                        else
                        {
                            Debug.LogWarning("Not enough items to Chest");
                            
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Cannot build Chest because the space isn't clear");
                    }
                    break;

                case "Cobblestone Path":
                    if (groundTile != tilePalette.water)
                    {
                        hasAllItems = inventory.checkIfItemsExistsAndRemove(keys);
                        if (hasAllItems)
                        {
                            tilePalette.ground.SetTile(buildSquareCellLocation, tilePalette.cobbleStonePath);
                            Debug.Log("Built Cobblestone Path");
                        }
                        else
                        {
                            Debug.LogWarning("Not enough items to build Cobblestone Path");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Cannot build Cobblestone Path on water");
                    }
                    break;

                default:
                    Debug.LogError($"Unknown blueprint title: {activeBlueprint.title}");
                    break;
            }

            if (choppableHit.collider != null)
            {
                Debug.Log("Choppable object hit detected");
            }
        }
        
        else if (type == "dig" && canModifyWorldAtBuildSquare){
            // tilePalette.ground.GetComponent<TilemapCollider2D>().enabled = false;
            groundHit = Physics2D.Raycast(buildSquare.transform.position,-Vector3.forward,1f,1<<groundLayer);
            // Debug.Log("We are attempting to dig at "+buildSquareCellLocation.ToString());
            if (tilePalette.ground.GetTile(buildSquareCellLocation) == tilePalette.grass ||
                tilePalette.ground.GetTile(buildSquareCellLocation) == tilePalette.dirt ||
                tilePalette.ground.GetTile(buildSquareCellLocation) == tilePalette.ploughedDirt){
                // Debug.Log("Grass is here");
                //For now lets make sure it's a center grass piece because we don't have the art
                Vector3Int checkPosition = buildSquareCellLocation + new Vector3Int(-1,-1,0);
                
                
                    
                
                
                tilePalette.ground.SetTile(buildSquareCellLocation,tilePalette.dirt);


                }
            }
            
        else if (type == "plant" && canModifyWorldAtBuildSquare){
            // Debug.Log("Trying to plant");
            if (tilePalette.ground.GetTile(buildSquareCellLocation) == tilePalette.dirt)
                tilePalette.ground.SetTile(buildSquareCellLocation,tilePalette.ploughedDirt);
        }

        else if (type == "irrigate" && canModifyWorldAtBuildSquare){
            // irrigation.SetTile(buildSquareCellLocation,hole);
            bool thereWaterAtNESW =false;

            // Vector for NESW we currently have the diagonals 
            
            for (int i = 0; i < 4; i++){
                if (tilePalette.ground.GetTile(buildSquareCellLocation+directionalPointers[i])==tilePalette.water){
                    thereWaterAtNESW = true;
                }
            }
            
            if (!thereWaterAtNESW){
                // tilePalette.ground.SetTile(buildSquareCellLocation,tilePalette.hole);
                // tilePalette.decor.SetTile(buildSquareCellLocation,null);
            }
            else{
                tilePalette.ground.SetTile(buildSquareCellLocation,tilePalette.water);
                tilePalette.decor.SetTile(buildSquareCellLocation,null);
            }
        }
        yield return null;
    }

    public void fallOffPlatform(){
        Debug.Log("Player fell to his death");
        transform.position = new Vector3(-13.39f,21.21f,0);
    }
    public void die(){
        Debug.Log("Player died from natural causes");
    }
    public void TakeDamage(float damage = 1){
        health -= damage;
        if (health <= 0)
            die();
    }
    public void recoil(float force, Vector3 origin){
        playerCenter = gameObject.GetComponent<BoxCollider2D>().bounds.center;
        Vector3 direction = (playerCenter-origin).normalized;
        rigidbody2D.isKinematic = false;
        rigidbody2D.AddForce(direction*force,ForceMode2D.Impulse);
        rigidbody2D.isKinematic = true;
        // Debug.Log("Direction of recoil is "+direction.ToString() + " and player center is "+ playerCenter.ToString());
    }


    void OnTriggerEnter2D(Collider2D collision){
            if ((collision.gameObject.GetComponent("Light") as Light)!= null){
            lightCount ++;
            setEnabledShadows(false);
        }

    }

    void OnTriggerExit2D(Collider2D collision){
        if ((collision.gameObject.GetComponent("Light") as Light)!= null){
        lightCount --;
        if ( lightCount <= 0){
            setEnabledShadows(true);
        }
        }
    }

    public void setEnabledShadows(bool enabled){
        // Debug.Log("Setting the shadows "+(enabled?"on":"off"));
        bool turnOn = enabled;
        if (torch.activeSelf && enabled)  
            turnOn = false;
        if (!sceneManager.isDark())
            turnOn = false;
        shadowsFollowing = turnOn;
        foreach(var shadow in shadows){
            shadow.gameObject.SetActive(turnOn);
        }
    }

    public void checkIfNewRealm(){
        if (sceneManager.isMajorRealm && !realmsDiscovered.Contains(sceneManager.realmName))
            realmsDiscovered.Add(sceneManager.realmName);
        else if (!sceneManager.isMajorRealm && !miniRealmsDiscovered.Contains(sceneManager.realmName))
            miniRealmsDiscovered.Add(sceneManager.realmName);
    }
    
    public int howManyRealmsHasPlayerDiscovered(bool majorRealms){
        if (majorRealms)
            return realmsDiscovered.Count;
        else
            return miniRealmsDiscovered.Count;    
    }
}
