using UnityEngine;

public class AutoDialogTrigger : NewDialogTrigger
{
    private void Start()
    {
        base.Start();

        // Automatically trigger the dialog at the start
        TriggerDialog();
    }
}