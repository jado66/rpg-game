using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : LivingEntity
{
    public bool canSwim;
    public bool alwaysMoving;
    public bool followingPlayer;
    public bool hostile;
    public bool resting;

    public float sightRange = 5f;
    public float runSpeed;
    public float walkSpeed;

    public string id;
    public Vector2 startingPosition = new Vector3(0, -1);

    protected TilePalette tilePalette;
    protected Vector3 wayPoint;
    protected GridLayout grid;
    protected GameObject player;
    protected Vector3 direction;
    protected List<Animator> animators = new List<Animator>();

    private float tempSightRange;

    void Awake()
    {
        alive = true;
        Animator animator = gameObject.GetComponent<Animator>();
        if (animator != null)
        {
            animators.Add(animator);
        }
    }

    void Start()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
        }

        if (followingPlayer)
        {
            DontDestroyOnLoad(this);
        }

        healthbarObject.SetActive(false);
        DestroyDuplicateEnemies();

        grid = GameObject.Find("Grid").GetComponent<GridLayout>();
        tilePalette = GameObject.Find("TilePalette").GetComponent<TilePalette>();
        tempSightRange = sightRange;
        FindNewWayPoint();
        player = GameObject.Find("Character");

        foreach (var animator in animators)
        {
            animator.SetFloat("moveX", startingPosition.x);
            animator.SetFloat("moveY", startingPosition.y);
        }
    }

    void Update()
    {
        if (!alive) return;
        if (health <= 0) kill();

        if (player == null){
            player = GameObject.FindWithTag("Player");
        }

        if (tilePalette == null)
        {
            tilePalette = GameObject.Find("TilePalette").GetComponent<TilePalette>();
        }
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < sightRange && hostile)
        {
            wayPoint = player.transform.position;
            resting = false;
        }

        if (!resting)
        {
            MoveTowardsWaypoint();
        }
        else
        {
            SetAnimatorsMoving(false);
        }

        if ((transform.position - wayPoint).magnitude < 3f)
        {
            FindNewWayPoint();
        }

        if (Random.Range(0, 1000) <= 6)
        {
            FindNewWayPoint();
        }
    }

    public void AddAnimator(Animator animator)
    {
        animators.Add(animator);
    }

    private void DestroyDuplicateEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies)
        {
            if (enemy.id == this.id && enemy != this)
            {
                if (!this.followingPlayer)
                {
                    Debug.Log("Destroying duplicate enemy " + this.gameObject.name + "!");
                    Destroy(this.gameObject);
                }
            }
        }
    }

    private void FindNewWayPoint()
    {
        Vector3 newWayPoint = new Vector3(
            transform.position.x + Random.Range(-sightRange, sightRange),
            transform.position.y + Random.Range(-sightRange, sightRange), 
            0
        );

        RaycastHit2D checkForGround = Physics2D.Raycast(newWayPoint + new Vector3(0, 0, .5f), Vector3.down, 1);

        if (checkForGround.collider == null)
        {
            Debug.Log("Waypoint at " + newWayPoint.ToString() + " is inaccessible");
            return;
        }

        wayPoint = newWayPoint;
    }

    private void MoveTowardsWaypoint()
    {
        tempSightRange = sightRange;
        direction = (wayPoint - transform.position).normalized;

        foreach (var animator in animators)
        {
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            animator.SetBool("moving", true);
        }

        transform.position += direction * walkSpeed * Time.deltaTime;
    }

    private void SetAnimatorsMoving(bool moving)
    {
        foreach (var animator in animators)
        {
            animator.SetBool("moving", moving);
        }
    }
}
