using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using PlantSystem; 

public class SceneManager : MonoBehaviour
{
    public Character player1;
    public TilePalette tilePalette;
    public bool developersMode;

    public string realmName;

    public bool isMajorRealm;


    public float timeBeforeFreeze = -1;

    public bool isPaused = false;

    public bool timeIsPaused;


    [Header("Music")]
    public MusicChanger musicChanger;


    [Header("Time Settings")]
    public float daytimeHours = 5f;
    public float nightTimeHours = 1f;
    public float gameHoursToRealSecs = 60f; // 1 game hour = 60 real seconds
    [Range(0f, 1f)] public float normalizedHour; // 0 to 1 representing progress through the day/night cycle
    public int day;
    public bool worldLightsOn;


    [Header("Light Settings")]
    public float minIntensity = 0.1f;
    public float maxIntensity = 1f;
    public float[] minIntensities;

    private float totalDayLength;

    private const float SUNRISE_HOUR = 6f;
    private const float SUNSET_HOUR = 22f; // 10 PM

    private DayNightState currentDayNightState;

    [Header("UI")]
    public CharacterUI characterUI;

    private Light worldLight;

    public FauxLoading fauxLoadingScreen;   

    public static event Action OnDayChanged;

    private void Start()
    {
        InitializeComponents();
        totalDayLength = daytimeHours + nightTimeHours;
        UpdateDayNightState();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeComponents();
    }

    void InitializeComponents()
    {
        player1 = GameObject.Find("Player1").GetComponent<Character>();
        tilePalette = GameObject.Find("TilePalette").GetComponent<TilePalette>();
        characterUI = GameObject.Find("PlayerUI").GetComponent<CharacterUI>();
        worldLight = GetComponent<Light>();

        if (developersMode)
            worldLight.intensity = maxIntensity / 2;

        try
        {
            GameObject.Find("PlayerName").GetComponent<Text>().text = player1.playerName;
        }
        catch
        {
            Debug.LogWarning("PlayerName UI element not found");
        }
    }

    void Update()
    {
        if (!timeIsPaused)
        {
            normalizedHour += Time.deltaTime / (totalDayLength * gameHoursToRealSecs);
            if (normalizedHour >= 1f)
            {
                ChangeDay();

                StartCoroutine(AdvancePlants()); 
                // musicChanger.OnDayChange();

            }
        }

        UpdateSunDialAngle();
        UpdateLightIntensity();
        UpdateDayNightState();
    }

    private void ChangeDay()
    {
        normalizedHour  = 0f;
 
        day++;
        OnDayChanged?.Invoke();
    }

    void UpdateDayNightState()
    {

        float dayProgress = normalizedHour * totalDayLength;
        float dayPercentage = dayProgress / daytimeHours;

        DayNightState newDayNightState;

        if (dayPercentage > 0 && dayPercentage < 0.7f)
        {
            newDayNightState = DayNightState.Day;
        }
        else if (dayPercentage >= 0.7f && dayPercentage < 0.95f)
        {
            newDayNightState = DayNightState.SunSetting;
        }
        else if (dayPercentage >= 0.95f || dayPercentage <= 0.0f)
        {
            newDayNightState = DayNightState.Night;
        }
        else
        {
            newDayNightState = DayNightState.SunRising; // Assumed condition, adjust as needed
        }

        if (newDayNightState != currentDayNightState)
        {
            currentDayNightState = newDayNightState;
            HandleDayNightStateChange(newDayNightState);
        }
    }

    void HandleDayNightStateChange(DayNightState state)
    {
        switch (state)
        {
            case DayNightState.SunRising:
                Debug.Log("Switching to Sun Rising");
                // Add logic for sun rising if needed
                break;

            case DayNightState.Day:
                characterUI.TriggerDay(gameHoursToRealSecs, false);
                Debug.Log("Switching to Day");
                musicChanger.OnDayChange(true);
                break;

            case DayNightState.SunSetting:
                Debug.Log("Switching to Sun Setting");
                characterUI.TriggerNight(gameHoursToRealSecs, false);
                // TurnOnWorldLights();
                break;

            case DayNightState.Night:
                Debug.Log("Switching to Night");
                musicChanger.OnDayChange(false);
                break;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
      
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
       
    }


    void UpdateSunDialAngle()
    {
        float dayProgress = normalizedHour * totalDayLength;
        float sunDialAngle;

        if (dayProgress < daytimeHours)
        {
            // During daytime
            sunDialAngle = (dayProgress / daytimeHours) * 180f;
        }
        else
        {
            // During nighttime
            float nightProgress = (dayProgress - daytimeHours) / nightTimeHours;
            sunDialAngle = 180f + (nightProgress * 180f);
        }

        characterUI.sunDialSun.transform.rotation = Quaternion.Euler(0, 0, -sunDialAngle);
        characterUI.sunDialMoon.transform.rotation = Quaternion.Euler(0, 0, -sunDialAngle - 180);
    }

    void UpdateLightIntensity()
    {
        if (developersMode) return;

        float dayProgress = normalizedHour * totalDayLength;
        if (dayProgress < daytimeHours)
        {
            // During daytime
            float dayPercentage = dayProgress / daytimeHours;
            float midday = 0.5f;

            if (dayPercentage < midday)
            {
                // Morning: Intensity increases from min to max
                worldLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, dayPercentage * 2);
            }
            else
            {
                // Afternoon: Intensity decreases from max to min
                worldLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, (dayPercentage - midday) * 2);
            }
        }
        else
        {
            // During nighttime
            worldLight.intensity = minIntensity;
        }
    }
    
    public string GetSceneName()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    // public bool IsDaytime(){
    //     return isSunOut;
    // }

    public bool TryTransitionToNight()
    {
        if (IsNight())
        {
            Debug.Log("Already night time");
            return false;
        }
        StartCoroutine(TransitionTimeForward(daytimeHours * 0.95f));
        return true;
    }

    public bool TryTransitionToDay()
    {
        if (IsDay())
        {
            Debug.Log("Already day time");
            return false;
        }
        StartCoroutine(TransitionTimeForward(daytimeHours * 0.05f / totalDayLength));
        return true;
    }
    public bool IsDay()
    {
        float currentTime = normalizedHour * totalDayLength;
        return currentTime >= daytimeHours * 0.05f && currentTime < daytimeHours * 0.95f;
    }

    public bool IsNight()
    {
        float currentTime = normalizedHour * totalDayLength;
        return currentTime < daytimeHours * 0.05f || currentTime >= daytimeHours * 0.95f;
    }
   public void PlayerSleeps()
    {
        bool success = TryTransitionToDay();

        if (!success){
            ToastNotification.Instance.Toast("no-sleep-day", "You aren't tired yet.");
        }

        // Heal character (uncomment when ready to implement)
        // player1.health = player1.healthMax;
    }
    

    private IEnumerator TransitionTimeForward(float targetHour)
    {
        timeIsPaused = true;
        float startTime = normalizedHour;
        float endTime = targetHour / totalDayLength;

        bool dayHasChanged = false;

        if (endTime <= startTime)
        {
            endTime += 1f; // Add a full day if we're transitioning to the next day
        }

        float transitionDuration = 3f; // Transition over 3 seconds
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            // Calculating intermediate normalized hour value
            float currentNormalizedHour = Mathf.Lerp(startTime, endTime, t);

            // Detecting day change
            if (!dayHasChanged && currentNormalizedHour >= 1f)
            {
                ChangeDay();
                dayHasChanged = true;
            }

            // If a day has changed, adjust currentNormalizedHour accordingly
            normalizedHour = dayHasChanged ? currentNormalizedHour - 1f : currentNormalizedHour;

            UpdateSunDialAngle();
            UpdateLightIntensity();
            UpdateDayNightState();

            yield return null;
        }

        // Ensure we end exactly at the target hour
        normalizedHour = endTime % 1f;

        if (dayHasChanged) 
        {
            StartCoroutine(AdvancePlants());
        }

        timeIsPaused = false;
    }

    public void GrowAllPlants(){
        PlantTileManager.Instance.AdvanceAllPlantsToNextStage();
    }

    public IEnumerator AdvancePlants()
    {
        // Implement plant advancement logic here
        PlantTileManager.Instance.GrowAllPlants();

        yield return null;
    }

    public void ToggleBuildMenuTab(int i){
        foreach(var tab in  characterUI.buildMenuTabs){
            tab.SetActive(false);
        }
        characterUI.buildMenuTabs[i].SetActive(true);

        player1.activeBlueprint = null;

        foreach( var buildSlot in characterUI.buildMenuTabs[i].GetComponent<BuildMenu>().bluePrintSlots){
            buildSlot.transform.parent.GetComponent<Image>().color = new Color(.2627f,.2627f,.2627f); 
        }
    }

    
    public void ToggleInventory(){
        if (characterUI.customSignBox.activeSelf)
            return;

        if (characterUI.buildGui.activeSelf){
            characterUI.buildGui.SetActive(false);
        }
        if (characterUI.mainMenuGui.activeSelf){
            characterUI.mainMenuGui.SetActive(false);
        }
        // if (character.currentState== PlayerState.standby){
        //     // Fix me
        //     // character.StartCoroutine(character.interact("interact"));
        // }
        characterUI.inventoryGui.SetActive(!characterUI.inventoryGui.activeSelf);
    }
    

    public void pauseTimeSpell(float time){
        if (timeIsPaused){
            StartCoroutine(transitionTime(timeBeforeFreeze));
            timeBeforeFreeze = -1;
            timeIsPaused = false;
        }
        else{
            timeBeforeFreeze = normalizedHour;
            timeIsPaused = true;
            StartCoroutine(transitionTime(time));
        }
        
    }
    
    public void ToggleBuildMenu(bool isBuilding){
        if (!isBuilding){
            characterUI.buildGui.SetActive(false);
            characterUI.buildMenuTabs[0].SetActive(true);
            return;
        }
        
        if (characterUI.inventoryGui.activeSelf){
            ToggleInventory();
        }
        if (characterUI.mainMenuGui.activeSelf){
            characterUI.mainMenuGui.SetActive(false);
        }
        characterUI.buildGui.SetActive(true);
        characterUI.buildMenuTabs[0].SetActive(true);
    }

    public bool isDark(){
        if (worldLight == null)
            worldLight = GetComponent<Light>();
        return (worldLight.intensity < .5f);
    }

    void ToggleWorldLights(){
        foreach(var worldLight in GameObject.FindGameObjectsWithTag("WordLight")){
            worldLight.SetActive(worldLightsOn);
        }
        
        worldLightsOn = !worldLightsOn;

    }

    public void UnpauseTime(){
        timeIsPaused = false;
    }
    private IEnumerator transitionTime(float time){
        int i = 0;
        // bool to see if we are incrementing the time up or down
        bool sunGoesUp = time > normalizedHour;

        float increment = Mathf.Abs(time-normalizedHour)/360;

        while (i < 10000 && normalizedHour != time){
            if (sunGoesUp)
                normalizedHour += increment;
            else
                normalizedHour -= increment;

            
            

            if (time > normalizedHour != sunGoesUp){
                normalizedHour = time;
                
            }

            if (i%6 == 0)
                yield return null;
        } 
        i++;
        
    }

    public void StartFakeLoading(){
        fauxLoadingScreen.StartFade();
    }


    // private IEnumerator advancePlants(){
    //     // Debug.Log("Advancing plants");
    //     System.Random _random = new System.Random (); // change this to a seeded value

    //     // Shuffle the list of plants so they pop advance in a fun order
    //     PlantTileData myGO;
 
    //     int n = plantTiles.Count;
    //     for (int i = 0; i < n; i++)
    //     {
    //          // NextDouble returns a random number between 0 and 1.
    //          // ... It is equivalent to Math.random() in Java.
    //         int r = i + (int)(_random.NextDouble() * (n - i));
    //         myGO = plantTiles[r];
    //         plantTiles[r] = plantTiles[i];
    //         plantTiles[i] = myGO;
    //     }

    //     List<int> indeciesToRemove = new List<int>();

    //     // Now advance the plants
    //     for (int i = 0; i < n; i++)
    //     {
    //         plantTiles[i].AdvanceAge();
    //         int[] growthProgression = ItemDatabase.plantDatabase[plantTiles[i].name].growthProgression;
            
    //         for (int j = 0; j< growthProgression.Length;j++){
    //             // Debug.Log("Plant age ="+plantTiles[i].age.ToString() +" and sapling stage is " + growthProgression[0]);
    //             if (plantTiles[i].age == growthProgression[j]){
    //                 // Debug.Log("We have a plant growing up");
    //                 if (j == 0){

    //                     // Check for if we are close to water
    //                     // bool waterIsNear = itemDatabase.plantDatabase[plantTiles[i].name].howFarFromWaterToGrow != 0;

    //                     bool waterIsNear = true;

    //                     if (!waterIsNear){
    //                         int distanceNeededToWater = ItemDatabase.plantDatabase[plantTiles[i].name].howFarFromWaterToGrow;
    //                         for (int k = 0; k < distanceNeededToWater;k++){
    //                             for (int l = 0; l < distanceNeededToWater;l++){
    //                                 if (tilePalette.ground.GetTile(plantTiles[i].location + new Vector3Int(-2+k,-2+l,0))==tilePalette.water){
    //                                     waterIsNear = true;
    //                                     Debug.Log("We've water");
    //                                     break;
    //                                 }
    //                             }
    //                             if (waterIsNear)
    //                                 break;
    //                         }
    //                     }

    //                     if (waterIsNear){
    //                         Debug.Log("Turning "+ plantTiles[i].name + " to sapling");
    //                         tilePalette.ground.SetTile(plantTiles[i].location,tilePalette.sapling);
    //                     }
    //                     else{
    //                         indeciesToRemove.Add(i);
    //                         tilePalette.ground.SetTile(plantTiles[i].location,tilePalette.dirt);
    //                     }
    //                     //Sapling
    //                 }
    //                 else if (j == 1){
    //                     //Ripe

    //                     // bool waterIsNear = itemDatabase.plantDatabase[plantTiles[i].name].howFarFromWaterToGrow != 0;
    //                     bool waterIsNear = true;
    //                     if (!waterIsNear){
    //                         int distanceNeededToWater = ItemDatabase.plantDatabase[plantTiles[i].name].howFarFromWaterToGrow;

    //                         for (int k = 0; k < distanceNeededToWater;k++){
    //                             for (int l = 0; l < distanceNeededToWater;l++){
    //                                 if (tilePalette.ground.GetTile(plantTiles[i].location + new Vector3Int(-2+k,-2+l,0))==tilePalette.water){
    //                                     waterIsNear = true;
    //                                     break;
    //                                 }
    //                             }
    //                             if (waterIsNear)
    //                                 break;
    //                         }
    //                     }

    //                     if (waterIsNear){

    //                         // Debug.Log(plantTiles[i].name + " is now ripe");
    //                         switch (plantTiles[i].name){
    //                             case "carrot":
    //                                 tilePalette.ground.SetTile(plantTiles[i].location,tilePalette.carrot);
    //                                 break;
    //                             case "tomato":
    //                                 tilePalette.ground.SetTile(plantTiles[i].location,tilePalette.tomato);
    //                                 break;
    //                             case "appleTreeSapling":
    //                                 tilePalette.ground.SetTile(plantTiles[i].location,tilePalette.grass);
    //                                 tilePalette.choppable.SetTile(plantTiles[i].location,tilePalette.appleTreeEmpty);
    //                                 // Add apple tree to list
    //                                 break;
    //                             case "bushSapling":
    //                                 tilePalette.choppable.SetTile(plantTiles[i].location,tilePalette.bush);
    //                                 break;
    //                             case "treeSapling":
    //                                 tilePalette.choppable.SetTile(plantTiles[i].location,tilePalette.tree);
    //                                 break;
    //                         }
    //                         if (ItemDatabase.plantDatabase[plantTiles[i].name].isPermanent){
    //                             indeciesToRemove.Add(i);
    //                             tilePalette.ground.SetTile(plantTiles[i].location,tilePalette.grass);
    //                         }
    //                     }
    //                     else{
    //                         indeciesToRemove.Add(i);
    //                         tilePalette.ground.SetTile(plantTiles[i].location,tilePalette.dirt);
    //                     }
    //                 } 
    //                 else if (j == 2){

    //                     tilePalette.ground.SetTile(plantTiles[i].location,tilePalette.dirt);
    //                     indeciesToRemove.Add(i);
    //                     //Dead
                        
    //                     //remove me from list
    //                 }
    //                 else if (j == 3){
    //                     // Check if periannual or whatnot
    //                 }
    //             }
    //         }
    //         yield return new WaitForSeconds(.3f); // Make this random we want to spread these out throughout the day
    //         //Make noise at build location
    //     }

    //     for (int i = indeciesToRemove.Count; i --> 0; ){
    //         // plantTiles.RemoveAt(indeciesToRemove[i]);
    //     }


    //     yield return null;
    // }


    
//     public IEnumerator addPlantTile(Vector3Int location, string plantName){
//         // if we don't have one already
//         bool duplicate = false;

//         for (int i = 0; i<plantTiles.Count;i++){
//             if (plantTiles[i].location == location)
//                 duplicate = true;
//                 yield return null;
//         }

//         if (!duplicate)
//             plantTiles.Add(new PlantTileData(location,plantName));
//     }

//     private IEnumerator resetPlant(Vector3Int location){
//         for (int i = 0; i < plantTiles.Count; i++){
//             if (plantTiles[i].location == location){
//                 plantTiles[i].resetAge();
//                 break;
//             }
//         }
//         yield return null;
//     }

    
//     void growAllPlants(){
//         Tilemap dirt = GameObject.Find("Dirt").GetComponent<Tilemap>();
//         Tilemap ground = GameObject.Find("Ground").GetComponent<Tilemap>();
//         Tilemap choppable = GameObject.Find("Choppable").GetComponent<Tilemap>();

//         foreach (var pos in ground.cellBounds.allPositionsWithin)
//         {   
//         Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
        
//             if (ground.GetTile(localPlace)==sapling){
//                 Debug.Log("We found a tree");
//                 ground.SetTile(localPlace,grass);
//                 choppable.SetTile(localPlace,tree);
//             }
//             if (dirt.GetTile(localPlace)==sapling){
//                 Debug.Log("We found a tree");
//                 ground.SetTile(localPlace,grass);
//                 choppable.SetTile(localPlace,tree);
//             }
//         }
//     }

}



public enum DayNightState
{
    SunRising,
    Day,
    SunSetting,
    Night
}