using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Character").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.animator.GetBool("swimming") == true){
            Vector3 position= this.transform.position;
            Vector3 direction = player.transform.position-transform.position;
            position += direction.normalized * Time.deltaTime *3;
            this.transform.position = position;

        }
    }
}
