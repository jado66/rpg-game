using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    
    public int recievingFrequency;

    public int sendingFrequency;

    public Vector2 playerFacingVector;

    public Vector2 movePlayerAfterTeleport = new Vector2(0,0);
    public string sceneName;

    public string entangledTeleporterName;

    public bool throwRandomDirection;

    static float teleporterTimer;

    public GameObject loadingScreen;

    public bool isExit;

    

    void Start(){
        // Debug.Log("Teleporting to "+PlayerPrefs.GetInt("teleportTo").ToString());
        if (PlayerPrefs.GetInt("teleportTo") == this.recievingFrequency){
            movePlayerAndCamera(this.gameObject,true);
            if (GameObject.Find("Character").GetComponent<Player>().hasPlayerStartedTheGame())
                GameObject.Find("SceneManager").GetComponent<SceneManager>().hour = PlayerPrefs.GetFloat("hour");
        }
        if (PlayerPrefs.GetInt("oneWay?")==1 || isExit){
            PlayerPrefs.SetInt("oneWay?",0);
            PlayerPrefs.Save();

            Disable();
        }
    }

    void Disable(){
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
    
    void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.name == "Character" || collider.gameObject.name == "Character(Clone)" ){
            if ((collider.gameObject.GetComponent<Player>().spriteAndAnimator.GetComponent<Animator>().GetFloat("moveX") == playerFacingVector.x &&
                collider.gameObject.GetComponent<Player>().spriteAndAnimator.GetComponent<Animator>().GetFloat("moveY") == playerFacingVector.y)||
                (playerFacingVector == new Vector2(0,0))){
                    teleportPlayer();
                    
            }
        }
    }   

    // void OnTriggerStay2D(Collider2D collider){
    //     if (collider.gameObject.name == "Character" || collider.gameObject.name == "Character(Clone)" ){
    //         if ((collider.gameObject.GetComponent<Animator>().GetFloat("moveX") == playerFacingVector.x &&
    //             collider.gameObject.GetComponent<Animator>().GetFloat("moveY") == playerFacingVector.y) ||
    //             (playerFacingVector == new Vector2(0,0))){
    //                 teleportPlayer();
    //         }
    //     }
    // }   

    void teleportPlayer(){

        Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        PlayerPrefs.SetInt("teleportTo",this.sendingFrequency);
        PlayerPrefs.SetInt("inventoryUp?",(GameObject.Find("InventoryMenu")!=null?1:0));
        
        if (throwRandomDirection){
            float angle = Random.Range(0,360f);
            Vector2 move = new Vector2( Mathf.Cos(angle*Mathf.Deg2Rad) , Mathf.Sin(angle*Mathf.Deg2Rad) ).normalized*2;
            Debug.Log("Move = " + move.ToString());
            PlayerPrefs.SetFloat("teleportMoveX",move.x);
            PlayerPrefs.SetFloat("teleportMoveY",move.y);
        }
        else{
            PlayerPrefs.SetFloat("teleportMoveX",movePlayerAfterTeleport.x);
            PlayerPrefs.SetFloat("teleportMoveY",movePlayerAfterTeleport.y);    
        }    
        PlayerPrefs.SetFloat("hour",GameObject.Find("SceneManager").GetComponent<SceneManager>().hour);
        PlayerPrefs.Save();


        if (currentScene.name == sceneName)
            movePlayerAndCamera(GameObject.Find("entangledTeleportName"),false);
        else{
            
            //UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            var followingPlayerObjects = GameObject.Find("Character").GetComponent<Player>().followingObjects;
                foreach(var followingObject in followingPlayerObjects ){
                    DontDestroyOnLoad(followingObject);
                }
            }
            LoadingData.sceneToLoad = sceneName;
            
            Instantiate(loadingScreen,Vector3.zero,Quaternion.identity);
        
    } 

    void movePlayerAndCamera(GameObject recievingTeleport, bool linkPlayer){
        GameObject player = GameObject.FindWithTag("Player");
        if (!player.GetComponent<Player>().hasPlayerStartedTheGame()){
            return;
        }
        
        // if (PlayerPrefs.GetInt("inventoryUp?")==1){
        //     GameObject.Find("SceneManager").GetComponent<SceneManager>().inventoryGui.SetActive(true);
        // }

        float moveX = PlayerPrefs.GetFloat("teleportMoveX");
        float moveY = PlayerPrefs.GetFloat("teleportMoveY");

        Vector3 playerPosition = player.transform.position;
        playerPosition.x = recievingTeleport.transform.position.x + moveX;
        playerPosition.y = recievingTeleport.transform.position.y + moveY;
        player.transform.position = playerPosition;
        player.GetComponent<Player>().checkIfNewRealm();

        var followers = player.GetComponent<Player>().followingObjects;
        foreach (var follower in followers){
            follower.transform.position = playerPosition + new Vector3(Random.Range(-1,1),Random.Range(-1,1),0);
        }

        GameObject mainCamera= GameObject.Find("MainCamera");
        Vector3 cameraPosition = mainCamera.transform.position;
        cameraPosition.x = recievingTeleport.transform.position.x + moveX;
        cameraPosition.y = recievingTeleport.transform.position.y + moveY;
        mainCamera.transform.position = cameraPosition;



        // if (linkPlayer)
        //     player.GetComponent<Player>().LinkPlayerToUI();
    }
}
