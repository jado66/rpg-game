﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : LivingEntity
{
    public bool canSwim;

    public Chest chest;

    public bool alwaysMoving;

    public bool canBeCarriedByPlayer;
    private TilePallete tilePallete;
    Vector3 wayPoint;
    private float timer;

    GridLayout grid;
 
    // private static cat _instance;

    // public static cat Instance { get { return _instance; } }
    GameObject playerObject;

    Player player;

    public bool afraidOfPlayer;
    public float sightRange = 5;

    float tempSightRange;

    public float runSpeed;

    public float walkSpeed;

    protected bool running;

    bool moving; 

    bool resting;

    bool pickedUp;

    public bool followingPlayer;

    public int id;

    public Vector2 startingPosition = new Vector3(0,-1);

    Vector3 direction;
    Animator animator;

    // Start is called before the first frame update
    void Awake(){
        animator = gameObject.GetComponent<Animator>();
        
    }
    void Start()
    {
        chest = gameObject.AddComponent<Chest>();
        chest.inventory = gameObject.GetComponent<Inventory>();
        chest.isLocked = true;

        if (followingPlayer) // or player is carrying
            DontDestroyOnLoad(this);
        
        // If following through portal delete if there is a clone (useful for going through and back from a portal)
        // This will be done away with when we have a saved state function for portal hopping.
        Animal[] animals = (Animal[]) FindObjectsOfType(typeof(Animal));
        foreach(var animal in animals){
            if (animal.id == this.id && animal !=this) {
                if (!this.followingPlayer){
                    Destroy(this.gameObject);
                }
            }
        }
        

        grid = GameObject.Find("Grid").GetComponent<GridLayout>(); 
        
        tilePallete = GameObject.Find("TilePallete").GetComponent<TilePallete>();
        tempSightRange = sightRange;
        findNewWayPoint();
        playerObject = GameObject.Find("Character");
        player = playerObject.GetComponent<Player>();

        animator = gameObject.GetComponent<Animator>();
        animator.SetFloat("moveX",startingPosition.x);
        animator.SetFloat("moveY",startingPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive)
            return;
        if (health <= 0)
            kill();

        if (playerObject == null)
            playerObject = GameObject.FindWithTag("Player");
        if (tilePallete == null)
            tilePallete = GameObject.Find("TilePallete").GetComponent<TilePallete>();
        Vector3 position = transform.position;
                
        if (Random.Range(0,1000) <=6){
            if (!alwaysMoving)
                resting = !resting;
            // Debug.Log(resting);
            findNewWayPoint();
        }
            
        float distance = Vector3.Distance(position,playerObject.transform.position);

        if (pickedUp)
            transform.position = playerObject.transform.position;
        else if (afraidOfPlayer && Vector3.Distance(position,playerObject.transform.position)<tempSightRange){
            if (tempSightRange == sightRange){
                findNewWayPoint();
            }
            tempSightRange = sightRange*1.5f;
            direction = (position-playerObject.transform.position);
            animator.SetFloat("moveX",direction.x);
            animator.SetFloat("moveY",direction.y);
            animator.SetBool("moving",true);
            position+= direction.normalized * runSpeed * Time.deltaTime;
            running = true;
            
            transform.position = position;
            resting = false;
        }
        else if (!resting){
            tempSightRange = sightRange;
            direction = (wayPoint-transform.position);
            animator.SetFloat("moveX",direction.x);
            animator.SetFloat("moveY",direction.y);
            animator.SetBool("moving",true);
            position+= direction.normalized * walkSpeed * Time.deltaTime;
            
            transform.position = position;
            resting = false;
            running = false;
        }
        
        if (resting){
            // Debug.Log("Idle");
            animator.SetBool("moving",false);
            running = false;
        }

        if((transform.position - wayPoint).magnitude < 3)
        {
         // when the distance between us and the target is less than 3
         // create a new way point target
            findNewWayPoint();
        }
    }

    protected override void kill(){
        animator.SetBool("moving",false);
        animator.SetTrigger("die");
        chest.isLocked = false;
        base.kill();
    }
    public override void onPlayerInteract(){
        // base.onPlayerInteract();

        if (!alive){
            Debug.Log("Interact with animal remains");
            // chest.onPlayerInteract();
            return;
        }

        Debug.Log("Interact with animal");
        if (canBeCarriedByPlayer && !pickedUp){
            player.SetObjectBeingCarried(this);
            pickedUp = true;
            GetComponent<BoxCollider2D>().enabled = false;
            animator.SetFloat("moveX",0);
            animator.SetFloat("moveY",0);
            animator.SetBool("moving",false);
        }
        else{
            player.SetObjectBeingCarried(null);
            pickedUp = false;
            GetComponent<BoxCollider2D>().enabled = true;
            animator.SetBool("moving",true);
        }
    }

    private void findNewWayPoint(){ 
    // does nothing except pick a new destination to go to

        try{
        Vector3 newWayPoint=  new Vector3(transform.position.x +Random.Range(-sightRange, sightRange), transform.position.y+ Random.Range(-sightRange,sightRange),0);
        if (tilePallete.ground.GetTile(grid.WorldToCell(newWayPoint))==tilePallete.water ||tilePallete.collidable.GetTile(grid.WorldToCell(newWayPoint))!=null){
            newWayPoint=  new Vector3(transform.position.x +Random.Range(-sightRange, sightRange), transform.position.y+ Random.Range(-sightRange,sightRange),0);
        }
        if (tilePallete.ground.GetTile(grid.WorldToCell(newWayPoint))==tilePallete.water || tilePallete.collidable.GetTile(grid.WorldToCell(newWayPoint))!=null){
            return;
        }
        else
            wayPoint = newWayPoint;
        }
        catch{
            return;
        }
    // don't need to change direction every frame seeing as you walk in a straight line only
    //  Debug.Log(wayPoint + " and " + (transform.position - wayPoint).magnitude);
    }
}



   