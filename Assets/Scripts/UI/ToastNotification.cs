using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ToastNotification : MonoBehaviour
{
    // Singleton instance
    public static ToastNotification Instance { get; private set; }

    public Image toastImage;           // Assign this in the inspector
    public Text toastText;             // Assign this in the inspector (Child text element)
    public GameObject closeButton;     // Assign this in the inspector

    private Coroutine currentToastCoroutine;
    private string currentMessage;

    void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: If you want the singleton to persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initially, disable Toast UI elements
        ResetToastElements();
    }

    public void Toast(string id, string message, float displayTime = 3f)
    {
        if (currentMessage == message && toastImage.gameObject.activeSelf)
        {
            // Reset the existing timer
            if (currentToastCoroutine != null)
            {
                StopCoroutine(currentToastCoroutine);
            }
        }
        else
        {
            // Update the message and reset the timer
            currentMessage = message;
            toastText.text = message;
            EnableToastElements();
        }

        // Start the toast coroutine to auto-dismiss after displayTime
        currentToastCoroutine = StartCoroutine(DismissToastAfterTime(displayTime));
    }

    IEnumerator DismissToastAfterTime(float displayTime)
    {
        yield return new WaitForSeconds(displayTime);
        DismissToast();
    }

    public void DismissToast()
    {
        ResetToastElements();
        currentMessage = null;
        
        if (currentToastCoroutine != null)
        {
            StopCoroutine(currentToastCoroutine);
            currentToastCoroutine = null;
        }
    }

    private void ResetToastElements()
    {
        toastImage.gameObject.SetActive(false);
        toastText.gameObject.SetActive(false);
        closeButton.SetActive(false);
    }

    private void EnableToastElements()
    {
        toastImage.gameObject.SetActive(true);
        toastText.gameObject.SetActive(true);
        closeButton.SetActive(true);
    }
}
