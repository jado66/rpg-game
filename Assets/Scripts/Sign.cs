using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Sign : Interactable
{
    [TextArea(3, 10)]
    public string signText = "Enter sign text here";

    public GameObject dialogPanel;
    public TMP_Text dialogText;

    private bool isDisplaying = false;
 
    public Button backButton; // Reference to the Back button
    public Button nextButton; // Reference to the Next button


    public override void OnCharacterInteract(CharacterWorldInteraction character)
    {
        if (isDisplaying)
        {
            HideSignText();
            character.StopReadingSign();

        }
        else
        {
            ShowSignText();
            character.ReadSign(this);
        }
    }

    private void ShowSignText()
    {

        dialogPanel.SetActive(true);
        dialogText.text = signText;
        isDisplaying = true;

        backButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);

    }

    public void HideSignText()
    {
        dialogPanel.SetActive(false);
        isDisplaying = false;
    }
}