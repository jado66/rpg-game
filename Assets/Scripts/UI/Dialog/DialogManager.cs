using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

[Serializable]
public class DialogPair
{
    public string key;
    public DialogData value;
}

public class DialogManager : MonoBehaviour
{
    [SerializeField] private DialogUI dialogUI;
    [SerializeField] private List<DialogPair> dialogPairs = new List<DialogPair>();
    private Dictionary<string, DialogData> dialogDataDict = new Dictionary<string, DialogData>();

    public GameObject exclamation;

    private DialogData currentDialog;
    private DialogNode currentNode;
    private int currentNodeIndex;

    [SerializeField] private string initialDialogKey;
    [SerializeField] private string currentDialogKey;
    [SerializeField] private bool canNavigateBackwards = false;

    private void Awake()
    {
        currentDialogKey = initialDialogKey;

        foreach (var pair in dialogPairs)
        {
            if (!string.IsNullOrEmpty(pair.key) && pair.value != null)
            {
                dialogDataDict[pair.key] = pair.value;
            }
        }
    }

    public void ShowExclamation(){
        if (exclamation != null){
            exclamation.SetActive(true);
        }
    }

    public void HideExclamation(){
        if (exclamation != null){
            exclamation.SetActive(false);
        }
    }

    public void SetDialogKey(string newKey)
    {
        currentDialogKey = newKey;
    }

     public void StartDialog()
    {
        dialogUI.gameObject.SetActive(true);
        dialogUI.ConnectDialogToUI(this);

        if (dialogDataDict.TryGetValue(currentDialogKey, out DialogData dialogData))
        {
            currentDialog = dialogData.CreatePlaythroughCopy();  // Create a deep copy of the DialogData
            currentNodeIndex = 0;
            currentNode = currentDialog.RootNode;
            if (currentNode == null)
            {
                Debug.LogError($"No nodes found in dialog {currentDialogKey}.");
                return;
            }
            dialogUI.ShowDialog();
            currentDialog.onDialogStart.Invoke();
            UpdateUI();
        }
        else
        {
            Debug.LogError($"Dialog with key {currentDialogKey} not found.");
        }
    }

    private void UpdateUI()
    {
        dialogUI.SetDialogText(currentNode.text);
        currentNode.OnNodeEnter.Invoke();

        dialogUI.SetBackButtonEnabled(canNavigateBackwards && currentNodeIndex > 0);

        if (currentNode.branchingType == BranchingType.Options)
        {
            dialogUI.SetButtons(currentNode.Options);
            dialogUI.SetNextButtonEnabled(false);
            dialogUI.SetNextButtonText("Next");
        }
        else if (currentNode.branchingType == BranchingType.BooleanEvent)
        {
            dialogUI.HideButtons();
            dialogUI.SetNextButtonEnabled(false);
            dialogUI.SetNextButtonText("Next");
            currentNode.BooleanEvent.RemoveAllListeners();
            currentNode.BooleanEvent.AddListener(OnBooleanEventResult);
            currentNode.BooleanEvent.Invoke(false);
        }
        else // BranchingType.None
        {
            dialogUI.HideButtons();
            bool isLastNode = currentNodeIndex == currentDialog.allNodes.Count - 1;
            dialogUI.SetNextButtonEnabled(true);
            dialogUI.SetNextButtonText(isLastNode ? "End" : "Next");
        }
    }

    private void OnBooleanEventResult(bool result)
    {
        List<DialogNode> newBranch = result ? currentNode.Branch2 : currentNode.Branch1;
        if (newBranch.Count > 0)
        {
            currentDialog.allNodes = new List<DialogNode>(newBranch);
            currentNodeIndex = 0;
            currentNode = currentDialog.allNodes[currentNodeIndex];
            UpdateUI();
        }
        else
        {
            ProcessNextNode();
        }
    }

    public void HandleOptionSelected(int optionIndex)
    {
        if (optionIndex >= 0 && optionIndex < currentNode.Options.Count)
        {
            currentNode.Options[optionIndex].onSelect.Invoke();
            List<DialogNode> newBranch = optionIndex == 0 ? currentNode.Branch1 : currentNode.Branch2;
            if (newBranch.Count > 0)
            {
                currentDialog.allNodes = new List<DialogNode>(newBranch);
                currentNodeIndex = 0;
                currentNode = currentDialog.allNodes[currentNodeIndex];
                UpdateUI();
            }
            else
            {
                ProcessNextNode();
            }
        }
    }

    public void HandleNextButton()
    {
        if (currentNode.branchingType == BranchingType.None)
        {
            if (currentNodeIndex == currentDialog.allNodes.Count - 1)
            {
                EndDialog();
            }
            else
            {
                ProcessNextNode();
            }
        }
    }

    public void HandleBackButton()
    {
        if (canNavigateBackwards && currentNodeIndex > 0)
        {
            currentNodeIndex--;
            currentNode = currentDialog.allNodes[currentNodeIndex];
            UpdateUI();
        }
    }

    private void ProcessNextNode()
    {
        currentNodeIndex++;
        if (currentNodeIndex < currentDialog.allNodes.Count)
        {
            currentNode = currentDialog.allNodes[currentNodeIndex];
            UpdateUI();
        }
        else
        {
            EndDialog();
        }
    }

    private void EndDialog()
    {
        if (currentDialog.hideExclamationOnClose){
            HideExclamation();
        }
        currentDialog.onDialogEnd.Invoke();
        currentDialog = null;
        currentNode = null;
        dialogUI.HideDialog();
    }
}