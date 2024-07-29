using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public static class LoadingData
{
    public static float hour { get; set; }
    public static int day { get; set; }

    public static string sceneToLoad { get; set; }
    static LoadingData()
    {

    }


}

public class LoadingScreen : MonoBehaviour
{
    // To be created and destroyed
    Slider progressBar;
    
    SceneManager sceneManager;

    AsyncOperation loadingOperation;

    bool loadingSceneLoaded;

    bool loadDifferentScene;

    public void Start(){
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
       
       // add the other mode later
        // if (currentSceneName == LoadingData.seneToLoad){
        //     loadDifferentScene = false;
        //     loadingOperation = SceneManager.LoadSceneAsync("LoadingScene",LoadSceneMode.Additive);
        // }
        // else{
            DontDestroyOnLoad(this);
            // Load loading scene
            loadDifferentScene = true;
            loadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("LoadingScreen");
            
            // This has a canvas 
        // }
    }

    void Update(){
        if (!loadingSceneLoaded){
        
            if (loadingOperation.progress == 1){
                loadingSceneLoaded = true;
                progressBar = GameObject.Find("LoadingProgressBar").GetComponent<Slider>();
                // if (loadDifferentScene)
                loadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(LoadingData.sceneToLoad);
                // else
                // {
                //     // Make a fake timer  
                // }
                // or wait to do a fade or something special 
            }
        }
        else
        {   
            progressBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            Debug.Log("Progress bar should be at "+(loadingOperation.progress).ToString());
            if (loadingOperation.progress == 1){
                Debug.Log("Deleted loading screen manager");
                Destroy(this.gameObject);
            }       
        }
    }    
}
