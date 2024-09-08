using UnityEngine;

public class NewDialogTrigger : Interactable
{
    [SerializeField] private DialogManager dialogManager;

    protected void Start()
    {
        if (dialogManager == null)
        {
            dialogManager = GetComponent<DialogManager>();
        }
    }

    public void TriggerDialog()
    {
        dialogManager.StartDialog();
    }

    public override void OnCharacterInteract(CharacterWorldInteraction interaction)
    {
        TriggerDialog();
    }
}