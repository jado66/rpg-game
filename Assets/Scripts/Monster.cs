using UnityEngine;
using System;

using System.Collections.Generic;

public class Monster : LivingEntity
{
    public bool canSwim;
    public bool alwaysMoving;
    public float sightRange = 5f;
    public float runSpeed;
    public float walkSpeed;
    public bool resting;
    public bool followingPlayer;
    public string id;
    public Vector2 startingPosition = new Vector2(0, -1);
    public bool hostile;
    public float maxWanderDistance = 10f;

    public float collisionDamage;
    public float collisionDamageCooldown = 1f; // Cooldown time in seconds
    private float lastCollisionDamageTime;
    protected TilePalette tilePalette;
    protected Vector3 wayPoint;
    protected GridLayout grid;
    protected GameObject player;
    protected Vector3 direction;
    protected List<Animator> animators = new List<Animator>(); 

    [SerializeField]
    private bool canPassThroughObstacles = false; // Set this in the inspector for enemies that can pass through

    private Vector3 startingLocation;

    public Chest chest;

    protected virtual void Awake()
    {
        alive = true;

        startingLocation = transform.position;

        if (string.IsNullOrEmpty(id))
        {
            id = Guid.NewGuid().ToString();
        }

        try
        {
            animators.Add(gameObject.GetComponent<Animator>());
        }
        catch { }
    }

    protected virtual void Start()
    {
        InitializeMonster();
        SetupAnimators();
    }

    protected virtual void Update()
    {
        if (!alive || health <= 0)
        {
            if (health <= 0) kill();
            return;
        }

        UpdateReferences();
        HandleMovement();
    }

    protected virtual void InitializeMonster()
    {
        id = string.IsNullOrEmpty(id) ? System.Guid.NewGuid().ToString() : id;
        if (followingPlayer) DontDestroyOnLoad(this);
        healthbarObject.SetActive(false);
        HandleDuplicateMonsters();

        chest = gameObject.AddComponent<Chest>();
        chest.inventory = gameObject.GetComponent<ExternalInventory>();
        chest.isLocked = true;
        
        grid = GameObject.Find("Grid").GetComponent<GridLayout>();
        tilePalette = GameObject.Find("TilePalette").GetComponent<TilePalette>();
        player = GameObject.FindWithTag("Player");
        
        FindNewWayPoint();
    }

    protected virtual void HandleDuplicateMonsters()
    {
        Monster[] monsters = FindObjectsOfType<Monster>();
        foreach (var monster in monsters)
        {
            if (monster.id == this.id && monster != this && !this.followingPlayer)
            {
                Debug.Log($"Destroying duplicate monster {gameObject.name}!");
                Destroy(gameObject);
            }
        }
    }

    protected virtual void SetupAnimators()
    {
        foreach (var animator in animators)
        {
            animator.SetFloat("moveX", startingPosition.x);
            animator.SetFloat("moveY", startingPosition.y);
        }
    }

    protected virtual void UpdateReferences()
    {
        if (player == null) player = GameObject.FindWithTag("Player");
        if (tilePalette == null) tilePalette = GameObject.Find("TilePalette").GetComponent<TilePalette>();
    }

    protected virtual void HandleMovement()
    {
        if (ShouldFindNewWaypoint()) FindNewWayPoint();

        Vector3 position = transform.position;
        float distanceToPlayer = Vector3.Distance(position, player.transform.position);

        if (distanceToPlayer < sightRange && hostile)
        {
            ChasePlayer(position);
        }
        else if (!resting)
        {
            MoveTowardsWaypoint(position);
        }
        else
        {
            SetIdleAnimation();
        }
    }

    void OnCollisionEnter2D(Collision2D  collision)
    {
        
        if (!alive)
            return;

        Debug.Log("Collided with object tag: " + collision.gameObject.tag);


        if (collision.gameObject.CompareTag("Character"))
        {
            ApplyCollisionDamage(collision.gameObject);        
        }
    }

    void OnCollisionStay2D(Collision2D  collision)
    {
        
        if (!alive)
            return;

        Debug.Log("Collided with object tag: " + collision.gameObject.tag);


        if (collision.gameObject.CompareTag("Character"))
        {
            ApplyCollisionDamage(collision.gameObject);        
        }
    }

    protected virtual void ApplyCollisionDamage(GameObject player)
    {
        if (collisionDamage > 0 && Time.time - lastCollisionDamageTime >= collisionDamageCooldown)
        {
            Character character = player.GetComponent<Character>();
            if (character != null)
            {
                character.TakeDamage(collisionDamage);
                lastCollisionDamageTime = Time.time;
            }
        }
    }

    protected virtual bool ShouldFindNewWaypoint()
    {
        return UnityEngine.Random.Range(0, 1000) <= 6 || Vector3.Distance(transform.position, wayPoint) < 0.1f;
    }

    protected virtual void ChasePlayer(Vector3 position)
    {
        wayPoint = player.transform.position;
        resting = false;
        MoveTowardsWaypoint(position, runSpeed);
    }

    protected virtual void MoveTowardsWaypoint(Vector3 position, float speed = -1)
    {
        if (speed < 0) speed = walkSpeed;
        
        direction = (wayPoint - position).normalized;
        SetMovementAnimation(direction);
        
        position += direction * speed * Time.deltaTime;
        transform.position = position;
    }

    protected virtual void SetMovementAnimation(Vector3 direction)
    {
        foreach (var animator in animators)
        {
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            animator.SetBool("moving", true);
        }
    }

    private void SetIdleAnimation()
    {
        foreach (var animator in animators)
        {
            animator.SetBool("moving", false);
        }
    }

    protected virtual void FindNewWayPoint()
    {
        if (!alive){
            return;
        }
        Vector3 spawnPoint = new Vector3(startingLocation.x, startingLocation.y, 0);
        Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * maxWanderDistance;
        randomOffset.z = 0;
        Vector3 newWayPoint = spawnPoint + randomOffset;

        if (canPassThroughObstacles)
        {
            // If the enemy can pass through obstacles, set the waypoint directly
            wayPoint = newWayPoint;
            // Debug.Log($"New waypoint set at {wayPoint} (Can pass through obstacles)");
        }
        else{

             // Create a LayerMask for layers 9, 10, and 11
            int layerMask = (1 << 9) | (1 << 10) | (1 << 11);

            // Check if the newWayPoint is hitting any colliders on the specified layers
            Collider2D hitCollider = Physics2D.OverlapCircle(newWayPoint, 0.1f, layerMask);


            if (hitCollider == null)
            {
                // No colliders hit, so this is a valid waypoint
                wayPoint = newWayPoint;
            }
            else
            {
                // Debug.Log($"Waypoint at {newWayPoint} is inaccessible (hitting {hitCollider.gameObject.name} on layer {hitCollider.gameObject.layer})");
            }
        }
    }

    protected override void kill(){

        if (!alive){
            return;
        }

        chest.isLocked = false;

        foreach (var animator in animators)
        {
            Debug.Log("Monster killed");
            animator.SetTrigger("die");
        }
       

        base.kill();
    }


    public void addAnimator(Animator animator)
    {
        animators.Add(animator);
    }
}