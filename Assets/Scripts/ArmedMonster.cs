using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmedMonster  : Monster
{
    
    public int damageDealt;
    public int recoilForce;
    public int timeBetweenSwings;

    int swingTimer;

    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // healthbarObject.SetActive(false);
    }
    
    void Update(){

        healthbar.fillAmount = (float)(health)/(float)(maxHealth);

        if (swingTimer > 0){
            swingTimer++;
            if (swingTimer >= timeBetweenSwings)
                swingTimer = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!alive)
            return;
        // if (health <= 0)
        //     kill();

        if (animators[0].GetBool("attacking")==true)
            animators[0].SetBool("attacking",false);
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
            
            wayPoint = player.transform.position;
            resting = false;
            
            
            
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

    private IEnumerator AttackCo(Collider2D collision){
        Debug.Log("Attacking");
        yield return null; //Wait a frame
        animators[0].SetTrigger("attack");
        yield return new WaitForSeconds(.2f);
        
        yield return null;
    }

    private void findNewWayPoint(){ 
    // does nothing except pick a new destination to go to

        // Debug.Log("Finding New waypoint");

        Vector3 newWayPoint=  new Vector3(transform.position.x +Random.Range(-sightRange, sightRange), transform.position.y+ Random.Range(-sightRange,sightRange),0);
        
        RaycastHit2D checkForGround = Physics2D.Raycast(newWayPoint+new Vector3(0,0,.5f),Vector3.down,1);

        if (checkForGround.collider == null)
        {
            // This isn't working. Green things are everywhere random
            Debug.DrawRay(newWayPoint+new Vector3(0,0,.5f),Vector3.down,Color.green,1f);
            // Debug.Log("Waypoint at "+newWayPoint.ToString()+" is inaccessible");
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

    // public void attack(){
    //     StartCoroutine(AttackCo());
    // }
    // void OnCollisionEnter2D(Collision2D collision){
    //     Debug.Log("Collision");
    //     if (collision.gameObject.tag == "Player"){
    //         StartCoroutine(AttackCo());
    //     }
    // }
    protected override void kill(){
        Debug.Log("Killing "+ name);
        healthbarObject.SetActive(false);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        alive = false;
        animators[0].SetTrigger("die");
        if (spriteRenderer != null)
        {
            StartCoroutine(FadeAndDestroy());
        }
        else
        {
            StartCoroutine(WaitAndDestroy());
        }
    }

    private IEnumerator FadeAndDestroy()
    {
        Debug.Log("Fading");
        float elapsedTime = 0f;
        yield return new WaitForSeconds(deathVanishDelay);

        Color originalColor = spriteRenderer.color;

        while (elapsedTime < 5.0f)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / 5.0f);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(this.gameObject);
    }


    void OnTriggerEnter2D(Collider2D collision){
        if (!alive)
            return;
        if (collision.gameObject.tag == "Player"){
            if (swingTimer <=0){
                StartCoroutine(AttackCo(collision));
                swingTimer++;
                }

                
            // Debug.Log("Collision");
            // collision.gameObject.GetComponent<Player>().TakeDamage(damageDealt);
            // collision.gameObject.GetComponent<Player>().recoil(recoilForce,transform.position);
        }
        
    }

    
}



   