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

    public string difficulty;

    public NighttimeMonsterManager nighttimeMonsterManager;

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

    public Light worldLight;

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

        if (!worldLight){
            worldLight = GetComponent<Light>();
        }

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
                DisableNighttimeMonsters();
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
                EnableNighttimeMonsters();
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
    
    private void EnableNighttimeMonsters(){

        if (difficulty == "creative"){
            return;
        }
        nighttimeMonsterManager.EnableNighttimeMonsters();
    }

    private void DisableNighttimeMonsters(){
        nighttimeMonsterManager.DisableNighttimeMonsters();
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
            StartCoroutine(ResetAllStoreInventories());
        }

        timeIsPaused = false;
    }

    public void GrowAllPlants(){
        PlantTileManager.Instance.AdvanceAllPlantsToNextStage();
    }

    public IEnumerator ResetAllStoreInventories()
    {
        // Find all game objects with the StoreInventory component
        StoreInventory[] storeInventories = FindObjectsOfType<StoreInventory>();

        // Iterate through each StoreInventory and call the Reset method
        foreach (StoreInventory inventory in storeInventories)
        {
            inventory.ResetInventory();
            yield return null; // Yielding null makes sure we don't block the main thread
        }
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
        characterUI.ToggleInventory();
    }

    public void ToggleInventory(bool forcedState)
    {
        if (characterUI.customSignBox.activeSelf)
            return;

        if (characterUI.buildGui.activeSelf){
            characterUI.buildGui.SetActive(false);
        }
        if (characterUI.mainMenuGui.activeSelf){
            characterUI.mainMenuGui.SetActive(false);
        }

        // If forcedState is true, always open the inventory; if false, close it.
        characterUI.inventoryGui.SetActive(forcedState);
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

    public void ToggleDifficulty(){
        if (difficulty == "creative"){
            difficulty = "easy";
        }
        else {
            difficulty = "creative";
        }
    }
}



public enum DayNightState
{
    SunRising,
    Day,
    SunSetting,
    Night
}