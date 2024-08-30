using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    private CharacterActions characterActions;
    public GameObject mobileController;

    public enum ControllerType
    {
        Desktop,
        Mobile,
        Console
    }

    public ControllerType currentControllerType;

    // Start is called before the first frame update
    void Start()
    {
        LoadPreferences();
    }

    void Awake()
    {
        characterActions = GameObject.FindObjectOfType<CharacterActions>();

        if (characterActions == null)
        {
            Debug.Log("Settings Manager could not find character actions");
        }
        if (mobileController == null)
        {
            Debug.Log("Settings Manager could not find mobile controller");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // You can place any periodic updates here if needed
    }

    public void ChangeControllerTypeByInt(TMP_Dropdown change)
    {
        int dropdownIndex = change.value;
        if (Enum.IsDefined(typeof(ControllerType), dropdownIndex))
        {
            ControllerType newControllerType = (ControllerType)dropdownIndex;
            SetControllerType(newControllerType);
        }
        else
        {
            Debug.LogError("Invalid controller type index: " + dropdownIndex);
        }
    }

    public void ChangeControllerType(string newControllerTypeString)
    {
        ToastNotification.Instance.Toast($"switch-to-{newControllerTypeString}", $"Switched to {newControllerTypeString} Controls.");

        if (Enum.TryParse<ControllerType>(newControllerTypeString, true, out var newControllerType))
        {
            SetControllerType(newControllerType);
        }
        else
        {
            Debug.LogError("Invalid controller type: " + newControllerTypeString);
        }
    }

    private void SetControllerType(ControllerType newControllerType)
    {


        switch (newControllerType)
        {
            case ControllerType.Desktop:
                SwitchToDesktop();
                break;
            case ControllerType.Mobile:
                SwitchToMobile();
                break;
            case ControllerType.Console:
                // SwitchToConsole();
                break;
            default:
                Debug.LogError("Unknown controller type: " + newControllerType);
                break;
        }
        SavePreferences();
    }

    private void SwitchToDesktop()
    {
        currentControllerType = ControllerType.Desktop;

        GameManager camera = UnityEngine.Object.FindObjectOfType<GameManager>();
        camera.SetZPos(-15f);

        characterActions.isMobileControlled = false;
        Debug.Log("Switched to Desktop controller type.");
        mobileController.SetActive(false);
    }

    private void SwitchToMobile()
    {
        
        GameManager camera = UnityEngine.Object.FindObjectOfType<GameManager>();
        camera.SetZPos(-12f);

        currentControllerType = ControllerType.Mobile;
        characterActions.isMobileControlled = true;
        Debug.Log("Switched to Mobile controller type.");
        mobileController.SetActive(true);
    }

    private void SwitchToConsole()
    {
        GameManager camera = UnityEngine.Object.FindObjectOfType<GameManager>();
        camera.SetZPos(-15f);

        ToastNotification.Instance.Toast("switch-to-controller", "Switched to Controller.");
        currentControllerType = ControllerType.Console;
        characterActions.isMobileControlled = false;
        Debug.Log("Switched to Console controller type.");
        mobileController.SetActive(false);
    }

    private void SavePreferences()
    {
        PlayerPrefs.SetInt("ControllerType", (int)currentControllerType);
        PlayerPrefs.Save();
    }

    private void LoadPreferences()
    {
        if (PlayerPrefs.HasKey("ControllerType"))
        {
            int savedControllerType = PlayerPrefs.GetInt("ControllerType");
            if (Enum.IsDefined(typeof(ControllerType), savedControllerType))
            {
                currentControllerType = (ControllerType)savedControllerType;
                SetControllerType(currentControllerType);
            }
            else
            {
                Debug.LogError("Saved controller type is invalid.");
            }
        }
        else
        {
            currentControllerType = ControllerType.Desktop; // Default value
            SetControllerType(currentControllerType);
        }
    }
}
