using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogTrigger : Interactable
{
    public GameObject dialogGameObject;
    private Dialog dialog;

    public UnityEvent onFinish;  



    private void Start()
    {
        dialog = GetComponent<Dialog>();
    }

    public override void OnCharacterInteract()
    {
        if (dialogGameObject.activeSelf)
        {
            dialogGameObject.SetActive(false);
        }
        else if (dialog != null)
        {
            dialog.InitializeDialog();
        }
    }
}