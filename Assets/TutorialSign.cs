using System.Collections.Generic;
using UnityEngine;

public class TutorialSign : Interactable
{
    public GameObject tutorialGameObject; // Reference to the entire tutorial object

    public List<string> signTutorialTexts = new List<string>();

    private TutorialText tutorialTextScript;

    private void Start()
    {
        // Assuming the TutorialText script is attached to the same GameObject
        tutorialTextScript = tutorialGameObject.GetComponent<TutorialText>();
    }

    public override void onCharacterInteract()
    {
        if (tutorialGameObject.activeSelf) // Check if the tutorialGameObject is currently active
        {
            tutorialGameObject.SetActive(false); // Close the tutorialGameObject if it's open
        }
        else if (tutorialTextScript != null)
        {
            tutorialTextScript.tutorialTexts = signTutorialTexts.ToArray();
            tutorialGameObject.SetActive(true);
            tutorialTextScript.ResetTutorial();
        }
    }
}
