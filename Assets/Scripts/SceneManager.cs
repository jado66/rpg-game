using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlantTileData{
    public Vector3Int location { get; set; }
    public int age{ get; set; }

    public string name{ get; set; }

    public PlantTileData(Vector3Int location,string name){
        this.location = location;
        this.age = 0;
        this.name = name;
    }

    public void AdvanceAge(){
        this.age = this.age+1;
        Debug.Log("Advancing age of "+name+" to "+(age).ToString());
    }

    public void resetAge(){
        age=0;
    }
    
}

public class SceneManager : MonoBehaviour
{
    //Change to scene manager
    string sceneName;
    public Player player;

    public TilePallete tilePallete;
 
    public bool developersMode;

    public GameObject playerUiGameObject;

    public List<PlantTileData> plantTiles = new List<PlantTileData>();

    public float sunDialAngle;
    public float daytimeHours; 
    public float nightTimeHours;
    public float gameHoursToRealSecs;
    public float minIntensity;
    public float maxIntensity;
    public float hour;

    public PlayerUI playerUI;

    public int day;

    public float[] minIntensities;

    float dayTransitionSpeed;
    float nightTransitionSpeed;

    float worldLightsOnTime;
    float worldLightsOutTime;
    
    Light worldLight;

    public bool timeIsPaused;

    bool worldLightsOn;

    public float timeBeforeFreeze = -1;

    bool sunRising;
    // Start is called before the first frame update
    private float[] keyCount = new float[10];

    public string realmName;

    public bool isMajorRealm;

    void Start()
    {
        player = GameObject.Find("Character").GetComponent<Player>();
        tilePallete = GameObject.Find("TilePallete").GetComponent<TilePallete>();
        // Setup Gui
        sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        playerUI = playerUiGameObject.GetComponent<PlayerUI>();

        //Hide GUI elements
        
        // Debug.Log("Buy/sell menu is "+(buyNSellMenu.activeSelf?"still on":"turned off"));


        worldLight = GetComponent<Light>(); 
        dayTransitionSpeed = ((maxIntensity-minIntensity)/2)/((daytimeHours*gameHoursToRealSecs)/4);
        nightTransitionSpeed = ((maxIntensity-minIntensity)/2)/((nightTimeHours*gameHoursToRealSecs)/4);
        // Debug.Log("Day transition speed is "+dayTransitionSpeed.ToString());
        // Debug.Log("Night transition speed is "+nightTransitionSpeed.ToString());
        if (developersMode)
            worldLight.intensity = maxIntensity/2;

        player.LinkPlayerToUI();

        try{
            GameObject.Find("PlayerName").GetComponent<Text>().text = player.playerName;
        }
        catch{

        }
    }

    public string getSceneName(){
        return sceneName;
    }


    public void toggleBuildMenuTab(int i){
        foreach(var tab in  playerUI.buildMenuTabs){
            tab.SetActive(false);
        }
        playerUI.buildMenuTabs[i].SetActive(true);

        player.activeBlueprint = null;

        foreach( var buildSlot in playerUI.buildMenuTabs[i].GetComponent<BuildMenu>().bluePrintSlots){
            buildSlot.transform.parent.GetComponent<Image>().color = new Color(.2627f,.2627f,.2627f); 
        }
    }
    public void ToggleInventory(){
        if (playerUI.customSignBox.activeSelf)
            return;

        if (playerUI.buildGui.activeSelf){
            playerUI.buildGui.SetActive(false);
        }
        if (playerUI.mainMenuGui.activeSelf){
            playerUI.mainMenuGui.SetActive(false);
        }
        if (player.currentState== PlayerState.standby){
            // Fix me
            player.StartCoroutine(player.interact("interact"));
        }
        playerUI.inventoryGui.SetActive(!playerUI.inventoryGui.activeSelf);
    }
    public void ToggleBuildMenu(){
        if (playerUI.inventoryGui.activeSelf){
            ToggleInventory();
        }
        playerUI.buildGui.SetActive(!playerUI.buildGui.activeSelf);
    }

    public bool isDark(){
        if (worldLight == null)
            worldLight = GetComponent<Light>();
        return (worldLight.intensity < .5f);
    }
    

    // Update is called once per frame

    void FixedUpdate(){
        if (Input.GetKey(KeyCode.I) && keyCount[1] == 0){
            ToggleInventory();
            keyCount[1]+=1;
        }
        StartCoroutine("handleButtonPressesCo");
    }


    
    public void loadAndUnloadChest(Inventory externalInventory, bool isOpen){
        if (! isOpen || player.currentState == PlayerState.standby)
            {
            // Debug.Log("Closing chest");
            player.currentState = PlayerState.walk;
            Debug.Log("Player was"+player.inventory.items.Count + " items long");
            // Set inventory to be whatever state it was left in
            List<Item> newItems = new List<Item>();
            UIInventory externalInventoryGui = playerUI.externalInventoryPanels.GetComponent<UIInventory>();
            for(int i = 0; i < externalInventoryGui.inventorySize; i++){
                if (externalInventoryGui.uiItems[i].item != null){
                    newItems.Add(externalInventoryGui.uiItems[i].item);
                    player.inventory.RemoveSpecificSlot(externalInventoryGui.uiItems[i].item);
                }
            }
            externalInventory.overrideInventory(newItems);
            
            
            player.inventory.items.Clear();
            player.getInventoryFromGui();

            Debug.Log("and is now"+player.inventory.items.Count + " items long");
           

            playerUI.externalInventoryGui.SetActive(false);   
            playerUI.externalInventoryPanels.SetActive(false);     
        }
        else{
            // Debug.Log("Opening Chest");
            player.currentState = PlayerState.standby;
            if (!playerUI.inventoryGui.activeSelf )
                playerUI.inventoryGui.SetActive(true);
            playerUI.externalInventoryGui.SetActive(true);  
            playerUI.externalInventoryPanels.SetActive(true);

            // Import the images
            UIInventory externalInventoryGui = playerUI.externalInventoryPanels.GetComponent<UIInventory>();
            int externalInventoryCount = externalInventory.items.Count;
            for(int i = 0; i < externalInventoryGui.inventorySize; i++){
                externalInventoryGui.uiItems[i].amount.text = "";
                externalInventoryGui.uiItems[i].item = null;
                if (i < externalInventoryCount){
                    
                    externalInventoryGui.uiItems[i].UpdateItem(externalInventory.items[i]);
                }
                else
                    externalInventoryGui.uiItems[i].UpdateItem(null);
            }
        }
    }   

    public void loadAndUnloadStoreInventory(Inventory storeInventory, bool isOpen){
        if (! isOpen || player.currentState == PlayerState.standby)
            {
            // Closing store
            player.currentState = PlayerState.walk;
            Debug.Log("Player was"+player.inventory.items.Count + " items long");
            // Set inventory to be whatever state it was left in
            List<Item> newItems = new List<Item>();
            StoreUiInventory storeInventoryUI = playerUI.buyNSellMenuPanel.GetComponent<StoreUiInventory>();
            for(int i = 0; i < storeInventoryUI.inventorySize; i++){
                if (storeInventoryUI.uiItems[i].item != null){
                    // Remove items sold (I really need to dynamically remove them as i click them)
                    newItems.Add(storeInventoryUI.uiItems[i].item);
                    player.inventory.RemoveSpecificSlot(storeInventoryUI.uiItems[i].item);
                }
            }
            storeInventory.overrideInventory(newItems);
            
            
            player.inventory.items.Clear();
            player.getInventoryFromGui();

            Debug.Log("and is now"+player.inventory.items.Count + " items long");
           

            playerUI.buyNSellMenu.SetActive(false);   
            playerUI.buyNSellMenuPanel.SetActive(false);     
        }
        else{
            // Opening Store
            player.currentState = PlayerState.standby;
            if (!playerUI.inventoryGui.activeSelf )
                playerUI.inventoryGui.SetActive(true);
            playerUI.buyNSellMenu.SetActive(true);  
            playerUI.buyNSellMenuPanel.SetActive(true);

            // Import the images
            StoreUiInventory storeInventoryUI = playerUI.buyNSellMenuPanel.GetComponent<StoreUiInventory>();
            int storeInventoryCount = storeInventory.items.Count;
            for(int i = 0; i < storeInventoryUI.inventorySize; i++){
                storeInventoryUI.uiItems[i].amount.text = "";
                storeInventoryUI.uiItems[i].item = null;
                if (i < storeInventoryCount){
                    
                    storeInventoryUI.uiItems[i].UpdateItem(storeInventory.items[i]);
                }
                else
                    storeInventoryUI.uiItems[i].UpdateItem(null);
            }
        }
    }   
    
    
    void Update()
    {   // 0 - 8
        
        // string debugInventoryText = "";
        
        // foreach(var item in player.inventory.items){
        //     debugInventoryText+=string.Format("{0},    ",item.title);
        // }
        // if (debugInventory == null)
        //     debugInventory = GameObject.Find("Debug Inventory");
        // debugInventory.GetComponent<Text>().text = debugInventoryText.ToString();
        // Debug.Log("Updating inventory text");

        if (!timeIsPaused)
            hour += Time.deltaTime;
        sunDialAngle = (hour< daytimeHours*gameHoursToRealSecs)?(hour/((daytimeHours)*gameHoursToRealSecs))*180:(hour-(daytimeHours*gameHoursToRealSecs))/((nightTimeHours)*gameHoursToRealSecs)*180+180;


        if (!developersMode){
            if (hour < daytimeHours * gameHoursToRealSecs/4){
                if (!isDark() && player.shadowsFollowing)
                    player.setEnabledShadows(false);
                sunRising = true;
                if (minIntensity!= maxIntensity)
                    worldLight.intensity = (maxIntensity-minIntensity)/2* (Mathf.Sin((2*Mathf.PI)/(daytimeHours*gameHoursToRealSecs) * (hour)-Mathf.PI/2)+1)+minIntensity;}
            else if (hour < 3 * daytimeHours * gameHoursToRealSecs/4){
                // Debug.Log("Midday"+ (Mathf.Sin((2*Mathf.PI*hour)/(daytimeHours*gameHoursToRealSecs) -Mathf.PI/2)+1).ToString());
                if (minIntensity!= maxIntensity)
                    worldLight.intensity = .5f*maxIntensity* (Mathf.Sin((2*Mathf.PI)/(daytimeHours*gameHoursToRealSecs) * (hour)-Mathf.PI/2)+1);
            }
            else if (hour > 3/4 * daytimeHours * gameHoursToRealSecs && hour < daytimeHours * gameHoursToRealSecs){
                if (sunRising){
                    if (minIntensities.Length > 0)
                        minIntensity = minIntensities[day%minIntensities.Length];
                }
                
                sunRising = false;
                if (minIntensity!= maxIntensity)
                    worldLight.intensity = (maxIntensity-minIntensity)/2* (Mathf.Sin((2*Mathf.PI*hour)/(daytimeHours*gameHoursToRealSecs) -Mathf.PI/2)+1)+minIntensity;}
            else if (hour > daytimeHours * gameHoursToRealSecs && hour < gameHoursToRealSecs* (daytimeHours+nightTimeHours))
                if (minIntensity!= maxIntensity)
                    worldLight.intensity = minIntensity;
                    if (isDark() && !player.shadowsFollowing)
                        player.setEnabledShadows(true);
            else if (hour > (daytimeHours+nightTimeHours )*gameHoursToRealSecs){
                Debug.Log("new day");
                
                hour = 0;
                day++;
                StartCoroutine(advancePlants()); // Maybe make this randomly happen throughout the day
                }
        }
        else{
            if (hour > gameHoursToRealSecs* (daytimeHours+nightTimeHours) ){
                hour = 0;
                day++;
                StartCoroutine(advancePlants());
            }
        }

        if (worldLightsOutTime != 0 && worldLightsOutTime != 0){
            if ((hour > worldLightsOutTime && worldLightsOn) || (hour > worldLightsOnTime && !worldLightsOn))
                toggleWorldLights();
        }
        playerUI.sunDialSun.transform.rotation = Quaternion.Euler(0,0,-sunDialAngle);
        playerUI.sunDialMoon.transform.rotation = Quaternion.Euler(0,0,-sunDialAngle-180);         
    }

    void toggleWorldLights(){
        foreach(var worldLight in GameObject.FindGameObjectsWithTag("WordLight")){
            worldLight.SetActive(worldLightsOn);
        }
        
        worldLightsOn = !worldLightsOn;

    }

    

    public void pauseTimeSpell(float time){
        if (timeIsPaused){
            StartCoroutine(transitionTime(timeBeforeFreeze));
            timeBeforeFreeze = -1;
            timeIsPaused = false;
        }
        else{
            timeBeforeFreeze = hour;
            timeIsPaused = true;
            StartCoroutine(transitionTime(time));
        }
        
    }
 
    public void playerSleeps(){
        if (hour > daytimeHours * gameHoursToRealSecs *.95f)  {
            StartCoroutine(transitionTimeForward((daytimeHours+nightTimeHours*.95f)*gameHoursToRealSecs));
            Debug.Log("Moving time to "+ (daytimeHours+nightTimeHours*.8f).ToString());
        }
        else{
            StartCoroutine(transitionTimeForward(daytimeHours*.95f*gameHoursToRealSecs));
            Debug.Log("Moving time to "+ (daytimeHours*.95f*gameHoursToRealSecs).ToString());
        }

        // Heal player
        player.health = player.healthMax;

        

    }
    private IEnumerator transitionTimeForward(float time){
        timeIsPaused = true;
        //are we going across a day? if so the time will be less than the day
        float increment;
        if (time > hour)
            increment = (time-hour)/360;
        else{
            increment = ((daytimeHours+nightTimeHours)*gameHoursToRealSecs-hour+time)/360;
        }

        Debug.Log("Incrementing time by "+increment.ToString());
        int i = 0;
        while (i < 360 ){
            if (hour >= (daytimeHours+nightTimeHours)*gameHoursToRealSecs){
                hour = 0;
                day++;
                StartCoroutine(advancePlants());
            }
            hour += increment;
            
            if (i%6 == 0)
                yield return new WaitForSeconds(.05f);
            i++;
            Debug.Log(i);

            if (Mathf.Abs(time-hour)<2*increment){
                break;
            }
        } 
        timeIsPaused = false;
        yield return null;
    }
    private IEnumerator transitionTime(float time){
        int i = 0;
        // bool to see if we are incrementing the time up or down
        bool sunGoesUp = time > hour;

        float increment = Mathf.Abs(time-hour)/360;

        while (i < 10000 && hour != time){
            if (sunGoesUp)
                hour += increment;
            else
                hour -= increment;

            
            

            if (time > hour != sunGoesUp){
                hour = time;
                
            }

            if (i%6 == 0)
                yield return null;
        } 
        i++;
        
    }

    private IEnumerator advancePlants(){
        // Debug.Log("Advancing plants");
        System.Random _random = new System.Random (); // change this to a seeded value

        // Shuffle the list of plants so they pop advance in a fun order
        PlantTileData myGO;
 
        int n = plantTiles.Count;
        for (int i = 0; i < n; i++)
        {
             // NextDouble returns a random number between 0 and 1.
             // ... It is equivalent to Math.random() in Java.
            int r = i + (int)(_random.NextDouble() * (n - i));
            myGO = plantTiles[r];
            plantTiles[r] = plantTiles[i];
            plantTiles[i] = myGO;
        }

        List<int> indeciesToRemove = new List<int>();

        // Now advance the plants
        for (int i = 0; i < n; i++)
        {
            plantTiles[i].AdvanceAge();
            int[] growthProgression = ItemDatabase.plantDatabase[plantTiles[i].name].growthProgression;
            
            for (int j = 0; j< growthProgression.Length;j++){
                // Debug.Log("Plant age ="+plantTiles[i].age.ToString() +" and sapling stage is " + growthProgression[0]);
                if (plantTiles[i].age == growthProgression[j]){
                    // Debug.Log("We have a plant growing up");
                    if (j == 0){

                        // Check for if we are close to water
                        // bool waterIsNear = itemDatabase.plantDatabase[plantTiles[i].name].howFarFromWaterToGrow != 0;

                        bool waterIsNear = true;

                        if (!waterIsNear){
                            int distanceNeededToWater = ItemDatabase.plantDatabase[plantTiles[i].name].howFarFromWaterToGrow;
                            for (int k = 0; k < distanceNeededToWater;k++){
                                for (int l = 0; l < distanceNeededToWater;l++){
                                    if (tilePallete.ground.GetTile(plantTiles[i].location + new Vector3Int(-2+k,-2+l,0))==tilePallete.water){
                                        waterIsNear = true;
                                        Debug.Log("We've water");
                                        break;
                                    }
                                }
                                if (waterIsNear)
                                    break;
                            }
                        }

                        if (waterIsNear){
                            Debug.Log("Turning "+ plantTiles[i].name + " to sapling");
                            tilePallete.ground.SetTile(plantTiles[i].location,tilePallete.sapling);
                        }
                        else{
                            indeciesToRemove.Add(i);
                            tilePallete.ground.SetTile(plantTiles[i].location,tilePallete.dirt);
                        }
                        //Sapling
                    }
                    else if (j == 1){
                        //Ripe

                        // bool waterIsNear = itemDatabase.plantDatabase[plantTiles[i].name].howFarFromWaterToGrow != 0;
                        bool waterIsNear = true;
                        if (!waterIsNear){
                            int distanceNeededToWater = ItemDatabase.plantDatabase[plantTiles[i].name].howFarFromWaterToGrow;

                            for (int k = 0; k < distanceNeededToWater;k++){
                                for (int l = 0; l < distanceNeededToWater;l++){
                                    if (tilePallete.ground.GetTile(plantTiles[i].location + new Vector3Int(-2+k,-2+l,0))==tilePallete.water){
                                        waterIsNear = true;
                                        break;
                                    }
                                }
                                if (waterIsNear)
                                    break;
                            }
                        }

                        if (waterIsNear){

                            // Debug.Log(plantTiles[i].name + " is now ripe");
                            switch (plantTiles[i].name){
                                case "carrot":
                                    tilePallete.ground.SetTile(plantTiles[i].location,tilePallete.carrot);
                                    break;
                                case "tomato":
                                    tilePallete.ground.SetTile(plantTiles[i].location,tilePallete.tomato);
                                    break;
                                case "appleTreeSapling":
                                    tilePallete.ground.SetTile(plantTiles[i].location,tilePallete.grass);
                                    tilePallete.choppable.SetTile(plantTiles[i].location,tilePallete.appleTreeEmpty);
                                    // Add apple tree to list
                                    break;
                                case "bushSapling":
                                    tilePallete.choppable.SetTile(plantTiles[i].location,tilePallete.bush);
                                    break;
                                case "treeSapling":
                                    tilePallete.choppable.SetTile(plantTiles[i].location,tilePallete.tree);
                                    break;
                            }
                            if (ItemDatabase.plantDatabase[plantTiles[i].name].isPermanent){
                                indeciesToRemove.Add(i);
                                tilePallete.ground.SetTile(plantTiles[i].location,tilePallete.grass);
                            }
                        }
                        else{
                            indeciesToRemove.Add(i);
                            tilePallete.ground.SetTile(plantTiles[i].location,tilePallete.dirt);
                        }
                    } 
                    else if (j == 2){

                        tilePallete.ground.SetTile(plantTiles[i].location,tilePallete.dirt);
                        indeciesToRemove.Add(i);
                        //Dead
                        
                        //remove me from list
                    }
                    else if (j == 3){
                        // Check if periannual or whatnot
                    }
                }
            }
            yield return new WaitForSeconds(.3f); // Make this random we want to spread these out throughout the day
            //Make noise at build location
        }

        for (int i = indeciesToRemove.Count; i --> 0; ){
            plantTiles.RemoveAt(indeciesToRemove[i]);
        }


        yield return null;
    }


    
    public IEnumerator addPlantTile(Vector3Int location, string plantName){
        // if we don't have one already
        bool duplicate = false;

        for (int i = 0; i<plantTiles.Count;i++){
            if (plantTiles[i].location == location)
                duplicate = true;
                yield return null;
        }

        if (!duplicate)
            plantTiles.Add(new PlantTileData(location,plantName));
    }

    private IEnumerator resetPlant(Vector3Int location){
        for (int i = 0; i < plantTiles.Count; i++){
            if (plantTiles[i].location == location){
                plantTiles[i].resetAge();
                break;
            }
        }
        yield return null;
    }

    
    void growAllPlants(){
        // Tilemap dirt = GameObject.Find("Dirt").GetComponent<Tilemap>();
        // Tilemap ground = GameObject.Find("Ground").GetComponent<Tilemap>();
        // Tilemap choppable = GameObject.Find("Choppable").GetComponent<Tilemap>();

        // foreach (var pos in ground.cellBounds.allPositionsWithin)
        // {   
        // Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
        
        //     if (ground.GetTile(localPlace)==sapling){
        //         Debug.Log("We found a tree");
        //         ground.SetTile(localPlace,grass);
        //         choppable.SetTile(localPlace,tree);
        //     }
        //     if (dirt.GetTile(localPlace)==sapling){
        //         Debug.Log("We found a tree");
        //         ground.SetTile(localPlace,grass);
        //         choppable.SetTile(localPlace,tree);
        //     }
        // }
    }

    private IEnumerator handleButtonPressesCo(){
        

        // reset button presses
        for (int i = 0; i <10; i++){
            if (keyCount[i] != 0)
                keyCount[i]+=1;
            if (keyCount[i]>=15)
                keyCount[i] = 0;
        }

        yield return null;
    }
}
