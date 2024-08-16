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

    public bool isPartOfDoor = false;

    public bool isGoingIndoors = false;

    public bool isGoingUnderground = false;

    public MusicChanger musicChanger;

    void Start(){
        // Find the MusicChanger component in the scene.
        musicChanger = FindObjectOfType<MusicChanger>();

        if (musicChanger != null)
        {
            Debug.Log("Found MusicChanger component.");
            // You can now interact with the musicChanger instance.
        }
        else
        {
            Debug.LogWarning("MusicChanger component not found.");
        }
        // Debug.Log("Teleporting to "+PlayerPrefs.GetInt("teleportTo").ToString());
        // if (PlayerPrefs.GetInt("teleportTo") == this.recievingFrequency){
        //     movePlayerAndCamera(this.gameObject,true);
        //     if (GameObject.Find("Player1").GetComponent<Player>().hasPlayerStartedTheGame())
        //         GameObject.Find("SceneManager").GetComponent<SceneManager>().normalizedHour = PlayerPrefs.GetFloat("hour");
        // }
        // if (PlayerPrefs.GetInt("oneWay?")==1 || isExit){
        //     PlayerPrefs.SetInt("oneWay?",0);
        //     PlayerPrefs.Save();

        //     Disable();
        // }
    }

    void Disable(){
        GetComponent<BoxCollider2D>().enabled = false;

        if (GetComponent<SpriteRenderer>()){
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    
    void OnTriggerEnter2D(Collider2D collider){

        if (!collider.gameObject.CompareTag("Character")) {
            return;
        }

        Debug.Log("collided with teleport");
        // if (collider.gameObject.name == "Player1" || collider.gameObject.name == "Character(Clone)" ){
        //     if ((collider.gameObject.GetComponent<Character>().GetMovement().playerFacingDirection.x == playerFacingVector.x &&
        //         collider.gameObject.GetComponent<Character>().GetMovement().playerFacingDirection.y == playerFacingVector.y )||
        //         (playerFacingVector == new Vector2(0,0))){
        teleportPlayer();

        
        
        if (isPartOfDoor){
            GetComponent<BoxCollider2D>().enabled = false;
        }
                    
       
    }   

    

    public void teleportPlayer(){
        StartCoroutine(TeleportWithDelay());
    }

    private IEnumerator TeleportWithDelay() {

        GameObject player = GameObject.FindWithTag("Character");
        CharacterMovement playerMovement = player.GetComponent<CharacterMovement>();

        if (!playerMovement.CanTeleport()){
            yield break;
        };

        playerMovement.ResetTeleportTimer();

        SceneManager sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        sceneManager.StartFakeLoading();
        yield return new WaitForSeconds(0.2f); // Wait for 0.2 seconds

        if (isGoingIndoors) {
            sceneManager.minIntensity = 20;
            sceneManager.maxIntensity = 80;
        } 
        else if (isGoingUnderground){
            sceneManager.minIntensity = 0;
            sceneManager.maxIntensity = 0; // TODO 0
            sceneManager.characterUI.TriggerNight(0f, true);
        }
        else {
            sceneManager.maxIntensity = 80;
            sceneManager.minIntensity = 0;
        }

        Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        PlayerPrefs.SetInt("teleportTo", this.sendingFrequency);
        PlayerPrefs.SetInt("inventoryUp?", (GameObject.Find("InventoryMenu") != null ? 1 : 0));

        if (throwRandomDirection) {
            float angle = Random.Range(0, 360f);
            Vector2 move = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized * 2;
            Debug.Log("Move = " + move.ToString());
            PlayerPrefs.SetFloat("teleportMoveX", move.x);
            PlayerPrefs.SetFloat("teleportMoveY", move.y);
        } else {
            PlayerPrefs.SetFloat("teleportMoveX", movePlayerAfterTeleport.x);
            PlayerPrefs.SetFloat("teleportMoveY", movePlayerAfterTeleport.y);
        }

        PlayerPrefs.SetFloat("normalizedHour", GameObject.Find("SceneManager").GetComponent<SceneManager>().normalizedHour);
        PlayerPrefs.Save();

        if (currentScene.name == sceneName)
            movePlayerAndCamera(player, GameObject.Find(entangledTeleporterName), false);
        else {
            
            Instantiate(loadingScreen, Vector3.zero, Quaternion.identity);
        }
    }

    void movePlayerAndCamera(GameObject player, GameObject recievingTeleport, bool linkPlayer){
        

        player.GetComponent<Character>().isIndoors = isGoingIndoors;
        musicChanger.OnPlayerLocationChange(isGoingIndoors, isGoingUnderground);
        
       

        Vector3 playerPosition = player.transform.position;
        playerPosition.x = recievingTeleport.transform.position.x + movePlayerAfterTeleport.x;
        playerPosition.y = recievingTeleport.transform.position.y + movePlayerAfterTeleport.y;
        player.transform.position = playerPosition;
        

        GameObject mainCamera= GameObject.Find("MainCamera");
        Vector3 cameraPosition = mainCamera.transform.position;
        cameraPosition.x = recievingTeleport.transform.position.x + movePlayerAfterTeleport.x;
        cameraPosition.y = recievingTeleport.transform.position.y + movePlayerAfterTeleport.y;
        mainCamera.transform.position = cameraPosition;



            // player.GetComponent<Player>().LinkPlayerToUI();
    }
}


 // if (!player.GetComponent<Character>().hasPlayerStartedTheGame()){
        //     return;
        // }
        
        // if (PlayerPrefs.GetInt("inventoryUp?")==1){
        //     GameObject.Find("SceneManager").GetComponent<SceneManager>().inventoryGui.SetActive(true);
        // }

        // float moveX = PlayerPrefs.GetFloat("teleportMoveX");
        // float moveY = PlayerPrefs.GetFloat("teleportMoveY"); // if (linkPlayer)
        //

        // player.GetComponent<Player>().checkIfNewRealm();

        // var followers = player.GetComponent<Player>().followingObjects;
        // foreach (var follower in followers){
        //     follower.transform.position = playerPosition + new Vector3(Random.Range(-1,1),Random.Range(-1,1),0);
        // }

        //UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            // var followingPlayerObjects = GameObject.Find("Player1").GetComponent<Character>().followingObjects;
            // foreach(var followingObject in followingPlayerObjects ){
            //     DontDestroyOnLoad(followingObject);
            // }
            // LoadingData.sceneToLoad = sceneName;

            // void OnTriggerStay2D(Collider2D collider){
    //     if (collider.gameObject.name == "Character" || collider.gameObject.name == "Character(Clone)" ){
    //         if ((collider.gameObject.GetComponent<Animator>().GetFloat("moveX") == playerFacingVector.x &&
    //             collider.gameObject.GetComponent<Animator>().GetFloat("moveY") == playerFacingVector.y) ||
    //             (playerFacingVector == new Vector2(0,0))){
    //                 teleportPlayer();
    //         }
    //     }
    // }   