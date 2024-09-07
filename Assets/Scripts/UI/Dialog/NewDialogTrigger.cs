using UnityEngine;

public class NewDialogTrigger : Interactable
{
    [SerializeField] private DialogManager dialogManager;

    private void Start()
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