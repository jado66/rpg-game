using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : LivingEntity
{
    public bool canSwim;

    public bool alwaysMoving;

    protected  TilePalette tilePalette;
    protected Vector3 wayPoint;
    private float timer;

    protected GridLayout grid;
 
    // private static cat _instance;

    // public static cat Instance { get { return _instance; } }
    protected GameObject player;

    public float sightRange = 5;

    protected float tempSightRange;

    public float runSpeed;

    public float walkSpeed;

    protected bool moving; 

    public bool resting;

    public bool followingPlayer;

    public string id;

    public Vector2 startingPosition = new Vector3(0,-1);

    protected Vector3 direction;
    protected List<Animator> animators = new List<Animator>();

    public bool hostile;

    


    // Start is called before the first frame update

    public void addAnimator(Animator animator){
        animators.Add(animator);
    }
    void Awake(){
        alive = true;
        try{
        animators.Add(gameObject.GetComponent<Animator>());
        }
        catch{}

    }
    void Start()
    {

        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
        }


        if (followingPlayer) // or player is carrying
            DontDestroyOnLoad(this);

        healthbarObject.SetActive(false);
        // If following through portal delete if there is a clone (useful for going through and back from a portal)
        // This will be done away with when we have a saved state function for portal hopping.
        Monster[] monsters = (Monster[]) FindObjectsOfType(typeof(Monster));
        foreach(var monster in monsters){
            if (monster.id == this.id && monster !=this) {
                if (!this.followingPlayer){
                    Debug.Log("Destroying duplicate monster " + this.gameObject.name + "!");
                    Destroy(this.gameObject);
                }
            }
        }
        

        grid = GameObject.Find("Grid").GetComponent<GridLayout>(); 
        
        tilePalette = GameObject.Find("TilePalette").GetComponent<TilePalette>();
        tempSightRange = sightRange;
        findNewWayPoint();
        player = GameObject.Find("Character");
        foreach(var animator in animators){
            animator.SetFloat("moveX",startingPosition.x);
            animator.SetFloat("moveY",startingPosition.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive)
            return;
        if (health <= 0)
            kill();

        if (player == null)
            player = GameObject.FindWithTag("Player");
        if (tilePalette == null)
            tilePalette = GameObject.Find("TilePalette").GetComponent<TilePalette>();
        Vector3 position = transform.position;
                
        if (Random.Range(0,1000) <=6){
            // if (!alwaysMoving)
            //     resting = !resting;
            // Debug.Log(resting);
            findNewWayPoint();
        }
            
        float distance = Vector3.Distance(position,player.transform.position);

        
        if ( Vector3.Distance(position,player.transform.position)<sightRange && hostile){
            // if (tempSightRange == sightRange){
            //     findNewWayPoint();
            // }
            wayPoint = player.transform.position;
            resting = false;
            // tempSightRange = sightRange*1.5f;
            // direction = (position-player.transform.position);
            // animator.SetFloat("moveX",direction.x);
            // animator.SetFloat("moveY",direction.y);
            // animator.SetBool("moving",true);
            // position+= direction.normalized * runSpeed * Time.deltaTime;
            
            // transform.position = position;
            // resting = false;
        }
        if (!resting){
            tempSightRange = sightRange;
            direction = (wayPoint-transform.position);
            foreach(var animator in animators){
                animator.SetFloat("moveX",direction.x);
                animator.SetFloat("moveY",direction.y);
                animator.SetBool("moving",true);
            }
            position+= direction.normalized * walkSpeed * Time.deltaTime;
            
            transform.position = position;
            resting = false;
        }
        
        if (resting){
            // Debug.Log("Idle");
            foreach(var animator in animators){
                animator.SetBool("moving",false);
            }
        }

        if((transform.position - wayPoint).magnitude < 3)
        {
         // when the distance between us and the target is less than 3
         // create a new way point target
            findNewWayPoint();
        }
    }

    

    
    protected void findNewWayPoint(){ 
    // does nothing except pick a new destination to go to

        // Debug.Log("Finding New waypoint");

        Vector3 newWayPoint=  new Vector3(transform.position.x +Random.Range(-sightRange, sightRange), transform.position.y+ Random.Range(-sightRange,sightRange),0);
        
        RaycastHit2D checkForGround = Physics2D.Raycast(newWayPoint+new Vector3(0,0,.5f),Vector3.down,1);

        if (checkForGround.collider == null)
        {
            Debug.DrawRay(newWayPoint+new Vector3(0,0,.5f),Vector3.down,Color.green,1f);
            Debug.Log("Waypoint at "+newWayPoint.ToString()+" is inaccessible");
            return;
        }
        else
        {   
            wayPoint = newWayPoint;
        }
        
        // try{
        // Vector3 newWayPoint=  new Vector3(transform.position.x +Random.Range(-sightRange, sightRange), transform.position.y+ Random.Range(-sightRange,sightRange),0);
        // if (tilePalette.ground.GetTile(grid.WorldToCell(newWayPoint))==tilePalette.water ||tilePalette.collidable.GetTile(grid.WorldToCell(newWayPoint))!=null){
        //     newWayPoint=  new Vector3(transform.position.x +Random.Range(-sightRange, sightRange), transform.position.y+ Random.Range(-sightRange,sightRange),0);
        // }
        // if (tilePalette.ground.GetTile(grid.WorldToCell(newWayPoint))==tilePalette.water || tilePalette.collidable.GetTile(grid.WorldToCell(newWayPoint))!=null){
        //     return;
        // }
        // else
        //     wayPoint = newWayPoint;
        // }
        // catch{
        //     return;
        // }
    // don't need to change direction every frame seeing as you walk in a straight line only
    //  Debug.Log(wayPoint + " and " + (transform.position - wayPoint).magnitude);
    }    
}



   