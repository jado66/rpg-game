using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Animal
{
    public GameObject babyChick;

    int collisionCounts;

    public int makeABabyCount;
    // Start is called before the first frame update

    // Update is called once per frame

    void OnCollisionEnter2D(Collision2D collision){
        // Debug.Log("collide");
        if ((collision.gameObject.GetComponent("Chicken") as Chicken)!= null){
            if (!running){
            collisionCounts++;
            // Debug.Log("collision count = "+collisionCounts.ToString());
            }
        }

        if (collisionCounts >= makeABabyCount){
            collisionCounts =0;
            GameObject clone = Instantiate(babyChick,transform.position+new Vector3(Random.Range(-1,1),Random.Range(-1,1),0),Quaternion.identity);
            clone.GetComponent<Animal>().id = System.Guid.NewGuid().ToString();
        }

    }
}
