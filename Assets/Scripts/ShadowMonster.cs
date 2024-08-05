using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMonster : MonoBehaviour
{
    public int appearRadius;
    Player player;

    public int id;
    public float speed;

    bool followPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Character").GetComponent<Player>();
        // gameObject.SetActive(false);
    }

    void OnEnable(){
        GetComponent<SpriteRenderer>().enabled = false;
        followPlayer = false;
        player = GameObject.Find("Character").GetComponent<Player>();
        transform.position = new Vector3(Mathf.Cos(Random.Range(0,360)*Mathf.Deg2Rad),Mathf.Sin(Random.Range(0,360)*Mathf.Deg2Rad),0)*appearRadius + player.transform.position;
        
        StartCoroutine(turnSpriteOn());
    }

    public void resetPosition(){
        GetComponent<SpriteRenderer>().enabled = false;
        followPlayer = false;
        player = GameObject.Find("Character").GetComponent<Player>();
        transform.position = new Vector3(Mathf.Cos(Random.Range(0,360)*Mathf.Deg2Rad),Mathf.Sin(Random.Range(0,360)*Mathf.Deg2Rad),0)*appearRadius + player.transform.position;
        StartCoroutine(turnSpriteOn());
    }
    // Update is called once per frame

    private IEnumerator turnSpriteOn(){
        yield return new WaitForSeconds(Random.Range(.2f,7.5f));
        GetComponent<SpriteRenderer>().enabled = true;
        followPlayer = true;
        // Debug.Log("Rendering");
        yield return null;
    }
    void Update()
    {
        if (followPlayer){
            transform.position+= (player.transform.position-transform.position).normalized * speed * Time.deltaTime;
        }  
        if (Vector3.Distance(player.transform.position,transform.position) > 12)
            resetPosition();
    }

    void OnTriggerEnter2D(Collider2D collision){
        if ((collision.gameObject.GetComponent("Light") as Light)!= null)
            resetPosition();

        if ((collision.gameObject.GetComponent("ShadowMonster") as ShadowMonster)!= null){
            // Debug.Log("Collided with "+collision.gameObject.name);
            if (id > collision.GetComponent<ShadowMonster>().id){
                Debug.Log("resetPositioning monster");
                collision.GetComponent<ShadowMonster>().resetPosition();
            }
        }
        if ((collision.gameObject.GetComponent("Player") as Player) != null)
            return;
        // Debug.Log("Collided with "+collision.gameObject.name);
        if ((collision.gameObject.GetComponent("Player") as Player) != null){
            collision.gameObject.GetComponent<Player>().TakeDamage(8);
        } 
    }

    void OnTriggerStay2D(Collider2D collision){
        if ((collision.gameObject.GetComponent("Light") as Light)!= null)
            resetPosition();
        if ((collision.gameObject.GetComponent("ShadowMonster") as ShadowMonster)!= null){
            Debug.Log("Collided with "+collision.gameObject.name);
            if (id > collision.GetComponent<ShadowMonster>().id){
                Debug.Log("resetPositioning monster");
                collision.GetComponent<ShadowMonster>().resetPosition();
            }
        }
        // Debug.Log("Collided with "+collision.gameObject.name);
        if ((collision.gameObject.GetComponent("Player") as Player) != null){
            collision.gameObject.GetComponent<Player>().TakeDamage(.1f);
        } 
    }
}
