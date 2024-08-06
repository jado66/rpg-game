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
    [SerializeField] private CharacterInventoryUI inventoryUI;



    [SerializeField] private CharacterCombat combat;

    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private Rigidbody2D rigidbody;

    // public visible fields
    public bool characterIsInControl = false;
    public string playerName = "Player";

    public BluePrint activeBlueprint;

    public Animator animator;

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
        inventory = GetComponent<CharacterInventory>();
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
        inventory.InitializeComponents(this);
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
    public CharacterCombat GetCombat(){
        return combat;
    }
    public BoxCollider2D GetBoxCollider2D(){
        return boxCollider2D;
    }

    public SceneManager GetSceneManager(){
        return sceneManager;
    }

    public void ToggleBuilding(bool callFromSceneManager){
        building.ToggleBuilding(callFromSceneManager);
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
}