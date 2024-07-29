using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;


public class StartGameScreen : MonoBehaviour
{
    public List<Text> titleScreenTexts;
    public List<GameObject> titleScreens;

    public Image line;

    int titleScreenCount;
    int titleScreenIndex;

    int previousIndex;
    int previousIndex2;

    int keyCount;

    public GameObject MainMenuScreen;
    public GameObject SavedGameScreen;

    public GameObject About;

    public GameObject NewGame;

    public GameObject loadingScreen;

    public Text newGameInput;



                                                            //Terra                         Ignis                           Snow                                Tropical                        Desert
    List<Color> titleColors = new List<Color>(){new Color(0.239f,0.180f,0.086f,1),new Color(0.952f,.642f,.071f,1),new Color(0.239f,0.180f,0.086f,1),new Color(0.0f,.383f,.192f,1),new Color(0.240f,0.694f,0.707f,1)};
    // Start is called before the first frame update
    void Start()
    {
        titleScreenCount = titleScreens.Count;
        foreach ( var titleScreen in titleScreens)
        {
            titleScreen.SetActive(false);
        }
        titleScreens[0].SetActive(true);

        MainMenuScreen.SetActive(false);
        SavedGameScreen.SetActive(false);
        About.SetActive(false);
        NewGame.SetActive(false);

        if (GameObject.Find("Character")!= null){
            Destroy(GameObject.Find("Character"));
        }
        if (GameObject.Find("Main Camera")!= null){
            Destroy(GameObject.Find("MainCamera"));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space) && keyCount==0){
            Debug.Log("Space");
            // rotateTitleScreen();
            if (!MainMenuScreen.activeSelf)
                goToMainScreen();
            keyCount ++;
        }

        if (Input.GetKey(KeyCode.R) && keyCount==0){
            Debug.Log("Space");
            rotateTitleScreen();
            
            keyCount ++;
        }

        
        if (keyCount != 0)
            keyCount+=1;
        if (keyCount>=20)
            keyCount = 0;
        
    }

    void rotateTitleScreen(){
        
        
        int newTitleScreenIndex = UnityEngine.Random.Range(0,titleScreenCount);

        int i = 0;
        while (newTitleScreenIndex == titleScreenIndex &&
               newTitleScreenIndex == previousIndex && i < 100 )
            i ++;
            newTitleScreenIndex = UnityEngine.Random.Range(0,titleScreenCount);

        previousIndex2 = previousIndex;
        previousIndex = titleScreenIndex;
        titleScreenIndex = newTitleScreenIndex;
        

        Debug.Log("Setting "+titleScreens[titleScreenIndex].name+" active");
        foreach ( var titleScreen in titleScreens)
        {
            titleScreen.SetActive(false);
        }
        titleScreens[titleScreenIndex].SetActive(true);

        foreach(var text in titleScreenTexts){
            text.color = titleColors[titleScreenIndex];
        }
        line.color = titleColors[titleScreenIndex];
    }

    public void savePlayerPrefs(){
        // PlayerPrefs.SetFloat("MusicVol",SoundScrollers[0].GetComponent<Scrollbar>().value);
        // PlayerPrefs.SetFloat("SoundEffectsVol",SoundScrollers[1].GetComponent<Scrollbar>().value);
        // PlayerPrefs.SetFloat("WarningVol",SoundScrollers[2].GetComponent<Scrollbar>().value);
        // PlayerPrefs.SetFloat("UiVol",SoundScrollers[3].GetComponent<Scrollbar>().value);
        PlayerPrefs.Save();
    }


    public void loadSavedGameNames(){
        string path = Application.persistentDataPath + string.Format("/savedGames.fun");
        if (File.Exists(path)){
            // Debug.Log("Load successful");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            // SavedGamesDatabase savedGameDatabase = formatter.Deserialize(stream) as SavedGamesDatabase;
            // stream.Close();

            // savedGameNames = savedGameDatabase.savedGameNames;
            // numberOfSavedGames = savedGameDatabase.numberOfSavedGames;

            
        }
        else{
            Debug.Log("File does not exist");
        }
    }

    public void loadTutorial(){
        LoadingData.sceneToLoad = "Tutorial";
            
        Instantiate(loadingScreen,Vector3.zero,Quaternion.identity);
    }

    public void startNewGame(){
        string name = newGameInput.GetComponent<Text>().text;

        if (name == "")
            return;
        PlayerPrefs.SetString("PlayerName",name);
        PlayerPrefs.Save();

        LoadingData.sceneToLoad = "Tutorial";
            
        Instantiate(loadingScreen,Vector3.zero,Quaternion.identity);
    }

    public void goToMainScreen(){
        MainMenuScreen.SetActive(true);
    }
}
