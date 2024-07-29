using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }
    GameObject player;

    public GameObject cameraTarget;

    Image playerHealth; 
    Player playerScript;

    SceneManager SceneManager;



    int waitASec = 0;
    // Start is called before the first frame update

    private float[] keyCount = new float[10];

    private void Awake()
    {
        
    }
    void Start()
    {
        DontDestroyOnLoad(this);
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        SceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        try{ SceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>(); } 
        catch (Exception e){
            Debug.LogException(e,this);
            Debug.Log("Problem finding sceneManager in player");
        }
        // try{ playerWood = GameObject.Find("PlayerWood").GetComponent<Text>(); } 
        // catch (Exception e){
        //     Debug.LogException(e,this);
        //     Debug.Log("Problem finding playerWood in game manager");
        // }
        // try{ playerMushrooms = GameObject.Find("PlayerMushrooms").GetComponent<Text>(); } 
        // catch (Exception e){
        //     Debug.LogException(e,this);
        //     Debug.Log("Problem finding playerMushrooms in game manager");
        // }
        // try{ playerGoldCoin = GameObject.Find("PlayerGoldCoin").GetComponent<Text>(); } 
        // catch (Exception e){
        //     Debug.LogException(e,this);
        //     Debug.Log("Problem finding PlayerGoldCoin in game manager");
        // }
        // try{ playerHealth = GameObject.Find("PlayerHealth").GetComponent<Image>(); } 
        // catch (Exception e){
        //     Debug.LogException(e,this);
        //     Debug.Log("Problem finding playerHealth in game manager");
        // }
        // if (!character.GetComponent<Player>().playerExists){
        // player = Instantiate(character,Vector3.zero,Quaternion.identity);
        // }
        // else{
        //     player = GameObject.FindWithTag("Player");
        // }

        // playerScript = player.GetComponent<Player>();

        //     if (player == null){
        //         
        //         Debug.Log("CreatingCharacter");
        //         player.name = "Character";
        //     }
        // }
        

    }

    // Update is called once per frame
    void Update()
    {
         
        if (player == null){
            player=GameObject.FindWithTag("Player");
            player.GetComponent<Player>().enabled = true;
            playerScript = player.GetComponent<Player>();
        }

        if (cameraTarget == null){
            cameraTarget=GameObject.FindWithTag("Player");
        }

        Vector3 position = transform.position;
        position.x = cameraTarget.transform.position.x;
        position.y = cameraTarget.transform.position.y;
        transform.position = position;

        updateUI();

        
        

    }

    

    void updateUI(){
        // playerWood.text = playerScript.wood.ToString();
        // playerMushrooms.text = playerScript.mushrooms.ToString();
        // playerGoldCoin.text = playerScript.goldCoin.ToString();
        // playerHealth.fillAmount = (float)(playerScript.health)/(float)(playerScript.healthMax);

    }
}
