using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq; // Add this line to import LINQ

public class TutorialExit : MonoBehaviour
{
    public string sceneToLoad = "StartingScreen"; // Set this in the Inspector

    public void ExitTutorial()
    {
        foreach (GameObject obj in Object.FindObjectsOfType<GameObject>())
        {
            Destroy(obj);
        }

        // Destroy all objects marked with DontDestroyOnLoad
        GameObject[] dontDestroyObjects = Object.FindObjectsOfType<GameObject>(true)
            .Where(go => go.scene.name == "DontDestroyOnLoad")
            .ToArray();

        foreach (GameObject obj in dontDestroyObjects)
        {
            Destroy(obj);
        }

        // Load the specified scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character")) // Assuming your player has a tag "Player"
        {
            ExitTutorial();
        }
    }
}