using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMonster : Monster
{
    public bool startSleeping;
    bool sleeping;
    // Start is called before the first frame update
    void Start()
    {
        if (startSleeping)
            sleeping = true;
        else{
            sleeping = false;
            animators[0].SetTrigger("awake");
        }

        if (followingPlayer) // or player is carrying
            DontDestroyOnLoad(this);

        healthbarObject.SetActive(false);
        

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
        if (!alive || sleeping)
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

    public override void TakeDamage(int amount)
    {
        if (health <= 0){
            kill();
        }
        if (sleeping){
            sleeping = false;
            animators[0].SetTrigger("awake");
        }
        else
            base.TakeDamage(amount);

    }

    protected override void kill(){
        Debug.Log("Killing "+ name);
        healthbarObject.SetActive(false);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        animators[0].SetTrigger("die");
        alive = false;
        
        StartCoroutine(WaitAndDestroy());
    }
}
