using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class TitleScreenManager : MonoBehaviour
{
    public GameObject[] titleScreens;
    public Text[] titleScreenTexts;
    public Image line;
    public Color[] titleColors;
    public float fadeDuration = 2f;
    public float rotationInterval = 5f;
    public GameObject MainMenuScreen;

    public GameObject titleText;

    private int currentIndex = 0;
    private bool isRotating = true;
    private Coroutine rotationCoroutine;

    public GameObject loadingScreen;

    public TMP_InputField  newGameInput;

    public GameObject ControllerPicker;

    private Action pendingAction;

    public enum ControllerType
    {
        Desktop,
        Mobile,
        Console
    }

    private void Start()
    {
        InitializeTitleScreens();
        StartTitleScreenRotation();
    }

    private void Update()
    {
        bool anyInput = Input.anyKey || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);

        if (anyInput && isRotating)
        {
            StopTitleScreenRotation();
            GoToMainMenu();
        }
    }

    private void InitializeTitleScreens()
    {
        for (int i = 0; i < titleScreens.Length; i++)
        {
            titleScreens[i].SetActive(i == 0);
            if (i == 0)
            {
                titleScreens[i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    private void StartTitleScreenRotation()
    {
        isRotating = true;
        rotationCoroutine = StartCoroutine(RotateTitleScreenRoutine());
    }

    private void StopTitleScreenRotation()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }
        isRotating = false;
    }

    private IEnumerator RotateTitleScreenRoutine()
    {
        while (isRotating)
        {
            yield return new WaitForSeconds(rotationInterval);
            yield return StartCoroutine(FadeToNextScreen());
        }
    }

    private IEnumerator FadeToNextScreen()
    {
        int nextIndex = (currentIndex + 1) % titleScreens.Length;

        Image currentImage = titleScreens[currentIndex].GetComponent<Image>();
        Image nextImage = titleScreens[nextIndex].GetComponent<Image>();

        titleScreens[nextIndex].SetActive(true);
        nextImage.color = new Color(nextImage.color.r, nextImage.color.g, nextImage.color.b, 0);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            currentImage.color = new Color(currentImage.color.r, currentImage.color.g, currentImage.color.b, 1 - alpha);
            nextImage.color = new Color(nextImage.color.r, nextImage.color.g, nextImage.color.b, alpha);

            yield return null;
        }

        currentImage.color = new Color(currentImage.color.r, currentImage.color.g, currentImage.color.b, 0);
        titleScreens[currentIndex].SetActive(false);

        nextImage.color = new Color(nextImage.color.r, nextImage.color.g, nextImage.color.b, 1);

        currentIndex = nextIndex;

        Debug.Log($"Transitioned to screen index: {currentIndex}");
    }

    private void GoToMainMenu()
    {
        Debug.Log("Going to Main Menu");
        foreach (var titleScreen in titleScreens)
        {
            titleScreen.SetActive(false);
        }
        MainMenuScreen.SetActive(true);
        titleText.SetActive(false);
    }

    // Public method to return to the starting screen
    public void ReturnToStartingScreen()
    {
        StopTitleScreenRotation();
        MainMenuScreen.SetActive(false);
        
        // Reset to the first title screen
        currentIndex = 0;
        InitializeTitleScreens();
        
        // Restart the rotation
        StartTitleScreenRotation();
        
        Debug.Log("Returned to starting screen");
        titleText.SetActive(true);
    }

    public void LoadTutorial(){
        CheckAndExecute(SetControllerAndContinue, PerformLoadTutorial);
    }

    public void StartNewGame(){
        CheckAndExecute(SetControllerAndContinue, PerformStartNewGame);
    }

    public void SetControllerAndContinue(string controllerTypeString)
    {
        if (Enum.TryParse<ControllerType>(controllerTypeString, true, out var controllerType))
        {
            PlayerPrefs.SetInt("ControllerType", (int)controllerType);
            PlayerPrefs.Save();
            pendingAction?.Invoke();
            pendingAction = null;
        }
        else
        {
            Debug.LogError("Invalid controller type: " + controllerTypeString);
        }
    }
    private void CheckAndExecute(Action<string> setControllerAndContinue, Action performAction)
    {
        if (PlayerPrefs.HasKey("ControllerType"))
        {
            performAction();
        }
        else
        {
            ControllerPicker.SetActive(true);
            pendingAction = performAction;
        }
    }
    

    private void PerformLoadTutorial(){
        LoadingData.sceneToLoad = "Tutorial";
            
        Instantiate(loadingScreen,Vector3.zero,Quaternion.identity);
    }

    private void PerformStartNewGame()
    {
        string name = newGameInput.text;

        string newName = name;

        if (newName == ""){
            newName = "player1";
        }
            
        PlayerPrefs.SetString("PlayerName",newName);
        PlayerPrefs.Save();

        LoadingData.sceneToLoad = "Terra";
            
        Instantiate(loadingScreen,Vector3.zero,Quaternion.identity);
    }

    
}