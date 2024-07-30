﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialText : MonoBehaviour
{
    public TMP_Text  textField; // Reference to the Text component
    public Button backButton; // Reference to the Back button
    public Button nextButton; // Reference to the Next button

    public TMP_Text nextButtonText; // Reference to the Next button

    public GameObject tutorialGameObject; // Reference to the entire tutorial object

    private string[] tutorialTexts = new string[]
    {
        "Welcome to the Rift tutorial!",
        "Press the arrow keys or WASD keys to move.",
        "Follow the cobble stone path to the chest.",
        "Press the E key to interact.",
        "Click and drop the items into your inventory. Make sure to get them all.",
        "Click the I key to open and close your inventory. ",
        "Move the axe into the Z box in your hotbar. Now try chopping a tree using the Z key.",
        "Take a look at your inventory! You can use your tools to chop trees, mine, etc.",
        "You can use E to interact with lots of things. Open the fence and pick some carrots.",
        "Now put the hammer into the X key of your hotbar.",
        "Open up the build menu, and select the fence icon.",
        "Now go to an open space and click the X key to build a fence.",
        "You can chop fences down with your axe so long as they don't belong to someone else.",
        "Now put your pickaxe into the C box in your hotbar.",
        "Go follow the cobblestone path until you come across some big rocks.",
        "Press the C key to mine stone. If you look hard enough you can find metal ores.",
        "Now lets try building a chest of our own",

        "Feel free to play around in this safe space. When you are done go through the portal."
    };

    private int currentIndex = 0;

    void Start()
    {
        UpdateUI();
       
    }

    void UpdateUI()
    {
        textField.text = tutorialTexts[currentIndex];

        backButton.gameObject.SetActive(currentIndex > 0);

        if (currentIndex == tutorialTexts.Length - 1)
        {
            nextButtonText.text = "Finish";
        }
        else
        {
            nextButtonText.text = "Next";
        }
    }

    public void OnBackClicked()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateUI();
        }
    }

    public  void OnNextClicked()
    {
        if (currentIndex < tutorialTexts.Length - 1)
        {
            currentIndex++;
            UpdateUI();
        }
        else
        {
            tutorialGameObject.SetActive(false); // Disable the tutorial game object
        }
    }
}