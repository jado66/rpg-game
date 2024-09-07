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

    private DialogData currentDialog;
    private DialogNode currentNode;
    private Stack<DialogNode> history = new Stack<DialogNode>();
    private Queue<DialogNode> currentBranch = new Queue<DialogNode>();

    [SerializeField] private string initialDialogKey;
    [SerializeField] private string currentDialogKey;
    [SerializeField] private bool canNavigateBackwards = true;

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

    public void SetDialogKey(string newKey)
    {
        currentDialogKey = newKey;
    }

    public void StartDialog()
    {
        dialogUI.ConnectDialogToUI(this);

        if (dialogDataDict.TryGetValue(currentDialogKey, out DialogData dialogData))
        {
            currentDialog = dialogData;
            currentNode = dialogData.RootNode;
            history.Clear();
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

        bool isFirstNode = history.Count == 0;
        dialogUI.SetBackButtonEnabled(canNavigateBackwards && !isFirstNode);

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
            bool isLastNode = (currentBranch.Count == 0);
            dialogUI.SetNextButtonEnabled(true);
            dialogUI.SetNextButtonText(isLastNode ? "End" : "Next");
        }
    }

    private void OnBooleanEventResult(bool result)
    {
        EnqueueBranch(result ? currentNode.Branch2 : currentNode.Branch1);
        ProcessNextNode();
    }

    public void HandleOptionSelected(int optionIndex)
    {
        if (optionIndex >= 0 && optionIndex < currentNode.Options.Count)
        {
            currentNode.Options[optionIndex].onSelect.Invoke();
            EnqueueBranch(optionIndex == 0 ? currentNode.Branch1 : currentNode.Branch2);
            history.Push(currentNode);
            ProcessNextNode();
        }
    }

    public void HandleNextButton()
    {
        if (currentNode.branchingType == BranchingType.None)
        {
            bool isLastNode = currentDialog.allNodes.IndexOf(currentNode) == currentDialog.allNodes.Count - 1;
            if (isLastNode)
            {
                EndDialog();
            }
            else
            {
                history.Push(currentNode);
                ProcessNextNode();
            }
            
        }
    }

    public void HandleBackButton()
    {
        if (canNavigateBackwards && history.Count > 0)
        {
            currentNode = history.Pop();
            currentBranch.Clear();
            UpdateUI();
        }
    }

    private void EnqueueBranch(List<DialogNode> branch)
    {
        foreach (var node in branch)
        {
            currentBranch.Enqueue(node);
        }
    }

    private bool IsLastNode()
    {
        return currentBranch.Count == 0 && GetNextMainSequenceNode() == null;
    }

    private void ProcessNextNode()
    {
        if (currentBranch.Count > 0)
        {
            currentNode = currentBranch.Dequeue();
            UpdateUI();
        }
        else
        {
            // We've reached the end of the current branch or main sequence
            EndDialog();
        }
    }

    private void EndDialog()
    {
        currentDialog.onDialogEnd.Invoke();
        currentDialog = null;
        currentNode = null;
        currentBranch.Clear();
        history.Clear();
        dialogUI.HideDialog();
    }
}