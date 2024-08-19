using UnityEngine;
using UnityEngine.Events;

public class Switch : Interactable
{
    [SerializeField] private bool isSwitchedOn = false;
    
    [SerializeField] private UnityEvent onToggleOn;
    [SerializeField] private UnityEvent onToggleOff;
    
    private Animator animator;

    private string SwitchOnSFX = "Assets/SFX/Effects/UI/switch28.wav";
    private string SwitchOffSFX = "Assets/SFX/Effects/UI/switch29.wav";

    [Header("Toast Messages")]

    [SerializeField] public string SwitchOnToastMessage;


    [SerializeField] public string SwitchOnToastKey;

    [SerializeField] public string SwitchOffToastMessage;

    [SerializeField] public string SwitchOffToastKey;


    private void Start()
    {
        animator = GetComponent<Animator>();
        UpdateAnimatorState();
        type = "Switch"; // Set the type for the Interactable base class
    }

    public override void OnCharacterInteract()
    {
        Toggle();
        base.OnCharacterInteract();
    }

    public override void OnCharacterInteract(CharacterActions interaction)
    {
        Toggle();
        base.OnCharacterInteract(interaction);
    }

    public void Toggle()
    {
        AddressableAudioPlayer player = Object.FindObjectOfType<AddressableAudioPlayer>();

        isSwitchedOn = !isSwitchedOn;
        UpdateAnimatorState();
        
        if (isSwitchedOn)
        {
            onToggleOn.Invoke();
            ToastNotification.Instance.Toast(SwitchOnToastKey, SwitchOnToastMessage);
        }
        else
        {
            onToggleOff.Invoke();
            ToastNotification.Instance.Toast(SwitchOffToastKey, SwitchOffToastMessage);
        }

        player.PlayAddressableSound(isSwitchedOn?SwitchOnSFX:SwitchOffSFX);
    }

    private void UpdateAnimatorState()
    {
        if (animator != null)
        {
            animator.SetBool("isSwitchOn", isSwitchedOn);
        }
    }
}