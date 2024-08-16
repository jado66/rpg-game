using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cat : Interactable
{
    // private static cat _instance;

    // public static cat Instance { get { return _instance; } }
    GameObject player;
    public float followDistance = 5;
    public float losePlayerDistance = 10;
    public float speed;

    static bool moving; 

    bool playerWantsToFollow;
    public int id;
    public bool followingPlayer;

    public Vector2 startingPosition = new Vector3(0,-1);


    Vector3 direction;
    Animator animator;

    // Start is called before the first frame update
    void Awake(){
        animator = gameObject.GetComponent<Animator>();

        // if (animator.GetBool("moving")){
        //     DontDestroyOnLoad(this);
        //     // Debug.Log("Cat was moving");
        //     if (_instance != null && _instance != this)
        //     {
        //         Destroy(this.gameObject);
        //     } else {
        //         _instance = this;
        //     }
        // }
        // else{
        //     // Debug.Log("Cat was not moving");    
        // }


    }
    void Start()
    {
        cat[] cats = (cat[]) FindObjectsOfType(typeof(cat));
        foreach(var cat in cats){
            if (cat.id == this.id && cat !=this){
                if (!this.followingPlayer){
                    Destroy(this.gameObject);
                }
            }
        }

        player = GameObject.Find("Character");
        animator = gameObject.GetComponent<Animator>();
        animator.SetFloat("moveX",startingPosition.x);
        animator.SetFloat("moveY",startingPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");
        Vector3 position = transform.position;

        float distance = Vector3.Distance(position,player.transform.position);
        if (losePlayerDistance>distance&&distance>followDistance && playerWantsToFollow){
            followingPlayer = true;
            direction = (player.transform.position-position);
            animator.SetFloat("moveX",direction.x);
            animator.SetFloat("moveY",direction.y);
            animator.SetBool("moving",true);
            position+= direction.normalized * speed * Time.deltaTime;
            
            transform.position = position;
            moving = true;
        }
        else{
            followingPlayer = false;
            animator.SetBool("moving",false);
            moving = false;
        }
    }

    public override void OnCharacterInteract(){
        base.OnCharacterInteract();
        playerWantsToFollow = !playerWantsToFollow;
        player.GetComponent<Player>().addOrRemoveFollower(this.gameObject,playerWantsToFollow);
    }
}
