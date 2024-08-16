﻿using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    private static Character _instance;
    public static Character Instance { get { return _instance; } }

    public SceneManager sceneManager;

    [SerializeField] private CharacterStats stats;
    [SerializeField] private CharacterMovement movement;
    [SerializeField] private CharacterActions actions;
    [SerializeField] private CharacterBuilding building;

    [SerializeField] private CharacterWorldInteraction worldInteraction;
    [SerializeField] private CharacterInventory inventory; 

    [SerializeField] private CharacterHotbar hotbar; 

    [SerializeField] private CharacterInventoryUI inventoryUI;


    [SerializeField] private CharacterCombat combat;

    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private Rigidbody2D rigidbody;

    // public visible fields
    public bool characterIsInControl = false;
    public string playerName = "Player";

    public bool isIndoors = false;

    public BluePrint activeBlueprint;

    public GameItemUI selectedItem;

    public Animator animator;

    public GameObject torch;

    public bool hasPlayerStartedTheGame;



    public List<GameObject> followingObjects = new List<GameObject>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (PlayerPrefs.GetString("PlayerName")!=""){
            playerName = PlayerPrefs.GetString("PlayerName");
            PlayerPrefs.SetString("PlayerName","");
            PlayerPrefs.Save();
        }
    }


    private void Start()
    {
        InitializeComponents();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        stats = GetComponent<CharacterStats>();
        building = GetComponent<CharacterBuilding>();
        movement = GetComponent<CharacterMovement>();
        actions = GetComponent<CharacterActions>();
        worldInteraction = GetComponent<CharacterWorldInteraction>();

        CharacterInventory[] inventories = GetComponents<CharacterInventory>();

        // Grab inventories
        foreach (CharacterInventory inv in inventories)
        {
            if (inv.inventoryIdentifier == "Primary")
            {
                inventory = inv;
            }
        }
        hotbar = GetComponent<CharacterHotbar>(); 

        inventoryUI = GetComponent<CharacterInventoryUI>(); 
        combat = GetComponent<CharacterCombat>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        animator = transform.Find("Animator").GetComponent<Animator>();

        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        stats.InitializeComponents(this);
        actions.InitializeComponents(this);
        building.InitializeComponents(this);
        movement.InitializeComponents(this);
        worldInteraction.InitializeComponents(this);
        // inventory.InitializeComponents(this);
        combat.InitializeComponents(this);

        if (characterIsInControl){
            inventoryUI.InitializeComponents(this);
        }
    }

    public Animator GetAnimator(){
        return animator;
    }
    public CharacterStats GetStats(){
        return stats;
    }
    public CharacterBuilding GetBuilding(){
        return building;
    }
    public CharacterMovement GetMovement(){
        return movement;
    }
    public CharacterWorldInteraction GetWorldInteraction(){
        return worldInteraction;
    }
    public CharacterInventory GetInventory(){
        return inventory;
    }

    public CharacterHotbar GetHotbar(){
        return hotbar;
    }
    public CharacterCombat GetCombat(){
        return combat;
    }
    public BoxCollider2D GetBoxCollider2D(){
        return boxCollider2D;
    }

    public SceneManager GetSceneManager(){
        return sceneManager;
    }

    public void ToggleBuilding(){
        building.ToggleBuilding();
    }

    public void TakeDamage(float damage)
    {
        stats.TakeDamage(damage);
    }

    public void addOrRemoveFollower(GameObject follower,bool add){
        if (add){
            followingObjects.Add(follower);
        }
        else{
            followingObjects.Remove(follower);
        }
    }

    public void Die()
    {
        Debug.Log("Character died");
    }

    public void Attack()
    {
        StartCoroutine(combat.Attack());
    }

    public void Chop()
    {
        if (stats.Stamina < 2f){
            ToastStaminaMessage();
            return;
        }

        StartCoroutine(worldInteraction.Chop());
        stats.DepleteStamina(2f);
    }

    public void Mine()
    {
        if (stats.Stamina < 2f){
            ToastStaminaMessage();
            return;
        }

        StartCoroutine(worldInteraction.Mine());
        stats.DepleteStamina(2f);
    }

    public IEnumerator Plant(string plantName, System.Action<bool> callback)
    {
        Debug.Log($"Character is attempting to plant {plantName}");
        
        if (stats.Stamina < 0.25f)
        {
            Debug.Log("Not enough stamina to plant");
            ToastNotification.Instance.Toast("low-stamina", "Not enough stamina to plant.");
            callback(false);
            yield break;
        }

        bool plantingSuccess = false;
        yield return StartCoroutine(worldInteraction.Plant(plantName, success => plantingSuccess = success));
        
        if (plantingSuccess)
        {
            stats.DepleteStamina(0.25f);
            Debug.Log($"Successfully planted {plantName}. Stamina depleted.");
        }

        callback(plantingSuccess);
    }

    public void IrrigateGround()
    {
        if (stats.Stamina < 2f){
            ToastStaminaMessage();
            return;
        }

        StartCoroutine(worldInteraction.IrrigateGround());
        stats.DepleteStamina(2f);

    }

    public void TillGround()
    {
        if (stats.Stamina < 2f){
            ToastStaminaMessage();
            return;
        }

        StartCoroutine(worldInteraction.TillGround());
        stats.DepleteStamina(2f);

    }

    public bool ToggleTorch(bool isOn)
    {
        if (torch.activeSelf == isOn)
        {
            return false;
        }

        torch.SetActive(isOn);
        return true;
    }

    public void GiveItem(string itemInfo) //item:amount its annoying
    {
        Debug.Log("Made it here");
        inventory.GiveItem(itemInfo);
    }

    public void ToastStaminaMessage(){
        if (!characterIsInControl)
            return;
        ToastNotification.Instance.Toast("no-stamina", "You are out of stamina!");
    }
}
