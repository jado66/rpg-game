using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private TMP_Text[] optionTexts;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject dialogPanel;

    private DialogManager dialogManager;

    private void Start()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }
        nextButton.onClick.AddListener(OnNextButtonClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    public void ConnectDialogToUI(DialogManager newDialogManager){
        dialogManager = newDialogManager;
    }

    public void SetDialogText(string text)
    {
        dialogText.text = text;
    }

    public void SetButtons(List<DialogOption> options)
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < options.Count)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionTexts[i].text = options[i].buttonText;
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void HideButtons()
    {
        foreach (var button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void SetNextButtonEnabled(bool enabled)
    {
        nextButton.gameObject.SetActive(enabled);
    }

    public void SetBackButtonEnabled(bool enabled)
    {
        backButton.gameObject.SetActive(enabled);
    }

    private void OnOptionSelected(int index)
    {
        dialogManager.HandleOptionSelected(index);
    }

    private void OnNextButtonClicked()
    {
        dialogManager.HandleNextButton();
    }

    private void OnBackButtonClicked()
    {
        dialogManager.HandleBackButton();
    }

    public void ShowDialog()
    {
        dialogPanel.SetActive(true);
    }

    public void HideDialog()
    {
        dialogPanel.SetActive(false);
    }

    public void SetNextButtonText(string text)
    {
        nextButton.GetComponentInChildren<TMP_Text>().text = text;
    }
}